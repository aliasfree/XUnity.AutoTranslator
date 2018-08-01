namespace XUnity.AutoTranslator.Plugin.Core.Tcp
{
   public interface ISendReceive<TResponse> : ISend
   {
      TResponse Response { get; }
   }

   public interface ISend
   {
      bool Succeeded { get; }
   }
}
