using ChensGradiusMod.Items.Accessories.Forces;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.Projectiles.Forces
{
  public class ForceBase : ModProjectile
  {
    private const int KeepAlive = 5;
    private const float AcceptedVerticalThreshold = .24f;

    private readonly float travelSpeed = 3f;
    private readonly float launchSpeed = 20f;
    private readonly float pullSpeed = 2f;
    private readonly float xDetachDistance = 300f;
    private readonly float xAttachDistance = 42f;
    private readonly int[] attackCooldowns = { 5, 5, 25 };
    private readonly int launchTickMax = 60;
    private readonly int inBattleExpire = 385;

    private int launchTick = 0;
    private int attackTick = 0;
    private int attackIndex = 0;
    private bool inBattle = false;
    private int inBattleTick = 0;

    public int mode = (int)States.Detached;
    public int oldMode = (int)States.Detached;
    public int attachSide = 1;

    public const int Dmg = 1;
    public const float Kb = 0.01f;

    public enum States : int { Attached, Launched, Detached, Pulled };

    public override void SetStaticDefaults()
    {
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
      if (ModOwner.forceBase)
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
        projectile.Kill();
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

      if(Main.myPlayer == projectile.owner) Engage();
      OverpowerProjectiles();
    }

    public override void SendExtraAI(BinaryWriter writer)
    {
      writer.Write(mode);
      switch (mode)
      {
        case (int)States.Attached:
          writer.Write(attachSide);
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
          attachSide = reader.ReadInt32();
          projectile.spriteDirection = reader.ReadInt32();
          projectile.direction = reader.ReadInt32();
          break;
      }
    }

    public override bool MinionContactDamage() => true;

    public override string Texture => "ChensGradiusMod/Sprites/ForceSheet";

    public override Color? GetAlpha(Color lightColor) => Color.White;

    public void BattleMode()
    {
      if (!inBattle)
      {
        PerformAttack();
        attackTick = 0;
        attackIndex = 0;
      }
      inBattleTick = 0;
      inBattle = true;
    }

    public void PerformAttack()
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
                                   mod.ProjectileType<ForceLightBullet>(),
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
                                   mod.ProjectileType<ForceLightBullet>(),
                                   projectile.damage, projectile.knockBack, Owner.whoAmI);
        }
      }

      Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Forces/LightBulletShoot"),
                         projectile.Center);
    }

    private Player Owner => Main.player[projectile.owner];

    private GradiusModPlayer ModOwner => Owner.GetModPlayer<GradiusModPlayer>();

    private int UpdateDirection(ref int variable, int fallbackNumber)
    {
      if (Owner.Center.X > projectile.Center.X) variable = -1;
      else if (Owner.Center.X < projectile.Center.X) variable = 1;
      else variable = fallbackNumber;

      return variable;
    }

    private void AttachedMovement()
    {
      projectile.Center = new Vector2(Owner.Center.X + (attachSide * xAttachDistance),
                                      Owner.Center.Y);
    }

    private bool DetachedMovementX()
    {
      float distance = Owner.Center.X + (xDetachDistance * Owner.direction) - projectile.Center.X;
      projectile.velocity.X = Math.Sign(distance) * Math.Min(travelSpeed, Math.Abs(distance));

      projectile.spriteDirection = UpdateDirection(ref projectile.direction, projectile.direction);

      return Math.Abs(distance) <= Math.Min(AcceptedVerticalThreshold, travelSpeed);
    }

    private void DetachedMovementY(bool readyForVertical)
    {
      if (readyForVertical || Collision.TileCollision(projectile.position, projectile.velocity, projectile.width, projectile.height).X == 0)
      {
        float distance = Owner.Center.Y - projectile.Center.Y;
        projectile.velocity.Y = Math.Sign(distance) * Math.Min(travelSpeed, Math.Abs(distance));
      }
      else projectile.velocity.Y = 0f;
    }

    private void LaunchedMovement()
    {
      if (launchTick++ > launchTickMax || (launchTick > 1 && projectile.velocity.X == 0f))
      {
        launchTick = 0;
        mode = (int)States.Detached;
        DetachedMovementY(DetachedMovementX());
      }
      else if (launchTick <= 1)
      {
        projectile.tileCollide = true;
        projectile.velocity.X = attachSide * launchSpeed;
        projectile.velocity.Y = 0f;
      }
    }

    private void PulledMovement()
    {
      projectile.Center += GradiusHelper.MoveToward(projectile.Center, Owner.Center, pullSpeed);

      projectile.spriteDirection = UpdateDirection(ref projectile.direction, projectile.direction);
    }

    private bool Reattach()
    {
      if (projectile.Hitbox.Intersects(Owner.Hitbox))
      {
        projectile.tileCollide = false;
        projectile.velocity = new Vector2();

        projectile.spriteDirection = UpdateDirection(ref attachSide, projectile.spriteDirection);
        projectile.direction = projectile.spriteDirection;

        mode = (int)States.Attached;
        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Forces/ForceGet"),
                        projectile.Center);

        return true;
      }

      return false;
    }

    private void Engage()
    {
      if (inBattle)
      {
        if (++inBattleTick < inBattleExpire)
        {
          if (++attackTick >= attackCooldowns[attackIndex])
          {
            PerformAttack();
            attackTick = 0;
            if (++attackIndex >= attackCooldowns.Length) attackIndex = 0;
          }
        }
        else inBattle = false;
      }
    }

    private void OverpowerProjectiles()
    {
      for (int i = 0; i < Main.maxProjectiles; i++)
      {
        Projectile selectProj = Main.projectile[i];
        if (selectProj.active && selectProj.hostile && GradiusHelper.CanDamage(selectProj) &&
            projectile.Hitbox.Intersects(selectProj.Hitbox))
        {
          selectProj.Kill();
        }
      }
    }

    private void UpdateDamage()
    {
      Item basis = null;
      if (!Main.mouseItem.IsAir) basis = Main.mouseItem;
      else if (!Owner.HeldItem.IsAir) basis = Owner.HeldItem;

      if (basis == null || !GradiusHelper.CanDamage(basis) || (basis.modItem is BydoEmbryo))
      {
        projectile.damage = Dmg;
        projectile.knockBack = Kb;
      }
      else
      {
        projectile.damage = basis.damage;
        projectile.knockBack = basis.knockBack;
      }
    }
  }
}
