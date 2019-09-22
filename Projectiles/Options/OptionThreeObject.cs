namespace ChensGradiusMod.Projectiles.Options
{
  public class OptionThreeObject : OptionBaseObject
  {
    public override int FrameDistance => 44;

    public override int Position => 3;

    public override bool PlayerHasAccessory() => ModOwner.optionThree;
  }
}
