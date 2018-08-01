using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using RestSharp.Contrib;

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

      public void OnExit()
      {
         var requestPacket = new Packet
         {
            method = "quit",
         };

         // FIXME: Does not complete in time; MUST BE SYNCHRONOUS!
         _stream.Send( requestPacket, () => _connection.Close() );
      }

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
               method = "translate",
               id = _id++,
               text = Uri.EscapeDataString( untranslatedText )
            };

            var response = _stream.Send<Packet, Packet>( requestPacket );

            yield return response;

            if( response.Succeeded )
            {
               var responsePacket = response.Response;
               Logger.Current.Debug( "responsePacket.success: " + responsePacket.success );


               if( responsePacket.success != false )
               {
                  var translatedText = HttpUtility.UrlDecode( responsePacket.translation );

                  success( translatedText );
               }
               else
               {
                  failure();
                  Logger.Current.Debug( "!responsePacket.success" );
               }
            }
            else
            {
               Logger.Current.Debug( "!response.Succeeded" );
               failure();
            }
         }
         finally
         {
            _isBusy = false;
         }
      }
   }
}
