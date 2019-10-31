using ChensGradiusMod.Items.Accessories.Options.Charge;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.Projectiles.Options.Charge
{
  public abstract class ChargeMultipleBaseObject : OptionBaseObject
  {
    private readonly int maxCharge = 180;
    private readonly int minCharge = 30;
    private readonly int chargeDustId = 71;
    private readonly int maxChargeDustId = 124;
    private readonly int chargeSoundRate = 60;
    private readonly int dustRate = 3;
    private int chargeTime = 0;
    private int chargeSoundTick = 0;
    private int dustTick = 0;

    public override string Texture => "ChensGradiusMod/Sprites/ChargeSheet";

    public override bool PreAI()
    {
      bool baseResult = base.PreAI();

      if (baseResult && ModOwner.chargeMode == (int)ChargeMultipleBase.States.Dying)
      {
        if (chargeTime > minCharge)
        {
          if (GradiusHelper.IsSameClientOwner(projectile))
          {
            float direction = GradiusHelper.GetBearing(projectile.Center, Main.MouseWorld);
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
              Item accessory = Owner.armor[(int)GradiusHelper.FindEquippedAccessory(Owner, OptionAccessoryType)];
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
        chargeTime = Math.Min(maxCharge, ++chargeTime);

        if (++dustTick >= dustRate && chargeTime <= maxCharge)
        {
          dustTick = 0;

          Dust.NewDust(projectile.position, projectile.width, projectile.height, chargeDustId);

          if (chargeTime == maxCharge)
          {
            Dust.NewDust(projectile.position, projectile.width, projectile.height, maxChargeDustId);
          }
        }

        if (Position <= 1)
        {
          if (++chargeSoundTick >= chargeSoundRate) chargeSoundTick = 0;
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
      if (ModOwner.chargeMode == (int)ChargeMultipleBase.States.Following)
      {
        return true;
      }

      return false;
    }

    protected virtual int OptionAccessoryType => 0;
  }
}
