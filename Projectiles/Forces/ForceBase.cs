using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.Projectiles.Forces
{
  public class ForceBase : ModProjectile
  {
    private const int KeepAlive = 5;
    private readonly float travelSpeed = 3f;
    private readonly float launchSpeed = 20f;
    private readonly float pullSpeed = 1.5f;
    private readonly float xDetachDistance = 250f;
    private readonly float xAttachDistance = 42f;
    private readonly int launchTickMax = 60;
    private int attachSide = 1;
    private int launchTick = 0;

    public int mode = 0;

    public enum States : int { Attached, Launched, Detached, Pulled };

    public override void SetStaticDefaults()
    {
      Main.projFrames[projectile.type] = 6;
      Main.projPet[projectile.type] = true;
    }

    public override void SetDefaults()
    {
      projectile.netImportant = true;
      projectile.width = 42;
      projectile.height = 44;
      projectile.light = .75f;
      projectile.friendly = true;
      projectile.hostile = false;
      projectile.tileCollide = false;
    }

    public override bool PreAI()
    {
      if (ModOwner.forceBase)
      {
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
      switch (mode)
      {
        case (int)States.Attached:
          AttachedMovement();
          break;

        case (int)States.Launched:
          LaunchedMovement();
          break;

        case (int)States.Detached:
          bool verticalReady = DetachedMovementX();
          DetachedMovementY(verticalReady);
          break;

        case (int)States.Pulled:
          PulledMovement();
          break;

        default:
          break;
      }

      if (++projectile.frameCounter >= 5)
      {
        projectile.frameCounter = 0;
        if (++projectile.frame >= Main.projFrames[projectile.type]) projectile.frame = 0;
      }
    }

    public override string Texture => "ChensGradiusMod/Sprites/ForceSheet";

    public override Color? GetAlpha(Color lightColor) => Color.White;

    private Player Owner => Main.player[projectile.owner];

    private GradiusModPlayer ModOwner => Owner.GetModPlayer<GradiusModPlayer>();

    private void AttachedMovement()
    {
      if (projectile.velocity.X != 0f || projectile.velocity.Y != 0f) projectile.velocity = new Vector2();
      projectile.Center = new Vector2(Owner.Center.X + (attachSide * xAttachDistance),
                                      Owner.Center.Y);
    }

    private bool DetachedMovementX()
    {
      float distance = Owner.Center.X + (xDetachDistance * Owner.direction) - projectile.Center.X;
      projectile.velocity.X = Math.Sign(distance) * Math.Min(travelSpeed, Math.Abs(distance));

      if (Owner.Center.X > projectile.Center.X) projectile.direction = -1; 
      else if (Owner.Center.X < projectile.Center.X) projectile.direction = 1;
      projectile.spriteDirection = projectile.direction;

      return Math.Abs(distance) <= travelSpeed;
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
      if (projectile.tileCollide)
      {
        projectile.tileCollide = false;
        projectile.velocity = new Vector2();
      }
      projectile.Center += GradiusHelper.MoveToward(projectile.Center, Owner.Center, pullSpeed);
    }
  }
}
