using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace XUnity.AutoTranslator.Plugin.Core.Tcp
{
   public class SendOperation<TRequest> : CustomYieldInstruction, ISend
   {
      private bool _completed;

      public SendOperation( Stream stream, TRequest packet, Action completed )
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

               Succeeded = true;
               _completed = true;
               completed?.Invoke();
            }, null );
         }, null );
      }

      public override bool keepWaiting => !_completed;

      public bool Succeeded { get; private set; }
   }
}
