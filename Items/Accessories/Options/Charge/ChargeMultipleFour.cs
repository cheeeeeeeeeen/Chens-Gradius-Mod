using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options.Charge
{
  public class ChargeMultipleFour : ChargeMultipleBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();

      DisplayName.SetDefault("Charge Multiple (Fourth)");
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.rare = 8;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      ModPlayer(player).optionFour = true;
      ModPlayer(player).chargeMultiple = true;

      base.UpdateAccessory(player, hideVisual);
    }

    protected override string ProjectileName => "MultipleFourObject";

    protected override int OptionPosition => 4;

    public override void AddRecipes()
    {
      ModRecipe recipe = new ModRecipe(mod);
      recipe.AddIngredient(mod, "OptionFour");
      recipe.AddIngredient(ItemID.TruffleWorm);
      recipe.AddIngredient(ItemID.Ectoplasm, 15);
      recipe.AddIngredient(ItemID.LunarTabletFragment, 15);
      recipe.AddIngredient(ItemID.ChlorophyteBar, 20);
      recipe.AddIngredient(ItemID.Wire, 300);
      recipe.AddTile(TileID.TinkerersWorkbench);
      recipe.AddTile(TileID.BewitchingTable);
      recipe.SetResult(this);
      recipe.AddRecipe();
    }
  }
}
