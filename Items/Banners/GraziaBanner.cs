using ChensGradiusMod.Tiles.Banners;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Banners
{
  public class GraziaBanner : ParentBanner
  {
    public override string Texture => "ChensGradiusMod/Sprites/GraziaBanner";

    protected override int PartnerTile => ModContent.TileType<GraziaBannerTile>();
  }
}