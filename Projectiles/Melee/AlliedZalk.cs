using ChensGradiusMod.Gores;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.Projectiles.Melee
{
    public class AlliedZalk : ModProjectile
    {
        private const float AngleDifference = 90f;
        private const float DistanceFromMother = 40f;
        private const float Speed = 20f;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Allied Zalk");
            Main.projFrames[projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            projectile.width = 28;
            projectile.height = 28;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.ignoreWater = true;
            projectile.penetrate = 5;
            projectile.tileCollide = false;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 12;
            projectile.aiStyle = 0;
            aiType = 0;
        }

        public override string Texture => "ChensGradiusMod/Sprites/ZalkAlly";

        public override void AI()
        {
            AnimateSprite();
            Movement();
        }

        public override void Kill(int timeLeft)
        {
            Gore.NewGorePerfect(GradiusExplode.CenterSpawn(projectile.Center), Vector2.Zero,
                                mod.GetGoreSlot("Gores/GradiusExplode"), .5f);
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Enemies/Gradius2Death"),
                           projectile.Center);
        }

        private void AnimateSprite()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter > 3)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
            if (projectile.frame >= 5)
            {
                projectile.frame = 0;
            }
        }

        private void Movement()
        {
            float angleInRadians = MathHelper.ToRadians(ActualMotherProjectile.currentAngle + AngleDifference * Numbering);
            Vector2 destination = MotherProjectile.Center + (angleInRadians.ToRotationVector2() * DistanceFromMother);
            Vector2 speedValues = MoveToward(projectile.Center, destination, Speed);
            float newX = ApproachValue(projectile.Center.X, destination.X, Math.Abs(speedValues.X));
            float newY = ApproachValue(projectile.Center.Y, destination.Y, Math.Abs(speedValues.Y));
            projectile.Center = new Vector2(newX, newY);
        }

        private Projectile MotherProjectile => Main.projectile[(int)projectile.ai[0]];

        private ZalkYoyoProjectile ActualMotherProjectile => MotherProjectile.modProjectile as ZalkYoyoProjectile;

        private int Numbering => (int)projectile.ai[1];
    }
}