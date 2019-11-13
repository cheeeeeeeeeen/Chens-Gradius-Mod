namespace ChensGradiusMod.Projectiles.Options.Search
{
  public class SearchOptionThreeObject : SearchOptionBaseObject
  {
    public override int Position => 3;

    public override bool PlayerHasAccessory() => ModOwner.optionThree;
  }
}