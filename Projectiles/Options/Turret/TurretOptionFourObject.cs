namespace ChensGradiusMod.Projectiles.Options.Turret
{
  public class TurretOptionFourObject : TurretOptionBaseObject
  {
    public override int Position => 4;

    public override bool PlayerHasAccessory() => ModOwner.optionFour;
  }
}