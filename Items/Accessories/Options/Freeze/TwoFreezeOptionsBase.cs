using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options.Freeze
{
  public abstract class TwoFreezeOptionsBase : FreezeOptionBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();
      Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 9));
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.width = 38;
      item.height = 30;
    }

    public override void PostUpdate()
    {
      Lighting.AddLight(item.Center, 0f, 1.1235f, 1.5f);
    }

    public override string Texture => $"ChensGradiusMod/Sprites/Two{ProjectileType}" +
                                      $"Options{OptionPosition[0]}{OptionPosition[1]}";

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      StoreProjectileCounts(player);
      for (int i = 0; i < 2; i++)
      {
        CreateOption(player, OptionPosition[i], ProjectileType + ProjectileName[i]);
        CreationOrderingBypass(player, OptionPosition[i]);
      }
      ResetProjectileCounts(player);
    }

    public override bool CanEquipAccessory(Player player, int slot)
    {
      return ModeChecks(player) &&
             GradiusHelper.OptionsPredecessorRequirement(ModPlayer(player),
                                                         OptionPosition[0]);
    }

    protected override void UpgradeUsualRecipe(ModRecipe recipe)
    {
      switch (OptionPosition[0])
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

    protected new virtual string[] ProjectileName { get; } = { "1", "2" };

    protected new virtual int[] OptionPosition { get; } = { 1, 2 };

    protected override string OptionTooltip =>
      "Deploys two Options type Freeze.\n" +
      "Some projectiles you create are copied by the drones.\n" +
      "The drones will follow your flight path.\n" +
      "Hold the Option Action Key to perform a different movement behavior!";

    public override void AddRecipes()
    {
      ModRecipe recipe = new ModRecipe(mod);
      UpgradeUsualRecipe(recipe);
      recipe.SetResult(this);
      recipe.AddRecipe();
    }
  }
}
