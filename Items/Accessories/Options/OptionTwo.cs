using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options
{
  public class OptionTwo : OptionBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();

      DisplayName.SetDefault("Option (Second)");
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.rare = 3;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      ModPlayer(player).optionTwo = true;
      ModPlayer(player).normalOption.Value = true;

      base.UpdateAccessory(player, hideVisual);
    }

    protected override string ProjectileName => "OptionTwoObject";

    protected override int OptionPosition => 2;

    public override void AddRecipes()
    {
      ModRecipe recipe = new ModRecipe(mod);
      recipe.AddIngredient(ItemID.SoulofLight, 10);
      recipe.AddIngredient(ItemID.SoulofNight, 7);
      recipe.AddRecipeGroup("ChensGradiusMod:CobaltTierBar", 20);
      recipe.AddRecipeGroup("ChensGradiusMod:TinTierBar", 80);
      recipe.AddIngredient(ItemID.Wire, 400);
      recipe.AddIngredient(ItemID.Sapphire, 12);
      recipe.AddTile(TileID.Furnaces);
      recipe.AddTile(TileID.Anvils);
      recipe.SetResult(this);
      recipe.AddRecipe();

      recipe = new ModRecipe(mod);
      recipe.AddIngredient(mod, "OptionSeed");
      recipe.AddIngredient(ItemID.SoulofLight, 7);
      recipe.AddIngredient(ItemID.SoulofNight, 4);
      recipe.AddRecipeGroup("ChensGradiusMod:CobaltTierBar", 14);
      recipe.AddRecipeGroup("ChensGradiusMod:TinTierBar", 56);
      recipe.AddIngredient(ItemID.Wire, 280);
      recipe.AddIngredient(ItemID.Sapphire, 8);
      recipe.AddTile(TileID.Furnaces);
      recipe.AddTile(TileID.Anvils);
      recipe.SetResult(this);
      recipe.AddRecipe();
    }
  }
}