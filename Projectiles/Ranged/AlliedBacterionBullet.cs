using ChensGradiusMod.Projectiles.Enemies;
using Terraria;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.Projectiles.Ranged
{
    public class AlliedBacterionBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Allied Bacterion Bullet");
            Main.projFrames[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.ignoreWater = true;
            projectile.penetrate = 1;
            projectile.tileCollide = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 5;
            projectile.aiStyle = 0;
            projectile.timeLeft = 240;
            projectile.extraUpdates = 1;
            projectile.light = .25f;
            aiType = 0;
        }

        public override void AI()
        {
            AnimateProjectile(projectile, BacterionBullet.FrameSpeed * 2);
        }

        public override string Texture => "ChensGradiusMod/Sprites/AllyBullet";
    }
}