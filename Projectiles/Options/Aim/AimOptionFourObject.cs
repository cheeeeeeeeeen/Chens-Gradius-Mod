namespace ChensGradiusMod.Projectiles.Options.Aim
{
  public class AimMultipleFourObject : AimOptionBaseObject
  {
    public override int Position => 4;

    public override bool PlayerHasAccessory() => ModOwner.optionFour;
  }
}