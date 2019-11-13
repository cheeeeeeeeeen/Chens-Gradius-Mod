namespace ChensGradiusMod.Projectiles.Options.Search
{
  public class SearchOptionTwoObject : SearchOptionBaseObject
  {
    public override int Position => 2;

    public override bool PlayerHasAccessory() => ModOwner.optionTwo;
  }
}