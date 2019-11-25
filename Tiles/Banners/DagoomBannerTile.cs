using ChensGradiusMod.Items.Banners;
using ChensGradiusMod.NPCs;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ChensGradiusMod.Tiles.Banners
{
  public class DagoomBannerTile : ParentBannerTile
  {
    protected override int NPCType => ModContent.NPCType<Dagoom>();

    protected override int ItemType => ModContent.ItemType<DagoomBanner>();

    protected override Color MinimapColor => new Color(64, 176, 176);
  }
}