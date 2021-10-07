using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options.Spread
{
    public class SpreadOptionOne : SpreadOptionBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Option type Spread (First)");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            item.rare = ItemRarityID.Orange; // 3
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ModPlayer(player).optionOne = true;
            ModPlayer(player).spreadOption = true;

            base.UpdateAccessory(player, hideVisual);
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            return ModeChecks(ModPlayer(player), false);
        }

        protected override string ProjectileName => "OptionOneObject";

        protected override int OptionPosition => 1;

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Minishark);
            UpgradeUsualRecipe(recipe);
            UpgradeUsualStations(recipe);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}