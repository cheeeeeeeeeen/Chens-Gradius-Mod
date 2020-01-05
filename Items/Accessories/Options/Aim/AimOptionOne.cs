using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options.Aim
{
  public class AimOptionOne : AimOptionBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();

      DisplayName.SetDefault("Option type Aim (First)");
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.rare = 3;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      ModPlayer(player).optionOne = true;
      ModPlayer(player).aimOption = true;

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
      recipe.AddIngredient(ItemID.Lens, 30);
      recipe.AddIngredient(ItemID.BlackLens, 3);
      UpgradeUsualRecipe(recipe);
      UpgradeUsualStations(recipe);
      recipe.SetResult(this);
      recipe.AddRecipe();
    }
  }
}