using ChensGradiusMod.Items.Banners;
using ChensGradiusMod.Projectiles.Enemies;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.NPCs
{
  public class Garun : GradiusEnemy
  {
    private const float WaveFrequency = .05f;
    private const float WaveAmplitude = 150f;
    private const float TravelSpeed = 5f;
    private const float AttackDistance = 1200;
    private const int FireRate = 20;
    private const int SyncRate = 120;

    private ushort timerTick = 0;
    private bool targetDetermined = false;
    private int persistDirection = 0;
    private int fireTick = 0;
    private int syncTick = 0;

    public static void PerformAttack(NPC npc, ref int fireTick, int damage, float knockback,
                                     int fireRate = FireRate, float atkDistance = AttackDistance)
    {
      if (IsNotMultiplayerClient() &&
          Vector2.Distance(npc.Center, Main.player[npc.target].Center) <= atkDistance)
      {
        if ((npc.direction >= 0 && npc.Center.X >= Main.player[npc.target].Center.X) ||
           (npc.direction <= 0 && npc.Center.X <= Main.player[npc.target].Center.X))
        {
          if (++fireTick >= fireRate)
          {
            fireTick = 0;
            Vector2 vel = MoveToward(npc.Center, Main.player[npc.target].Center, BacterionBullet.Spd);
            Projectile.NewProjectile(npc.Center, vel, ModContent.ProjectileType<BacterionBullet>(),
                                     damage, knockback, Main.myPlayer);
          }
        }
        else fireTick = 0;
      }
    }

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Garun");
      Main.npcFrameCount[npc.type] = 9;
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      npc.width = 28;
      npc.height = 22;
      npc.damage = 100;
      npc.lifeMax = 240;
      npc.value = 1000f;
      npc.knockBackResist = 0f;
      npc.defense = 30;
      npc.noGravity = true;
      npc.noTileCollide = true;
      bannerItem = ModContent.ItemType<GarunBanner>();

      ScaleStats();
    }

    public override bool PreAI()
    {
      npc.velocity = Vector2.Zero;

      if (!targetDetermined)
      {
        npc.TargetClosest(false);
        targetDetermined = true;
        if (persistDirection == 0)
        {
          if (npc.Center.X > Main.player[npc.target].Center.X) persistDirection = -1;
          else persistDirection = 1;
        }
      }

      return targetDetermined;
    }

    public override void AI()
    {
      npc.spriteDirection = npc.direction = persistDirection;

      float xTo = (float)Math.Cos(GetDirection());
      float yTo = (float)Math.Sin(GetDirection());
      float wobble = WaveAmplitude * (float)Math.Cos(WaveFrequency * timerTick++) * WaveFrequency;
      npc.velocity.X += xTo * TravelSpeed - yTo * wobble;
      npc.velocity.Y += -yTo * TravelSpeed + xTo * wobble;

      PerformAttack(npc, ref fireTick, BulletFinalDamage(), BulletFinalKnockback());

      ConstantSync(ref syncTick, SyncRate);
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
      return base.SpawnChance(spawnInfo) * 2f;
    }

    public override string Texture => "ChensGradiusMod/Sprites/Garun";

    public override void SendExtraAI(BinaryWriter writer)
    {
      writer.Write(timerTick);
    }

    public override void ReceiveExtraAI(BinaryReader reader)
    {
      timerTick = reader.ReadUInt16();
    }

    protected override int FrameSpeed => 4;

    protected override Types EnemyType => Types.Small;

    protected override float RetaliationBulletSpeed => base.RetaliationBulletSpeed * 2.3f;

    protected override int RetaliationSpreadBulletNumber => 1;

    protected override float RetaliationSpreadAngleDifference => 0f;

    private double GetDirection()
    {
      float degreeAngle;

      if (npc.direction < 0) degreeAngle = 180f;
      else degreeAngle = 0f;

      return MathHelper.ToRadians(degreeAngle);
    }
  }
}