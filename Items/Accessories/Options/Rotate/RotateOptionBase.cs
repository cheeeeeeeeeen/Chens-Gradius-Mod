using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options.Rotate
{
  public abstract class RotateOptionBase : OptionBase
  {
    public enum States : int { Following, Grouping, Rotating, Recovering };

    public const float Radius = 100f;
    public const float Speed = 10f;
    public const float AcceptedThreshold = .01f;

    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();
      Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(4, 6));
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.width = 40;
      item.height = 50;
    }

    public override void PostUpdate()
    {
      Lighting.AddLight(item.Center, .498f, 1f, 0f);
    }

    protected override string ProjectileType => "Rotate";

    protected override string OptionTooltip =>
      "Deploys an Option type Rotate.\n" +
      "Some projectiles you create are copied by the drone.\n" +
      "The drone will follow your flight path.\n" +
      "Hold the Option Action Key to have the drone revolve you!";

    protected override void UpgradeUsualStations(ModRecipe recipe)
    {
      recipe.AddTile(TileID.Loom);
    }

    protected override bool ModeChecks(GradiusModPlayer gmPlayer, bool includeSelf = true)
    {
      bool result = true;
      if (includeSelf) result &= gmPlayer.rotateOption;

      result &= !gmPlayer.normalOption
             && !gmPlayer.recurveOption
             && !gmPlayer.aimOption
             && !gmPlayer.freezeOption
             && !gmPlayer.chargeMultiple
             && !gmPlayer.spreadOption
             && !gmPlayer.searchOption;

      return result;
    }
  }
}