using Terraria;

namespace ChensGradiusMod.Items.Accessories.Options
{
  public abstract class FreezeOptionBase : OptionBase
  {
    protected override string OptionTooltip =>
      "Deploys an Option type Freeze.\n" +
      "Some projectiles you create are copied by the drone.\n" +
      "The drone will follow your flight path.\n" +
      "Hold the Option Action Key to perform a different movement behavior!\n";

    protected override bool ModeChecks(Player player, bool hideVisual)
    {
      return ModPlayer(player).freezeOption;
    }
  }
}