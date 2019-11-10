using ChensGradiusMod.Projectiles.Enemies;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.NPCs
{
  public class Sagna : GradiusEnemy
  {
    private readonly int cancelDeployThreshold = 500;
    private readonly float xSpeed = 3f;
    private readonly float yGravity = .1f;
    private readonly float maxYSpeed = 5f;
    private readonly int jumpCountForSpray = 1;
    private readonly int syncRate = 60;

    private bool initialized = false;
    private int persistDirection = 0;
    private int animateDirection = 1;
    private States mode = States.Fall;
    private States oldMode = States.Fall;
    private int xDirection = 0;
    private int yDirection = 0;
    private int jumpCount = 0;
    private int startFrame = 0;
    private int endFrame = 4;
    private int maxFrame = 8;
    private int syncTick = 0;

    public enum States { Hop, Fall, Spray };

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Sagna");
      Main.npcFrameCount[npc.type] = 16;
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      npc.width = 28;
      npc.height = 28;
      npc.damage = 100;
      npc.lifeMax = 150;
      npc.value = 2000f;
      npc.knockBackResist = 0f;
      npc.defense = 50;
      npc.noGravity = true;
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
      return GradiusHelper.NPCSpawnRate(Name, spawnInfo);
    }

    public override string Texture => "ChensGradiusMod/Sprites/Sagna";

    public override bool PreAI()
    {
      if (!initialized)
      {
        initialized = true;
        npc.TargetClosest(false);
        if (npc.Center.X > Main.player[npc.target].Center.X) persistDirection = -1;
        else persistDirection = 1;
        xDirection = persistDirection;
        yDirection = DecideYDeploy(npc.height, cancelDeployThreshold, false, true, 1);
        if (yDirection < 0)
        {
          startFrame = 8;
          endFrame = 12;
          maxFrame = Main.npcFrameCount[npc.type];
          FrameCounter = startFrame;
        }
      }

      return initialized;
    }

    public override void AI()
    {
      npc.spriteDirection = npc.direction = persistDirection;

      MoveHorizontally();
      MoveVertically();
      if (mode != States.Spray) AdjustMovementBehavior();

      
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

    public override void FindFrame(int frameHeight)
    {
      if (++FrameTick >= FrameSpeed)
      {
        FrameTick = 0;
        if (mode == States.Spray)
        {
          FrameCounter += animateDirection;
          if (animateDirection > 0 && FrameCounter >= maxFrame)
          {
            animateDirection = -1;
            FrameCounter -= 1;
            PerformAttack();
          }
          else if (animateDirection < 0 && FrameCounter < endFrame)
          {
            animateDirection = 1;
            mode = States.Fall;
          }
        }
        else
        {
          FrameCounter += animateDirection;
          if (FrameCounter >= endFrame) FrameCounter = startFrame;
        }
      }

      npc.frame.Y = frameHeight * FrameCounter;
    }

    public override void SendExtraAI(BinaryWriter writer)
    {
      writer.Write(animateDirection);
      writer.Write((byte)mode);
      writer.Write((byte)oldMode);
      writer.Write(xDirection);
      writer.Write(yDirection);
      writer.Write(jumpCount);
      writer.Write(startFrame);
      writer.Write(endFrame);
      writer.Write(maxFrame);
    }

    public override void ReceiveExtraAI(BinaryReader reader)
    {
      animateDirection = reader.ReadInt32();
      mode = (States)reader.ReadByte();
      oldMode = (States)reader.ReadByte();
      xDirection = reader.ReadInt32();
      yDirection = reader.ReadInt32();
      jumpCount = reader.ReadInt32();
      startFrame = reader.ReadInt32();
      endFrame = reader.ReadInt32();
      maxFrame = reader.ReadInt32();
    }

    protected override float RetaliationBulletSpeed => base.RetaliationBulletSpeed * 0.9f;

    protected override int RetaliationSpreadBulletNumber => 10;

    protected override float RetaliationSpreadAngleDifference => 180f;

    protected override int FrameSpeed { get; set; } = 6;

    private void MoveHorizontally()
    {
      switch (mode)
      {
        case States.Hop:
        case States.Fall:
          npc.velocity.X = xSpeed * xDirection;
          break;
        case States.Spray:
          npc.velocity.X = 0f;
          break;
      }
    }

    private void MoveVertically()
    {
      switch (mode)
      {
        case States.Hop:
          npc.velocity.Y += yGravity * yDirection;
          if (yDirection > 0) npc.velocity.Y = Math.Min(npc.velocity.Y, 0f);
          else npc.velocity.Y = Math.Max(npc.velocity.Y, 0f);
          break;
        case States.Fall:
          npc.velocity.Y += yGravity * yDirection;
          if (yDirection > 0) npc.velocity.Y = Math.Min(npc.velocity.Y, maxYSpeed);
          else npc.velocity.Y = Math.Max(npc.velocity.Y, -maxYSpeed);
          break;
        case States.Spray:
          npc.velocity.Y = 0f;
          break;
      }
    }

    private void AdjustMovementBehavior()
    {
      Vector2 beforeVelocity = npc.velocity;
      npc.velocity = Collision.TileCollision(npc.position, npc.velocity, npc.width, npc.height);

      if (beforeVelocity.X != npc.velocity.X) xDirection = -xDirection;

      switch (mode)
      {
        case States.Hop:
          if (yDirection > 0 && npc.velocity.Y >= 0f ||
              yDirection < 0 && npc.velocity.Y <= 0f)
          {
            npc.velocity.Y = 0f;
            if (++jumpCount >= jumpCountForSpray)
            {
              jumpCount = 0;
              mode = States.Spray;
            }
            else mode = States.Fall;
          }
          break;
        case States.Fall:
          if (beforeVelocity.Y != npc.velocity.Y)
          {
            mode = States.Hop;
            npc.velocity.Y = maxYSpeed * -yDirection;
          }
          break;
      }
    }

    private void PerformAttack()
    {
      if (GradiusHelper.IsNotMultiplayerClient())
      {
        float radianAngle = 0f;
        for (int i = 0; i < 8; i++)
        {
          Projectile.NewProjectile(npc.Center, radianAngle.ToRotationVector2() * GradiusEnemyBullet.Spd,
                                   ModContent.ProjectileType<GradiusEnemyBullet>(),
                                   GradiusEnemyBullet.Dmg, GradiusEnemyBullet.Kb, Main.myPlayer);
          radianAngle += MathHelper.PiOver4;
        }
        npc.netUpdate = true;
      }
    }
  }
}