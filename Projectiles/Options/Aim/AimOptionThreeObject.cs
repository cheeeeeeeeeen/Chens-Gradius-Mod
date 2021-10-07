namespace ChensGradiusMod.Projectiles.Options.Aim
{
    public class AimOptionThreeObject : AimOptionBaseObject
    {
        public override int Position => 3;

        public override bool PlayerHasAccessory() => ModOwner.optionThree;
    }
}