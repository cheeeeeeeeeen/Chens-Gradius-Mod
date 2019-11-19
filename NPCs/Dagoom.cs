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
    private const int CancelDeployThreshold = 400;
    private const float CustomGravity = 5f;
    private const int RedeployRate = 300;
    private const int DeployRate = 15;
    private const float DetectionRange = 700;

    private bool initialized = false;
    private int yDirection = 0;
    private int redeployTick = 0;
    private int deployTick = 0;
    private int rushCount = 0;

    private States mode = States.Standby;

    public enum States { Standby, Open, Deploy, Close };

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Dagoom");
      Main.npcFrameCount[npc.type] = 4;
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      npc.width = 64;
      npc.height = 56;
      npc.damage = 100;
      npc.lifeMax = 50;
      npc.value = 4000;
      npc.knockBackResist = 0f;
      npc.defense = 0;
      npc.behindTiles = true;
      npc.noGravity = true;
      npc.noTileCollide = false;
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
      if (Main.hardMode) return .02f;
      else return 0f;
    }

    public override string Texture => "ChensGradiusMod/Sprites/Dagoom";

    public override bool PreAI()
    {
      if (!initialized)
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
            if (++rushCount >= TotalRushCount)
            {
              rushCount = 0;
              mode = States.Close;
            }
          }
          break;
      }
    }

    public override void FindFrame(int frameHeight)
    {
      if (++FrameTick >= FrameSpeed)
      {
        switch (mode)
        {
          case States.Open:
            if (++FrameCounter >= Main.npcFrameCount[npc.type] - 1) mode = States.Deploy;
            break;
          case States.Close:
            if (--FrameCounter <= 0) mode = States.Standby;
            break;
        }

        npc.frame.Y = FrameCounter * frameHeight;
        FrameTick = 0;
      }
    }

    public override void SendExtraAI(BinaryWriter writer)
    {
      writer.Write(yDirection);
    }

    public override void ReceiveExtraAI(BinaryReader reader)
    {
      yDirection = reader.ReadSByte();
    }

    protected override Types EnemyType => Types.Large;

    protected override int FrameSpeed { get; set; } = 9;

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
                             ai0: xDirection, ai1: -yDirection, ai3: npc.target);
      }
    }
  }
}