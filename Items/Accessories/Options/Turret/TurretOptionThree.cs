using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options.Turret
{
    public class TurretOptionThree : TurretOptionBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Option type Turret (Third)");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            item.rare = ItemRarityID.LightPurple; // 6
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ModPlayer(player).optionThree = true;
            ModPlayer(player).turretOption = true;

            base.UpdateAccessory(player, hideVisual);
        }

        protected override string ProjectileName => "OptionThreeObject";

        protected override int OptionPosition => 3;

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Cannon);
            recipe.AddIngredient(ItemID.BunnyCannon);
            UpgradeUsualRecipe(recipe);
            UpgradeUsualStations(recipe);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}