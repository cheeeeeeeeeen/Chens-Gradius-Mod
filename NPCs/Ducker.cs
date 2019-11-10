using ChensGradiusMod.Projectiles.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.NPCs
{
  public class Ducker : GradiusEnemy
  {
    private const float CustomGravity = 8f;
    private const int CancelThreshold = 500;
    private const float AttackAngleDifference = 13f;

    private readonly float runSpeed = 4f;
    private readonly float fallSpeedYAccel = .5f;
    private readonly float fallSpeedXAccel = .1f;
    private readonly float targetDistance = 500f;
    private readonly int syncRate = 30;

    private States mode = States.Run;
    private States oldMode = States.Run;
    private bool initialized = false;
    private int persistDirection = 0;
    private int yDirection = 0;
    private bool hasJumped = false;
    private Vector2 targetLastSeen = Vector2.Zero;
    private int syncTick = 0;

    public enum States { Run, Target, Fire, Recompose, Jump, Fall, Land };

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Ducker");
      Main.npcFrameCount[npc.type] = 14;
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      npc.width = 36;
      npc.height = 26;
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
        //case States.Fall:
        //  if (++FrameTick >= FrameSpeed)
        //  {
        //    FrameTick = 0;
        //    FrameCounter--;
        //    if (FrameCounter <= 9 && FrameCounter > 6) FrameCounter = 10;
        //  }
        //  break;
        case States.Land:
          if (++FrameTick >= FrameSpeed)
          {
            FrameTick = 0;
            FrameCounter--;
            if (FrameCounter <= 9 && FrameCounter > 5) SwitchRunMode();
          }
          break;
        case States.Jump:
          if (FrameCounter < 13)
          {
            if (++FrameTick >= FrameSpeed)
            {
              FrameTick = 0;
              if (++FrameCounter <= 7) FrameCounter = 10;
            }
          }
          break;
        case States.Target:
          if (++FrameTick >= FrameSpeed)
          {
            FrameTick = 0;
            if (TurretManagement()) mode = States.Fire;
          }
          break;
        case States.Fire:
          if (++FrameTick >= FrameSpeed)
          {
            FrameTick = 0;
            PerformAttack();
            mode = States.Recompose;
          }
          break;
        case States.Recompose:
          if (++FrameTick >= FrameSpeed)
          {
            FrameTick = 0;
            if (--FrameCounter <= 5) SwitchRunMode();
          }
          break;
      }

      npc.frame.Y = FrameCounter * frameHeight;
    }

    public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
    {
      Vector2 drawPos, origin = Vector2.Zero;
      SpriteEffects spriteEffects = SpriteEffects.None;

      if (yDirection > 0)
      {
        drawPos = npc.TopLeft - Main.screenPosition;
        drawPos.Y -= npc.height;
      }
      else
      {
        spriteEffects |= SpriteEffects.FlipVertically;
        drawPos = npc.TopLeft - Main.screenPosition;
        drawPos.Y -= 4;
      }

      if (persistDirection > 0)
      {
        spriteEffects |= SpriteEffects.FlipHorizontally;
        drawPos.X -= 6;
      }
      else drawPos.X -= npc.width - 16;

      spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, npc.frame,
                       drawColor, 0f, origin, 1f, spriteEffects, 0f);
      return false;
    }

    public override bool PreAI()
    {
      if (!initialized)
      {
        initialized = true;
        npc.TargetClosest(false);
        if (persistDirection == 0) FaceTarget(Target.Center);
        yDirection = DecideYDeploy(npc.height * .5f, CancelThreshold, false, true, 1);
        npc.oldPosition = npc.position;
      }
      return initialized;
    }

    public override void AI()
    {
      Vector2 oldP, oldV;

      switch (mode)
      {
        case States.Run:
          if (ConfirmTarget())
          {
            mode = States.Target;
            HaltMovement();
            FrameCounter = 6;
            FrameTick = 0;
            FaceTarget(Target.Center);
          }
          else
          {
            if (GradiusHelper.VanillaStepUpTileComputationCatcher(npc.position, npc.velocity, npc.width))
            {
              npc.life = 0;
              npc.active = false;
              return;
            }

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
              HaltMovement();
            }
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

          if (IsSteppingOnTiles())
          {
            mode = States.Land;
            UsualMovement();
            oldP = npc.position;
            oldV = npc.velocity;
            if ((oldP == npc.position || oldV == npc.velocity) && WillHitWall())
            {
              persistDirection = -persistDirection;
            }
            npc.velocity.X = 0f;
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
        case States.Fire:
        case States.Land:
        case States.Recompose:
        case States.Target:
          HaltMovement();
          break;
      }

      npc.spriteDirection = npc.direction = persistDirection;
    }

    public override void PostAI()
    {
      base.PostAI();
      if (!ConstantSync(ref syncTick, syncRate) && GradiusHelper.IsNotMultiplayerClient()
          && oldMode != mode)
      {
        npc.netUpdate = true;
        oldMode = mode;
      }
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
      return GradiusHelper.NPCSpawnRate("Sagna", spawnInfo);
    }

    public override string Texture => "ChensGradiusMod/Sprites/Ducker";

    public override void SendExtraAI(BinaryWriter writer)
    {
      writer.Write((byte)mode);
      writer.Write((byte)oldMode);
      writer.Write(persistDirection);
      writer.Write(yDirection);
      writer.Write(hasJumped);
      writer.WriteVector2(targetLastSeen);
    }

    public override void ReceiveExtraAI(BinaryReader reader)
    {
      mode = (States)reader.ReadByte();
      oldMode = (States)reader.ReadByte();
      persistDirection = reader.ReadInt32();
      yDirection = reader.ReadInt32();
      hasJumped = reader.ReadBoolean();
      targetLastSeen = reader.ReadVector2();
  }

    protected override int FrameSpeed { get; set; } = 5;

    protected override int FrameCounter { get; set; } = 6;

    protected override Types EnemyType => Types.Small;

    protected override float RetaliationBulletSpeed => base.RetaliationBulletSpeed * 2f;

    protected override int RetaliationSpreadBulletNumber => 1;

    protected override float RetaliationSpreadAngleDifference => 0f;

    private Player Target => Main.player[npc.target];

    private bool IsSteppingOnTiles()
    {
      Vector2 newV = Collision.TileCollision(npc.position, npc.velocity, npc.width,
                                             npc.height, false, false, yDirection);
      npc.velocity = newV;
      return Collision.SolidCollision(npc.position + newV + new Vector2(0, 2 * yDirection),
                                      npc.width, npc.height);
    }

    private bool WillHitWall()
    {

      return Collision.SolidCollision(npc.position + new Vector2(npc.velocity.X, 0),
                                      npc.width, npc.height);
    }

    private void UsualMovement()
    {
      npc.velocity = new Vector2
      {
        X = runSpeed * persistDirection,
        Y = CustomGravity * yDirection
      };
    }

    private bool ConfirmTarget()
    {
      npc.TargetClosest(false);
      targetLastSeen = Target.Center;
      return Vector2.Distance(Target.Center, npc.Center) <= targetDistance
             && ((yDirection > 0 && npc.Center.Y >= Target.Center.Y)
                 || (yDirection < 0 && npc.Center.Y <= Target.Center.Y));
    }

    private void HaltMovement()
    {
      npc.velocity = Vector2.Zero;
      npc.velocity.Y = CustomGravity * yDirection;
      npc.velocity = Collision.TileCollision(npc.position, npc.velocity, npc.width, npc.height);
    }

    private void FaceTarget(Vector2 targetPosition)
    {
      if (npc.Center.X > targetPosition.X) persistDirection = -1;
      else persistDirection = 1;
    }

    private bool TurretManagement()
    {
      float angle = GradiusHelper.GetAngleRelativeXDirection(npc.Center, targetLastSeen);
      switch (FrameCounter)
      {
        case 6 when angle < 15:
        case 7 when angle >= 15 && angle < 40:
        case 8 when angle >= 40 && angle < 65:
        case 9 when angle >= 65:
          return true;
      }

      if (++FrameCounter >= 10) SwitchRunMode();
      return false;
    }

    private void PerformAttack()
    {
      if (GradiusHelper.IsNotMultiplayerClient())
      {
        float direction = (targetLastSeen - npc.Center).ToRotation();
        direction = MathHelper.ToDegrees(direction);

        float[] angles = new float[2]
        {
          MathHelper.ToRadians(direction + AttackAngleDifference),
          MathHelper.ToRadians(direction - AttackAngleDifference)
        };

        for (int i = 0; i < angles.Length; i++)
        {
          Projectile.NewProjectile(npc.Center, angles[i].ToRotationVector2() * GradiusEnemyBullet.Spd,
                                   ModContent.ProjectileType<GradiusEnemyBullet>(),
                                   GradiusEnemyBullet.Dmg, GradiusEnemyBullet.Kb, Main.myPlayer);
        }
        npc.netUpdate = true;
      }
    }

    private void SwitchRunMode()
    {
      FrameCounter = 6;
      mode = States.Run;
    }
  }
}
