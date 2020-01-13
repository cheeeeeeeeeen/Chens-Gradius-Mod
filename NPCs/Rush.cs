using ChensGradiusMod.Items.Banners;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.NPCs
{
  public class Rush : GradiusEnemy
  {
    private const float VerticalSpeed = 3f;
    private const float HorizontalSpeed = 6f;
    private const int FireRate = 30;
    private const float AttackDistance = 700f;
    private const int CanGoHorizontalTime = 60;

    private readonly bool[] frameSwitcher = new bool[2] { true, true };

    private int persistDirection = 0;
    private int xDirection = 0;
    private int yDirection = 0;
    private bool initialized = false;
    private States mode = States.Vertical;
    private States oldMode = States.Vertical;
    private int fireTick = 0;
    private int canGoHorizontalTick = 0;

    public enum States { Vertical, Horizontal };

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Rush");
      Main.npcFrameCount[npc.type] = 10;
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      npc.width = 28;
      npc.height = 28;
      npc.damage = 100;
      npc.lifeMax = 150;
      npc.value = 0f;
      npc.knockBackResist = 0f;
      npc.defense = 17;
      npc.noGravity = true;
      npc.noTileCollide = true;
      bannerItem = ModContent.ItemType<RushBanner>();

      ScaleStats();
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo) => 0f;

    public override bool PreAI()
    {
      if (!initialized)
      {
        persistDirection = (int)npc.ai[0];
        yDirection = (int)npc.ai[1];
        npc.target = (int)npc.ai[2];

        if (persistDirection == 0)
        {
          Deactivate();
          initialized = false;
        }
        else initialized = true;
      }

      return initialized;
    }

    public override void AI()
    {
      npc.spriteDirection = npc.direction = persistDirection;

      switch (mode)
      {
        case States.Vertical:
          MoveVertically();
          break;

        case States.Horizontal:
          MoveHorizontally();
          break;
      }

      PerformAttack();

      if (IsServer() && oldMode != mode)
      {
        oldMode = mode;
        npc.netUpdate = true;
      }
    }

    public override void FindFrame(int frameHeight)
    {
      if (++FrameTick >= FrameSpeed)
      {
        FrameTick = 0;
        if (++FrameCounter >= Main.npcFrameCount[npc.type]) FrameCounter = 0;
        int actualFrame = FrameCounter;

        if (FrameCounter <= 5 && FrameCounter >= 4)
        {
          if (frameSwitcher[0])
          {
            actualFrame = 4;
            frameSwitcher[0] = false;
          }
          else
          {
            actualFrame = 5;
            frameSwitcher[0] = true;
          }
          FrameCounter = 5;
        }
        else if (FrameCounter >= 9 || FrameCounter <= 0)
        {
          if (frameSwitcher[1])
          {
            actualFrame = 0;
            frameSwitcher[1] = false;
          }
          else
          {
            actualFrame = 9;
            frameSwitcher[1] = true;
          }
          FrameCounter = 0;
        }

        npc.frame.Y = frameHeight * actualFrame;
      }
    }

    public override string Texture => "ChensGradiusMod/Sprites/Rush";

    public override void SendExtraAI(BinaryWriter writer)
    {
      writer.Write((byte)mode);
      writer.Write((byte)oldMode);
    }

    public override void ReceiveExtraAI(BinaryReader reader)
    {
      mode = (States)reader.ReadByte();
      oldMode = (States)reader.ReadByte();
    }

    protected override int FrameSpeed => 3;

    protected override Types EnemyType => Types.Small;

    protected override float RetaliationBulletSpeed => base.RetaliationBulletSpeed * .5f;

    protected override int RetaliationSpreadBulletNumber => 1;

    protected override float RetaliationSpreadAngleDifference => 0f;

    private Player Target => Main.player[npc.target];

    private void MoveHorizontally()
    {
      npc.velocity = new Vector2(HorizontalSpeed * xDirection, 0);
    }

    private void MoveVertically()
    {
      npc.velocity = new Vector2(0, VerticalSpeed * yDirection);

      if (++canGoHorizontalTick >= CanGoHorizontalTime)
      {
        if ((yDirection > 0 && npc.Center.Y >= Target.Center.Y) ||
            (yDirection < 0 && npc.Center.Y <= Target.Center.Y))
        {
          xDirection = Math.Sign(Target.Center.X - npc.Center.X);
          if (xDirection == 0) xDirection = yDirection;
          mode = States.Horizontal;
        }
      }
      canGoHorizontalTick = Math.Min(canGoHorizontalTick, CanGoHorizontalTime);
    }

    private void PerformAttack()
    {
      Garun.PerformAttack(npc, ref fireTick, BulletFinalDamage(),
                          BulletFinalKnockback(), FireRate, AttackDistance);
    }
  }
}