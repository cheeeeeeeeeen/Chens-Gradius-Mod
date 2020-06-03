namespace ChensGradiusMod.Projectiles.Options.Spread
{
  public class SpreadOptionFourObject : SpreadOptionBaseObject
  {
    public override int Position => 4;

    public override bool PlayerHasAccessory() => ModOwner.optionFour;
  }
}