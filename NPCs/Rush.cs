using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace ChensGradiusMod.NPCs
{
  public class Rush : GradiusEnemy
  {
    private const float VerticalSpeed = .23f;
    private const float HorizontalSpeed = .71f;
    private const int FireRate = 51;
    private const float AttackDistance = 800f;
    private const int CanGoHorizontalTime = 60;

    private int persistDirection = 0;
    private int yDirection = 0;
    private bool initialized = false;
    private States mode = States.Vertical;
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

      npc.width = 32;
      npc.height = 34;
      npc.damage = 100;
      npc.lifeMax = 150;
      npc.value = 0f;
      npc.knockBackResist = 0f;
      npc.defense = 17;
    }

    public override bool PreAI()
    {
      if (!initialized)
      {
        persistDirection = (int)npc.ai[0];
        yDirection = (int)npc.ai[1];
        npc.target = (int)npc.ai[2];

        if (persistDirection == 0)
        {
          initialized = false;
          npc.active = false;
          npc.life = 0;
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
    }

    public override string Texture => "ChensGradiusMod/Sprites/Rush";

    protected override int FrameSpeed { get; set; } = 4;

    protected override Types EnemyType => Types.Small;

    protected override float RetaliationBulletSpeed => base.RetaliationBulletSpeed * .5f;

    protected override int RetaliationSpreadBulletNumber => 1;

    protected override float RetaliationSpreadAngleDifference => 0f;

    private Player Target => Main.player[npc.target];

    private void MoveHorizontally()
    {
      int xDirection = Math.Sign(Target.Center.X - npc.Center.X);
      if (xDirection == 0) xDirection = yDirection;

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
          mode = States.Horizontal;
        }
      }
      canGoHorizontalTick = Math.Min(canGoHorizontalTick, CanGoHorizontalTime);
    }

    private void PerformAttack() => Garun.PerformAttack(npc, ref fireTick, FireRate, AttackDistance);
  }
}