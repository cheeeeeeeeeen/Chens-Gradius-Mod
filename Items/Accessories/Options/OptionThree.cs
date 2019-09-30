﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options
{
  public class OptionThree : OptionBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();

      DisplayName.SetDefault("Option (Third)");
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.rare = 5;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      ModPlayer(player).optionThree = true;

      base.UpdateAccessory(player, hideVisual);
    }

    public override bool CanEquipAccessory(Player player, int slot)
    {
      return player.GetModPlayer<GradiusModPlayer>().optionTwo &&
             player.GetModPlayer<GradiusModPlayer>().optionOne;
    }

    protected override string ProjectileName => "OptionThreeObject";

    protected override int OptionPosition => 3;

    public override void AddRecipes()
    {
      ModRecipe recipe = new ModRecipe(mod);
      recipe.AddRecipeGroup("ChensGradiusMod:MechSoul", 15);
      recipe.AddIngredient(ItemID.HallowedBar, 25);
      recipe.AddRecipeGroup("ChensGradiusMod:SilverTierBar", 100);
      recipe.AddIngredient(ItemID.Wire, 500);
      recipe.AddIngredient(ItemID.Emerald, 18);
      recipe.AddTile(TileID.Furnaces);
      recipe.AddTile(TileID.Anvils);
      recipe.SetResult(this);
      recipe.AddRecipe();
    }
  }
}