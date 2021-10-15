using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.Projectiles.Forces
{
    public class ForceLightBullet : ModProjectile
    {
        public const float Spd = 20f;

        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.timeLeft = 300;
            projectile.light = 0.25f;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            ComputeCenterFromHitbox(projectile, ref drawOffsetX, ref drawOriginOffsetY, 16, 8, 1);
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
        }

        public override string Texture => "ChensGradiusMod/Sprites/ForceLightBullet";

        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}