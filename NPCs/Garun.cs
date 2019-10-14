using ChensGradiusMod.Projectiles.Enemies;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.NPCs
{
  public class Garun : GradiusEnemy
  {
    private readonly float waveFrequency = .05f;
    private readonly float waveAmplitude = 150f;
    private readonly float travelSpeed = 5f;
    private readonly float attackDistance = 1200;
    private readonly int fireRate = 20;
    private int timerTick = 0;
    private bool targetDetermined = false;
    private int persistDirection = 0;
    private int fireTick = 0;

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
      npc.lifeMax = 80;
      npc.value = 1000f;
      npc.knockBackResist = 0f;
      npc.defense = 100;
      npc.noGravity = true;
      npc.noTileCollide = true;
    }

    public override bool PreAI()
    {
      npc.velocity = Vector2.Zero;

      if (!targetDetermined)
      {
        npc.TargetClosest(true);
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
      npc.spriteDirection = npc.direction = -persistDirection;

      float xTo = (float)Math.Cos(GetDirection());
      float yTo = (float)Math.Sin(GetDirection());
      float wobble = waveAmplitude * (float)Math.Cos(waveFrequency * timerTick++) * waveFrequency;

      npc.velocity.X += xTo * travelSpeed - yTo * wobble;
      npc.velocity.Y += -yTo * travelSpeed + xTo * wobble;

      PerformAttack();
    }

    public override string Texture => "ChensGradiusMod/Sprites/Garun";

    public override void SendExtraAI(BinaryWriter writer)
    {
      writer.Write(persistDirection);
    }

    public override void ReceiveExtraAI(BinaryReader reader)
    {
      persistDirection = reader.ReadInt32();
    }

    protected override int FrameSpeed { get; set; } = 4;

    protected override Types EnemyType => Types.Small;

    private double GetDirection()
    {
      float degreeAngle;

      if (npc.direction > 0) degreeAngle = 180f;
      else degreeAngle = 0f;

      return MathHelper.ToRadians(degreeAngle);
    }

    private void PerformAttack()
    {
      if (GradiusHelper.IsNotMultiplayerClient() &&
          Vector2.Distance(npc.Center, Main.player[npc.target].Center) <= attackDistance)
      {
        if ((npc.direction >= 0 && npc.Center.X <= Main.player[npc.target].Center.X) ||
           (npc.direction <= 0 && npc.Center.X >= Main.player[npc.target].Center.X))
        {
          if (++fireTick >= fireRate)
          {
            fireTick = 0;
            Vector2 vel = GradiusHelper.MoveToward(npc.Center, Main.player[npc.target].Center, GradiusEnemyBullet.Spd);
            Projectile.NewProjectile(npc.Center, vel, ModContent.ProjectileType<GradiusEnemyBullet>(),
                                     GradiusEnemyBullet.Dmg, GradiusEnemyBullet.Kb, Main.myPlayer);
          }
        }
        else fireTick = 0;
      }
    }
  }
}
