namespace ChensGradiusMod.Projectiles.Options.Spread
{
    public class SpreadOptionOneObject : SpreadOptionBaseObject
    {
        public override int Position => 1;

        public override bool PlayerHasAccessory() => ModOwner.optionOne;
    }
}