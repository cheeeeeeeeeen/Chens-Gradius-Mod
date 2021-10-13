using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace ChensGradiusMod.Projectiles.Magic
{
    public class Death2Laser : ModProjectile
    {
        private const int FullLength = 10;
        private const int SpriteSegmentLength = 32;

        private readonly Color color = new Color(255, 255, 255, 150);
        private int length = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Death Laser");
        }

        public override void SetDefaults()
        {
            projectile.width = 52;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.magic = true;
            projectile.timeLeft = 300;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            float rotation = Direction.ToRotation() - 1.57f;
            spriteBatch.Draw(texture, LaserTailPosition() - Main.screenPosition, new Rectangle(0, 0, 52, 32),
                             color, rotation, new Vector2(52 * .5f, 32 * .5f), 1f, SpriteEffects.None, 0);
            for (int i = 1; i < length; i++)
            {
                spriteBatch.Draw(texture, projectile.Center + (Direction * SpriteSegmentLength * (i + 1)) - Main.screenPosition, new Rectangle(0, 32, 52, 32),
                                 color, rotation, new Vector2(52 * .5f, 32 * .5f), 1f, SpriteEffects.None, 0);
            }
            spriteBatch.Draw(texture, LaserHeadPosition() - Main.screenPosition, new Rectangle(0, 64, 52, 32),
                             color, rotation, new Vector2(52 * .5f, 32 * .5f), 1f, SpriteEffects.None, 0);
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), LaserTailPosition(),
                                                     LaserHeadPosition(), 50, ref point);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 2;
        }

        public override void AI()
        {
            if (FullyErect)
            {
                projectile.velocity.Normalize();
                projectile.velocity *= SpriteSegmentLength;
            }
            else
            {
                projectile.Center = Main.player[projectile.owner].Center;
                length++;
            }
            CastLights();
        }

        public override bool ShouldUpdatePosition() => FullyErect;

        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Utils.PlotTileLine(LaserTailPosition(), LaserHeadPosition(),
                               projectile.width * projectile.scale, DelegateMethods.CutTiles);
        }

        public override string Texture => "ChensGradiusMod/Sprites/DeathLaser";

        private bool FullyErect => length >= FullLength;

        private Vector2 Direction
        {
            get
            {
                Vector2 d = projectile.velocity;
                d.Normalize();
                return d;
            }
        }

        private Vector2 LaserHeadPosition(int offset = 0) => projectile.Center + (Direction * SpriteSegmentLength * (length + 1 + offset));

        private Vector2 LaserTailPosition(int offset = 0) => projectile.Center + (Direction * SpriteSegmentLength * (1 + offset));

        private void CastLights()
        {
            DelegateMethods.v3_1 = new Vector3(0.7f, 0.7f, 1f);
            Utils.PlotTileLine(LaserTailPosition(10), LaserHeadPosition(10),
                               projectile.width * projectile.scale, DelegateMethods.CastLight);
        }
    }
}