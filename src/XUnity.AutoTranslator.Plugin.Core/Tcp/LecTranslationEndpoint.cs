using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace XUnity.AutoTranslator.Plugin.Core.Tcp
{
   public class LecTranslationEndpoint : IKnownEndpoint
   {
      private static int _id = 1;

      private TcpClient _connection;
      private Stream _stream;
      private bool _isBusy = false;

      public LecTranslationEndpoint()
      {
         _connection = new TcpClient( "localhost", 9586 );
         _stream = _connection.GetStream();
      }

      public bool IsBusy => _isBusy;

      public void OnUpdate()
      {
      }

      public bool ShouldGetSecondChanceAfterFailure()
      {
         return false;
      }

      public IEnumerator Translate( string untranslatedText, string from, string to, Action<string> success, Action failure )
      {
         _isBusy = true;
         try
         {
            var requestPacket = new Packet
            {
               method = PacketMethod.translate,
               id = _id++,
               text = Uri.EscapeDataString( untranslatedText )
            };

            var response = _stream.Send<Packet, Packet>( requestPacket );

            yield return response;

            var responsePacket = response.Response;

            var translatedText = Uri.UnescapeDataString( responsePacket.translation );

            success( translatedText );
         }
         finally
         {
            _isBusy = false;
         }
      }
   }

   public class SendAndReceiveOperation<TRequest, TResponse> : CustomYieldInstruction, ISendResponse<TResponse>
   {
      private bool _completed;

      public SendAndReceiveOperation( Stream stream, TRequest packet )
      {
         var str = JsonUtility.ToJson( packet );
         var bytes = Encoding.UTF8.GetBytes( str );

         var size = bytes.Length;
         var sizeBytes = BitConverter.GetBytes( System.Net.IPAddress.HostToNetworkOrder( size ) );

         stream.BeginWrite( sizeBytes, 0, sizeBytes.Length, ar =>
         {
            stream.EndWrite( ar );
            stream.BeginWrite( bytes, 0, bytes.Length, br =>
            {
               stream.EndWrite( br );

               byte[] buffer = new byte[ 4 ];
               stream.BeginRead( buffer, 0, size, cr =>
               {
                  var count = stream.EndRead( cr );

                  size = System.Net.IPAddress.NetworkToHostOrder( BitConverter.ToInt32( buffer, 0 ) );
                  buffer = new byte[ size ];
                  stream.BeginRead( buffer, 0, size, dr =>
                  {
                     count = stream.EndRead( dr );

                     var packetString = Encoding.UTF8.GetString( buffer );
                     Response = JsonUtility.FromJson<TResponse>( packetString );

                     _completed = true;
                  }, null );
               }, null );
            }, null );
         }, null );
      }

      public override bool keepWaiting => !_completed;

      public TResponse Response { get; private set; }
   }

   public interface ISendResponse<TResponse>
   {
      TResponse Response { get; }
   }
}
