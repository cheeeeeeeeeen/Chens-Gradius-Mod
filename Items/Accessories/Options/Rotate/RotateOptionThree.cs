using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options.Rotate
{
  public class RotateOptionThree : RotateOptionBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();

      DisplayName.SetDefault("Option type Rotate (Third)");
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.rare = 6;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      ModPlayer(player).optionThree = true;
      ModPlayer(player).rotateOption = true;

      base.UpdateAccessory(player, hideVisual);
    }

    protected override string ProjectileName => "OptionThreeObject";

    protected override int OptionPosition => 3;

    public override void AddRecipes()
    {
      ModRecipe recipe = new ModRecipe(mod);
      recipe.AddIngredient(mod, "OptionThree");
      recipe.AddIngredient(ItemID.Yelets);
      recipe.AddRecipeGroup("ChensGradiusMod:MechSoul", 8);
      recipe.AddIngredient(ItemID.HallowedBar, 12);
      recipe.AddRecipeGroup("ChensGradiusMod:SilverTierBar", 50);
      recipe.AddIngredient(ItemID.Wire, 250);
      recipe.AddTile(TileID.TinkerersWorkbench);
      recipe.AddTile(TileID.Loom);
      recipe.SetResult(this);
      recipe.AddRecipe();
    }
  }
}
