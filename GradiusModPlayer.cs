using ChensGradiusMod.Items;
using ChensGradiusMod.Items.Accessories.Options.Charge;
using ChensGradiusMod.Items.Accessories.Options.Recurve;
using ChensGradiusMod.Items.Accessories.Options.Rotate;
using ChensGradiusMod.Projectiles.Forces;
using ChensGradiusMod.Projectiles.Options;
using ChensGradiusMod.Projectiles.Options.Miscellaneous;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod
{
  public class GradiusModPlayer : ModPlayer
  {
    private const int MaxFlightPathCount = 60;
    private const float EquiAngle = FullAngle / MaxFlightPathCount;

    private Vector2 baitPoint;
    private int baitDirection;
    private float baitAngle;

    public int rotateMode;
    public sbyte revolveDirection;
    public bool wasHolding;
    public bool isFreezing;
    public bool isAiming;
    public bool isSearching;
    public bool forceBase;
    public bool needleForce;
    public Projectile forceProjectile;
    public bool optionOne;
    public bool optionTwo;
    public bool optionThree;
    public bool optionFour;
    public bool normalOption;
    public bool freezeOption;
    public bool aimOption;
    public bool rotateOption;
    public bool searchOption;
    public bool chargeMultiple;
    public int chargeMode;
    public bool optionSeed;
    public Projectile seedProjectile;
    public sbyte seedRotateDirection;
    public bool recurveOption;
    public bool recurveSide;
    public bool recurveActionMode;
    public bool isRecurving;
    public float recurveDistance;
    public bool spreadOption;
    public bool isSpreading;
    public Item[] optionRuleAmmoFilter = new Item[2];

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
      optionSeed = false;
      OptionFlagReset();
      optionRuleAmmoFilter[0] = new Item();
      optionRuleAmmoFilter[1] = new Item();
    }

    public override void UpdateDead()
    {
      ResetEffects();
      ResetOptionVariables();
      ResetOtherVariables();
    }

    public override void clientClone(ModPlayer clientClone)
    {
      GradiusModPlayer clone = clientClone as GradiusModPlayer;
      clone.isFreezing = isFreezing;
      clone.rotateMode = rotateMode;
      clone.seedRotateDirection = seedRotateDirection;
      clone.chargeMode = chargeMode;
      clone.isSearching = isSearching;
    }

    public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
    {
      ModPacket packet = mod.GetPacket();
      packet.Write((byte)ChensGradiusMod.PacketMessageType.GradiusModSyncPlayer);
      packet.Write((byte)player.whoAmI);
      packet.Write(isFreezing);
      packet.Write(rotateMode);
      packet.Write(revolveDirection);
      packet.Write(wasHolding);
      packet.Write(forceBase);
      packet.Write(needleForce);
      packet.Write(optionOne);
      packet.Write(optionTwo);
      packet.Write(optionThree);
      packet.Write(optionFour);
      packet.Write(normalOption);
      packet.Write(freezeOption);
      packet.Write(rotateOption);
      packet.Write(optionSeed);
      packet.Write(seedRotateDirection);
      packet.Write(chargeMultiple);
      packet.Write(chargeMode);
      packet.Write(aimOption);
      packet.Write(searchOption);
      packet.Write(isSearching);
      packet.Write(recurveOption);
      packet.Write(spreadOption);
      packet.Send(toWho, fromWho);
    }

    public override void SendClientChanges(ModPlayer clientPlayer)
    {
      if (clientPlayer is GradiusModPlayer clone)
      {
        ModPacket packet;

        if (clone.isFreezing != isFreezing)
        {
          packet = mod.GetPacket();
          packet.Write((byte)ChensGradiusMod.PacketMessageType.ClientChangesFreezeOption);
          packet.Write((byte)player.whoAmI);
          packet.Write(isFreezing);
          packet.Write(wasHolding);
          packet.Send();
        }

        if (clone.rotateMode != rotateMode)
        {
          packet = mod.GetPacket();
          packet.Write((byte)ChensGradiusMod.PacketMessageType.ClientChangesRotateOption);
          packet.Write((byte)player.whoAmI);
          packet.Write(rotateMode);
          packet.Write(revolveDirection);
          packet.Write(wasHolding);
          packet.Send();
        }

        if (clone.seedRotateDirection != seedRotateDirection)
        {
          packet = mod.GetPacket();
          packet.Write((byte)ChensGradiusMod.PacketMessageType.ClientChangesSeedDirection);
          packet.Write((byte)player.whoAmI);
          packet.Write(seedRotateDirection);
          packet.Send();
        }

        if (clone.chargeMode != chargeMode)
        {
          packet = mod.GetPacket();
          packet.Write((byte)ChensGradiusMod.PacketMessageType.ClientChangesChargeMultiple);
          packet.Write((byte)player.whoAmI);
          packet.Write(chargeMode);
          packet.Write(wasHolding);
          packet.Send();
        }

        if (clone.isSearching != isSearching)
        {
          packet = mod.GetPacket();
          packet.Write((byte)ChensGradiusMod.PacketMessageType.ClientChangesSearchOption);
          packet.Write((byte)player.whoAmI);
          packet.Write(isSearching);
          packet.Write(wasHolding);
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
        if (ChensGradiusMod.optionActionKey.JustPressed) wasHolding = isFreezing = true;
        if (ChensGradiusMod.optionActionKey.JustReleased && wasHolding) wasHolding = isFreezing = false;
      }
      else if (aimOption)
      {
        if (ChensGradiusMod.optionActionKey.JustPressed) wasHolding = isAiming = true;
        if (ChensGradiusMod.optionActionKey.JustReleased && wasHolding) wasHolding = isAiming = false;
      }
      else if (rotateOption)
      {
        if (ChensGradiusMod.optionActionKey.JustPressed)
        {
          wasHolding = true;
          rotateMode = (int)RotateOptionBase.States.Grouping;
        }
        if (ChensGradiusMod.optionActionKey.JustReleased && wasHolding)
        {
          wasHolding = false;
          rotateMode = (int)RotateOptionBase.States.Recovering;
          revolveDirection = (sbyte)-revolveDirection;
        }
      }
      else if (searchOption)
      {
        if (ChensGradiusMod.optionActionKey.JustPressed) wasHolding = isSearching = true;
        if (ChensGradiusMod.optionActionKey.JustReleased && wasHolding) wasHolding = isSearching = false;
      }
      else if (chargeMultiple && HasAnyChargeMultipleAccessory())
      {
        if (ChensGradiusMod.optionActionKey.JustPressed)
        {
          wasHolding = true;
          chargeMode = (int)ChargeMultipleBase.States.Charging;
        }
        if (ChensGradiusMod.optionActionKey.JustReleased && wasHolding)
        {
          wasHolding = false;
          chargeMode = (int)ChargeMultipleBase.States.Dying;
        }
      }
      else if (recurveOption)
      {
        if (ChensGradiusMod.optionActionKey.JustPressed) wasHolding = isRecurving = true;
        if (ChensGradiusMod.optionActionKey.JustReleased && wasHolding)
        {
          wasHolding = isRecurving = false;
          recurveActionMode = !recurveActionMode;
        }
      }
      else if (spreadOption)
      {
        if (ChensGradiusMod.optionActionKey.JustPressed) wasHolding = isSpreading = true;
        if (ChensGradiusMod.optionActionKey.JustReleased && wasHolding) wasHolding = isSpreading = false;
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
          if (!recurveOption && (!IsEqualWithThreshold(optionFlightPath[0], player.Center, .01f)
                                 || isRotateButNotFollowing))
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
          else if (recurveOption) RecurveBehavior();
        }
        else optionFlightPath.Insert(0, player.Center);
      }
      else ResetOptionVariables();
    }

    public override void PostUpdate()
    {
      if (HasAnyOptions()) FreeListData(ref optionAlreadyProducedProjectiles, OptionBaseObject.MaxBuffer);
      if (GradiusGlobalItem.meleeHitbox[player.whoAmI].HasValue) GradiusGlobalItem.meleeHitbox[player.whoAmI] = null;
    }

    public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
    {
      MakeForceBattle();
      MakeOptionSeedBattle();
    }

    public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
    {
      MakeForceBattle();
      MakeOptionSeedBattle();
    }

    public override void OnConsumeAmmo(Item weapon, Item ammo)
    {
      optionRuleAmmoFilter[0] = weapon;
      optionRuleAmmoFilter[1] = ammo;
    }

    public void PopulateOptionFlight()
    {
      while (optionFlightPath.Count < MaxFlightPathCount)
      {
        optionFlightPath.Add(player.Center);
      }
    }

    private void ResetOptionVariables()
    {
      optionFlightPath.Clear();
      optionFlightPath = new List<Vector2>();
      optionAlreadyProducedProjectiles.Clear();
      optionAlreadyProducedProjectiles = new List<int>();
      isFreezing = false;
      isAiming = false;
      rotateMode = (int)RotateOptionBase.States.Following;
      baitDirection = 0;
      revolveDirection = 1;
      wasHolding = false;
      chargeMode = (int)ChargeMultipleBase.States.Following;
      isSearching = false;
      recurveActionMode = false;
      recurveDistance = 96f;
      isSpreading = false;
    }

    private void ResetOtherVariables()
    {
      seedRotateDirection = 0;
      recurveSide = false;
    }

    private void OptionFlagReset()
    {
      normalOption = false;
      rotateOption = false;
      freezeOption = false;
      aimOption = false;
      chargeMultiple = false;
      recurveOption = false;
      searchOption = false;
      spreadOption = false;
    }

    private bool HasAnyOptions()
    {
      return optionOne ||
             (optionTwo && OptionsPredecessorRequirement(this, 2)) ||
             (optionThree && OptionsPredecessorRequirement(this, 3)) ||
             (optionFour && OptionsPredecessorRequirement(this, 4));
    }

    private bool HasAnyChargeMultipleAccessory()
    {
      return FindEquippedAccessory(player, ModContent.ItemType<ChargeMultipleOne>()) != null;
    }

    private void MakeForceBattle()
    {
      if (HasAForce() && IsSameClientOwner(forceProjectile) &&
          forceProjectile.modProjectile is ForceBase fbProj)
      {
        fbProj.BattleMode();
      }
    }

    private void MakeOptionSeedBattle()
    {
      if (optionSeed && IsSameClientOwner(seedProjectile) &&
          seedProjectile.modProjectile is OptionSeedObject opSeed)
      {
        opSeed.BattleMode();
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

      NormalizeAngleDegrees(ref baitAngle);
      baitAngle += EquiAngle * revolveDirection;

      baitPoint.X = player.Center.X + ((float)Math.Cos(MathHelper.ToRadians(baitAngle)) * RotateOptionBase.Radius);
      baitPoint.Y = player.Center.Y - ((float)Math.Sin(MathHelper.ToRadians(baitAngle)) * RotateOptionBase.Radius);

      optionFlightPath.Insert(0, baitPoint);
    }

    private void RotateBehaviorRecovering()
    {
      float recoverSpeed = Math.Min(RotateOptionBase.Speed, Vector2.Distance(player.Center, baitPoint));
      baitPoint += MoveToward(baitPoint, player.Center, recoverSpeed);
      optionFlightPath.Insert(0, baitPoint);

      if (IsEqualWithThreshold(baitPoint, player.Center, RotateOptionBase.AcceptedThreshold))
      {
        rotateMode = (int)RotateOptionBase.States.Following;
      }
    }

    private void RecurveBehavior()
    {
      PopulateOptionFlight();

      if (IsSameClientOwner(player))
      {
        if (isRecurving)
        {
          recurveDistance += RecurveOptionBase.AdjustSpeed * recurveActionMode.ToDirectionInt();
          recurveDistance = Math.Max(recurveDistance, RecurveOptionBase.LeastAdjustment);
          recurveDistance = Math.Min(recurveDistance, RecurveOptionBase.CapAdjustment);
        }

        int distance = OptionBaseObject.DistanceInterval;
        double direction = GetDirection(Main.MouseWorld) - MathHelper.Pi;
        float offsetY = recurveDistance * recurveSide.ToDirectionInt();
        Vector2 oldPath = optionFlightPath[distance - 1];

        optionFlightPath[1 * distance - 1] = player.Center + new Vector2
        {
          X = (float)Math.Cos(direction) * RecurveOptionBase.FixedAxisDistance +
              (float)Math.Cos(direction + MathHelper.PiOver2) * offsetY,
          Y = (float)Math.Sin(direction) * RecurveOptionBase.FixedAxisDistance +
              (float)Math.Sin(direction + MathHelper.PiOver2) * offsetY
        };
        optionFlightPath[2 * distance - 1] = player.Center + new Vector2
        {
          X = (float)Math.Cos(direction) * RecurveOptionBase.FixedAxisDistance +
              (float)Math.Cos(direction + MathHelper.PiOver2) * -offsetY,
          Y = (float)Math.Sin(direction) * RecurveOptionBase.FixedAxisDistance +
              (float)Math.Sin(direction + MathHelper.PiOver2) * -offsetY
        };
        optionFlightPath[3 * distance - 1] = player.Center + new Vector2
        {
          X = (float)Math.Cos(direction) * RecurveOptionBase.FixedAxisDistance * 2 +
              (float)Math.Cos(direction + MathHelper.PiOver2) * offsetY * 2,
          Y = (float)Math.Sin(direction) * RecurveOptionBase.FixedAxisDistance * 2 +
              (float)Math.Sin(direction + MathHelper.PiOver2) * offsetY * 2
        };
        optionFlightPath[4 * distance - 1] = player.Center + new Vector2
        {
          X = (float)Math.Cos(direction) * RecurveOptionBase.FixedAxisDistance * 2 +
              (float)Math.Cos(direction + MathHelper.PiOver2) * -offsetY * 2,
          Y = (float)Math.Sin(direction) * RecurveOptionBase.FixedAxisDistance * 2 +
              (float)Math.Sin(direction + MathHelper.PiOver2) * -offsetY * 2
        };

        if (IsNotSinglePlayer() &&
            !IsEqualWithThreshold(optionFlightPath[distance - 1], oldPath, .05f))
        {
          ModPacket packet = mod.GetPacket();
          packet.Write((byte)ChensGradiusMod.PacketMessageType.RecurveUpdatePositions);
          packet.Write((byte)player.whoAmI);
          packet.WriteVector2(optionFlightPath[OptionBaseObject.DistanceInterval - 1]);
          packet.WriteVector2(optionFlightPath[OptionBaseObject.DistanceInterval * 2 - 1]);
          packet.WriteVector2(optionFlightPath[OptionBaseObject.DistanceInterval * 3 - 1]);
          packet.WriteVector2(optionFlightPath[OptionBaseObject.DistanceInterval * 4 - 1]);
          packet.Send();
        }
      }
    }

    private double GetDirection(Vector2 secondPoint) => (secondPoint - player.Center).ToRotation();
  }
}