using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.Projectiles.Forces
{
  public class ForceBase : ModProjectile
  {
    private const int KeepAlive = 5;

    private bool attached = false;
    private readonly float travelSpeed = 3f;

    private readonly float xDetachDistance = 250f;
    private Vector2 savedPosition = new Vector2();

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
      projectile.damage = 30;
      projectile.tileCollide = true;
    }

    public override bool PreAI()
    {
      if (ModOwner.forceBase)
      {
        projectile.timeLeft = KeepAlive;
        projectile.velocity = new Vector2();
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
      if (attached) { }
      else
      {
        savedPosition = projectile.Center;
        DetachedMovementY(DetachedMovementX());
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

    private bool DetachedMovementX()
    {
      float distance = Owner.Center.X + (xDetachDistance * Owner.direction) - projectile.Center.X;
      projectile.velocity.X = Math.Sign(distance) * Math.Min(travelSpeed, Math.Abs(distance));

      if (Owner.Center.X > savedPosition.X) projectile.direction = -1; 
      else if (Owner.Center.X < savedPosition.X) projectile.direction = 1;
      projectile.spriteDirection = projectile.direction;

      return Math.Abs(distance) <= travelSpeed;
    }

    private void DetachedMovementY(bool readyForVertical)
    {
      Vector2 resultVelocity = Collision.TileCollision(projectile.position, projectile.velocity, projectile.width, projectile.height);
      if (resultVelocity.X == 0 || readyForVertical)
      {
        float distance = Owner.Center.Y - projectile.Center.Y;
        projectile.velocity.Y = Math.Sign(distance) * Math.Min(travelSpeed, Math.Abs(distance));
      }
    }
  }
}
