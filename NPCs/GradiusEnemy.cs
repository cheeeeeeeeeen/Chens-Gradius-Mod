using ChensGradiusMod.Gores;
using ChensGradiusMod.Projectiles.Enemies;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.NPCs
{
  public abstract class GradiusEnemy : ModNPC
  {
    public enum Types { Small, Large, Boss };

    public override void SetDefaults()
    {
      npc.friendly = false;

      switch (EnemyType)
      {
        case Types.Small:
          npc.HitSound = SoundID.NPCHit4;
          npc.DeathSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Enemies/Gradius2Death");
          break;
        case Types.Large:
          npc.HitSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Enemies/Gradius2Hit");
          npc.DeathSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Enemies/Gradius2Destroy");
          break;
        case Types.Boss:
          npc.HitSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Enemies/BigCoreHit");
          npc.DeathSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Enemies/BossDeath");
          break;
      }

      npc.aiStyle = -1;
      aiType = 0;
    }

    public override void PostAI() => ForceDefaults();

    public override void HitEffect(int hitDirection, double damage)
    {
      if (npc.life <= 0)
      {
        Gore.NewGorePerfect(GradiusExplode.CenterSpawn(npc.Center), Vector2.Zero,
                            mod.GetGoreSlot("Gores/GradiusExplode"));
        if (Main.expertMode)
        {
          switch (EnemyType)
          {
            case Types.Small:
            case Types.Large:
              RetaliationSpread(npc.Center);
              break;
            case Types.Boss:
              RetaliationExplode(npc.Center);
              break;
          }
        }
      }
    }

    public override void FindFrame(int frameHeight)
    {
      if (++FrameTick >= FrameSpeed)
      {
        FrameTick = 0;
        if (++FrameCounter >= Main.npcFrameCount[npc.type]) FrameCounter = 0;
        npc.frame.Y = frameHeight * FrameCounter;
      }
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
      if (Main.hardMode && spawnInfo.spawnTileY < Main.worldSurface) return .075f;
      else return 0f;
    }

    public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
    {
      if (Main.expertMode)
      {
        switch (EnemyType)
        {
          case Types.Boss:
          case Types.Large:
            RetaliationSpray(projectile.Center);
            break;
        }
      }
    }

    protected virtual int FrameTick { get; set; } = 0;

    protected virtual int FrameSpeed { get; set; } = 0;

    protected virtual int FrameCounter { get; set; } = 0;

    protected virtual Types EnemyType => Types.Small;

    protected virtual float RetaliationSprayRandomAngleDifference => 5f;

    protected virtual float RetaliationSpreadAngleDifference => 45f;

    protected virtual int RetaliationSpreadBulletNumber => 4;

    protected virtual int RetaliationExplodeBulletNumberPerLayer => 24;

    protected virtual int RetaliationExplodeBulletLayers => 2;

    protected virtual float RetaliationExplodeBulletAcceleration => 2f;

    protected virtual float RetaliationBulletSpeed => GradiusEnemyBullet.Spd;

    //protected virtual Rectangle[] InvulnerableHitboxes
    //{
    //  get { return new Rectangle[0]; }
    //}

    protected void ReduceDamage(ref int damage, ref float knockback, ref bool crit)
    {
      damage = 1;
      crit = false;
      knockback = 0f;
    }

    protected void ImmuneToBuffs()
    {
      for (int i = 0; i < npc.buffImmune.Length; i++)
      {
        npc.buffImmune[i] = true;
      }
    }

    protected void ForceDefaults()
    {
      if (npc.aiStyle != -1) npc.aiStyle = -1;
      if (aiType != 0) aiType = 0;
    }

    protected void RetaliationSpray(Vector2 spawnPoint)
    {
      int targetIndex = npc.target;
      npc.TargetClosest(false);
      Player retaliationTarget = Main.player[npc.target];

      float direction = GradiusHelper.MoveToward(npc.Center, retaliationTarget.Center).ToRotation();
      direction = MathHelper.ToDegrees(direction);
      direction = Main.rand.NextFloat(direction - RetaliationSprayRandomAngleDifference,
                                      direction + RetaliationSprayRandomAngleDifference + .0001f);
      direction = MathHelper.ToRadians((float)Math.Round(direction, 4, MidpointRounding.AwayFromZero));
      Vector2 spawnVelocity = direction.ToRotationVector2() * RetaliationBulletSpeed;

      if (GradiusHelper.IsSinglePlayer())
      {
        Projectile.NewProjectile(spawnPoint, spawnVelocity, ModContent.ProjectileType<GradiusEnemyBullet>(),
                                 GradiusEnemyBullet.Dmg, GradiusEnemyBullet.Kb, Main.myPlayer);
      }
      else if (GradiusHelper.IsMultiplayerClient())
      {
        ModPacket packet = mod.GetPacket();

        packet.Write((byte)ChensGradiusMod.PacketMessageType.SpawnRetaliationBullet);
        packet.WriteVector2(spawnPoint);
        packet.WriteVector2(spawnVelocity);
        packet.Write(GradiusEnemyBullet.Dmg);
        packet.Write(GradiusEnemyBullet.Kb);
        packet.Send();
      }

      npc.target = targetIndex;
    }

    protected void RetaliationSpread(Vector2 spawnPoint)
    {
      if (GradiusHelper.IsNotMultiplayerClient())
      {
        int targetIndex = npc.target;
        npc.TargetClosest(false);
        Player retaliationTarget = Main.player[npc.target];

        float direction = GradiusHelper.MoveToward(npc.Center, retaliationTarget.Center).ToRotation();
        direction = MathHelper.ToDegrees(direction);
        float higherAngleBound = direction + RetaliationSpreadAngleDifference;
        float lowerAngleBound = direction - RetaliationSpreadAngleDifference;
        float angleDifference = higherAngleBound - lowerAngleBound;
        float angleLength = angleDifference / RetaliationSpreadBulletNumber;

        float currentAngle = lowerAngleBound;
        for (int i = 0; i < RetaliationSpreadBulletNumber; i++)
        {
          Vector2 vel = MathHelper.ToRadians(currentAngle).ToRotationVector2() * RetaliationBulletSpeed;
          Projectile.NewProjectile(spawnPoint, vel, ModContent.ProjectileType<GradiusEnemyBullet>(),
                                   GradiusEnemyBullet.Dmg, GradiusEnemyBullet.Kb, Main.myPlayer);
          currentAngle += angleLength;
        }

        npc.target = targetIndex;
      }
    }

    protected void RetaliationExplode(Vector2 spawnPoint)
    {
      if (GradiusHelper.IsNotMultiplayerClient())
      {
        float angleLength = GradiusHelper.FullAngle / RetaliationExplodeBulletNumberPerLayer;
        float currentVelocity = GradiusEnemyBullet.Spd;
        int halfCounter = GradiusHelper.RoundOffToWhole(RetaliationExplodeBulletLayers * .5f);

        for (int i = 0; i < RetaliationExplodeBulletLayers; i++)
        {
          float currentAngle = 0;
          for (int j = 0; j < RetaliationExplodeBulletNumberPerLayer; j++)
          {
            Vector2 vel = MathHelper.ToRadians(currentAngle).ToRotationVector2() * currentVelocity;
            Projectile.NewProjectile(spawnPoint, vel, ModContent.ProjectileType<GradiusEnemyBullet>(),
                                     GradiusEnemyBullet.Dmg, GradiusEnemyBullet.Kb, Main.myPlayer);
            currentAngle += angleLength;
          }

          if (i == halfCounter) currentVelocity = GradiusEnemyBullet.Spd;

          if (i < halfCounter) currentVelocity += RetaliationExplodeBulletAcceleration;
          else currentVelocity -= RetaliationExplodeBulletAcceleration;
        }
      }
    }

    protected int DecideYDeploy(float yLength, int checkLimit, bool moveNpc = true,
                                bool forceSpawn = false, int fallbackValue = 0)
    {
      Vector2 savedPosition = npc.position,
              upwardV = new Vector2(0, -yLength),
              downwardV = new Vector2(0, yLength),
              upwardP = npc.position,
              downwardP = npc.position,
              velocityOnCollide;

      for (int i = 0; i < checkLimit; i++)
      {
        velocityOnCollide = Collision.TileCollision(downwardP, downwardV, npc.width, npc.height);
        if (downwardV != velocityOnCollide)
        {
          npc.position = moveNpc ? downwardP : savedPosition;
          npc.velocity = moveNpc ? velocityOnCollide : Vector2.Zero;
          return 1;
        }
        else downwardP += downwardV;

        velocityOnCollide = Collision.TileCollision(upwardP, upwardV, npc.width, npc.height);
        if (upwardV != velocityOnCollide)
        {
          npc.position = moveNpc ? upwardP : savedPosition;
          npc.velocity = moveNpc ? velocityOnCollide : Vector2.Zero;
          return -1;
        }
        else upwardP += upwardV;
      }

      if (!forceSpawn)
      {
        npc.friendly = true;
        npc.active = false;
        npc.life = 0;
      }
      return fallbackValue;
    }
  }
}
