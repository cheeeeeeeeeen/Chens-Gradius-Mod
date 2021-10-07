﻿using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.Projectiles.Options.Miscellaneous
{
    public class OptionSeedObject : OptionBaseObject
    {
        public const float SeedDistance = 50f;

        public int rotateDirection = 0;

        private const int FireRate = 23;
        private const float RotateSpeed = 7f;
        private const int InBattleDuration = 180;

        private bool isSpawning = true;
        private float currentAngle = 0f;
        private int fireTick = 0;
        private int inBattleTick = 0;
        private bool inBattle = false;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 6;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            projectile.width = 16;
            projectile.height = 10;
        }

        public override bool PreAI()
        {
            if (PlayerHasAccessory())
            {
                if (isSpawning)
                {
                    isSpawning = false;
                    if (Owner.direction < 0) currentAngle = HalfAngle;
                }
                projectile.timeLeft = KeepAlive;
                return true;
            }
            else
            {
                projectile.active = false;
                ModOwner.seedProjectile = null;
                return false;
            }
        }

        public override void AI()
        {
            rotateDirection = ModOwner.seedRotateDirection;
            OptionAnimate();
            if (inBattle)
            {
                if (++inBattleTick >= InBattleDuration) inBattle = false;
            }
            if (inBattle && SetProjectileToSpawn()) PerformAttack();
            MoveSeed();
            OptionSpawnSoundEffect();
        }

        public override void PostAI()
        {
        }

        public override string Texture => "ChensGradiusMod/Sprites/OptionSeedSheet";

        public override bool PlayerHasAccessory() => ModOwner.optionSeed;

        public void BattleMode()
        {
            if (!inBattle) PerformAttack(fireNow: true);
            inBattleTick = 0;
            inBattle = true;
        }

        protected override float[] LightValues { get; } = { .02f, .03f, .04f, .04f, .03f, .02f };

        private int SpawnProjectileType { get; set; } = 0;

        private float SpawnProjectileSpeed { get; set; } = 0f;

        private int SpawnProjectileDamage { get; set; } = 0;

        private float SpawnProjectileKnockback { get; set; } = 0f;

        private bool SetProjectileToSpawn()
        {
            for (int i = LowerAmmoSlot; i <= HigherAmmoSlot; i++)
            {
                Item item = Owner.inventory[i];

                if (item.ammo == AmmoID.Arrow || item.ammo == AmmoID.Bullet)
                {
                    SpawnProjectileType = item.shoot;
                    SpawnProjectileSpeed = item.shootSpeed;
                    SpawnProjectileKnockback = item.knockBack;
                    SpawnProjectileDamage = item.damage;
                    return true;
                }
            }

            return false;
        }

        private void PerformAttack(bool fireNow = false)
        {
            if (IsSameClientOwner(projectile))
            {
                if (++fireTick >= FireRate || fireNow)
                {
                    fireTick = 0;

                    Item weapon = Owner.HeldItem;
                    int dmg = RoundOffToWhole((SpawnProjectileDamage + weapon.damage));
                    float kb = (SpawnProjectileKnockback + weapon.knockBack);
                    float spd = SpawnProjectileSpeed + weapon.shootSpeed;
                    Vector2 shootToward = MoveToward(projectile.Center,
                                                                   Main.MouseWorld, spd);

                    int pInd = Projectile.NewProjectile(projectile.Center, shootToward, SpawnProjectileType,
                                                        dmg, kb, projectile.owner);
                    Main.projectile[pInd].noDropItem = true;
                    ModOwner.optionAlreadyProducedProjectiles.Add(pInd);

                    Main.PlaySound(weapon.UseSound, projectile.Center);
                }
            }
        }

        private void MoveSeed()
        {
            float directionRadians = MathHelper.ToRadians(currentAngle);
            projectile.Center = new Vector2
            {
                X = Owner.Center.X + (float)Math.Cos(directionRadians) * SeedDistance,
                Y = Owner.Center.Y - (float)Math.Sin(directionRadians) * SeedDistance
            };

            projectile.velocity = Vector2.Zero;
            currentAngle += RotateSpeed * rotateDirection;
            NormalizeAngleDegrees(ref currentAngle);
        }
    }
}