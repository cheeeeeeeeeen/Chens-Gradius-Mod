using System;
using System.IO;
using ChensGradiusMod.Projectiles.Enemies;
using Microsoft.Xna.Framework;
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

    public enum States { Standby, Open, Deploy, Close };

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
      npc.velocity.Y = CustomGravity * yDirection;
      npc.velocity = Collision.TileCollision(npc.position, npc.velocity, npc.width, npc.height);
    }

    public override void FindFrame(int frameHeight)
    {
      switch (mode)
      {
        case States.Standby:
        case States.Deploy:
          break;
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

    protected override Action<Vector2> RetaliationOverride => RetaliationExplode;

    protected override int RetaliationExplodeBulletLayers => 2;

    protected override int RetaliationExplodeBulletNumberPerLayer => 8;

    protected override float RetaliationExplodeBulletAcceleration => -(GradiusEnemyBullet.Spd * .5f);
  }
}