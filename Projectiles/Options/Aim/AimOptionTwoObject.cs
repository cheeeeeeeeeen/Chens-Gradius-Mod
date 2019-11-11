namespace ChensGradiusMod.Projectiles.Options.Aim
{
  public class AimOptionTwoObject : AimOptionBaseObject
  {
    public override int Position => 2;

    public override bool PlayerHasAccessory() => ModOwner.optionTwo;
  }
}