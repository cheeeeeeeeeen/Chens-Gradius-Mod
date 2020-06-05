using ChensGradiusMod.Items.Banners;
using ChensGradiusMod.Projectiles.Enemies;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.NPCs
{
  public class Sagna : GradiusEnemy
  {
    private const float XSpeed = 3f;
    private const float YGravity = .1f;
    private const float MaxYSpeed = 5f;
    private const int JumpCountForSpray = 1;
    private const int SyncRate = 60;
    private const int UnstuckTime = 30;

    private bool initialized = false;
    private sbyte persistDirection = 0;
    private sbyte animateDirection = 1;
    private States mode = States.Fall;
    private States oldMode = States.Fall;
    private sbyte xDirection = 0;
    private sbyte yDirection = 0;
    private byte jumpCount = 0;
    private byte startFrame = 0;
    private byte endFrame = 4;
    private byte maxFrame = 8;
    private int syncTick = 0;
    private int unstuckTick = 0;

    public enum States { Hop, Fall, Spray };

    public static float SpawnCondition(NPCSpawnInfo spawnInfo)
    {
      if (UsualSpawnConditions(spawnInfo) && spawnInfo.spawnTileY < UnderworldTilesYLocation
          && spawnInfo.spawnTileY > (Main.worldSurface - Main.worldSurface * .1f))
      {
        return ActualSpawnRate(.075f);
      }
      else return 0f;
    }

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
      bannerItem = ModContent.ItemType<SagnaBanner>();

      ScaleStats();
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
      return SpawnCondition(spawnInfo);
    }

    public override string Texture => "ChensGradiusMod/Sprites/Sagna";

    public override bool PreAI()
    {
      if (IsNotMultiplayerClient() && !initialized)
      {
        int chosenYDir = Main.rand.NextBool().ToDirectionInt();
        Vector2 spawnPos = npc.position;

        npc.netUpdate = initialized = true;
        Dagoom.GroundDeploy(npc, ref yDirection, spawnPos, chosenYDir, DecideYDeploy);
        if (yDirection == 0 && !Dagoom.GroundDeploy(npc, ref yDirection, spawnPos,
                                                    -chosenYDir, DecideYDeploy))
        {
          Deactivate();
          return false;
        }
        else if (yDirection < 0)
        {
          startFrame = 8;
          endFrame = 12;
          maxFrame = (byte)Main.npcFrameCount[npc.type];
          FrameCounter = startFrame;
        }

        npc.TargetClosest(false);
        if (npc.Center.X > Main.player[npc.target].Center.X) persistDirection = -1;
        else persistDirection = 1;
        xDirection = persistDirection;
      }

      return initialized;
    }

    public override void AI()
    {
      npc.spriteDirection = npc.direction = persistDirection;

      if (npc.position == npc.oldPosition && mode != States.Spray)
      {
        if (++unstuckTick >= UnstuckTime)
        {
          unstuckTick = 0;
          xDirection = (sbyte)-xDirection;
          mode = States.Spray;
        }
      }

      MoveHorizontally();
      MoveVertically();
      if (mode != States.Spray) AdjustMovementBehavior();
    }

    public override void PostAI()
    {
      base.PostAI();
      if (!ConstantSync(ref syncTick, SyncRate) && oldMode != mode)
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
      base.SendExtraAI(writer);
      writer.Write(animateDirection);
      writer.Write((byte)mode);
      writer.Write((byte)oldMode);
      writer.Write(xDirection);
      writer.Write(yDirection);
      writer.Write(jumpCount);
      writer.Write(startFrame);
      writer.Write(endFrame);
      writer.Write(maxFrame);
      writer.Write(persistDirection);
      writer.Write(initialized);
    }

    public override void ReceiveExtraAI(BinaryReader reader)
    {
      base.ReceiveExtraAI(reader);
      animateDirection = reader.ReadSByte();
      mode = (States)reader.ReadByte();
      oldMode = (States)reader.ReadByte();
      xDirection = reader.ReadSByte();
      yDirection = reader.ReadSByte();
      jumpCount = reader.ReadByte();
      startFrame = reader.ReadByte();
      endFrame = reader.ReadByte();
      maxFrame = reader.ReadByte();
      persistDirection = reader.ReadSByte();
      initialized = reader.ReadBoolean();
    }

    protected override float RetaliationBulletSpeed => base.RetaliationBulletSpeed * 0.9f;

    protected override int RetaliationSpreadBulletNumber => 24;

    protected override float RetaliationSpreadAngleDifference => 180f;

    protected override int FrameSpeed => 6;

    private void MoveHorizontally()
    {
      switch (mode)
      {
        case States.Hop:
        case States.Fall:
          npc.velocity.X = XSpeed * xDirection;
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
          npc.velocity.Y += YGravity * yDirection;
          if (yDirection > 0) npc.velocity.Y = Math.Min(npc.velocity.Y, 0f);
          else npc.velocity.Y = Math.Max(npc.velocity.Y, 0f);
          break;

        case States.Fall:
          npc.velocity.Y += YGravity * yDirection;
          if (yDirection > 0) npc.velocity.Y = Math.Min(npc.velocity.Y, MaxYSpeed);
          else npc.velocity.Y = Math.Max(npc.velocity.Y, -MaxYSpeed);
          break;

        case States.Spray:
          npc.velocity.Y = 0f;
          break;
      }
    }

    private void AdjustMovementBehavior()
    {
      Vector2 beforeVelocity = npc.velocity;
      npc.velocity = Collision.TileCollision(npc.position, npc.velocity, npc.width, npc.height, false, false, yDirection);

      if (beforeVelocity.X != npc.velocity.X) xDirection = (sbyte)-xDirection;

      switch (mode)
      {
        case States.Hop:
          if (yDirection > 0 && npc.velocity.Y >= 0f ||
              yDirection < 0 && npc.velocity.Y <= 0f)
          {
            npc.velocity.Y = 0f;
            if (++jumpCount >= JumpCountForSpray)
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
            npc.velocity.Y = MaxYSpeed * -yDirection;
          }
          break;
      }
    }

    private void PerformAttack()
    {
      if (IsNotMultiplayerClient())
      {
        float radianAngle = 0f;
        for (int i = 0; i < 24; i++)
        {
          Projectile.NewProjectile(npc.Center, radianAngle.ToRotationVector2() * BacterionBullet.Spd,
                                   ModContent.ProjectileType<BacterionBullet>(),
                                   BulletFinalDamage(), BulletFinalKnockback(), Main.myPlayer);
          radianAngle = MathHelper.ToRadians(MathHelper.ToDegrees(radianAngle) + 15f);
        }
        npc.netUpdate = true;
      }
    }
  }
}