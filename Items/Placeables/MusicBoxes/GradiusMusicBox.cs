using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Placeables.MusicBoxes
{
    public abstract class GradiusMusicBox : ModItem
    {
        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.consumable = true;
            item.width = 32;
            item.height = 20;
            item.rare = ItemRarityID.Yellow; // 8
            item.value = 100000;
            item.accessory = true;
        }

        public override string Texture => "ChensGradiusMod/Sprites/PlaceholderMusicBox";

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Cloud, 50);
            recipe.AddIngredient(ItemID.Wire, 5);
            recipe.AddIngredient(ItemID.FallenStar, 12);
            recipe.AddIngredient(ItemID.MusicBox, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}