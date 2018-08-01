using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XUnity.AutoTranslator.Plugin.Core.Tcp
{
   public static class StreamExtensions
   {
      public static ISendResponse<TResponse> Send<TRequest, TResponse>( this Stream stream, TRequest request )
      {
         var operation = new SendAndReceiveOperation<TRequest, TResponse>( stream, request );
         return operation;
      }
   }
}
