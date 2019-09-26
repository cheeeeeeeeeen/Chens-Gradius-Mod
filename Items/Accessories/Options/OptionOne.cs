using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options
{
  public class OptionOne : OptionBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();

      DisplayName.SetDefault("Option (First)");
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.rare = 2;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      ModPlayer(player).optionOne = true;

      base.UpdateAccessory(player, hideVisual);
    }

    public override string ProjectileName => "OptionOneObject";

    public override int OptionPosition => 1;

    public override void AddRecipes()
    {
      ModRecipe recipe = new ModRecipe(mod);
      recipe.AddIngredient(ItemID.Gel, 100);
      recipe.AddIngredient(ItemID.Bone, 150);
      recipe.AddRecipeGroup("ChensGradiusMod:GoldTierBar", 20);
      recipe.AddIngredient(ItemID.Wire, 300);
      recipe.AddIngredient(ItemID.Topaz, 8);
      recipe.AddTile(TileID.Furnaces);
      recipe.AddTile(TileID.Anvils);
      recipe.SetResult(this);
      recipe.AddRecipe();
    }
  }
}
