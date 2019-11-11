using Terraria;
using Terraria.DataStructures;

namespace ChensGradiusMod.Items.Accessories.Options.Aim
{
  public abstract class AimOptionBase : OptionBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();
      Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(10, 5));
    }

    public override void PostUpdate()
    {
      Lighting.AddLight(item.Center, 1f, .2f, .2f);
    }

    public override string Texture => $"ChensGradiusMod/Sprites/AimInv{OptionPosition}";

    protected override string ProjectileType => "Aim";

    protected override string OptionTooltip =>
      "Deploys an Option type Aim.\n" +
      "Some projectiles you create are copied by the drone.\n" +
      "The drone will follow your flight path.\n" +
      "Hold the Option Action Key to allow the option to shoot towards cursor position!";

    protected override bool ModeChecks(Player player, bool includeSelf = true)
    {
      bool result = true;
      if (includeSelf) result &= ModPlayer(player).aimOption;
      return result &&
             !ModPlayer(player).freezeOption &&
             !ModPlayer(player).rotateOption &&
             !ModPlayer(player).normalOption &&
             !ModPlayer(player).chargeMultiple;
    }
  }
}