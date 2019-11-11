namespace ChensGradiusMod.Projectiles.Options.Aim
{
  public class AimMultipleTwoObject : AimOptionBaseObject
  {
    public override int Position => 2;

    public override bool PlayerHasAccessory() => ModOwner.optionTwo;
  }
}