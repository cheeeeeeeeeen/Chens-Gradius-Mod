using ChensGradiusMod.Items.Accessories.Options.Charge;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.Projectiles.Options.Charge
{
    public abstract class ChargeMultipleBaseObject : OptionBaseObject
    {
        private const int MaxCharge = 180;
        private const int MinCharge = 30;
        private const int ChargeDustId = 71;
        private const int MaxChargeDustId = 124;
        private const int ChargeSoundRate = 60;
        private const int DustRate = 3;

        private int chargeTime = 0;
        private int chargeSoundTick = 0;
        private int dustTick = 0;

        public override string Texture => "ChensGradiusMod/Sprites/ChargeSheet";

        public override bool PreAI()
        {
            bool baseResult = base.PreAI();

            if (baseResult && ModOwner.chargeMode == (int)ChargeMultipleBase.States.Dying)
            {
                if (chargeTime > MinCharge)
                {
                    if (IsSameClientOwner(projectile))
                    {
                        float direction = GetBearingUpwards(projectile.Center, Main.MouseWorld);
                        Vector2 vel = ChargeMultipleMissile.Spd * new Vector2
                        {
                            X = (float)Math.Cos(MathHelper.ToRadians(direction)),
                            Y = -(float)Math.Sin(MathHelper.ToRadians(direction))
                        };
                        int dmg = Owner.HeldItem.damage;

                        int pInd = Projectile.NewProjectile(projectile.Center, vel, ModContent.ProjectileType<ChargeMultipleMissile>(),
                                                            dmg, 0f, projectile.owner, direction, chargeTime);

                        if (Position % 2 != 0)
                        {
                            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Options/OptionMissile"),
                                           projectile.Center);
                        }

                        if (Main.projectile[pInd].modProjectile is ChargeMultipleMissile cMM)
                        {
                            Item accessory = Owner.armor[(int)FindEquippedAccessory(Owner, OptionAccessoryType)];
                            cMM.clonedAccessory = accessory.Clone();

                            accessory.TurnToAir();
                            accessory.accessory = false;
                        }
                    }

                    projectile.active = false;
                    return false;
                }
                else
                {
                    if (Position <= 1) ModOwner.chargeMode = (int)ChargeMultipleBase.States.Following;
                    chargeSoundTick = chargeTime = 0;
                    return true;
                }
            }

            return baseResult;
        }

        public override void AI()
        {
            base.AI();

            if (ModOwner.chargeMode == (int)ChargeMultipleBase.States.Charging)
            {
                chargeTime = Math.Min(MaxCharge, ++chargeTime);

                if (++dustTick >= DustRate && chargeTime <= MaxCharge)
                {
                    dustTick = 0;

                    Dust.NewDust(projectile.position, projectile.width, projectile.height, ChargeDustId);

                    if (chargeTime == MaxCharge)
                    {
                        Dust.NewDust(projectile.position, projectile.width, projectile.height, MaxChargeDustId);
                    }
                }

                if (Position <= 1)
                {
                    if (++chargeSoundTick >= ChargeSoundRate) chargeSoundTick = 0;
                    else if (chargeSoundTick <= 1)
                    {
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Options/OptionCharging"),
                                       projectile.Center);
                    }
                }
            }
        }

        protected override bool AttackLimitation()
        {
            bool result = ModOwner.chargeMode == (int)ChargeMultipleBase.States.Following;

            return base.AttackLimitation() && result;
        }

        protected virtual int OptionAccessoryType => 0;
    }
}