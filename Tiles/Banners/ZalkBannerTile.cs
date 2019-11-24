using ChensGradiusMod.Items.Banners;
using ChensGradiusMod.NPCs;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ChensGradiusMod.Tiles.Banners
{
  public class ZalkBannerTile : ParentBannerTile
  {
    protected override int NPCType => ModContent.NPCType<Zalk>();

    protected override int ItemType => ModContent.ItemType<ZalkBanner>();

    protected override string MapName => "Zalk Banner";

    protected override Color MinimapColor => new Color(160, 208, 192);
  }
}