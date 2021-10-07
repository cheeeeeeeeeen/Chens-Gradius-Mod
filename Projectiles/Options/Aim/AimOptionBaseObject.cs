using Microsoft.Xna.Framework;
using Terraria;

namespace ChensGradiusMod.Projectiles.Options.Aim
{
    public abstract class AimOptionBaseObject : OptionBaseObject
    {
        public override string Texture => "ChensGradiusMod/Sprites/AimSheet";

        protected override int SpawnDuplicateProjectile(Projectile p)
        {
            int pInd;

            if (ModOwner.isAiming)
            {
                Vector2 pPosition = ComputeOffset(Main.player[p.owner].Center, p.Center);
                Vector2 offsetVelocity = ComputeVelocityOffsetFromCursorAim(p, pPosition, Main.MouseWorld);
                pInd = Projectile.NewProjectile(pPosition, offsetVelocity, p.type, p.damage,
                                                p.knockBack, projectile.owner, 0f, 0f);
            }
            else pInd = base.SpawnDuplicateProjectile(p);

            return pInd;
        }
    }
}