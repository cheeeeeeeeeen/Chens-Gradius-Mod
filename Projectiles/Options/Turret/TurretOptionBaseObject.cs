using Microsoft.Xna.Framework;
using System;

namespace ChensGradiusMod.Projectiles.Options.Turret
{
    public abstract class TurretOptionBaseObject : OptionBaseObject
    {
        private const float InterpolateValue = .1f;
        private const float ReturnToFollowThreshold = 4f;

        private States mode = States.Follow;

        public enum States { Follow, Stay, Return };

        public override string Texture => "ChensGradiusMod/Sprites/TurretSheet";

        protected override void OptionMovement()
        {
            switch (mode)
            {
                case States.Follow:
                    if (ModOwner.isTurreting) mode = States.Stay;
                    else base.OptionMovement();
                    break;

                case States.Stay:
                    if (!ModOwner.isTurreting) mode = States.Return;
                    break;

                case States.Return:
                    if (ModOwner.isTurreting) mode = States.Stay;
                    else
                    {
                        Vector2 dest = ModOwner.optionFlightPath[Math.Min(PathListSize - 1, FrameDistance)];
                        projectile.Center = Vector2.Lerp(projectile.Center, dest, InterpolateValue);

                        if (Vector2.Distance(dest, projectile.Center) <= ReturnToFollowThreshold)
                        {
                            mode = States.Follow;
                        }
                    }

                    break;
            }
        }
    }
}