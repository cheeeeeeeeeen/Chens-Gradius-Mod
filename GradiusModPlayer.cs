using ChensGradiusMod.Items;
using ChensGradiusMod.Items.Accessories.Options.Rotate;
using ChensGradiusMod.Projectiles.Forces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace ChensGradiusMod
{
  public class GradiusModPlayer : ModPlayer
  {
    private const int MaxFlightPathCount = 60;
    private const int MaxProducedProjectileBuffer = 300;
    private const float EquiAngle = GradiusHelper.FullAngle / MaxFlightPathCount;

    private Vector2 baitPoint;
    private int baitDirection;
    private float baitAngle;

    public int rotateMode;
    public int revolveDirection;
    public bool isFreezing;
    public bool forceBase;
    public bool needleForce;
    public Projectile forceProjectile;
    public bool optionOne;
    public bool optionTwo;
    public bool optionThree;
    public bool optionFour;
    public bool normalOption;
    public bool freezeOption;
    public bool rotateOption;

    public List<Vector2> optionFlightPath = new List<Vector2>();
    public List<int> optionAlreadyProducedProjectiles = new List<int>();

    public GradiusModPlayer() => UpdateDead();

    public override void ResetEffects()
    {
      forceBase = false;
      needleForce = false;
      optionOne = false;
      optionTwo = false;
      optionThree = false;
      optionFour = false;
      normalOption = false;
      freezeOption = false;
      rotateOption = false;
    }

    public override void UpdateDead()
    {
      ResetEffects();
      ResetOptionVariables();
    }

    public override void clientClone(ModPlayer clientClone)
    {
      GradiusModPlayer clone = clientClone as GradiusModPlayer;
      clone.isFreezing = isFreezing;
      clone.rotateMode = rotateMode;
    }

    public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
    {
      ModPacket packet = mod.GetPacket();
      packet.Write((byte)ChensGradiusMod.PacketMessageType.GradiusModSyncPlayer);
      packet.Write((byte)player.whoAmI);
      packet.Write(isFreezing);
      packet.Write(rotateMode);
      packet.Write(revolveDirection);
      packet.Write(forceBase);
      packet.Write(needleForce);
      packet.Write(optionOne);
      packet.Write(optionTwo);
      packet.Write(optionThree);
      packet.Write(optionFour);
      packet.Write(normalOption);
      packet.Write(freezeOption);
      packet.Write(rotateOption);
      packet.Write(optionFlightPath.Count);
      for (int i = 0; i < optionFlightPath.Count; i++) packet.WriteVector2(optionFlightPath[i]);
      packet.Send(toWho, fromWho);
    }

    public override void SendClientChanges(ModPlayer clientPlayer)
    {
      if (clientPlayer is GradiusModPlayer clone)
      {
        ModPacket packet = mod.GetPacket();

        if (clone.isFreezing != isFreezing)
        {
          packet.Write((byte)ChensGradiusMod.PacketMessageType.ClientChangesFreezeOption);
          packet.Write((byte)player.whoAmI);
          packet.Write(isFreezing);
          packet.Send();
        }

        if (clone.rotateMode != rotateMode)
        {
          packet.Write((byte)ChensGradiusMod.PacketMessageType.ClientChangesRotateOption);
          packet.Write((byte)player.whoAmI);
          packet.Write(rotateMode);
          packet.Write(revolveDirection);
          packet.Send();
        }
      }
    }

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
      if (HasAForce() && ChensGradiusMod.forceActionKey.JustReleased &&
          forceProjectile.modProjectile is ForceBase fbProj)
      {
        switch (fbProj.mode)
        {
          case (int)ForceBase.States.Attached:
            fbProj.mode = (int)ForceBase.States.Launched;
            fbProj.SpecialDetachActions();
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Forces/ForceOut"),
                           forceProjectile.Center);
            break;
          case (int)ForceBase.States.Detached:
            forceProjectile.tileCollide = false;
            forceProjectile.velocity = new Vector2();
            fbProj.mode = (int)ForceBase.States.Pulled;
            break;
          case (int)ForceBase.States.Launched:
          case (int)ForceBase.States.Pulled:
            break;
        }
      }

      if (freezeOption)
      {
        if (ChensGradiusMod.optionActionKey.JustPressed) isFreezing = true;
        if (ChensGradiusMod.optionActionKey.JustReleased) isFreezing = false;
      }
      else if (rotateOption)
      {
        if (ChensGradiusMod.optionActionKey.JustPressed) rotateMode = (int)RotateOptionBase.States.Grouping;
        if (ChensGradiusMod.optionActionKey.JustReleased)
        {
          rotateMode = (int)RotateOptionBase.States.Recovering;
          revolveDirection = -revolveDirection;
        }
      }
    }

    public override void PreUpdate()
    {
      if (HasAnyOptions())
      {
        if (optionFlightPath.Count > 0)
        {
          for (int h = 0; h < optionAlreadyProducedProjectiles.Count; h++)
          {
            Projectile p = Main.projectile[optionAlreadyProducedProjectiles[h]];
            if (!p.active) optionAlreadyProducedProjectiles.RemoveAt(h--);
          }

          bool isRotateButNotFollowing = rotateMode != (int)RotateOptionBase.States.Following;
          if (!(optionFlightPath[0].X == player.Center.X && optionFlightPath[0].Y == player.Center.Y) ||
              isRotateButNotFollowing)
          {
            if (optionFlightPath.Count >= MaxFlightPathCount) optionFlightPath.RemoveAt(optionFlightPath.Count - 1);

            if (freezeOption && isFreezing) FreezeBehavior();
            else if (rotateOption && isRotateButNotFollowing)
            {
              switch (rotateMode)
              {
                case (int)RotateOptionBase.States.Grouping:
                  if (baitDirection == 0)
                  {
                    baitPoint = player.Center;
                    baitDirection = player.direction;
                    baitAngle = baitDirection > 0 ? 0 : 180;
                  }
                  RotateBehaviorGrouping();
                  break;
                case (int)RotateOptionBase.States.Rotating:
                  RotateBehaviorRevolving();
                  break;
                case (int)RotateOptionBase.States.Recovering:
                  RotateBehaviorRecovering();
                  break;
              }
            }
            else optionFlightPath.Insert(0, player.Center);
          }
        }
        else optionFlightPath.Insert(0, player.Center);
      }
      else ResetOptionVariables();
    }

    public override void PostUpdate()
    {
      if (HasAnyOptions()) GradiusHelper.FreeListData(ref optionAlreadyProducedProjectiles, MaxProducedProjectileBuffer);
      if (GradiusGlobalItem.meleeHitbox[player.whoAmI].HasValue) GradiusGlobalItem.meleeHitbox[player.whoAmI] = null;
    }

    public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
    {
      MakeForceBattle();
    }

    public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
    {
      MakeForceBattle();
    }

    private void ResetOptionVariables()
    {
      optionFlightPath.Clear();
      optionFlightPath = new List<Vector2>();
      optionAlreadyProducedProjectiles.Clear();
      optionAlreadyProducedProjectiles = new List<int>();
      isFreezing = false;
      rotateMode = (int)RotateOptionBase.States.Following;
      baitDirection = 0;
      revolveDirection = 1;
    }

    private bool HasAnyOptions()
    {
      return optionOne ||
             (optionTwo && GradiusHelper.OptionsPredecessorRequirement(this, 2)) ||
             (optionThree && GradiusHelper.OptionsPredecessorRequirement(this, 3)) ||
             (optionFour && GradiusHelper.OptionsPredecessorRequirement(this, 4));
    }

    private void MakeForceBattle()
    {
      if (HasAForce() && GradiusHelper.IsSameClientOwner(forceProjectile) &&
          forceProjectile.modProjectile is ForceBase fbProj)
      {
        fbProj.BattleMode();
      }
    }

    private bool HasAForce() => forceBase || needleForce;

    private void FreezeBehavior(bool rotateActually = false)
    {
      if (rotateActually) baitPoint += player.position - player.oldPosition;
      for (int p = 0; p < optionFlightPath.Count; p++)
      {
        optionFlightPath[p] += player.position - player.oldPosition;
      }
    }

    private void RotateBehaviorGrouping()
    {
      FreezeBehavior(rotateActually: true);

      Vector2 basisPoint = new Vector2(1, 0);
      Vector2 newPosition = basisPoint * baitDirection * RotateOptionBase.Speed;
      Vector2 limitPosition = basisPoint * baitDirection * RotateOptionBase.Radius;

      if (Vector2.Distance(player.Center, baitPoint + newPosition) <=
          Vector2.Distance(player.Center, player.Center + limitPosition))
      {
        baitPoint += newPosition;
      }
      else
      {
        baitPoint = player.Center + limitPosition;
        rotateMode = (int)RotateOptionBase.States.Rotating;
        baitDirection = 0;
      }

      optionFlightPath.Insert(0, baitPoint);
    }

    private void RotateBehaviorRevolving()
    {
      FreezeBehavior(rotateActually: true);

      GradiusHelper.NormalizeAngleDegrees(ref baitAngle);
      baitAngle += EquiAngle * revolveDirection;

      baitPoint.X = player.Center.X + ((float)Math.Cos(MathHelper.ToRadians(baitAngle)) * RotateOptionBase.Radius);
      baitPoint.Y = player.Center.Y - ((float)Math.Sin(MathHelper.ToRadians(baitAngle)) * RotateOptionBase.Radius);

      optionFlightPath.Insert(0, baitPoint);
    }

    private void RotateBehaviorRecovering()
    {
      if (!float.IsNaN(baitPoint.X) && !float.IsNaN(baitPoint.Y))
      {
        float recoverSpeed = Math.Min(RotateOptionBase.Speed, Vector2.Distance(player.Center, baitPoint));
        baitPoint += GradiusHelper.MoveToward(baitPoint, player.Center, recoverSpeed);
        optionFlightPath.Insert(0, baitPoint);

        if (GradiusHelper.IsEqualWithThreshold(baitPoint, player.Center, RotateOptionBase.AcceptedThreshold))
        {
          rotateMode = (int)RotateOptionBase.States.Following;
        }
      }
      else rotateMode = (int)RotateOptionBase.States.Following;
    }
  }
}