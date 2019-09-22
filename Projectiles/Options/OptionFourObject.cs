namespace ChensGradiusMod.Projectiles.Options
{
  public class OptionFourObject : OptionBaseObject
  {
    public override int FrameDistance => 59;

    public override int Position => 4;

    public override bool PlayerHasAccessory() => ModOwner.optionFour;
  }
}
