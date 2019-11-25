using ChensGradiusMod.Items.Banners;
using ChensGradiusMod.NPCs;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ChensGradiusMod.Tiles.Banners
{
  public class BigCoreCustomBannerTile : ParentBannerTile
  {
    protected override int NPCType => ModContent.NPCType<BigCoreCustom>();

    protected override int ItemType => ModContent.ItemType<BigCoreCustomBanner>();

    protected override Color MinimapColor => new Color(8, 96, 192);
  }
}