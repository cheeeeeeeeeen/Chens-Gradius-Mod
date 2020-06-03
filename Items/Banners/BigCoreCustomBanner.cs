using ChensGradiusMod.Tiles.Banners;
using Terraria.ModLoader;
using Terraria.ID;

namespace ChensGradiusMod.Items.Banners
{
  public class BigCoreCustomBanner : ParentBanner
  {
    public override void SetDefaults()
    {
      base.SetDefaults();

      item.rare = ItemRarityID.Green; // 2
    }

    protected override int PartnerTile => ModContent.TileType<BigCoreCustomBannerTile>();
  }
}