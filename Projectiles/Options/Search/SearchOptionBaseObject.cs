using ChensGradiusMod.Projectiles.Options.Aim;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.Projectiles.Options.Search
{
  public abstract class SearchOptionBaseObject : OptionBaseObject
  {
    private const int FireRate = 2;
    private const float SeekDistance = 650f;
    private const float InterpolateValue = .1f;
    private const float PursueDistance = 100f;
    private const int ReseekCooldown = 30;
    private const float Percentage = .25f;
    private const float ReturnToFollowThreshold = 4f;
    private const float RotateAccel = .15f;
    private const float MaxRotateSpeed = 7f;

    private States mode = States.Follow;
    private int fireCounter = 0;
    private int target = -1;
    private int reseekTick = ReseekCooldown;
    private int rotateDirection = 1;
    private bool initialized = false;
    private float currentAngle = 0f;
    private float rotateSpeed = 0f;

    public enum States { Follow, Seek, Pursue, Return };

    public override bool PreAI()
    {
      if (!initialized)
      {
        if (Position % 2 == 0) rotateDirection = -rotateDirection;
        initialized = true;
      }
      return initialized && base.PreAI();
    }

    public override string Texture => "ChensGradiusMod/Sprites/SearchSheet";

    protected override void OptionMovement()
    {
      Vector2 dest;

      switch (mode)
      {
        case States.Follow:
          base.OptionMovement();
          IsAbleToSeek();
          break;

        case States.Seek:
          if (ModOwner.isSearching)
          {
            dest = ComputeTargetOffset(Target.Center, projectile.Center,
                                       PursueDistance * Percentage);
            projectile.Center = Vector2.Lerp(projectile.Center, dest, InterpolateValue);

            if (Vector2.Distance(projectile.Center, Owner.Center) > SeekDistance)
            {
              SetReturnVariables(false);
            }
            else if (IsInRange()) mode = States.Pursue;
          }
          else SetReturnVariables();
          break;

        case States.Pursue:
          if (ModOwner.isSearching)
          {
            if (Vector2.Distance(Owner.Center, Target.Center) > SeekDistance + PursueDistance)
            {
              SetReturnVariables(false);
            }
            else if (!Target.active || Target.life <= 0)
            {
              if (!IsAbleToSeek()) SetReturnVariables(false);
            }
            else
            {
              dest = Target.Center + currentAngle.ToRotationVector2();
              projectile.Center = ComputeTargetOffset(Target.Center, dest, PursueDistance);
              currentAngle += MathHelper.ToRadians(rotateSpeed);
              rotateSpeed += RotateAccel * rotateDirection;
              rotateSpeed = Math.Min(Math.Abs(rotateSpeed), MaxRotateSpeed) * rotateDirection;
            }
          }
          else SetReturnVariables();
          break;

        case States.Return:
          dest = ModOwner.optionFlightPath[Math.Min(PathListSize - 1, FrameDistance)];
          projectile.Center = Vector2.Lerp(projectile.Center, dest, InterpolateValue);

          if (!IsAbleToSeek() &&
              Vector2.Distance(dest, projectile.Center) <= ReturnToFollowThreshold)
          {
            mode = States.Follow;
          }
          break;
      }
    }

    protected override int SpawnDuplicateProjectile(Projectile p)
    {
      int result = -1;

      switch (mode)
      {
        case States.Follow:
          result = base.SpawnDuplicateProjectile(p);
          break;

        case States.Seek:
        case States.Return:
          result = -1;
          break;

        case States.Pursue:
          if (++fireCounter < FireRate) goto case States.Seek;
          else
          {
            fireCounter = 0;
            Vector2 pos = ComputeOffset(Main.player[p.owner].Center, p.Center);
            Vector2 vel = ComputeVelocityOffsetFromCursorAim(p, pos, Target.Center);
            result = Projectile.NewProjectile(pos, vel, p.type, p.damage,
                                              p.knockBack, projectile.owner, 0f, 0f);
          }
          break;
      }

      return result;
    }

    private NPC Target => Main.npc[target];

    private Vector2 ComputeTargetOffset(Vector2 origin, Vector2 destination, float offDistance)
    {
      currentAngle = GetBearing(origin, destination, false);
      currentAngle = MathHelper.ToRadians(currentAngle);

      return origin + currentAngle.ToRotationVector2() * offDistance;
    }

    private bool IsInRange() => Vector2.Distance(projectile.Center, Target.Center) <= PursueDistance;

    private bool Retarget()
    {
      if (target < 0 || !Target.active || Target.life <= 0)
      {
        target = FindTarget(projectile.Center, Owner.position, SeekDistance);
      }

      return target >= 0;
    }

    private bool IsAbleToSeek()
    {
      bool result = false;

      if (++reseekTick >= ReseekCooldown && ModOwner.isSearching && Retarget())
      {
        mode = States.Seek;
        AlwaysResetVariables();
        result = true;
      }

      reseekTick = Math.Min(reseekTick, ReseekCooldown);
      return result;
    }

    private void SetReturnVariables(bool enterCooldown = true)
    {
      target = -1;
      mode = States.Return;
      if (enterCooldown) reseekTick = 0;
      AlwaysResetVariables();
    }

    private void AlwaysResetVariables()
    {
      fireCounter = 0;
      rotateSpeed = 0;
    }
  }
}