using ChensGradiusMod.Items;
using ChensGradiusMod.Projectiles.Enemies;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.NPCs
{
  public class Moai : GradiusEnemy
  {
    private const float DetectionRange = 1400f;

    private readonly int attackTickDelay = 5;
    private readonly int restingTime = 40;
    private readonly int vulnerableTime = 5;

    private int persistDirection = 0;
    private int currentTarget = -1;
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
      npc.lifeMax = 10;
      npc.value = 5000f;
      npc.knockBackResist = 0f;
      npc.defense = 1000;
      npc.behindTiles = true; 
      ImmuneToBuffs();
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
      if (Main.hardMode && spawnInfo.desertCave) return .1f;
      else if (spawnInfo.lihzahrd) return .2f;
      else return 0f;
    }

    public override string Texture => "ChensGradiusMod/Sprites/Moai";

    public override void AI()
    {
      if (persistDirection == 0) persistDirection = Main.rand.NextBool().ToDirectionInt();
      npc.spriteDirection = npc.direction = persistDirection;

      switch (mode)
      {
        case (int)States.Dormant:
          currentTarget = DetectPlayer();
          if (currentTarget >= 0)
          {
            mode = (int)States.Aggressive;
            goto case (int)States.Aggressive;
          }
          break;
        case (int)States.Aggressive:
          if (++attackTick >= attackTickDelay)
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
          if (++vulnerableTick >= vulnerableTime)
          {
            vulnerableTick = 0;
            mode = (int)States.Resting;
          }
          break;
        case (int)States.Resting:
          if (++restTick >= restingTime)
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
      if (!(projectile.modProjectile is MoaiBubble) && projectile.active &&
          GradiusHelper.CanDamage(projectile) && projectile.friendly && !projectile.hostile)
      {
        if (!projectile.minion && !Main.projPet[projectile.type])
        {
          for (int i = 0; i < InvulnerableHitboxes.Length; i++)
          {
            if (InvulnerableHitboxes[i].Intersects(projectile.Hitbox))
            {
              projectile.Kill();
              break;
            }
          }
        }

        if (IsHitInMouth(projectile.Hitbox)) return true;
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

          if (IsHitInMouth(hitbox)) return true;
        }
      }

      return false;
    }

    public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
      => ReduceDamage(ref damage, ref knockback, ref crit);

    public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
      => ReduceDamage(ref damage, ref knockback, ref crit);

    public override void SendExtraAI(BinaryWriter writer)
    {
      writer.Write(mode);
      writer.Write(persistDirection);
      writer.Write(npc.position.X);
      writer.Write(npc.position.Y);
      writer.Write(npc.aiStyle);
    }

    public override void ReceiveExtraAI(BinaryReader reader)
    {
      mode = reader.ReadInt32();
      persistDirection = reader.ReadInt32();
      npc.position.X = reader.ReadSingle();
      npc.position.Y = reader.ReadSingle();
      npc.aiStyle = reader.ReadInt32();
    }

    protected override Types EnemyType => Types.Large;

    protected override Rectangle[] InvulnerableHitboxes
    {
      get
      {
        if (persistDirection < 0)
        {
          return new Rectangle[]
          {
            new Rectangle((int)npc.position.X, (int)npc.position.Y, 84, 62),
            new Rectangle((int)npc.position.X, (int)npc.position.Y + 76, 84, 50),
            new Rectangle((int)npc.position.X + 14, (int)npc.position.Y, 70, 126)
          };
        }
        else
        {
          return new Rectangle[]
          {
            new Rectangle((int)npc.position.X, (int)npc.position.Y, 84, 62),
            new Rectangle((int)npc.position.X, (int)npc.position.Y + 76, 84, 50),
            new Rectangle((int)npc.position.X, (int)npc.position.Y, 70, 126)
          };
        }
      }
    }

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
        Vector2 vel = GradiusHelper.MoveToward(MouthCenter, Main.player[currentTarget].Center, MoaiBubble.Spd);
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
