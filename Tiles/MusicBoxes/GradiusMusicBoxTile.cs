using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ObjectData;

namespace ChensGradiusMod.Tiles.MusicBoxes
{
  public abstract class GradiusMusicBoxTile : ParentGradiusTile
  {
    public override void SetDefaults()
    {
      Main.tileFrameImportant[Type] = true;
      Main.tileObsidianKill[Type] = true;
      TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
      TileObjectData.newTile.Origin = new Point16(0, 1);
      TileObjectData.newTile.LavaDeath = false;
      TileObjectData.newTile.DrawYOffset = 2;

      base.SetDefaults();

      disableSmartCursor = true;
    }

    public override void MouseOver(int i, int j)
    {
      Player player = Main.LocalPlayer;
      player.noThrow = 2;
      player.showItemIcon = true;
      player.showItemIcon2 = ItemType;
    }

    protected override string MapName => $"Music Box ({ MusicName })";

    protected override string Texture => "ChensGradiusMod/Sprites/PlaceholderMusicBoxTile";

    protected virtual string MusicName => GradiusHelper.SplitCamelCase(Name.Substring(0, Name.Length - 12));

    protected override Color MinimapColor => new Color(100, 100, 100);
  }
}