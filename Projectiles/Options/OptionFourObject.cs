namespace ChensGradiusMod.Projectiles.Options
{
  public class OptionFourObject : OptionBaseObject
  {
    public override int Position => 4;

    public override bool PlayerHasAccessory() => ModOwner.optionFour;
  }
}