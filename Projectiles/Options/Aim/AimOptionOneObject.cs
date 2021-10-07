namespace ChensGradiusMod.Projectiles.Options.Aim
{
    public class AimOptionOneObject : AimOptionBaseObject
    {
        public override int Position => 1;

        public override bool PlayerHasAccessory() => ModOwner.optionOne;
    }
}