using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.Projectiles.Options.Charge
{
    public class ChargeMultipleMissile : ModProjectile
    {
        public const float Spd = 18f;

        public Item clonedAccessory = null;

        private const float DetectionRange = 600f;
        private const float AngleSpeed = 20f;
        private const float AngleVariation = 5f;
        private const int DelayToRotate = 10;
        private const int DustRate = 4;

        private readonly int[] dustIds = new int[2] { 55, 112 };
        private readonly int trailType = ModContent.ProjectileType<ChargeMultipleTrail>();

        private float oldCurrentAngle = 0f;
        private float currentAngle = 0f;
        private int oldEnemyTarget = -1;
        private int enemyTarget = -1;
        private bool initialized = false;
        private int delayTick = 0;
        private int dustTick = 0;
        private float trailRotation = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Charged Multiple");
            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 42;
            projectile.height = 44;
            projectile.light = 1f;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
        }

        public override string Texture => "ChensGradiusMod/Sprites/ChargeMultipleMissile";

        public override bool PreAI()
        {
            if (!initialized)
            {
                initialized = true;
                oldCurrentAngle = currentAngle = projectile.ai[0];
                trailRotation = MathHelper.ToRadians(MaxRotate - currentAngle);
                projectile.timeLeft = RoundOffToWhole(projectile.ai[1]);
            }

            return initialized;
        }

        public override void AI()
        {
            CreateTrail();
            ProjectileAnimate();
            SeekEnemy();
            RotateTowardsTarget();
            SpecialEffects();
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox = new Rectangle
            {
                X = hitbox.X + 6,
                Y = hitbox.Y + 6,
                Width = hitbox.Width - 4,
                Height = hitbox.Height - 4
            };
        }

        public override void Kill(int timeLeft)
        {
            if (IsSameClientOwner(projectile))
            {
                SpawnClonedItem(clonedAccessory, projectile.Center, projectile.velocity);
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(enemyTarget);
            writer.Write(currentAngle);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            enemyTarget = reader.ReadInt32();
            currentAngle = reader.ReadSingle();
        }

        private void CreateTrail()
        {
            if (IsSameClientOwner(projectile))
            {
                Projectile.NewProjectile(projectile.Center, Vector2.Zero, trailType, projectile.damage,
                                         projectile.knockBack, projectile.owner, trailRotation);
            }
        }

        private void ProjectileAnimate()
        {
            if (++projectile.frameCounter >= 4)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= Main.projFrames[projectile.type]) projectile.frame = 0;
            }
        }

        private void SeekEnemy()
        {
            Vector2 ownerPosition = Main.player[projectile.owner].Center;
            enemyTarget = FindTarget(projectile.Center, ownerPosition, DetectionRange);

            if (oldEnemyTarget != enemyTarget)
            {
                oldEnemyTarget = enemyTarget;
                projectile.netUpdate = true;
            }
        }

        private void RotateTowardsTarget()
        {
            if (++delayTick >= DelayToRotate)
            {
                float targetAngle;
                Vector2 targetVector;
                if (enemyTarget >= 0)
                {
                    NPC npcTarget = Main.npc[enemyTarget];
                    targetVector = npcTarget.Center;
                }
                else
                {
                    Player playerTarget = Main.player[projectile.owner];
                    targetVector = playerTarget.Center;
                }

                targetAngle = GetBearing(projectile.Center, targetVector);
                if (IsSameClientOwner(projectile))
                {
                    float chosenVariant = Main.rand.NextFloat(AngleVariation);
                    currentAngle = AngularRotate(currentAngle, targetAngle, MinRotate,
                                                 MaxRotate, AngleSpeed - chosenVariant);
                    if (oldCurrentAngle != currentAngle)
                    {
                        oldCurrentAngle = currentAngle;
                        projectile.netUpdate = true;
                    }
                }
                else currentAngle = AngularRotate(currentAngle, targetAngle, MinRotate,
                                                  MaxRotate, AngleSpeed);
                trailRotation = MathHelper.ToRadians(MaxRotate - currentAngle);

                projectile.velocity = Spd * new Vector2
                {
                    X = (float)Math.Cos(MathHelper.ToRadians(currentAngle)),
                    Y = -(float)Math.Sin(MathHelper.ToRadians(currentAngle))
                };
            }
        }

        private void SpecialEffects()
        {
            if (++dustTick >= DustRate)
            {
                dustTick = 0;
                for (int i = 0; i < dustIds.Length; i++)
                {
                    Dust.NewDust(projectile.position, projectile.width, projectile.height, dustIds[i],
                                 0, 0, 0, default, 2);
                }
            }
        }
    }
}