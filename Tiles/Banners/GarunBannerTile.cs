using ChensGradiusMod.Items.Banners;
using ChensGradiusMod.NPCs;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ChensGradiusMod.Tiles.Banners
{
  public class GarunBannerTile : ParentBannerTile
  {
    protected override int NPCType => ModContent.NPCType<Garun>();

    protected override int ItemType => ModContent.ItemType<GarunBanner>();

    protected override Color MinimapColor => new Color(32, 80, 64);
  }
}