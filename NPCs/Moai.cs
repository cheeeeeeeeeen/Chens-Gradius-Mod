using ChensGradiusMod.Projectiles.Enemies;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.NPCs
{
  public class Moai : ModNPC
  {
    private const float DetectionRange = 1400f;

    private readonly int attackTickDelay = 5;
    private readonly int restingTime = 40;

    private int persistDirection = 0;
    private int currentTarget = -1;
    private int mode = (int)States.Dormant;
    private int currentProjNumber = 0;
    private int attackTick = 0;
    private int restTick = 0;

    public enum States : int { Dormant, Aggressive, Resting };

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Moai");
      Main.npcFrameCount[npc.type] = 2;
    }

    public override void SetDefaults()
    {
      npc.width = 84;
      npc.height = 126;
      npc.damage = 100;
      npc.lifeMax = 10;
      npc.HitSound = SoundID.NPCHit1;
      npc.DeathSound = SoundID.NPCDeath2;
      npc.value = 50f;
      npc.friendly = false;
      npc.knockBackResist = 0f;
      npc.dontTakeDamage = true;
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
      if (Main.hardMode) return .1f;
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
              mode = (int)States.Resting;
            }
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

      InteractProjectiles();
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
          npc.frame.Y = frameHeight;
          break;
      }
    }

    private void InteractProjectiles()
    {
      for (int i = 0; i < Main.maxProjectiles; i++)
      {
        Projectile selectProj = Main.projectile[i];
        if (!(selectProj.modProjectile is MoaiBubble))
        {
          if (selectProj.active && GradiusHelper.CanDamage(selectProj))
          {
            if (mode == (int)States.Aggressive && MouthHitbox.Intersects(selectProj.Hitbox))
            {
              npc.life--;
            }
            if (!selectProj.minion && !Main.projPet[selectProj.type] &&
                npc.Hitbox.Intersects(selectProj.Hitbox))
            {
              selectProj.Kill();
            }
          }
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
      if (Main.netMode != NetmodeID.MultiplayerClient)
      {
        Vector2 vel = GradiusHelper.MoveToward(MouthCenter, Main.player[currentTarget].Center, 3);
        Projectile.NewProjectile(MouthCenter, vel, mod.ProjectileType<MoaiBubble>(),
                                 MoaiBubble.Dmg, MoaiBubble.Kb, Main.myPlayer);
      }
    }
  }
}
