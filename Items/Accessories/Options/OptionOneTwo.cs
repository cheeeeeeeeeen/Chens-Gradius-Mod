using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options
{
  public class OptionOneTwo : TwoOptionsBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();

      DisplayName.SetDefault("Options (1st & 2nd)");
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.rare = 6;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      ModPlayer(player).optionOne = true;
      ModPlayer(player).optionTwo = true;

      base.UpdateAccessory(player, hideVisual);
    }

    public override bool CanEquipAccessory(Player player, int slot) => true;

    protected override string[] ProjectileName { get; } = { "OptionOneObject",
                                                            "OptionTwoObject" };

    protected override int[] OptionPosition { get; } = { 1, 2 };

    public override void AddRecipes()
    {
      ModRecipe recipe = new ModRecipe(mod);
      recipe.AddIngredient(mod, "OptionOne");
      recipe.AddIngredient(mod, "OptionTwo");
      recipe.AddIngredient(ItemID.ChlorophyteOre, 5);
      recipe.AddTile(TileID.TinkerersWorkbench);
      recipe.SetResult(this);
      recipe.AddRecipe();
    }
  }
}
