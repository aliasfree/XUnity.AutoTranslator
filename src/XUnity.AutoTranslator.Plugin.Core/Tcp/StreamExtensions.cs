using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XUnity.AutoTranslator.Plugin.Core.Tcp
{
   public static class StreamExtensions
   {
      public static ISendReceive<TResponse> Send<TRequest, TResponse>( this Stream stream, TRequest request, Action<TResponse> completed = null )
      {
         return new SendAndReceiveOperation<TRequest, TResponse>( stream, request, completed );
      }

      public static ISend Send<TRequest>( this Stream stream, TRequest request, Action completed = null )
      {
         return new SendOperation<TRequest>( stream, request, completed );
      }
   }
}
