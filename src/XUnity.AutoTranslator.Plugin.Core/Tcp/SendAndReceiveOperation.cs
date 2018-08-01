using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace XUnity.AutoTranslator.Plugin.Core.Tcp
{
   public class SendAndReceiveOperation<TRequest, TResponse> : CustomYieldInstruction, ISendReceive<TResponse>
   {
      private bool _completed;

      public SendAndReceiveOperation( Stream stream, TRequest packet, Action<TResponse> completed )
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
               stream.BeginRead( buffer, 0, buffer.Length, cr =>
               {
                  var count = stream.EndRead( cr );
                  if( count != buffer.Length )
                  {
                     Succeeded = false;
                     return;
                  }

                  size = System.Net.IPAddress.NetworkToHostOrder( BitConverter.ToInt32( buffer, 0 ) );
                  buffer = new byte[ size ];
                  stream.BeginRead( buffer, 0, size, dr =>
                  {
                     count = stream.EndRead( dr );
                     if( count != buffer.Length )
                     {
                        Succeeded = false;
                        return;
                     }

                     try
                     {
                        var packetString = Encoding.UTF8.GetString( buffer );
                        Response = JsonUtility.FromJson<TResponse>( packetString );
                        Succeeded = true;
                        completed?.Invoke( Response );
                     }
                     catch( Exception )
                     {
                        Succeeded = false;
                     }
                     finally
                     {
                        _completed = true;
                     }
                  }, null );
               }, null );
            }, null );
         }, null );
      }

      public override bool keepWaiting => !_completed;

      public TResponse Response { get; private set; }

      public bool Succeeded { get; private set; }
   }
}
