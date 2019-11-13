using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options.Search
{
  public class SearchOptionThreeFour : TwoSearchOptionsBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();

      DisplayName.SetDefault("Options type Search (3rd & 4th)");
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.rare = 9;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      ModPlayer(player).optionThree = true;
      ModPlayer(player).optionFour = true;
      ModPlayer(player).searchOption = true;

      base.UpdateAccessory(player, hideVisual);
    }

    protected override string[] ProjectileName { get; } = { "OptionThreeObject",
                                                            "OptionFourObject" };

    protected override int[] OptionPosition { get; } = { 3, 4 };

    public override void AddRecipes()
    {
      ModRecipe recipe = new ModRecipe(mod);
      recipe.AddIngredient(mod, "SearchOptionThree");
      recipe.AddIngredient(mod, "SearchOptionFour");
      recipe.AddIngredient(ItemID.BeetleHusk, 5);
      recipe.AddTile(TileID.TinkerersWorkbench);
      recipe.SetResult(this);
      recipe.AddRecipe();
    }
  }
}
