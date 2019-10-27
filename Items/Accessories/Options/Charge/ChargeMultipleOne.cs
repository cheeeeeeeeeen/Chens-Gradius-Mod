using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options.Charge
{
  public class ChargeMultipleOne : ChargeMultipleBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();

      DisplayName.SetDefault("Charge Multiple (First)");
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.rare = 3;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      ModPlayer(player).optionOne = true;
      ModPlayer(player).chargeMultiple = true;

      base.UpdateAccessory(player, hideVisual);
    }

    public override bool CanEquipAccessory(Player player, int slot)
    {
      return !ModPlayer(player).freezeOption &&
             !ModPlayer(player).normalOption &&
             !ModPlayer(player).rotateOption;
    }

    protected override string ProjectileName => "MultipleOneObject";

    protected override int OptionPosition => 1;

    public override void AddRecipes()
    {
      ModRecipe recipe = new ModRecipe(mod);
      recipe.AddIngredient(mod, "OptionOne");
      recipe.AddIngredient(ItemID.Firefly, 4);
      recipe.AddIngredient(ItemID.Gel, 50);
      recipe.AddIngredient(ItemID.Bone, 75);
      recipe.AddRecipeGroup("ChensGradiusMod:GoldTierBar", 10);
      recipe.AddIngredient(ItemID.Wire, 150);
      recipe.AddTile(TileID.TinkerersWorkbench);
      recipe.AddTile(TileID.Loom);
      recipe.SetResult(this);
      recipe.AddRecipe();
    }
  }
}
