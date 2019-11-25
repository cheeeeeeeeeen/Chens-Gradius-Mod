using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ObjectData;

namespace ChensGradiusMod.Tiles.Banners
{
  public abstract class ParentBannerTile : ParentGradiusTile
  {
    protected virtual int NPCType => 0;

    protected override string MapName => GradiusHelper.SplitCamelCase(Name.Substring(0, Name.Length - 4));

    public override void SetDefaults()
    {
      Main.tileFrameImportant[Type] = true;
      Main.tileNoAttach[Type] = true;
      Main.tileLavaDeath[Type] = true;
      TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
      TileObjectData.newTile.Height = 3;
      TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
      TileObjectData.newTile.StyleHorizontal = true;
      TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom,
                                                        TileObjectData.newTile.Width, 0);
      TileObjectData.newTile.StyleWrapLimit = 111;

      base.SetDefaults();

      dustType = -1;
      disableSmartCursor = true;
    }

    public override void NearbyEffects(int i, int j, bool closer)
    {
      if (closer)
      {
        Player player = Main.LocalPlayer;
        player.NPCBannerBuff[NPCType] = true;
        player.hasBanner = true;
      }
    }

    public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
    {
      if (i % 2 == 1) spriteEffects = SpriteEffects.FlipHorizontally;
    }

    protected override string Texture => $"ChensGradiusMod/Sprites/{Name}";
  }
}