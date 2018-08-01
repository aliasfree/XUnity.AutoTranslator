using System.ComponentModel;

namespace XUnity.AutoTranslator.Plugin.Core.Tcp
{
   [System.Serializable]
   internal class Packet
   {
      public PacketMethod method { get; set; }

      [DefaultValue( null )] public int? id = null;
      [DefaultValue( null )] public string text;
      [DefaultValue( null )] public string translation;
      [DefaultValue( null )] public bool? success = null;
   }
}
