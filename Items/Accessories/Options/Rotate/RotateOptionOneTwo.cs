using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options.Rotate
{
  public class RotateOptionOneTwo : TwoRotateOptionsBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();

      DisplayName.SetDefault("Options type Rotate (1st & 2nd)");
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.rare = 7;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      ModPlayer(player).optionOne = true;
      ModPlayer(player).optionTwo = true;
      ModPlayer(player).rotateOption = true;

      base.UpdateAccessory(player, hideVisual);
    }

    public override bool CanEquipAccessory(Player player, int slot)
    {
      return ModeChecks(player, false);
    }

    protected override string[] ProjectileName { get; } = { "OptionOneObject",
                                                            "OptionTwoObject" };

    protected override int[] OptionPosition { get; } = { 1, 2 };

    public override void AddRecipes()
    {
      ModRecipe recipe = new ModRecipe(mod);
      recipe.AddIngredient(mod, "RotateOptionOne");
      recipe.AddIngredient(mod, "RotateOptionTwo");
      recipe.AddIngredient(ItemID.ChlorophyteOre, 5);
      recipe.AddTile(TileID.TinkerersWorkbench);
      recipe.SetResult(this);
      recipe.AddRecipe();
    }
  }
}
