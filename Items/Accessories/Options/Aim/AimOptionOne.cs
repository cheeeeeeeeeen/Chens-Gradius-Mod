using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options.Aim
{
  public class AimOptionOne : AimOptionBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();

      DisplayName.SetDefault("Option type Aim (First)");
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.rare = 3;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      ModPlayer(player).optionOne = true;
      ModPlayer(player).aimOption = true;

      base.UpdateAccessory(player, hideVisual);
    }

    public override bool CanEquipAccessory(Player player, int slot)
    {
      return ModeChecks(player, false);
    }

    protected override string ProjectileName => "OptionOneObject";

    protected override int OptionPosition => 1;

    public override void AddRecipes()
    {
      ModRecipe recipe = new ModRecipe(mod);
      recipe.AddIngredient(mod, "OptionOne");
      recipe.AddIngredient(ItemID.Lens, 30);
      recipe.AddIngredient(ItemID.BlackLens, 3);
      recipe.AddIngredient(ItemID.Gel, 50);
      recipe.AddIngredient(ItemID.Bone, 75);
      recipe.AddRecipeGroup("ChensGradiusMod:GoldTierBar", 10);
      recipe.AddIngredient(ItemID.Wire, 150);
      recipe.AddTile(TileID.TinkerersWorkbench);
      recipe.AddTile(TileID.HeavyWorkBench);
      recipe.SetResult(this);
      recipe.AddRecipe();
    }
  }
}