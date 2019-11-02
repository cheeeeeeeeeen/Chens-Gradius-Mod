using Terraria;
using Terraria.DataStructures;

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

    public override void PostUpdate()
    {
      Lighting.AddLight(item.Center, .498f, 1f, 0f);
    }

    public override string Texture => $"ChensGradiusMod/Sprites/RotateInv{OptionPosition}";

    protected override string ProjectileType => "Rotate";

    protected override string OptionTooltip =>
      "Deploys an Option type Rotate.\n" +
      "Some projectiles you create are copied by the drone.\n" +
      "The drone will follow your flight path.\n" +
      "Hold the Option Action Key to have the drone revolve you!";

    protected override bool ModeChecks(Player player, bool hideVisual)
    {
      return ModPlayer(player).rotateOption &&
             !ModPlayer(player).freezeOption &&
             !ModPlayer(player).normalOption &&
             !ModPlayer(player).chargeMultiple;
    }
  }
}