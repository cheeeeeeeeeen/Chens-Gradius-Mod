using ChensGradiusMod.Items;
using ChensGradiusMod.Projectiles.Enemies;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.NPCs
{
  public class Moai : GradiusEnemy
  {
    private const float DetectionRange = 1400f;
    private const int AttackTickDelay = 5;
    private const int RestingTime = 40;
    private const int VulnerableTime = 10;

    private sbyte persistDirection = 0;
    private int mode = (int)States.Dormant;
    private int currentProjNumber = 0;
    private int attackTick = 0;
    private int restTick = 0;
    private int vulnerableTick = 0;

    public enum States : int { Dormant, Aggressive, Vulnerable, Resting };

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Moai");
      Main.npcFrameCount[npc.type] = 2;
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      npc.width = 80;
      npc.height = 122;
      npc.damage = 100;
      npc.lifeMax = 15;
      npc.value = 4000;
      npc.knockBackResist = 0f;
      npc.defense = 0;
      npc.behindTiles = true;
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
      if (Main.hardMode && spawnInfo.desertCave) return .1f;
      else if (spawnInfo.lihzahrd) return .2f;
      else return 0f;
    }

    public override string Texture => "ChensGradiusMod/Sprites/Moai";

    public override bool PreAI()
    {
      if (GradiusHelper.IsNotMultiplayerClient() && persistDirection == 0)
      {
        persistDirection = (sbyte)Main.rand.NextBool().ToDirectionInt();
        npc.netUpdate = true;
      }

      return persistDirection == 0 ? false : true;
    }

    public override void AI()
    {
      npc.spriteDirection = npc.direction = persistDirection;

      switch (mode)
      {
        case (int)States.Dormant:
          npc.target = DetectPlayer();
          if (npc.target >= 0)
          {
            mode = (int)States.Aggressive;
            goto case (int)States.Aggressive;
          }
          break;
        case (int)States.Aggressive:
          if (++attackTick >= AttackTickDelay)
          {
            attackTick = 0;
            PerformAttack();
            if (++currentProjNumber >= ProjectileNumber())
            {
              currentProjNumber = 0;
              mode = (int)States.Vulnerable;
            }
          }
          break;
        case (int)States.Vulnerable:
          if (++vulnerableTick >= VulnerableTime)
          {
            vulnerableTick = 0;
            mode = (int)States.Resting;
          }
          break;
        case (int)States.Resting:
          if (++restTick >= RestingTime)
          {
            restTick = 0;
            mode = (int)States.Dormant;
          }
          break;
      }
    }

    public override void FindFrame(int frameHeight)
    {
      switch (mode)
      {
        case (int)States.Dormant:
        case (int)States.Resting:
          npc.frame.Y = 0;
          break;
        case (int)States.Aggressive:
        case (int)States.Vulnerable:
          npc.frame.Y = frameHeight;
          break;
      }
    }

    public override bool? CanBeHitByProjectile(Projectile projectile)
    {
      if (IsHitInMouth(projectile.Hitbox))
      {
        int reverseVeloDiff = Math.Sign(projectile.oldPosition.X - projectile.position.X);
        if (reverseVeloDiff == npc.spriteDirection) return null;
      }

      return false;
    }

    public override bool? CanBeHitByItem(Player player, Item item)
    {
      for (int i = 0; i < Main.maxPlayers; i++)
      {
        if (GradiusGlobalItem.meleeHitbox[i].HasValue)
        {
          Rectangle hitbox = GradiusGlobalItem.meleeHitbox[i].Value;

          if (IsHitInMouth(hitbox))
          {
            int positionDiff = Math.Sign(player.Center.X - npc.Center.X);
            if (positionDiff == npc.spriteDirection) return null;
          }
        }
      }

      return false;
    }

    public override void SendExtraAI(BinaryWriter writer)
    {
      writer.Write(persistDirection);
    }

    public override void ReceiveExtraAI(BinaryReader reader)
    {
      persistDirection = reader.ReadSByte();
    }

    protected override Types EnemyType => Types.Large;

    protected override int RetaliationSpreadBulletNumber => 9;

    protected override float RetaliationSpreadAngleDifference => 25f;

    //protected override Rectangle[] InvulnerableHitboxes
    //{
    //  get
    //  {
    //    if (persistDirection < 0)
    //    {
    //      return new Rectangle[]
    //      {
    //        new Rectangle((int)npc.position.X, (int)npc.position.Y, 84, 62),
    //        new Rectangle((int)npc.position.X, (int)npc.position.Y + 76, 84, 50),
    //        new Rectangle((int)npc.position.X + 14, (int)npc.position.Y, 70, 126)
    //      };
    //    }
    //    else
    //    {
    //      return new Rectangle[]
    //      {
    //        new Rectangle((int)npc.position.X, (int)npc.position.Y, 84, 62),
    //        new Rectangle((int)npc.position.X, (int)npc.position.Y + 76, 84, 50),
    //        new Rectangle((int)npc.position.X, (int)npc.position.Y, 70, 126)
    //      };
    //    }
    //  }
    //}

    private int DetectPlayer()
    {
      float shortestDistance = DetectionRange;
      int nearestPlayer = -1;

      for (int i = 0; i < Main.maxPlayers; i++)
      {
        Player selectPlayer = Main.player[i];

        if (selectPlayer.active && !selectPlayer.dead)
        {
          if ((persistDirection > 0 && npc.Center.X < selectPlayer.Center.X) ||
              (persistDirection < 0 && npc.Center.X > selectPlayer.Center.X))
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

    private Rectangle MouthHitbox
    {
      get
      {
        if (persistDirection < 0) return new Rectangle((int)npc.position.X, (int)(npc.position.Y + 62f), 14, 14);
        else return new Rectangle((int)(npc.position.X + 70f), (int)(npc.position.Y + 62f), 14, 14);
      }
    }

    private Vector2 MouthCenter
    {
      get
      {
        return new Vector2
        {
          X = MouthHitbox.X + (MouthHitbox.Width * .5f),
          Y = MouthHitbox.Y + (MouthHitbox.Height * .5f)
        };
      }
    }

    private int ProjectileNumber()
    {
      int projNumber = 1;
      if (Main.expertMode) projNumber += 1;
      if (Main.hardMode) projNumber += 1;

      return projNumber;
    }

    private void PerformAttack()
    {
      if (GradiusHelper.IsNotMultiplayerClient())
      {
        Vector2 vel = GradiusHelper.MoveToward(MouthCenter, Main.player[npc.target].Center, MoaiBubble.Spd);
        Projectile.NewProjectile(MouthCenter, vel, ModContent.ProjectileType<MoaiBubble>(),
                                 MoaiBubble.Dmg, MoaiBubble.Kb, Main.myPlayer);
      }
    }

    private bool IsHitInMouth(Rectangle hitbox)
    {
      return (mode == (int)States.Aggressive || mode == (int)States.Vulnerable) &&
             MouthHitbox.Intersects(hitbox);
    }
  }
}
