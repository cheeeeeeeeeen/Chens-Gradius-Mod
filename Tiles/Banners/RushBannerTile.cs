using ChensGradiusMod.Items.Banners;
using ChensGradiusMod.NPCs;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ChensGradiusMod.Tiles.Banners
{
  public class RushBannerTile : ParentBannerTile
  {
    protected override int NPCType => ModContent.NPCType<Rush>();

    protected override int ItemType => ModContent.ItemType<RushBanner>();

    protected override string MapName => "Rush Banner";

    protected override Color MinimapColor => new Color(136, 64, 176);
  }
}