namespace ChensGradiusMod.Projectiles.Options.Turret
{
  public class TurretOptionOneObject : TurretOptionBaseObject
  {
    public override int Position => 1;

    public override bool PlayerHasAccessory() => ModOwner.optionOne;
  }
}