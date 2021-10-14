using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.Projectiles.Summon
{
    public class MiniCoveredCore : ModProjectile
    {
        private const float Speed = 5f;
        private const float AngleSpeed = .05f;
        private float currentAngle = 90f;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mini Covered Core");
            Main.projFrames[projectile.type] = 10;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 56;
            projectile.height = 56;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.sentry = true;
            projectile.timeLeft = Projectile.SentryLifeTime;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
            projectile.friendly = true;
            projectile.light = .5f;
            ComputeCenterFromHitbox(projectile, ref drawOffsetX, ref drawOriginOffsetY, 64, 660, 10);
        }

        public override string Texture => "ChensGradiusMod/Sprites/MiniCoveredCoreSentry";

        public override bool CanDamage() => true;

        public override bool MinionContactDamage() => true;

        public override void AI()
        {
            MovementBehavior();
            AttackVector();
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.ApprenticeStorm, 0f, 0f, 0, default, 1.4f);
            }
        }

        private void MovementBehavior()
        {
            projectile.velocity = new Vector2((float)Math.Cos(currentAngle), (float)Math.Sin(currentAngle));
            currentAngle += AngleSpeed * projectile.spriteDirection;
            NormalizeAngleDegrees(ref currentAngle);
            projectile.frameCounter++;
            if (projectile.frameCounter > 10)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
            if (projectile.frame >= Main.projFrames[projectile.type])
            {
                projectile.frame = 0;
            }
        }

        private void AttackVector()
        {
            if (projectile.frame == 2 && projectile.frameCounter <= 0)
            {
                for (int i = 1; i <= 3; i++)
                {
                    float computedSpeed = Speed * i;
                    Projectile.NewProjectile(projectile.Center + new Vector2(0, 36), new Vector2(0, 1) * computedSpeed, ModContent.ProjectileType<MiniCoveredCoreMissile>(),
                                             projectile.damage, projectile.knockBack, projectile.owner, projectile.whoAmI, computedSpeed);
                }
            }
            else if (projectile.frame == 7 && projectile.frameCounter <= 1)
            {
                for (int i = 1; i <= 3; i++)
                {
                    float computedSpeed = Speed * i;
                    Projectile.NewProjectile(projectile.Center - new Vector2(0, 36), new Vector2(0, -1) * computedSpeed, ModContent.ProjectileType<MiniCoveredCoreMissile>(),
                                             projectile.damage, projectile.knockBack, projectile.owner, projectile.whoAmI, computedSpeed);
                }
            }
        }
    }
}