namespace ChensGradiusMod.Projectiles.Options.Aim
{
  public class AimMultipleOneObject : AimOptionBaseObject
  {
    public override int Position => 1;

    public override bool PlayerHasAccessory() => ModOwner.optionOne;
  }
}