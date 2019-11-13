namespace ChensGradiusMod.Projectiles.Options.Search
{
  public class SearchOptionFourObject : SearchOptionBaseObject
  {
    public override int Position => 4;

    public override bool PlayerHasAccessory() => ModOwner.optionFour;
  }
}