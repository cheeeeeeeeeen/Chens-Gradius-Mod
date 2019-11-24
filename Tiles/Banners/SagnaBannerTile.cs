using ChensGradiusMod.Items.Banners;
using ChensGradiusMod.NPCs;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ChensGradiusMod.Tiles.Banners
{
  public class SagnaBannerTile : ParentBannerTile
  {
    protected override int NPCType => ModContent.NPCType<Sagna>();

    protected override int ItemType => ModContent.ItemType<SagnaBanner>();

    protected override string MapName => "Sagna Banner";

    protected override Color MinimapColor => new Color(128, 176, 160);
  }
}