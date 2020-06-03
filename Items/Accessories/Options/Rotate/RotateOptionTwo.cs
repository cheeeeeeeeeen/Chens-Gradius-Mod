﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options.Rotate
{
  public class RotateOptionTwo : RotateOptionBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();

      DisplayName.SetDefault("Option type Rotate (Second)");
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.rare = ItemRarityID.LightRed; // 4
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      ModPlayer(player).optionTwo = true;
      ModPlayer(player).rotateOption = true;

      base.UpdateAccessory(player, hideVisual);
    }

    protected override string ProjectileName => "OptionTwoObject";

    protected override int OptionPosition => 2;

    public override void AddRecipes()
    {
      ModRecipe recipe = new ModRecipe(mod);
      recipe.AddIngredient(ItemID.HelFire);
      UpgradeUsualRecipe(recipe);
      UpgradeUsualStations(recipe);
      recipe.SetResult(this);
      recipe.AddRecipe();
    }
  }
}