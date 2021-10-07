using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options.Recurve
{
    public class RecurveOptionFour : RecurveOptionBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Option type Recurve (Fourth)");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            item.rare = ItemRarityID.Yellow; // 8
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ModPlayer(player).optionFour = true;
            ModPlayer(player).recurveOption = true;

            base.UpdateAccessory(player, hideVisual);
        }

        protected override string ProjectileName => "OptionFourObject";

        protected override int OptionPosition => 4;

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.PulseBow);
            recipe.AddIngredient(ItemID.Bananarang);
            UpgradeUsualRecipe(recipe);
            UpgradeUsualStations(recipe);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}