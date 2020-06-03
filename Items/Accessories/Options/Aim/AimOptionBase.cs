using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options.Aim
{
  public abstract class AimOptionBase : OptionBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();
      Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(10, 6));
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.width = 34;
    }

    public override void PostUpdate()
    {
      Lighting.AddLight(item.Center, 1f, .2f, .2f);
    }

    protected override string ProjectileType => "Aim";

    protected override string OptionTooltip =>
      "Deploys an Option type Aim.\n" +
      "Some projectiles you create are copied by the drone.\n" +
      "The drone will follow your flight path.\n" +
      "Hold the Option Action Key to allow the option to shoot towards cursor position!";

    protected override void UpgradeUsualStations(ModRecipe recipe)
    {
      recipe.AddTile(TileID.AmmoBox);
    }

    protected override bool ModeChecks(GradiusModPlayer gmPlayer, bool includeSelf = true)
    {
      bool result = true;
      if (includeSelf) result &= gmPlayer.aimOption;

      result &= !gmPlayer.normalOption
             && !gmPlayer.recurveOption
             && !gmPlayer.rotateOption
             && !gmPlayer.freezeOption
             && !gmPlayer.chargeMultiple
             && !gmPlayer.spreadOption
             && !gmPlayer.searchOption;

      return result;
    }
  }
}