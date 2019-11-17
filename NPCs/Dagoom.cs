using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.NPCs
{
  public class Dagoom : GradiusEnemy
  {
    private const int PersistDirection = 1;
    private const int CancelDeployThreshold = 400;
    private const float CustomGravity = 5f;

    private bool initialized = false;
    private int yDirection = 0;

    private States mode = States.Standby;

    public enum States { Standby, Deploy };

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Dagoom");
      // Main.npcFrameCount[npc.type] = 0;
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      // npc.width = 80;
      // npc.height = 122;
      npc.damage = 100;
      npc.lifeMax = 50;
      npc.value = 4000;
      npc.knockBackResist = 0f;
      npc.defense = 0;
      npc.behindTiles = true;
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
      if (Main.hardMode) return .02f;
      else return 0f;
    }

    public override string Texture => "ChensGradiusMod/Sprites/Dagoom";

    public override bool PreAI()
    {
      if (initialized)
      {
        initialized = true;
        if (yDirection == 0)
        {
          yDirection = DecideYDeploy(npc.height * .5f, CancelDeployThreshold);
          if (yDirection == 0)
          {
            npc.active = false;
            npc.life = 0;
            return false;
          }
        }
      }

      return initialized;
    }

    public override void AI()
    {
      npc.spriteDirection = npc.direction = PersistDirection;
    }
  }
}