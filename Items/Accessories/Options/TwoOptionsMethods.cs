using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options
{
  public static class TwoOptionsMethods
  {
    public static int ComputeItemValue(int[] OptionPosition, Func<int, int> ComputeItemValue)
    {
      int value = 0;
      for (int i = 0; i < OptionPosition.Length; i++)
      {
        value += ComputeItemValue(OptionPosition[i]);
      }

      return value;
    }

    public static void AddRecipes(Mod mod, ModItem thisItem, Action<ModRecipe> UpgradeUsualRecipe)
    {
      ModRecipe recipe = new ModRecipe(mod);
      UpgradeUsualRecipe(recipe);
      recipe.SetResult(thisItem);
      recipe.AddRecipe();
    }

    public static void UpgradeUsualRecipe(Mod mod, string ProjectileType, int position, ModRecipe recipe)
    {
      switch (position)
      {
        case 1:
        case 2:
          recipe.AddIngredient(mod, $"{ProjectileType}OptionOne");
          recipe.AddIngredient(mod, $"{ProjectileType}OptionTwo");
          recipe.AddIngredient(ItemID.ChlorophyteOre, 5);
          goto case -1;
        case 3:
        case 4:
          recipe.AddIngredient(mod, $"{ProjectileType}OptionThree");
          recipe.AddIngredient(mod, $"{ProjectileType}OptionFour");
          recipe.AddIngredient(ItemID.BeetleHusk, 5);
          goto case -1;
        case -1:
          recipe.AddTile(TileID.TinkerersWorkbench);
          break;
      }
    }

    public static void UpdateAccessory(Player player, int[] OptionPosition,
                                       string ProjectileType, string[] ProjectileName,
                                       Action<Player> StoreProjectileCounts,
                                       Action<Player> ResetProjectileCounts,
                                       Action<Player, int, string> CreateOption,
                                       Action<Player, int> CreationOrderingBypass)
    {
      StoreProjectileCounts(player);
      for (int i = 0; i < OptionPosition.Length; i++)
      {
        CreateOption(player, OptionPosition[i], ProjectileType + ProjectileName[i]);
        CreationOrderingBypass(player, OptionPosition[i]);
      }
      ResetProjectileCounts(player);
    }
  }
}