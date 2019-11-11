using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options.Charge
{
  public class ChargeMultipleTwo : ChargeMultipleBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();

      DisplayName.SetDefault("Charge Multiple (Second)");
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.rare = 4;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      ModPlayer(player).optionTwo = true;
      ModPlayer(player).chargeMultiple = true;

      base.UpdateAccessory(player, hideVisual);
    }

    public override bool CanEquipAccessory(Player player, int slot)
    {
      return base.CanEquipAccessory(player, slot) &&
             ModPlayer(player).chargeMultiple &&
             ModPlayer(player).optionOne;
    }

    protected override string ProjectileName => "MultipleTwoObject";

    protected override int OptionPosition => 2;

    public override void AddRecipes()
    {
      ModRecipe recipe = new ModRecipe(mod);
      recipe.AddIngredient(mod, "OptionTwo");
      recipe.AddIngredient(ItemID.GlowingSnail, 2);
      recipe.AddIngredient(ItemID.SoulofLight, 5);
      recipe.AddIngredient(ItemID.SoulofNight, 3);
      recipe.AddRecipeGroup("ChensGradiusMod:CobaltTierBar", 10);
      recipe.AddRecipeGroup("ChensGradiusMod:TinTierBar", 40);
      recipe.AddIngredient(ItemID.Wire, 200);
      recipe.AddTile(TileID.TinkerersWorkbench);
      recipe.AddTile(TileID.BewitchingTable);
      recipe.SetResult(this);
      recipe.AddRecipe();
    }
  }
}
