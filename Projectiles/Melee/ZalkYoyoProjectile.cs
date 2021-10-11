using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.Projectiles.Melee
{
    public class ZalkYoyoProjectile : ModProjectile
    {
        private const int Cooldown = 30;
        private const float AngleSpeed = 10f;

        private int cooldownTick = Cooldown;

        public List<Projectile> alliedZalks = new List<Projectile>();
        public float currentAngle = 0f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = -1f;
            ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 500f;
            ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 20f;
        }

        public override void SetDefaults()
        {
            projectile.extraUpdates = 0;
            projectile.width = 24;
            projectile.height = 24;
            projectile.aiStyle = 99;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.melee = true;
            projectile.scale = 1f;
            projectile.localNPCHitCooldown = 10;
            projectile.usesLocalNPCImmunity = true;
        }

        public override string Texture => "ChensGradiusMod/Sprites/ZalkYoyoProjectile";

        public override void AI()
        {
            ProcessCooldown();
            UpdateReferenceAngle();
            DestroyAlliesWhenReturning();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (cooldownTick >= Cooldown)
            {
                float xSpawn;
                if (Owner.direction == 1) xSpawn = Main.screenPosition.X - 28;
                else xSpawn = Main.screenPosition.X + Main.screenWidth + 28;
                int newAllyInd = Projectile.NewProjectile(xSpawn, projectile.Center.Y, 0, 0, ModContent.ProjectileType<AlliedZalk>(),
                                                          projectile.damage, projectile.knockBack,
                                                          projectile.owner, projectile.whoAmI);
                if (newAllyInd >= 0)
                {
                    Projectile newProj = Main.projectile[newAllyInd];
                    newProj.spriteDirection = Owner.direction;
                    alliedZalks.Add(newProj);
                    cooldownTick = 0;
                }
            }
        }

        public float AngleDifference
        {
            get
            {
                if (alliedZalks.Count <= 0) return -1f;
                else return FullAngle / alliedZalks.Count;
            }
        }

        private Player Owner => Main.player[projectile.owner];

        private void ProcessCooldown()
        {
            if (IsSameClientOwner(projectile))
            {
                cooldownTick = Math.Min(cooldownTick + 1, Cooldown);
            }
        }

        private void UpdateReferenceAngle()
        {
            currentAngle += AngleSpeed * Owner.direction;
            NormalizeAngleDegrees(ref currentAngle);
        }

        private void DestroyAlliesWhenReturning()
        {
            if (projectile.ai[0] < 0)
            {
                foreach (var zalk in alliedZalks.ToArray())
                {
                    if (zalk != null && zalk.active) zalk.Kill();
                }
                alliedZalks.Clear();
            }
        }
    }
}