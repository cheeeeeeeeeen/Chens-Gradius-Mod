using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options.Search
{
  public class SearchOptionFour : SearchOptionBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();

      DisplayName.SetDefault("Option type Search (Fourth)");
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.rare = 8;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      ModPlayer(player).optionFour = true;
      ModPlayer(player).searchOption = true;

      base.UpdateAccessory(player, hideVisual);
    }

    protected override string ProjectileName => "OptionFourObject";

    protected override int OptionPosition => 4;

    public override void AddRecipes()
    {
      ModRecipe recipe = new ModRecipe(mod);
      recipe.AddIngredient(mod, "OptionFour");
      recipe.AddIngredient(ItemID.Ectoplasm, 15);
      recipe.AddIngredient(ItemID.LunarTabletFragment, 15);
      recipe.AddIngredient(ItemID.ChlorophyteBar, 20);
      recipe.AddIngredient(ItemID.Wire, 300);
      recipe.AddTile(TileID.TinkerersWorkbench);
      // recipe.AddTile(TileID.IceMachine);
      recipe.SetResult(this);
      recipe.AddRecipe();
    }
  }
}
