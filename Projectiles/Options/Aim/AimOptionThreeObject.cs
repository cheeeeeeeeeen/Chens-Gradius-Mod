namespace ChensGradiusMod.Projectiles.Options.Aim
{
  public class AimMultipleThreeObject : AimOptionBaseObject
  {
    public override int Position => 3;

    public override bool PlayerHasAccessory() => ModOwner.optionThree;
  }
}