using ChensGradiusMod.Items.Banners;
using ChensGradiusMod.NPCs;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ChensGradiusMod.Tiles.Banners
{
  public class MoaiBannerTile : ParentBannerTile
  {
    protected override int NPCType => ModContent.NPCType<Moai>();

    protected override int ItemType => ModContent.ItemType<MoaiBanner>();

    protected override string MapName => "Moai Banner";

    protected override Color MinimapColor => new Color(120, 72, 0);
  }
}