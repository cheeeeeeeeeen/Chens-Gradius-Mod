using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.Projectiles.Forces
{
  public class ForceBase : ModProjectile
  {
    public const int Dmg = 1;
    public const float Kb = 0.01f;

    protected const int KeepAlive = 5;
    protected const float AcceptedVerticalThreshold = .24f;

    public enum States : int { Attached, Launched, Detached, Pulled };

    public int mode = (int)States.Detached;
    public int oldMode = (int)States.Detached;

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Standard Force");
      Main.projFrames[projectile.type] = 6;
      Main.projPet[projectile.type] = true;
    }

    public override void SetDefaults()
    {
      projectile.netImportant = true;
      projectile.width = 36;
      projectile.height = 36;
      projectile.light = .75f;
      projectile.friendly = true;
      projectile.hostile = false;
      projectile.tileCollide = true;
      projectile.penetrate = -1;
      projectile.usesLocalNPCImmunity = true;
      projectile.localNPCHitCooldown = 3;
    }

    public override bool PreAI()
    {
      if (ForceCheck())
      {
        if (oldMode != mode)
        {
          oldMode = mode;
          projectile.netUpdate = true;
        }

        projectile.timeLeft = KeepAlive;
        return true;
      }
      else
      {
        ModOwner.forceProjectile = null;
        projectile.active = false;
        return false;
      }
    }

    public override void AI()
    {
      UpdateDamage();

      switch (mode)
      {
        case (int)States.Attached:
          AttachedMovement();
          break;

        case (int)States.Launched:
          LaunchedMovement();
          break;

        case (int)States.Detached:
          if (!Reattach()) DetachedMovementY(DetachedMovementX());
          break;

        case (int)States.Pulled:
          if (!Reattach()) PulledMovement();
          break;
      }

      if (++projectile.frameCounter >= 5)
      {
        projectile.frameCounter = 0;
        if (++projectile.frame >= Main.projFrames[projectile.type]) projectile.frame = 0;
      }

      if (Main.myPlayer == projectile.owner) Engage();
      OverpowerProjectiles();
    }

    public override void SendExtraAI(BinaryWriter writer)
    {
      writer.Write(mode);
      switch (mode)
      {
        case (int)States.Attached:
          writer.Write(AttachSide);
          writer.Write(projectile.spriteDirection);
          writer.Write(projectile.direction);
          break;
      }
    }

    public override void ReceiveExtraAI(BinaryReader reader)
    {
      mode = reader.ReadInt32();
      switch (mode)
      {
        case (int)States.Attached:
          AttachSide = reader.ReadInt32();
          projectile.spriteDirection = reader.ReadInt32();
          projectile.direction = reader.ReadInt32();
          break;
      }
    }

    public override bool MinionContactDamage() => true;

    public override string Texture => "ChensGradiusMod/Sprites/ForceSheet";

    public override Color? GetAlpha(Color lightColor) => Color.White;

    public virtual void PerformAttack()
    {
      float vX = 0f, vY = 0f;

      if (mode != (int)States.Attached)
      {
        for (int i = 0; i < 4; i++)
        {
          switch (i)
          {
            case 0:
              vX = 0f;
              vY = 1f;
              break;
            case 1:
              vX = 0f;
              vY = -1f;
              break;
            case 2:
              vX = (float)Math.Cos(MathHelper.ToRadians(15f)) * projectile.spriteDirection;
              vY = (float)-Math.Sin(MathHelper.ToRadians(15f));
              break;
            case 3:
              vX = (float)Math.Cos(MathHelper.ToRadians(15f)) * projectile.spriteDirection;
              vY = (float)Math.Sin(MathHelper.ToRadians(15f));
              break;
          }

          Projectile.NewProjectile(projectile.Center, new Vector2(vX, vY) * ForceLightBullet.Spd,
                                   ModContent.ProjectileType<ForceLightBullet>(),
                                   projectile.damage, projectile.knockBack, Owner.whoAmI);
        }
      }
      else
      {
        for (int j = 0; j < 2; j++)
        {
          switch (j)
          {
            case 0:
              vY = 5f;
              break;
            case 1:
              vY = -5f;
              break;
          }

          Projectile.NewProjectile(projectile.Center + new Vector2(vX, vY), new Vector2(1f, 0f) * ForceLightBullet.Spd * projectile.spriteDirection,
                                   ModContent.ProjectileType<ForceLightBullet>(),
                                   projectile.damage, projectile.knockBack, Owner.whoAmI);
        }
      }

      Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Forces/LightBulletShoot"),
                         projectile.Center);
    }

    public virtual void SpecialDetachActions() { }

    public void BattleMode()
    {
      if (!InBattle)
      {
        PerformAttack();
        AttackTick = 0;
        AttackIndex = 0;
      }
      InBattleTick = 0;
      InBattle = true;
    }

    protected virtual bool ForceCheck() => ModOwner.forceBase;

    protected virtual float BasisMultiplier { get; } = 2f;

    protected virtual float TravelSpeed { get; } = 3f;

    protected virtual float LaunchSpeed { get; } = 20f;

    protected virtual float PullSpeed { get; } = 2f;

    protected virtual float XDetachDistance { get; } = 300f;

    protected virtual float XAttachDistance { get; } = 42f;

    protected virtual int[] AttackCooldowns { get; } = { 5, 5, 5, 5, 25 };

    protected virtual int LaunchTickMax { get; } = 60;

    protected virtual int InBattleExpire { get; } = 385;

    protected virtual int LaunchTick { get; set; } = 0;

    protected virtual int AttackTick { get; set; } = 0;

    protected virtual int AttackIndex { get; set; } = 0;

    protected virtual bool InBattle { get; set; } = false;

    protected virtual int InBattleTick { get; set; } = 0;

    protected virtual int AttachSide { get; set; } = 1;

    protected virtual void Engage()
    {
      if (InBattle)
      {
        if (++InBattleTick < InBattleExpire)
        {
          if (++AttackTick >= AttackCooldowns[AttackIndex])
          {
            PerformAttack();
            AttackTick = 0;
            if (++AttackIndex >= AttackCooldowns.Length) AttackIndex = 0;
          }
        }
        else InBattle = false;
      }
    }

    protected virtual void UpdateDamage()
    {
      Item basis = null;
      if (!Main.mouseItem.IsAir) basis = Main.mouseItem;
      else if (!Owner.HeldItem.IsAir) basis = Owner.HeldItem;

      if (basis == null || !GradiusHelper.CanDamage(basis) || GradiusHelper.IsBydoAccessory(basis.modItem))
      {
        projectile.damage = Dmg;
        projectile.knockBack = Kb;
      }
      else
      {
        projectile.damage = GradiusHelper.RoundOffToWhole(basis.damage * BasisMultiplier);
        projectile.knockBack = basis.knockBack * BasisMultiplier;
      }
    }

    protected virtual bool Reattach()
    {
      if (projectile.Hitbox.Intersects(Owner.Hitbox))
      {
        projectile.tileCollide = false;
        projectile.velocity = new Vector2();

        projectile.spriteDirection = UpdateDirection(projectile.spriteDirection);
        projectile.direction = AttachSide = projectile.spriteDirection;

        mode = (int)States.Attached;
        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Forces/ForceGet"),
                       projectile.Center);

        return true;
      }

      return false;
    }

    protected int UpdateDirection(int fallbackNumber)
    {
      if (Owner.Center.X > projectile.Center.X) return -1;
      else if (Owner.Center.X < projectile.Center.X) return 1;
      else return fallbackNumber;
    }

    protected void AttachedMovement()
    {
      projectile.Center = new Vector2(Owner.Center.X + (AttachSide * XAttachDistance),
                                      Owner.Center.Y);
    }

    protected bool DetachedMovementX()
    {
      float distance = Owner.Center.X + (XDetachDistance * Owner.direction) - projectile.Center.X;
      projectile.velocity.X = Math.Sign(distance) * Math.Min(TravelSpeed, Math.Abs(distance));
      projectile.spriteDirection = projectile.direction = UpdateDirection(projectile.direction);

      return Math.Abs(distance) <= Math.Min(AcceptedVerticalThreshold, TravelSpeed);
    }

    protected void DetachedMovementY(bool readyForVertical)
    {
      if (readyForVertical || Collision.TileCollision(projectile.position, projectile.velocity, projectile.width, projectile.height).X == 0)
      {
        float distance = Owner.Center.Y - projectile.Center.Y;
        projectile.velocity.Y = Math.Sign(distance) * Math.Min(TravelSpeed, Math.Abs(distance));
      }
      else projectile.velocity.Y = 0f;
    }

    protected void LaunchedMovement()
    {
      if (LaunchTick++ > LaunchTickMax || (LaunchTick > 1 && projectile.velocity.X == 0f))
      {
        LaunchTick = 0;
        mode = (int)States.Detached;
        DetachedMovementY(DetachedMovementX());
      }
      else if (LaunchTick <= 1)
      {
        projectile.tileCollide = true;
        projectile.velocity.X = AttachSide * LaunchSpeed;
        projectile.velocity.Y = 0f;
      }
    }

    protected void PulledMovement()
    {
      projectile.Center += GradiusHelper.MoveToward(projectile.Center, Owner.Center, PullSpeed);
      projectile.spriteDirection = projectile.direction = UpdateDirection(projectile.direction);
    }

    protected void OverpowerProjectiles()
    {
      for (int i = 0; i < Main.maxProjectiles; i++)
      {
        Projectile selectProj = Main.projectile[i];
        if (selectProj.active && selectProj.hostile && GradiusHelper.CanDamage(selectProj) &&
            projectile.Hitbox.Intersects(selectProj.Hitbox))
        {
          GradiusHelper.ProjectileDestroy(selectProj);
        }
      }
    }

    protected Player Owner => Main.player[projectile.owner];

    protected GradiusModPlayer ModOwner => Owner.GetModPlayer<GradiusModPlayer>();
  }
}
