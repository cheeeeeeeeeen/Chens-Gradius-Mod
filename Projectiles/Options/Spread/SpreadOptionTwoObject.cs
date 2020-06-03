namespace ChensGradiusMod.Projectiles.Options.Spread
{
  public class SpreadOptionTwoObject : SpreadOptionBaseObject
  {
    public override int Position => 2;

    public override bool PlayerHasAccessory() => ModOwner.optionTwo;
  }
}