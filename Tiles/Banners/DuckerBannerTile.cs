using ChensGradiusMod.Items.Banners;
using ChensGradiusMod.NPCs;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ChensGradiusMod.Tiles.Banners
{
    public class DuckerBannerTile : ParentBannerTile
    {
        protected override int NPCType => ModContent.NPCType<Ducker>();

        protected override int ItemType => ModContent.ItemType<DuckerBanner>();

        protected override Color MinimapColor => new Color(224, 104, 72);
    }
}