using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options
{
  public class OptionFour : OptionBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();

      DisplayName.SetDefault("Option (Fourth)");
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.rare = 5;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      ModPlayer(player).optionFour = true;

      base.UpdateAccessory(player, hideVisual);
    }

    public override bool CanEquipAccessory(Player player, int slot) => player.GetModPlayer<GradiusModPlayer>().optionThree;

    public override string ProjectileName => "OptionFourObject";

    public override int OptionPosition => 4;

    public override void AddRecipes()
    {
      ModRecipe recipe = new ModRecipe(mod);
      recipe.AddIngredient(ItemID.SoulofLight, 20);
      recipe.AddIngredient(ItemID.SoulofNight, 20);
      recipe.AddIngredient(ItemID.SoulofMight, 5);
      recipe.AddIngredient(ItemID.SoulofFright, 5);
      recipe.AddIngredient(ItemID.SoulofSight, 5);
      recipe.AddIngredient(ItemID.LunarTabletFragment, 40);
      recipe.AddIngredient(ItemID.ChlorophyteBar, 40);
      recipe.AddIngredient(ItemID.Wire, 600);
      recipe.AddIngredient(ItemID.Ruby, 24);
      recipe.AddTile(TileID.Furnaces);
      recipe.AddTile(TileID.Anvils);
      recipe.SetResult(this);
      recipe.AddRecipe();
    }
  }
}
