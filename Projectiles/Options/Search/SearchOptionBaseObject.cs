using Terraria;

namespace ChensGradiusMod.Projectiles.Options.Search
{
  public abstract class SearchOptionBaseObject : OptionBaseObject
  {
    private const int FireRate = 3;
    private const float SeekDistance = 500f;

    private States mode = States.Follow;
    private int fireCounter = 0;
    private int target = -1;

    public enum States { Follow, Seek, Pursue, Return };

    protected override void OptionMovement()
    {
      switch (mode)
      {
        case States.Follow:
          base.OptionMovement();
          break;
        case States.Seek:
          FindTarget();
          break;
        case States.Pursue:
        case States.Return:
          break;
      }
    }

    protected override int SpawnDuplicateProjectile(Projectile p)
    {
      int result = -1;

      switch(mode)
      {
        case States.Follow:
          result = base.SpawnDuplicateProjectile(p);
          break;
        case States.Seek:
        case States.Return:
          result = -1;
          break;
        case States.Pursue:
          if (++fireCounter < FireRate) goto case States.Seek;
          else goto case States.Follow;
      }

      return result;
    }

    private NPC Target => Main.npc[target];

    private void FindTarget()
    {

    }
  }
}