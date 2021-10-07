using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Banners
{
    public abstract class ParentBanner : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 10;
            item.height = 24;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
            item.rare = ItemRarityID.Blue; // 1
            item.value = Item.buyPrice(silver: 10);
            item.createTile = PartnerTile;
            item.placeStyle = PlaceStyle;
        }

        public override string Texture => $"ChensGradiusMod/Sprites/{Name}";

        protected virtual int PartnerTile => 0;

        protected virtual int PlaceStyle => 0;
    }
}