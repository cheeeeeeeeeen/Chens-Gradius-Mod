using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.Projectiles.Summon
{
    public class MiniCoveredCoreMissile : ModProjectile
    {
        private const float RotationSpeed = 12f;
        private const float DetectionRange = 1000f;
        private const float RotationRate = .1f;
        private int currentTarget = -1;
        private float currentRotationSpeed = RotationSpeed;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mini Covered Core Missile");
            ProjectileID.Sets.MinionShot[projectile.type] = true;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            Main.projFrames[projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            projectile.width = 28;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.minionSlots = 0f;
            projectile.ignoreWater = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 600;
            projectile.tileCollide = false;
        }

        public override string Texture => "ChensGradiusMod/Sprites/Missile";

        public override void AI()
        {
            if (Vector2.Distance(projectile.Center, MasterProjectile.Center) > DetectionRange)
            {
                projectile.Kill();
                return;
            }
            projectile.rotation = projectile.velocity.ToRotation();
            if (currentTarget < 0 || (currentTarget >= 0 && !EntityTarget.active))
            {
                currentTarget = FindTarget(projectile.Center, MasterProjectile.Center, DetectionRange);
            }
            if (currentTarget >= 0 && EntityTarget.active)
            {
                float targetDirection = GetBearing(projectile.Center, EntityTarget.Center);
                projectile.rotation = AngularRotateRadians(projectile.rotation, targetDirection, MinRotate, MaxRotate, currentRotationSpeed);
                currentRotationSpeed += RotationRate;
                projectile.velocity = projectile.rotation.ToRotationVector2() * SpeedMultiplier;
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item14, projectile.Center);
            for (int i = 0; i < 5; i++)
            {
                int newDust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Fire, 0f, 0f, 0, default, 2f);
                Main.dust[newDust].velocity *= 2f;
            }
        }

        private Projectile MasterProjectile => Main.projectile[(int)projectile.ai[0]];

        private NPC EntityTarget => Main.npc[currentTarget];

        private float SpeedMultiplier => projectile.ai[1];
    }
}