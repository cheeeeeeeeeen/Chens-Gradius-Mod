using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options.Charge
{
  public class ChargeMultipleThree : ChargeMultipleBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();

      DisplayName.SetDefault("Charge Multiple (Third)");
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.rare = 6;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      ModPlayer(player).optionThree = true;
      ModPlayer(player).chargeMultiple = true;

      base.UpdateAccessory(player, hideVisual);
    }

    public override bool CanEquipAccessory(Player player, int slot)
    {
      return base.CanEquipAccessory(player, slot) &&
             player.GetModPlayer<GradiusModPlayer>().chargeMultiple &&
             player.GetModPlayer<GradiusModPlayer>().optionTwo &&
             player.GetModPlayer<GradiusModPlayer>().optionOne;
    }

    protected override string ProjectileName => "MultipleThreeObject";

    protected override int OptionPosition => 3;

    public override void AddRecipes()
    {
      ModRecipe recipe = new ModRecipe(mod);
      recipe.AddIngredient(mod, "OptionThree");
      recipe.AddIngredient(ItemID.LightningBug, 3);
      recipe.AddRecipeGroup("ChensGradiusMod:MechSoul", 8);
      recipe.AddIngredient(ItemID.HallowedBar, 12);
      recipe.AddRecipeGroup("ChensGradiusMod:SilverTierBar", 50);
      recipe.AddIngredient(ItemID.Wire, 250);
      recipe.AddTile(TileID.TinkerersWorkbench);
      recipe.AddTile(TileID.BewitchingTable);
      recipe.SetResult(this);
      recipe.AddRecipe();
    }
  }
}
