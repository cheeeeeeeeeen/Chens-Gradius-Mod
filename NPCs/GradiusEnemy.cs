using ChensGradiusMod.Gores;
using ChensGradiusMod.Projectiles.Enemies;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.NPCs
{
  public abstract class GradiusEnemy : ModNPC
  {
    private const float IncomingDamageMultiplier = .05f;

    public enum Types { Small, Large, Boss };

    public override void SetDefaults()
    {
      npc.friendly = false;

      switch ((sbyte)EnemyType)
      {
        case (sbyte)Types.Small:
          npc.HitSound = SoundID.NPCHit4;
          npc.DeathSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Enemies/Gradius2Death");
          break;
        case (sbyte)Types.Large:
          npc.HitSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Enemies/Gradius2Hit");
          npc.DeathSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Enemies/Gradius2Destroy");
          goto case -1;
        case (sbyte)Types.Boss:
          npc.HitSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Enemies/BigCoreHit");
          npc.DeathSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Enemies/BossDeath");
          goto case -1;
        case -1:
          ImmuneToBuffs();
          break;
      }

      npc.aiStyle = -1;
      aiType = 0;
      banner = npc.type;
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
          if (RetaliationOverride == null)
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
          else RetaliationOverride(npc.Center);
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
      if (Main.hardMode && spawnInfo.spawnTileY < Main.worldSurface) return .05f;
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

    public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback,
                                               ref bool crit, ref int hitDirection)
    {
      ReduceDamage(ref damage, ref knockback, ref crit);
    }

    public override void ModifyHitByItem(Player player, Item item, ref int damage,
                                         ref float knockback, ref bool crit)
    {
      ReduceDamage(ref damage, ref knockback, ref crit);
    }

    public override void SendExtraAI(BinaryWriter writer)
    {
      writer.Write((byte)FrameTick);
      writer.Write((byte)FrameCounter);
    }

    public override void ReceiveExtraAI(BinaryReader reader)
    {
      FrameTick = reader.ReadByte();
      FrameCounter = reader.ReadByte();
    }

    protected virtual int FrameTick { get; set; } = 0;

    protected virtual int FrameSpeed => 0;

    protected virtual int FrameCounter { get; set; } = 0;

    protected virtual Types EnemyType => Types.Small;

    protected virtual float RetaliationSprayRandomAngleDifference => 5f;

    protected virtual float RetaliationSpreadAngleDifference => 45f;

    protected virtual int RetaliationSpreadBulletNumber => 4;

    protected virtual int RetaliationExplodeBulletNumberPerLayer => 24;

    protected virtual int RetaliationExplodeBulletLayers => 2;

    protected virtual float RetaliationExplodeBulletAcceleration => 2f;

    protected virtual float RetaliationBulletSpeed => GradiusEnemyBullet.Spd;

    protected virtual Action<Vector2> RetaliationOverride => null;

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

    protected sbyte DecideYDeploy(float yLength, int checkLimit, sbyte direction,
                                  bool moveNpc = true)
    {
      Vector2 savedP = npc.position,
              movedV = new Vector2(0, yLength * direction),
              movedP = savedP,
              vOnCollide;

      for (int i = 0; i < checkLimit; i++)
      {
        vOnCollide = Collision.TileCollision(movedP, movedV, npc.width, npc.height);
        if (movedV != vOnCollide)
        {
          npc.position = moveNpc ? movedP : savedP;
          npc.velocity = moveNpc ? vOnCollide : Vector2.Zero;
          return direction;
        }
        else movedP += movedV;
      }

      return 0;
    }

    protected bool ConstantSync(ref int tick, int rate)
    {
      if (GradiusHelper.IsServer())
      {
        if (++tick >= rate)
        {
          tick = 0;
          npc.netUpdate = true;
          return true;
        }
      }

      return false;
    }

    protected void Deactivate()
    {
      npc.netUpdate = true;
      npc.active = false;
      npc.life = 0;
    }

    private void ReduceDamage(ref int damage, ref float knockback, ref bool crit)
    {
      switch (EnemyType)
      {
        case Types.Boss:
        case Types.Large:
          damage = GradiusHelper.RoundOffToWhole(damage * IncomingDamageMultiplier);
          crit = false;
          knockback = 0f;
          break;
      }
    }
  }
}