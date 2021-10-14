using ChensGradiusMod.Gores;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.Projectiles.Ranged
{
    public class AlliedGarun : ModProjectile
    {
        private const int ReloadTime = 10;

        private int reloadTick = ReloadTime;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Allied Garun");
            Main.projFrames[projectile.type] = 9;
        }

        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.tileCollide = true;
            projectile.usesIDStaticNPCImmunity = true;
            projectile.idStaticNPCHitCooldown = 30;
            projectile.aiStyle = 0;
            projectile.timeLeft = 9480;
            aiType = 0;
            ComputeCenterFromHitbox(projectile, ref drawOffsetX, ref drawOriginOffsetY, 32, 252, 9);
        }

        public override string Texture => "ChensGradiusMod/Sprites/GarunAlly";

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
            AnimateProjectile(projectile, 2);
            if (++reloadTick >= ReloadTime)
            {
                int npcInd = FindTarget(projectile.Center, projectile.Center, 500);
                if (npcInd >= 0)
                {
                    Vector2 velocity = Main.npc[npcInd].Center - projectile.Center;
                    velocity.Normalize();
                    velocity *= 10;
                    Projectile.NewProjectile(projectile.Center, velocity, ModContent.ProjectileType<AlliedBacterionBullet>(), projectile.damage, projectile.knockBack, projectile.owner);
                    reloadTick = 0;
                }
            }
            reloadTick = Math.Min(reloadTick, ReloadTime);
        }

        public override void Kill(int timeLeft)
        {
            Gore.NewGorePerfect(GradiusExplode.CenterSpawn(projectile.Center), Vector2.Zero,
                                mod.GetGoreSlot("Gores/GradiusExplode"), .5f);
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Enemies/Gradius2Death"),
                           projectile.Center);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage *= 5;
            knockback *= 5;
        }
    }
}