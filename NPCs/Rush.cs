using Terraria;

namespace ChensGradiusMod.NPCs
{
  public class Rush : GradiusEnemy
  {
    private const float VerticalSpeed = .23f;
    private const float HorizontalSpeed = .71f;
    private const int FireRate = 51;
    private const float AttackDistance = 800f;

    private int persistDirection = 0;
    private int yDirection = 0;
    private bool initialized = false;
    private States mode = States.Vertical;
    private int fireTick = 0;

    public enum States { Vertical, Horizontal };

    public override bool PreAI()
    {
      if (!initialized)
      {
        initialized = true;
        persistDirection = (int)npc.ai[0];
        yDirection = (int)npc.ai[1];
        npc.target = (int)npc.ai[2];
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

    }

    private void MoveVertically()
    {

    }

    private void PerformAttack() => Garun.PerformAttack(npc, ref fireTick, FireRate, AttackDistance);
  }
}