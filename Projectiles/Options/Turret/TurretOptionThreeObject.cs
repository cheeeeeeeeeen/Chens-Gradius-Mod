namespace ChensGradiusMod.Projectiles.Options.Turret
{
    public class TurretOptionThreeObject : TurretOptionBaseObject
    {
        public override int Position => 3;

        public override bool PlayerHasAccessory() => ModOwner.optionThree;
    }
}