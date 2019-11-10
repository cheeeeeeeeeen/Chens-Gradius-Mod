using ChensGradiusMod.Projectiles.Enemies;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.NPCs
{
  public class Zalk : GradiusEnemy
  {
    private const float TravelSpeed = 5f;
    private const float XDistanceToIntercept = 120f;
    private const float YThresholdToRetreat = .01f;
    private const float XDistanceSeries = 60f;
    private const int FireRate = 57;
    private const float AttackDistance = 1200;
    private const int RandomFireInterval = 30;

    private bool targetDetermined = false;
    private int persistDirection = 0;
    private States mode = States.Attack;
    private bool initializedAction = false;
    private int fireTick = 0;
    private int setFireInterval = 0;

    public enum States { Attack, Intercept, Retreat, Fire };

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Zalk");
      Main.npcFrameCount[npc.type] = 5;
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      npc.width = 28;
      npc.height = 28;
      npc.damage = 100;
      npc.lifeMax = 110;
      npc.value = 200f;
      npc.knockBackResist = 0f;
      npc.defense = 40;
      npc.noGravity = true;
      npc.noTileCollide = true;
    }

    public override bool PreAI()
    {
      if (!targetDetermined)
      {
        npc.TargetClosest(false);
        targetDetermined = true;
        if (persistDirection == 0)
        {
          if (npc.Center.X > Main.player[npc.target].Center.X) persistDirection = -1;
          else persistDirection = 1;
        }
        CreateSeries();
      }

      return targetDetermined;
    }

    public override void AI()
    {
      npc.spriteDirection = npc.direction = persistDirection;

      switch (mode)
      {
        case States.Attack:
          if (!initializedAction) initializedAction = AttackBehavior();
          if (AttackToIntercept())
          {
            mode = States.Intercept;
            initializedAction = false;
          }
          break;
        case States.Intercept:
          if (!initializedAction) initializedAction = InterceptBehavior();
          if (InterceptToRetreat())
          {
            mode = States.Retreat;
            initializedAction = false;
          }
          goto case States.Fire;
        case States.Retreat:
          if (!initializedAction) initializedAction = RetreatBehavior();
          goto case States.Fire;
        case States.Fire:
          if (GradiusHelper.IsNotMultiplayerClient())
          {
            if (Vector2.Distance(npc.Center, Main.player[npc.target].Center) <= AttackDistance)
            {
              int adjustRate = FireRate;
              if (fireTick <= 0) setFireInterval = Main.rand.Next(0, RandomFireInterval + 1);
              adjustRate += setFireInterval;
              if (mode == States.Retreat) adjustRate = GradiusHelper.RoundOffToWhole(adjustRate * .5f);
              PerformAttack(adjustRate);
            }
            else fireTick = 0;
          }
          break;
      }
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
      return base.SpawnChance(spawnInfo) * .9f;
    }

    public override string Texture => "ChensGradiusMod/Sprites/Zalk";

    protected override int FrameSpeed { get; set; } = 3;

    protected override Types EnemyType => Types.Small;

    protected override float RetaliationBulletSpeed => base.RetaliationBulletSpeed * .7f;

    protected override int RetaliationSpreadBulletNumber => 1;

    protected override float RetaliationSpreadAngleDifference => 0f;

    private Player TargetPlayer => Main.player[npc.target];

    private int SeriesCount => Main.expertMode ? 9 : 4;

    private bool AttackBehavior()
    {
      npc.velocity = new Vector2(persistDirection, 0f) * TravelSpeed;
      return true;
    }

    private bool AttackToIntercept()
    {
      float xCurrentDistance = Math.Abs(npc.Center.X - TargetPlayer.Center.X);
      return xCurrentDistance <= XDistanceToIntercept;
    }

    private bool InterceptBehavior()
    {
      float angleDirection, yCurrentDirection;

      yCurrentDirection = Math.Sign(TargetPlayer.Center.Y - npc.Center.Y);

      if (persistDirection > 0)
      {
        if (yCurrentDirection < 0) angleDirection = 135f;
        else angleDirection = 225f;
      }
      else
      {
        if (yCurrentDirection < 0) angleDirection = 45f;
        else angleDirection = 315f;
      }

      npc.velocity = new Vector2
      {
        X = (float)Math.Cos(MathHelper.ToRadians(angleDirection)),
        Y = (float)-Math.Sin(MathHelper.ToRadians(angleDirection))
      } * TravelSpeed;

      return true;
    }

    private bool InterceptToRetreat()
    {
      return GradiusHelper.IsEqualWithThreshold(npc.Center.Y,
                                                TargetPlayer.Center.Y,
                                                TravelSpeed + YThresholdToRetreat);
    }

    private bool RetreatBehavior()
    {
      npc.velocity = new Vector2(-persistDirection, 0f) * TravelSpeed * 2;
      return true;
    }

    private void CreateSeries()
    {
      if (GradiusHelper.IsNotMultiplayerClient() && npc.ai[0] < 1)
      {
        for (int i = 1; i <= SeriesCount; i++)
        {
          GradiusHelper.NewNPC(npc.Bottom.X + (XDistanceSeries * i * -persistDirection),
                               npc.Bottom.Y, ModContent.NPCType<Zalk>(), 0, 1);
        }
      }
    }

    private void PerformAttack(int conditionRate)
    {
      if (++fireTick >= conditionRate)
      {
        fireTick = 0;
        Vector2 vel = GradiusHelper.MoveToward(npc.Center, Main.player[npc.target].Center, GradiusEnemyBullet.Spd);
        Projectile.NewProjectile(npc.Center, vel, ModContent.ProjectileType<GradiusEnemyBullet>(),
                                 GradiusEnemyBullet.Dmg, GradiusEnemyBullet.Kb, Main.myPlayer);
      }
    }
  }
}
