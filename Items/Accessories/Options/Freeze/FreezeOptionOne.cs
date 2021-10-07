using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options.Freeze
{
    public class FreezeOptionOne : FreezeOptionBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Option type Freeze (First)");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            item.rare = ItemRarityID.Orange; // 3
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ModPlayer(player).optionOne = true;
            ModPlayer(player).freezeOption = true;

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
            recipe.AddIngredient(ItemID.SnowBlock, 150);
            UpgradeUsualRecipe(recipe);
            UpgradeUsualStations(recipe);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}