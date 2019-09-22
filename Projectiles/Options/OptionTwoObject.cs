namespace ChensGradiusMod.Projectiles.Options
{
  public class OptionTwoObject : OptionBaseObject
  {
    public override int FrameDistance => 29;

    public override int Position => 2;

    public override bool PlayerHasAccessory() => ModOwner.optionTwo;
  }
}
