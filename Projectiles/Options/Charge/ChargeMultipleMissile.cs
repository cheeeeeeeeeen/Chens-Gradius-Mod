using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.Projectiles.Options.Charge
{
  public class ChargeMultipleMissile : ModProjectile
  {
    public const float Spd = 18f;
    public Item clonedAccessory = null;

    private readonly int[] dustIds = new int[2] { 159, 55 };
    private readonly int trailType = ModContent.ProjectileType<ChargeMultipleTrail>();
    private readonly float detectionRange = 600f;
    private readonly float angleSpeed = 20f;
    private readonly float angleVariation = 5f;
    private readonly int delayToRotate = 10;
    private float oldCurrentAngle = 0f;
    private float currentAngle = 0f;
    private int oldEnemyTarget = -1;
    private int enemyTarget = -1;
    private bool initialized = false;
    private int delayTick = 0;

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Charged Multiple");
      Main.projFrames[projectile.type] = 1;
    }

    public override void SetDefaults()
    {
      projectile.netImportant = true;
      projectile.width = 32;
      projectile.height = 32;
      projectile.light = 1f;
      projectile.friendly = true;
      projectile.hostile = false;
      projectile.tileCollide = false;
      projectile.penetrate = -1;
      projectile.usesLocalNPCImmunity = true;
      projectile.localNPCHitCooldown = 10;
    }

    public override string Texture => "ChensGradiusMod/Sprites/ChargeMultipleMissile";

    public override bool PreAI()
    {
      if (!initialized)
      {
        initialized = true;
        oldCurrentAngle = currentAngle = projectile.ai[0];
        projectile.timeLeft = GradiusHelper.RoundOffToWhole(projectile.ai[1]);
      }

      return initialized;
    }

    public override void AI()
    {
      CreateTrail();
      ProjectileAnimate();
      SeekEnemy();
      RotateTowardsTarget();
      SpecialEffects();
    }

    public override void Kill(int timeLeft)
    {
      if (GradiusHelper.IsSameClientOwner(projectile))
      {
        GradiusHelper.SpawnItem(clonedAccessory, projectile.Center, Vector2.Zero);
      }
    }

    public override void SendExtraAI(BinaryWriter writer)
    {
      writer.Write(enemyTarget);
      writer.Write(currentAngle);
    }

    public override void ReceiveExtraAI(BinaryReader reader)
    {
      enemyTarget = reader.ReadInt32();
      currentAngle = reader.ReadSingle();
    }

    private void CreateTrail()
    {
      if (GradiusHelper.IsSameClientOwner(projectile))
      {
        Projectile.NewProjectile(projectile.Center, Vector2.Zero, trailType,
                                 projectile.damage, projectile.knockBack, projectile.owner);
      }
    }

    private void ProjectileAnimate()
    {
      if (++projectile.frameCounter >= 5)
      {
        projectile.frameCounter = 0;
        if (++projectile.frame >= Main.projFrames[projectile.type]) projectile.frame = 0;
      }
    }

    private void SeekEnemy()
    {
      Player owner = Main.player[projectile.owner];
      float shortestDistance = detectionRange;
      enemyTarget = -1;

      for (int i = 0; i < Main.maxNPCs; i++)
      {
        NPC selectNpc = Main.npc[i];
        float distance = Vector2.Distance(projectile.Center, selectNpc.Center);
        float enemyDistance = Vector2.Distance(owner.Center, selectNpc.Center);

        if (enemyDistance <= detectionRange && distance < shortestDistance &&
            !selectNpc.friendly && selectNpc.active)
        {
          shortestDistance = distance;
          enemyTarget = i;
        }
      }

      if (oldEnemyTarget != enemyTarget)
      {
        oldEnemyTarget = enemyTarget;
        projectile.netUpdate = true;
      }
    }

    private void RotateTowardsTarget()
    {
      if (++delayTick >= delayToRotate)
      {
        float targetAngle;
        Vector2 targetVector;
        if (enemyTarget >= 0)
        {
          NPC npcTarget = Main.npc[enemyTarget];
          targetVector = npcTarget.Center;
        }
        else
        {
          Player playerTarget = Main.player[projectile.owner];
          targetVector = playerTarget.Center;
        }

        targetAngle = GradiusHelper.GetBearing(projectile.Center, targetVector);
        if (GradiusHelper.IsSameClientOwner(projectile))
        {
          float chosenVariant = Main.rand.NextFloat(angleVariation);
          currentAngle = GradiusHelper.AngularRotate(currentAngle, targetAngle, GradiusHelper.MinRotate,
                                                     GradiusHelper.MaxRotate, angleSpeed - chosenVariant);
          if (oldCurrentAngle != currentAngle)
          {
            oldCurrentAngle = currentAngle;
            projectile.netUpdate = true;
          }
        }
        projectile.rotation = MathHelper.ToRadians(currentAngle);

        projectile.velocity = Spd * new Vector2
        {
          X = (float)Math.Cos(MathHelper.ToRadians(currentAngle)),
          Y = -(float)Math.Sin(MathHelper.ToRadians(currentAngle))
        };
      }
    }

    private void SpecialEffects()
    {
      for (int i = 0; i < dustIds.Length; i++)
      {
        Dust.NewDust(projectile.position, projectile.width, projectile.height, dustIds[i],
                     0, 0, 0, default, 2);
      }
    }
  }
}
