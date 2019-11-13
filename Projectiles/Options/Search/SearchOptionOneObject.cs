namespace ChensGradiusMod.Projectiles.Options.Search
{
  public class SearchOptionOneObject : SearchOptionBaseObject
  {
    public override int Position => 1;

    public override bool PlayerHasAccessory() => ModOwner.optionOne;
  }
}