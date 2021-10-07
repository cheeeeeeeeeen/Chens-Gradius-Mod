namespace ChensGradiusMod.Projectiles.Options
{
    public class OptionOneObject : OptionBaseObject
    {
        public override int Position => 1;

        public override bool PlayerHasAccessory() => ModOwner.optionOne;
    }
}