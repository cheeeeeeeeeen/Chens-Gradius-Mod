using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options.Turret
{
    public class TurretOptionFour : TurretOptionBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Option type Turret (Fourth)");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            item.rare = ItemRarityID.Yellow; // 8
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ModPlayer(player).optionFour = true;
            ModPlayer(player).turretOption = true;

            base.UpdateAccessory(player, hideVisual);
        }

        protected override string ProjectileName => "OptionFourObject";

        protected override int OptionPosition => 4;

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SuperDartTrap, 2);
            recipe.AddIngredient(ItemID.FlameTrap, 2);
            recipe.AddIngredient(ItemID.SpearTrap, 2);
            recipe.AddIngredient(ItemID.SpikyBallTrap, 2);
            UpgradeUsualRecipe(recipe);
            UpgradeUsualStations(recipe);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}