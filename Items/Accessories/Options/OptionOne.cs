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
      ModPlayer(player).normalOption.Value = true;

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
      recipe.AddIngredient(ItemID.Gel, 100);
      recipe.AddIngredient(ItemID.Bone, 150);
      recipe.AddRecipeGroup("ChensGradiusMod:GoldTierBar", 20);
      recipe.AddIngredient(ItemID.Wire, 300);
      recipe.AddIngredient(ItemID.Topaz, 8);
      recipe.AddTile(TileID.Furnaces);
      recipe.AddTile(TileID.Anvils);
      recipe.SetResult(this);
      recipe.AddRecipe();

      recipe = new ModRecipe(mod);
      recipe.AddIngredient(mod, "OptionSeed");
      recipe.AddIngredient(ItemID.Gel, 70);
      recipe.AddIngredient(ItemID.Bone, 105);
      recipe.AddRecipeGroup("ChensGradiusMod:GoldTierBar", 14);
      recipe.AddIngredient(ItemID.Wire, 210);
      recipe.AddIngredient(ItemID.Topaz, 5);
      recipe.AddTile(TileID.Furnaces);
      recipe.AddTile(TileID.Anvils);
      recipe.SetResult(this);
      recipe.AddRecipe();
    }
  }
}