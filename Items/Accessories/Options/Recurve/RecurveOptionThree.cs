using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options.Recurve
{
    public class RecurveOptionThree : RecurveOptionBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Option type Recurve (Third)");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            item.rare = ItemRarityID.LightPurple; // 6
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ModPlayer(player).optionThree = true;
            ModPlayer(player).recurveOption = true;

            base.UpdateAccessory(player, hideVisual);
        }

        protected override string ProjectileName => "OptionThreeObject";

        protected override int OptionPosition => 3;

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IceBow);
            recipe.AddIngredient(ItemID.IceBoomerang);
            UpgradeUsualRecipe(recipe);
            UpgradeUsualStations(recipe);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}