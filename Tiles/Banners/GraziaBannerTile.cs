using ChensGradiusMod.Items.Banners;
using ChensGradiusMod.NPCs;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ChensGradiusMod.Tiles.Banners
{
  public class GraziaBannerTile : ParentBannerTile
  {
    protected override int NPCType => ModContent.NPCType<Grazia>();

    protected override int ItemType => ModContent.ItemType<GraziaBanner>();

    protected override Color MinimapColor => new Color(216, 176, 0);
  }
}