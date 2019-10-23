using ChensGradiusMod.Projectiles.Enemies;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.NPCs
{
  public class Grazia : GradiusEnemy
  {
    private const float DetectionRange = 800f;
    private const int PersistDirection = -1;
    private const float CustomGravity = 5f;

    private readonly int[] directLowerAngleAim = { 0, 21, 41, 61, 81, 100, 120, 140, 160 };
    private readonly int[] directHigherAngleAim = { 20, 40, 60, 80, 99, 119, 139, 159, 180 };
    private readonly int[] directFrameAngleAim = { 8, 7, 6, 5, 4, 3, 2, 1, 0 };
    private readonly int[] inverseLowerAngleAim = { 180, 201, 221, 241, 261, 280, 300, 320, 340 };
    private readonly int[] inverseHigherAngleAim = { 200, 220, 240, 260, 279, 299, 319, 339, 360 };
    private readonly int[] inverseFrameAngleAim = { 17, 16, 15, 14, 13, 12, 11, 10, 9 };
    private readonly int fireRate = 50;
    private readonly int cancelDeployThreshold = 500;

    private int yDirection = 0;
    private int fireTick = 0;

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Grazia");
      Main.npcFrameCount[npc.type] = 18;
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      npc.width = 28;
      npc.height = 28;
      npc.damage = 100;
      npc.lifeMax = 200;
      npc.value = 2000f;
      npc.knockBackResist = 0f;
      npc.defense = 50;
      npc.noGravity = true;
      npc.behindTiles = true;
      npc.frame.Y = 0;
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
      if (Main.hardMode && spawnInfo.spawnTileY < GradiusHelper.UnderworldTilesYLocation &&
          spawnInfo.spawnTileY > (GradiusHelper.SkyTilesYLocation + Main.worldSurface) * .5f)
      {
        return .05f;
      }
      else return 0f;
    }

    public override string Texture => "ChensGradiusMod/Sprites/Grazia";

    public override void AI()
    {
      npc.direction = npc.spriteDirection = PersistDirection;

      if (yDirection == 0)
      {
        yDirection = DecideDeploy();
        if (yDirection < 0) npc.frame.Y = 416;
      }
      else
      {
        npc.velocity.Y = CustomGravity * yDirection;
        npc.velocity = Collision.TileCollision(npc.position, npc.velocity, npc.width, npc.height);
      }

      if (GradiusHelper.IsNotMultiplayerClient())
      {
        if (npc.target >= 0) PerformAttack();
        else fireTick = 0;
      }
    }

    public override void FindFrame(int frameHeight)
    {
      if (TargetPlayer() != null)
      {
        int direction = GradiusHelper.RoundOffToWhole(GradiusHelper.GetBearing(npc.Center, TargetPlayer().Center));

        if (yDirection > 0)
        {
          for (int i = 0; i < directFrameAngleAim.Length; i++)
          {
            if (direction >= directLowerAngleAim[i] && direction <= directHigherAngleAim[i])
            {
              npc.frame.Y = frameHeight * directFrameAngleAim[i];
              break;
            }
          }
        }
        else
        {
          for (int i = 0; i < inverseFrameAngleAim.Length; i++)
          {
            if (direction >= inverseLowerAngleAim[i] && direction <= inverseHigherAngleAim[i])
            {
              npc.frame.Y = frameHeight * inverseFrameAngleAim[i];
              break;
            }
          }
        }
      }
    }

    public override void SendExtraAI(BinaryWriter writer)
    {
      writer.Write(yDirection);
      writer.Write(npc.position.X);
      writer.Write(npc.position.Y);
      writer.Write(npc.target);
    }

    public override void ReceiveExtraAI(BinaryReader reader)
    {
      yDirection = reader.ReadInt32();
      npc.position.X = reader.ReadSingle();
      npc.position.Y = reader.ReadSingle();
      npc.target = reader.ReadInt32();
    }

    protected override float RetaliationBulletSpeed => base.RetaliationBulletSpeed * 2f;

    protected override int RetaliationSpreadBulletNumber => 3;

    protected override float RetaliationSpreadAngleDifference => 3f;

    private Player TargetPlayer()
    {
      npc.target = DetectTarget();
      return npc.target == -1 ? null : Main.player[npc.target];
    }

    private int DetectTarget()
    {
      float shortestDistance = DetectionRange;
      int nearestPlayer = -1;

      for (int i = 0; i < Main.maxPlayers; i++)
      {
        Player selectPlayer = Main.player[i];

        if (selectPlayer.active && !selectPlayer.dead)
        {
          if ((yDirection > 0 && npc.Center.Y >= selectPlayer.Center.Y) ||
              (yDirection < 0 && npc.Center.Y <= selectPlayer.Center.Y))
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

    private void PerformAttack()
    {
      if (++fireTick >= fireRate)
      {
        fireTick = 0;
        Vector2 vel = GradiusHelper.MoveToward(npc.Center, Main.player[npc.target].Center, GradiusEnemyBullet.Spd);
        Projectile.NewProjectile(npc.Center, vel, ModContent.ProjectileType<GradiusEnemyBullet>(),
                                 GradiusEnemyBullet.Dmg, GradiusEnemyBullet.Kb, Main.myPlayer);
      }
    }

    private int DecideDeploy()
    {
      Vector2 upwardV = new Vector2(0, -CustomGravity),
              downwardV = new Vector2(0, CustomGravity),
              upwardP = npc.position,
              downwardP = npc.position,
              velocityOnCollide;

      for (int i = 0; i < cancelDeployThreshold; i++)
      {
        velocityOnCollide = Collision.TileCollision(downwardP, downwardV, npc.width, npc.height);
        if (downwardV != velocityOnCollide)
        {
          npc.position = downwardP;
          npc.velocity = velocityOnCollide;
          return 1;
        }
        else downwardP += downwardV;

        velocityOnCollide = Collision.TileCollision(upwardP, upwardV, npc.width, npc.height);
        if (upwardV != velocityOnCollide)
        {
          npc.position = upwardP;
          npc.velocity = velocityOnCollide;
          return -1;
        }
        else upwardP += upwardV;
      }

      npc.friendly = true;
      npc.active = false;
      npc.life = 0;
      return 0;
    }
  }
}
