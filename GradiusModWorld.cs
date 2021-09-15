using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ChensGradiusMod
{
  public class GradiusModWorld : ModWorld
  {
    public static bool bigcoreDowned;

    public override void Initialize()
    {
      bigcoreDowned = false;
    }

    public override void Load(TagCompound tag)
    {
      var downed = tag.GetList<string>("downed");
      bigcoreDowned = downed.Contains("bigcorecustom");
    }

    public override TagCompound Save()
    {
      var downed = new List<string>();
      if (bigcoreDowned) downed.Add("bigcorecustom");

      return new TagCompound { ["downed"] = downed };
    }

    public override void NetSend(BinaryWriter writer)
    {
      var flags = new BitsByte();
      flags[0] = bigcoreDowned;
      writer.Write(flags);
    }

    public override void NetReceive(BinaryReader reader)
    {
      BitsByte flags = reader.ReadByte();
      bigcoreDowned = flags[0];
    }

    public static bool IsBigCoreDowned() => bigcoreDowned;
  }
}