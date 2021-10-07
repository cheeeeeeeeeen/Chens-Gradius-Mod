using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.Projectiles.Options.Charge
{
    public class ChargeMultipleTrail : ModProjectile
    {
        private const int DustId = 112;
        private const int DustRate = 10;

        private int dustTick = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Charged Multiple");
            Main.projFrames[projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 32;
            projectile.height = 14;
            projectile.light = .7f;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
            projectile.timeLeft = 15;
        }

        public override string Texture => "ChensGradiusMod/Sprites/ChargeMultipleTrail";

        public override void AI()
        {
            projectile.rotation = projectile.ai[0];

            if (++projectile.frameCounter >= 4)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= Main.projFrames[projectile.type]) projectile.frame = 0;
            }

            if (++dustTick >= DustRate)
            {
                dustTick = 0;
                Dust.NewDust(projectile.position, projectile.width, projectile.height, DustId);
            }
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox = new Rectangle
            {
                X = hitbox.Center.X - 12,
                Y = hitbox.Center.Y - 12,
                Width = 24,
                Height = 24
            };
        }
    }
}