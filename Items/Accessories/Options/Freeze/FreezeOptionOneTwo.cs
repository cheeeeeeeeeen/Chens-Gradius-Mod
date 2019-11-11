using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options.Freeze
{
  public class FreezeOptionOneTwo : TwoFreezeOptionsBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();

      DisplayName.SetDefault("Options type Freeze (1st & 2nd)");
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
      ModPlayer(player).freezeOption = true;

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
      recipe.AddIngredient(mod, "FreezeOptionOne");
      recipe.AddIngredient(mod, "FreezeOptionTwo");
      recipe.AddIngredient(ItemID.ChlorophyteOre, 5);
      recipe.AddTile(TileID.TinkerersWorkbench);
      recipe.SetResult(this);
      recipe.AddRecipe();
    }
  }
}
