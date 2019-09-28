using Terraria;

namespace ChensGradiusMod.Items.Accessories.Options.Rotate
{
  public abstract class RotateOptionBase : OptionBase
  {
    public enum States : int { Following, Grouping, Rotating, Recovering };

    public static float radius = 100f;
    public static float speed = 10f;
    public static float acceptedThreshold = .01f;

    protected override string OptionTooltip =>
      "Deploys an Option type Rotate.\n" +
      "Some projectiles you create are copied by the drone.\n" +
      "The drone will follow your flight path.\n" +
      "Hold the Option Action Key to have the drone revolve you!\n";

    protected override bool ModeChecks(Player player, bool hideVisual)
    {
      return ModPlayer(player).rotateOption;
    }
  }
}