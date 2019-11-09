using ChensGradiusMod.Projectiles.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.NPCs
{
  [AutoloadBossHead]
  public class BigCoreCustom : GradiusEnemy
  {
    private const int FrameWidth = 192;
    private const int FrameHeight = 130;

    private readonly int openCoreTime = 1200;
    private readonly int maxFrameIndex = 4;
    private readonly int fireRate = 25;
    private bool openCore = false;
    private States mode = States.RegularAssault;
    private int fireTick = 0;
    private int existenceTick = 0;
    private int frameCounterX = 7;

    private readonly float regularAssaultMaxSpeed = 30f;
    private readonly float regularAssaultAcceleration = 1f;
    private readonly float regularAssaultHorizontalGap = 600f;
    private float regularAssaultXCurrentSpeed = 0f;
    private float regularAssaultYCurrentSpeed = 0f;
    private int regularAssaultDirection = 0;

    private readonly Vector2 exitVelocity = new Vector2(.1f, 0);

    public enum States { RegularAssault, Exit };

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Big Core Custom");
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      npc.width = 188;
      npc.height = 124;
      npc.damage = 200;
      npc.lifeMax = 1500;
      npc.value = 10000f;
      npc.knockBackResist = 0f;
      npc.defense = 90;
      npc.noGravity = true;
      npc.noTileCollide = true;
      npc.npcSlots = 1;
      npc.boss = true;
      music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/AircraftCarrier");

      ImmuneToBuffs();
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
      if (Main.hardMode && NPC.downedGolemBoss && spawnInfo.spawnTileY < GradiusHelper.SkyTilesYLocation) return .04f;
      else return 0f;
    }

    public override string Texture => "ChensGradiusMod/Sprites/BigCoreCustom";

    public override string BossHeadTexture => "ChensGradiusMod/Sprites/BigCoreCustomHead";

    public override void AI()
    {
      CoreManagement();
      BarrierStatus();

      switch (mode)
      {
        case States.RegularAssault:
          existenceTick++;
          RegularAssaultBehavior();
          break;
        case States.Exit:
          ExitBehavior();
          break;
      }

      PerformAttack();
    }

    public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
    {
      SpriteEffects spriteEffects = SpriteEffects.None;
      if (npc.spriteDirection > 0) spriteEffects = SpriteEffects.FlipHorizontally;
      Vector2 drawPos = npc.TopLeft - Main.screenPosition;
      Rectangle frame = new Rectangle(frameCounterX * FrameWidth, FrameCounter * FrameHeight,
                                      FrameWidth, FrameHeight);
      Vector2 drawCenter = new Vector2(2f, 2f);

      spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, frame, drawColor,
                       0f, drawCenter, 1f, spriteEffects, 0f);

      return false;
    }

    public override void FindFrame(int frameHeight) { }

    public override bool? CanBeHitByItem(Player player, Item item)
    {
      if (openCore) return null;
      else return false;
    }

    public override bool? CanBeHitByProjectile(Projectile projectile)
    {
      if (openCore) return null;
      else return false;
    }

    public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
      => ReduceDamage(ref damage, ref knockback, ref crit);

    public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
      => ReduceDamage(ref damage, ref knockback, ref crit);

    public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
    {
      scale = 1.5f;
      return null;
    }

    public override void BossLoot(ref string name, ref int potionType)
    {
      potionType = ItemID.GreaterHealingPotion;
    }

    public override bool CheckDead()
    {
      // GradiusHelper.AchievementLibUnlock("From Myth To Legend");
      return base.CheckDead();
    }

    public override void SendExtraAI(BinaryWriter writer)
    {
      writer.Write((byte)mode);
      writer.Write(npc.target);
      writer.Write(regularAssaultDirection);
      writer.Write(regularAssaultYCurrentSpeed);
    }

    public override void ReceiveExtraAI(BinaryReader reader)
    {
      mode = (States)reader.ReadByte();
      npc.target = reader.ReadInt32();
      regularAssaultDirection = reader.ReadInt32();
      regularAssaultYCurrentSpeed = reader.ReadSingle();
    }

    protected override Types EnemyType => Types.Boss;

    protected override int FrameSpeed { get; set; } = 30;

    protected override float RetaliationBulletSpeed => 12;

    protected override int RetaliationExplodeBulletLayers => 5;

    protected override int RetaliationExplodeBulletNumberPerLayer => 40;

    private void RegularAssaultBehavior()
    {
      Player target = Main.player[npc.target];

      if (regularAssaultDirection == 0 || target.dead || !target.active)
      {
        npc.TargetClosest(false);
        target = Main.player[npc.target];
        if (GradiusHelper.IsNotMultiplayerClient()) npc.netUpdate = true;

        if (target.dead || !target.active)
        {
          mode = States.Exit;
          npc.timeLeft = 150;
          npc.velocity = Vector2.Zero;
          openCore = false;
          return;
        }
        else
        {
          if (target.Center.X > npc.Center.X) regularAssaultDirection = 1;
          else if (target.Center.X < npc.Center.X) regularAssaultDirection = -1;
          else if (GradiusHelper.IsNotMultiplayerClient())
          {
            regularAssaultDirection = Main.rand.NextBool().ToDirectionInt();
            npc.netUpdate = true;
          }
          else regularAssaultDirection = 0;

          npc.spriteDirection = npc.direction = regularAssaultDirection;
        }
      }

      if (target.Center.Y > npc.Center.Y)
      {
        regularAssaultYCurrentSpeed += regularAssaultAcceleration;
        regularAssaultYCurrentSpeed = Math.Min(regularAssaultYCurrentSpeed, regularAssaultMaxSpeed);
      }
      else if (target.Center.Y < npc.Center.Y)
      {
        regularAssaultYCurrentSpeed -= regularAssaultAcceleration;
        regularAssaultYCurrentSpeed = Math.Max(regularAssaultYCurrentSpeed, -regularAssaultMaxSpeed);
      }
      npc.position.Y += regularAssaultYCurrentSpeed;
      if (GradiusHelper.IsNotMultiplayerClient() &&
          GradiusHelper.IsEqualWithThreshold(regularAssaultYCurrentSpeed, 0f, 3f))
      {
        npc.netUpdate = true;
      }

      float destinationX = target.Center.X + regularAssaultHorizontalGap * -regularAssaultDirection;
      regularAssaultXCurrentSpeed += regularAssaultAcceleration;
      regularAssaultXCurrentSpeed = Math.Min(regularAssaultXCurrentSpeed, regularAssaultMaxSpeed);
      float newX = GradiusHelper.ApproachValue(npc.Center.X, destinationX, regularAssaultXCurrentSpeed);
      if (newX == destinationX) regularAssaultXCurrentSpeed = 0f;
      npc.Center = new Vector2(newX, npc.Center.Y);
    }

    private void PerformAttack()
    {
      if (GradiusHelper.IsNotMultiplayerClient() && mode != States.Exit)
      {
        if (++fireTick >= fireRate)
        {
          fireTick = 0;

          Vector2 pVel = new Vector2(1, 0) * CoreLaser.Spd * regularAssaultDirection;

          for (int i = 0; i < AttackVectors.Length; i++)
          {
            Projectile.NewProjectile(AttackVectors[i], pVel, ModContent.ProjectileType<CoreLaser>(),
                                     CoreLaser.Dmg, CoreLaser.Kb, Main.myPlayer);
          }

          Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Enemies/BigCoreShoot"),
                          npc.Center);
        }
      }
    }

    private Vector2[] AttackVectors
    {
      get
      {
        if (regularAssaultDirection > 0)
        {
          return new Vector2[]
          {
            npc.position + new Vector2(141, 16),
            npc.position + new Vector2(189, 44),
            npc.position + new Vector2(189, 80),
            npc.position + new Vector2(141, 108)
          };
        }
        else
        {
          return new Vector2[]
          {
            npc.position + new Vector2(45, 16),
            npc.position + new Vector2(-3, 44),
            npc.position + new Vector2(-3, 80),
            npc.position + new Vector2(45, 108)
          };
        }
      }
    }

    private void ExitBehavior()
    {
      if (FrameCounter <= 0) npc.velocity += exitVelocity * npc.spriteDirection;
    }

    private void CoreManagement()
    {
      if (mode == States.Exit && FrameCounter > 0)
      {
        if (++FrameTick >= FrameSpeed)
        {
          FrameTick = 0;
          FrameCounter--;
        }
      }
      else if (mode != States.Exit && !openCore && existenceTick >= openCoreTime)
      {
        if (++FrameTick >= FrameSpeed)
        {
          FrameTick = 0;
          if (++FrameCounter >= maxFrameIndex) openCore = true;
        }
      }
    }

    private void BarrierStatus()
    {
      if (npc.life >= GradiusHelper.RoundOffToWhole(npc.lifeMax * .875f)) frameCounterX = 7;
      else if (npc.life >= GradiusHelper.RoundOffToWhole(npc.lifeMax * .75f)) frameCounterX = 6;
      else if (npc.life >= GradiusHelper.RoundOffToWhole(npc.lifeMax * .625f)) frameCounterX = 5;
      else if (npc.life >= GradiusHelper.RoundOffToWhole(npc.lifeMax * .5f)) frameCounterX = 4;
      else if (npc.life >= GradiusHelper.RoundOffToWhole(npc.lifeMax * .375f)) frameCounterX = 3;
      else if (npc.life >= GradiusHelper.RoundOffToWhole(npc.lifeMax * .25f)) frameCounterX = 2;
      else if (npc.life >= GradiusHelper.RoundOffToWhole(npc.lifeMax * .125f)) frameCounterX = 1;
      else frameCounterX = 0;
    }
  }
}