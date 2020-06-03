namespace ChensGradiusMod.Projectiles.Options.Spread
{
  public class SpreadOptionThreeObject : SpreadOptionBaseObject
  {
    public override int Position => 3;

    public override bool PlayerHasAccessory() => ModOwner.optionThree;
  }
}