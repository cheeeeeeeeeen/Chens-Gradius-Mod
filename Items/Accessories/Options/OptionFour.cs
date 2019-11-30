using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options
{
  public class OptionFour : OptionBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();

      DisplayName.SetDefault("Option (Fourth)");
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.rare = 7;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      ModPlayer(player).optionFour = true;
      ModPlayer(player).normalOption.Value = true;

      base.UpdateAccessory(player, hideVisual);
    }

    protected override string ProjectileName => "OptionFourObject";

    protected override int OptionPosition => 4;

    public override void AddRecipes()
    {
      ModRecipe recipe = new ModRecipe(mod);
      recipe.AddIngredient(ItemID.Ectoplasm, 30);
      recipe.AddIngredient(ItemID.LunarTabletFragment, 30);
      recipe.AddIngredient(ItemID.ChlorophyteBar, 40);
      recipe.AddIngredient(ItemID.Wire, 600);
      recipe.AddIngredient(ItemID.Ruby, 24);
      recipe.AddTile(TileID.Furnaces);
      recipe.AddTile(TileID.Anvils);
      recipe.SetResult(this);
      recipe.AddRecipe();

      recipe = new ModRecipe(mod);
      recipe.AddIngredient(mod, "OptionSeed");
      recipe.AddIngredient(ItemID.Ectoplasm, 21);
      recipe.AddIngredient(ItemID.LunarTabletFragment, 21);
      recipe.AddIngredient(ItemID.ChlorophyteBar, 28);
      recipe.AddIngredient(ItemID.Wire, 420);
      recipe.AddIngredient(ItemID.Ruby, 16);
      recipe.AddTile(TileID.Furnaces);
      recipe.AddTile(TileID.Anvils);
      recipe.SetResult(this);
      recipe.AddRecipe();
    }
  }
}