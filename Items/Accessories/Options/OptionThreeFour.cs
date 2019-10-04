using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options
{
  public class OptionThreeFour : TwoOptionsBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();

      DisplayName.SetDefault("Options (3rd & 4th)");
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.rare = 8;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      ModPlayer(player).optionThree = true;
      ModPlayer(player).optionFour = true;

      base.UpdateAccessory(player, hideVisual);
    }

    public override bool CanEquipAccessory(Player player, int slot)
    {
      return base.CanEquipAccessory(player, slot) &&
             player.GetModPlayer<GradiusModPlayer>().optionTwo &&
             player.GetModPlayer<GradiusModPlayer>().optionOne;
    }

    protected override string[] ProjectileName { get; } = { "OptionThreeObject",
                                                            "OptionFourObject" };

    protected override int[] OptionPosition { get; } = { 3, 4 };

    public override void AddRecipes()
    {
      ModRecipe recipe = new ModRecipe(mod);
      recipe.AddIngredient(mod, "OptionThree");
      recipe.AddIngredient(mod, "OptionFour");
      recipe.AddIngredient(ItemID.BeetleHusk, 5);
      recipe.AddTile(TileID.TinkerersWorkbench);
      recipe.SetResult(this);
      recipe.AddRecipe();
    }
  }
}
