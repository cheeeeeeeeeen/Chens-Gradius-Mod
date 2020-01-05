using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.Items.Accessories.Options
{
  public abstract class TwoOptionsBase : OptionBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();
      Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 9));
    }

    public override void SetDefaults()
    {
      base.SetDefaults();
      TwoOptionsMethods.SetDefaults(item);
    }

    public override string Texture => $"ChensGradiusMod/Sprites/Two{ProjectileType}" +
                                      $"Options{OptionPosition[0]}{OptionPosition[1]}";

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      TwoOptionsMethods.UpdateAccessory(player, OptionPosition, ProjectileType, ProjectileName,
                                        UpdateOptionFlag, StoreProjectileCounts, ResetProjectileCounts,
                                        CreateOption, CreationOrderingBypass);
    }

    public override void PostUpdate()
    {
      Lighting.AddLight(item.Center, 1.5f, .747f, 0f);
    }

    public override bool CanEquipAccessory(Player player, int slot)
    {
      return ModeChecks(ModPlayer(player)) &&
             OptionsPredecessorRequirement(ModPlayer(player), OptionPosition[0]);
    }

    protected override void UpgradeUsualRecipe(ModRecipe recipe)
    {
      TwoOptionsMethods.UpgradeUsualRecipe(mod, ProjectileType, OptionPosition[0], recipe);
    }

    protected new virtual string[] ProjectileName { get; } = { "1", "2" };

    protected new virtual int[] OptionPosition { get; } = { 1, 2 };

    protected override string OptionTooltip =>
      "Deploys two Options.\n" +
      "Some projectiles you create are copied by the drones.\n" +
      "The drones will follow your flight path.\n" +
      "These advanced drones uses Wreek technology,\n" +
      "infusing both technology and psychic elements together.";

    protected override int ComputeItemValue(int multiplier)
    {
      return TwoOptionsMethods.ComputeItemValue(OptionPosition, base.ComputeItemValue);
    }

    public override void AddRecipes()
    {
      TwoOptionsMethods.AddRecipes(mod, this, UpgradeUsualRecipe);
    }
  }
}