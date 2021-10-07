namespace ChensGradiusMod.Projectiles.Options.Turret
{
    public class TurretOptionTwoObject : TurretOptionBaseObject
    {
        public override int Position => 2;

        public override bool PlayerHasAccessory() => ModOwner.optionTwo;
    }
}