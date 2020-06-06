﻿using ChensGradiusMod.Items.Banners;
using ChensGradiusMod.Projectiles.Enemies;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.NPCs
{
  public class Grazia : GradiusEnemy
  {
    private const float DetectionRange = 1000f;
    private const sbyte PersistDirection = -1;
    private const float CustomGravity = 5f;
    private const int FireRate = 43;
    private const int SyncRate = 300;

    private readonly int[] directLowerAngleAim = { 0, 21, 41, 61, 81, 100, 120, 140, 160 };
    private readonly int[] directHigherAngleAim = { 20, 40, 60, 80, 99, 119, 139, 159, 180 };
    private readonly int[] directFrameAngleAim = { 8, 7, 6, 5, 4, 3, 2, 1, 0 };
    private readonly int[] inverseLowerAngleAim = { 180, 201, 221, 241, 261, 280, 300, 320, 340 };
    private readonly int[] inverseHigherAngleAim = { 200, 220, 240, 260, 279, 299, 319, 339, 360 };
    private readonly int[] inverseFrameAngleAim = { 17, 16, 15, 14, 13, 12, 11, 10, 9 };

    private sbyte yDirection = 0;
    private int fireTick = 0;
    private int syncTick = 0;
    private bool initialized = false;

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Grazia");
      Main.npcFrameCount[npc.type] = 18;
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      npc.width = 28;
      npc.height = 28;
      npc.damage = 100;
      npc.lifeMax = 200;
      npc.value = 2000f;
      npc.knockBackResist = 0f;
      npc.defense = 50;
      npc.noGravity = true;
      npc.behindTiles = true;
      bannerItem = ModContent.ItemType<GraziaBanner>();

      ScaleStats();
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
      if (UsualSpawnConditions(spawnInfo) && AboveUnderworldCondition(spawnInfo)
          && spawnInfo.spawnTileY > BelowSkyButAboveSurfaceArea)
      {
        return ActualSpawnRate(.05f);
      }
      else return 0f;
    }

    public override string Texture => "ChensGradiusMod/Sprites/Grazia";

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
          npc.frame.Y = 442;
          FrameCounter = 13;
        }
      }

      return initialized;
    }

    public override void AI()
    {
      npc.direction = npc.spriteDirection = PersistDirection;

      npc.velocity.Y = CustomGravity * yDirection;
      npc.velocity = Collision.TileCollision(npc.position, npc.velocity, npc.width, npc.height);

      if (IsNotMultiplayerClient())
      {
        if (npc.target >= 0) PerformAttack();
        else fireTick = 0;
      }

      ConstantSync(ref syncTick, SyncRate);
    }

    public override void FindFrame(int frameHeight)
    {
      if (TargetPlayer() != null)
      {
        int direction = RoundOffToWhole(GetBearing(npc.Center, TargetPlayer().Center));

        if (yDirection > 0)
        {
          for (int i = 0; i < directFrameAngleAim.Length; i++)
          {
            if (direction >= directLowerAngleAim[i] && direction <= directHigherAngleAim[i])
            {
              npc.frame.Y = frameHeight * directFrameAngleAim[i];
              break;
            }
          }
        }
        else
        {
          for (int i = 0; i < inverseFrameAngleAim.Length; i++)
          {
            if (direction >= inverseLowerAngleAim[i] && direction <= inverseHigherAngleAim[i])
            {
              npc.frame.Y = frameHeight * inverseFrameAngleAim[i];
              break;
            }
          }
        }
      }
    }

    public override void SendExtraAI(BinaryWriter writer)
    {
      base.SendExtraAI(writer);
      writer.Write(yDirection);
      writer.Write(initialized);
      writer.Write((ushort)npc.frame.Y);
    }

    public override void ReceiveExtraAI(BinaryReader reader)
    {
      base.ReceiveExtraAI(reader);
      yDirection = reader.ReadSByte();
      initialized = reader.ReadBoolean();
      npc.frame.Y = reader.ReadUInt16();
    }

    protected override int RetaliationSpreadBulletNumber => 3;

    protected override float RetaliationSpreadAngleDifference => 3f;

    private Player TargetPlayer()
    {
      npc.target = DetectTarget();
      return npc.target == -1 ? null : Main.player[npc.target];
    }

    private int DetectTarget()
    {
      float shortestDistance = DetectionRange;
      int nearestPlayer = -1;

      for (int i = 0; i < Main.maxPlayers; i++)
      {
        Player selectPlayer = Main.player[i];

        if (selectPlayer.active && !selectPlayer.dead)
        {
          if ((yDirection > 0 && npc.Center.Y >= selectPlayer.Center.Y) ||
              (yDirection < 0 && npc.Center.Y <= selectPlayer.Center.Y))
          {
            float distance = Vector2.Distance(npc.Center, selectPlayer.Center);
            if (distance < shortestDistance)
            {
              shortestDistance = distance;
              nearestPlayer = i;
            }
          }
        }
      }

      return nearestPlayer;
    }

    private void PerformAttack()
    {
      if (++fireTick >= FireRate)
      {
        fireTick = 0;
        Vector2 vel = MoveToward(npc.Center, Main.player[npc.target].Center, BacterionBullet.Spd);
        Projectile.NewProjectile(npc.Center, vel, ModContent.ProjectileType<BacterionBullet>(),
                                 BulletFinalDamage(), BulletFinalKnockback(), Main.myPlayer);
      }
    }

    private double BelowSkyButAboveSurfaceArea => (SkyTilesYLocation + Main.worldSurface) * .5f;
  }
}