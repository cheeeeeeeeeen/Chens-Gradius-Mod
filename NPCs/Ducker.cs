using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.NPCs
{
  public class Ducker : GradiusEnemy
  {
    private const float CustomGravity = 5f;
    private const int CancelThreshold = 500;

    private readonly float runSpeed = 4f;
    private readonly float fallSpeedYAccel = .5f;
    private readonly float fallSpeedXAccel = .1f;

    private States mode = States.Run;
    private bool initialized = false;
    private int persistDirection = 0;
    private int yDirection = 0;
    private bool hasJumped = false;

    public enum States { Run, Target, Jump, Fall, Land, Retreat };

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Ducker");
      Main.npcFrameCount[npc.type] = 14;
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      npc.width = 36;
      npc.height = 30;
      npc.damage = 100;
      npc.lifeMax = 170;
      npc.value = 2000f;
      npc.knockBackResist = 0f;
      npc.defense = 50;
      npc.noGravity = true;
    }

    public override void FindFrame(int frameHeight)
    {
      switch (mode)
      {
        case States.Run:
          if (++FrameTick >= FrameSpeed)
          {
            FrameTick = 0;
            if (++FrameCounter > 5) FrameCounter = 0;
          }
          break;
        case States.Fall:
          if (FrameCounter < 13)
          {
            if (++FrameTick >= FrameSpeed)
            {
              FrameTick = 0;
              FrameCounter++;
            }
          }
          break;
        case States.Land:
          if (++FrameTick >= FrameSpeed)
          {
            FrameTick = 0;
            FrameCounter--;
            if (FrameCounter <= 9 && FrameCounter > 5)
            {
              FrameCounter = 6;
              mode = States.Run;
            }
          }
          break;
        case States.Jump:
          if (FrameCounter < 13)
          {
            if (++FrameTick >= FrameSpeed)
            {
              FrameTick = 0;
              FrameCounter++;
              if (FrameCounter <= 7) FrameCounter = 10;
            }
          }
          break;
        case States.Target:
          break;
      }

      npc.frame.Y = FrameCounter * frameHeight;
    }

    public override bool PreAI()
    {
      if (!initialized)
      {
        initialized = true;
        npc.TargetClosest(false);
        if (persistDirection == 0)
        {
          if (npc.Center.X > Main.player[npc.target].Center.X) persistDirection = -1;
          else persistDirection = 1;
        }
        yDirection = DecideYDeploy(npc.height * .5f, CancelThreshold, false, true, 1);
        npc.oldPosition = npc.position;
      }
      return initialized;
    }

    public override void AI()
    {
      Vector2 oldP, oldV;

      npc.spriteDirection = npc.direction = persistDirection;

      switch (mode)
      {
        case States.Run:
          UsualMovement();

          oldP = npc.position;
          oldV = npc.velocity;
          Collision.StepUp(ref npc.position, ref npc.velocity, npc.width, npc.height,
                           ref npc.stepSpeed, ref npc.gfxOffY, yDirection);
          if ((oldP == npc.position || oldV == npc.velocity) && WillHitWall())
          {
            mode = States.Jump;
            FrameCounter = 6;
            FrameTick = 0;
            npc.velocity = Vector2.Zero;
          }
          break;

        case States.Fall:
          npc.velocity += new Vector2
          {
            X = fallSpeedXAccel * persistDirection,
            Y = fallSpeedYAccel * yDirection
          };
          if (persistDirection > 0) npc.velocity.X = Math.Min(npc.velocity.X, runSpeed);
          else npc.velocity.X = Math.Max(npc.velocity.X, -runSpeed);
          if (yDirection > 0) npc.velocity.Y = Math.Min(npc.velocity.Y, CustomGravity);
          else npc.velocity.Y = Math.Max(npc.velocity.Y, -CustomGravity);

          if (IsSteppingOnTiles(update: true) || npc.oldPosition == npc.position)
          {
            mode = States.Land;
            UsualMovement();
            oldP = npc.position;
            oldV = npc.velocity;
            if ((oldP == npc.position || oldV == npc.velocity) && WillHitWall())
            {
              persistDirection = -persistDirection;
            }
            npc.velocity = Vector2.Zero;
          }
          break;

        case States.Jump:
          if (FrameCounter >= 13)
          {
            if (!hasJumped && GradiusHelper.IsEqualWithThreshold(npc.velocity, Vector2.Zero, .01f))
            {
              npc.velocity = new Vector2(0, 20f * -yDirection);
              hasJumped = true;
            }
            else npc.velocity += new Vector2(fallSpeedXAccel * persistDirection, fallSpeedYAccel * yDirection);

            if (hasJumped && ((yDirection > 0 && npc.velocity.Y >= 0) ||
                              (yDirection < 0 && npc.velocity.Y <= 0)))
            {
              mode = States.Fall;
              FrameTick = 0;
              hasJumped = false;
            }
          }
          break;
      }
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
      return GradiusHelper.NPCSpawnRate("Sagna", spawnInfo);
    }

    public override string Texture => "ChensGradiusMod/Sprites/Ducker";

    protected override int FrameSpeed { get; set; } = 5;

    protected override int FrameCounter { get; set; } = 6;

    protected override Types EnemyType => Types.Small;

    protected override float RetaliationBulletSpeed => base.RetaliationBulletSpeed * 2f;

    protected override int RetaliationSpreadBulletNumber => 1;

    protected override float RetaliationSpreadAngleDifference => 0f;

    private bool IsSteppingOnTiles(bool update = false)
    {
      Vector2 oldV = npc.velocity;
      Vector2 newV = Collision.TileCollision(npc.position, npc.velocity, npc.width, npc.height,
                                             false, false, yDirection);
      if (update) npc.velocity = newV;
      if (yDirection > 0) return newV.Y < oldV.Y;
      else return newV.Y > oldV.Y;
    }

    private bool WillHitWall()
    {
      Vector2 oldV = npc.velocity;
      Vector2 newV = Collision.TileCollision(npc.position, npc.velocity, npc.width, npc.height,
                                             false, false, yDirection);
      if (persistDirection > 0) return newV.X < oldV.X;
      else return newV.X > oldV.X;
    }

    private void UsualMovement()
    {
      npc.velocity = new Vector2
      {
        X = runSpeed * persistDirection,
        Y = CustomGravity * yDirection
      };
    }
  }
}
