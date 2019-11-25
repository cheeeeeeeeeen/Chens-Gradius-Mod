using ChensGradiusMod.Items.Banners;
using ChensGradiusMod.Projectiles.Enemies;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.NPCs
{
  public class Dagoom : GradiusEnemy
  {
    private const int PersistDirection = 1;
    private const float CustomGravity = 5f;
    private const int RedeployRate = 300;
    private const int DeployRate = 15;
    private const float DetectionRange = 700;
    private const int SyncRate = 300;

    private bool initialized = false;
    private sbyte yDirection = 0;
    private int redeployTick = 0;
    private int deployTick = 0;
    private int rushCount = 0;
    private int syncTick = 0;
    private States oldMode = States.Standby;
    private States mode = States.Standby;

    public enum States { Standby, Open, Deploy, Close };

    public static bool GroundDeploy(NPC npc, ref sbyte yDirection, Vector2 spawnPos, int chosenYDir,
                                    Func<float, int, sbyte, bool, sbyte> DecideYDeploy,
                                    float yLength = 2f, int cancelDeployThreshold = 500)
    {
      npc.position = spawnPos;
      yDirection = DecideYDeploy(yLength, cancelDeployThreshold, (sbyte)chosenYDir, true);

      return yDirection != 0;
    }

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Dagoom");
      Main.npcFrameCount[npc.type] = 8;
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      npc.width = 60;
      npc.height = 50;
      npc.damage = 100;
      npc.lifeMax = 50;
      npc.value = 4000;
      npc.knockBackResist = 0f;
      npc.defense = 0;
      npc.behindTiles = true;
      npc.noGravity = true;
      npc.noTileCollide = false;
      bannerItem = ModContent.ItemType<DagoomBanner>();
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
      if (Main.hardMode) return .02f;
      else return 0f;
    }

    public override string Texture => "ChensGradiusMod/Sprites/Dagoom";

    public override bool PreAI()
    {
      if (GradiusHelper.IsNotMultiplayerClient() && !initialized)
      {
        int chosenYDir = Main.rand.NextBool().ToDirectionInt();
        Vector2 spawnPos = npc.position;

        npc.netUpdate = initialized = true;
        GroundDeploy(npc, ref yDirection, spawnPos, chosenYDir, DecideYDeploy);
        if (yDirection == 0 && !GroundDeploy(npc, ref yDirection, spawnPos,
                                             -chosenYDir, DecideYDeploy))
        {
          Deactivate();
          return false;
        }
        else if (yDirection < 0) FrameCounter = 4;
      }

      return initialized;
    }

    public override void AI()
    {
      npc.spriteDirection = npc.direction = PersistDirection;
      npc.velocity.Y = CustomGravity * yDirection;
      npc.velocity = Collision.TileCollision(npc.position, npc.velocity, npc.width, npc.height);

      switch (mode)
      {
        case States.Standby:
          npc.TargetClosest();
          if (++redeployTick >= RedeployRate &&
              Vector2.Distance(Target.Center, npc.Center) <= DetectionRange)
          {
            redeployTick = 0;
            mode = States.Open;
          }
          redeployTick = Math.Min(redeployTick, RedeployRate);
          break;

        case States.Deploy:
          if (++deployTick >= DeployRate)
          {
            deployTick = 0;
            SpawnRush();
          }
          break;
      }

      ConstantSync(ref syncTick, SyncRate);
    }

    public override void PostAI()
    {
      base.PostAI();
      if (!ConstantSync(ref syncTick, SyncRate) && oldMode != mode)
      {
        npc.netUpdate = true;
        oldMode = mode;
      }
    }

    public override void FindFrame(int frameHeight)
    {
      if (++FrameTick >= FrameSpeed)
      {
        FrameTick = 0;
        int limit;

        switch (mode)
        {
          case States.Open:
            if (yDirection > 0) limit = 4;
            else limit = Main.npcFrameCount[npc.type];
            if (++FrameCounter >= limit - 1) mode = States.Deploy;
            break;

          case States.Close:
            if (yDirection > 0) limit = 0;
            else limit = 4;
            if (--FrameCounter <= limit) mode = States.Standby;
            break;
        }
      }

      npc.frame.Y = FrameCounter * frameHeight;
    }

    public override void SendExtraAI(BinaryWriter writer)
    {
      base.SendExtraAI(writer);
      writer.Write(yDirection);
      writer.Write((byte)mode);
      writer.Write((byte)oldMode);
      writer.Write(initialized);
    }

    public override void ReceiveExtraAI(BinaryReader reader)
    {
      base.ReceiveExtraAI(reader);
      yDirection = reader.ReadSByte();
      mode = (States)reader.ReadByte();
      oldMode = (States)reader.ReadByte();
      initialized = reader.ReadBoolean();
    }

    protected override Types EnemyType => Types.Large;

    protected override int FrameSpeed => 9;

    protected override Action<Vector2> RetaliationOverride => RetaliationExplode;

    protected override int RetaliationExplodeBulletLayers => 2;

    protected override int RetaliationExplodeBulletNumberPerLayer => 16;

    protected override float RetaliationExplodeBulletAcceleration => -(GradiusEnemyBullet.Spd * .5f);

    private int TotalRushCount
    {
      get
      {
        int count = 2;
        if (Main.hardMode) count += 2;
        if (Main.expertMode) count += 3;

        return count;
      }
    }

    private Player Target => Main.player[npc.target];

    private void SpawnRush()
    {
      if (GradiusHelper.IsNotMultiplayerClient())
      {
        npc.TargetClosest(false);
        int xDirection = Math.Sign(Target.Center.X - npc.Center.X);
        GradiusHelper.NewNPC(npc.Center.X, npc.Center.Y, ModContent.NPCType<Rush>(),
                             ai0: xDirection, ai1: -yDirection, ai3: npc.target,
                             center: true);

        if (++rushCount >= TotalRushCount)
        {
          rushCount = 0;
          mode = States.Close;
        }
      }
    }
  }
}