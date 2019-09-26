using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Options
{
  public class OptionTwo : OptionBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();

      DisplayName.SetDefault("Option (Second)");
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.rare = 3;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      ModPlayer(player).optionTwo = true;

      base.UpdateAccessory(player, hideVisual);
    }

    public override bool CanEquipAccessory(Player player, int slot) => player.GetModPlayer<GradiusModPlayer>().optionOne;

    public override string ProjectileName => "OptionTwoObject";

    public override int OptionPosition => 2;

    public override void AddRecipes()
    {
      ModRecipe recipe = new ModRecipe(mod);
      recipe.AddIngredient(ItemID.SoulofLight, 10);
      recipe.AddIngredient(ItemID.SoulofNight, 7);
      recipe.AddRecipeGroup("ChensGradiusMod:CobaltTierBar", 20);
      recipe.AddRecipeGroup("ChensGradiusMod:TinTierBar", 80);
      recipe.AddIngredient(ItemID.Wire, 400);
      recipe.AddIngredient(ItemID.Sapphire, 12);
      recipe.AddTile(TileID.Furnaces);
      recipe.AddTile(TileID.Anvils);
      recipe.SetResult(this);
      recipe.AddRecipe();
    }
  }
}
