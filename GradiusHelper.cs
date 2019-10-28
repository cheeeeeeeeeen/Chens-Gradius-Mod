using ChensGradiusMod.Items.Accessories.Forces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod
{
  public static class GradiusHelper
  {
    public const string InternalModName = "ChensGradiusMod";
    public const float FullAngle = 360f;
    public const float HalfAngle = 180f;
    public const float MinRotate = -180f;
    public const float MaxRotate = 180f;
    public const int LowerAmmoSlot = 54;
    public const int HigherAmmoSlot = 57;
    public const int LowerAccessorySlot = 3;
    public const int HigherAccessorySlot = 9;

    public static void FreeListData(ref List<int> list, int buffer)
    {
      if (list.Count > buffer)
      {
        int bufferExcess = list.Count - buffer;
        for (int j = 0; j < bufferExcess; j++)
        {
          Main.projectile[list[0]].active = false;
          list.RemoveAt(0);
        }
      }
    }

    public static bool OptionsPredecessorRequirement(GradiusModPlayer gp, int pos)
    {
      if (pos == 1) return true;
      else if (pos == 2) return gp.optionOne;
      else if (pos == 3) return gp.optionOne && gp.optionTwo;
      else if (pos == 4) return gp.optionOne && gp.optionTwo && gp.optionThree;
      else return false;
    }

    public static bool OptionOwnPositionCheck(GradiusModPlayer gp, int pos)
    {
      if (pos == 1) return gp.optionOne;
      else if (pos == 2) return gp.optionTwo;
      else if (pos == 3) return gp.optionThree;
      else if (pos == 4) return gp.optionFour;
      else return false;
    }

    public static bool OptionCheckSelfAndPredecessors(GradiusModPlayer gp, int pos)
    {
      return OptionsPredecessorRequirement(gp, pos) &&
             OptionOwnPositionCheck(gp, pos);
    }

    public static Vector2 MoveToward(Vector2 origin, Vector2 destination, float speed = 1)
    {
      float hypotenuse = Vector2.Distance(origin, destination);
      float opposite = destination.Y - origin.Y;
      float adjacent = destination.X - origin.X;

      float direction = (float)Math.Acos(Math.Abs(adjacent) / hypotenuse);

      float newX = (float)Math.Cos(direction) * Math.Sign(adjacent) * speed;
      float newY = -(float)Math.Sin(direction) * -Math.Sign(opposite) * speed;

      return new Vector2(newX, newY);
    }

    public static float GetBearing(Vector2 origin, Vector2 destination, bool reverse = true)
    {
      float hypotenuse = Vector2.Distance(origin, destination);
      float opposite = (destination.Y - origin.Y) * (reverse ? -1 : 1);
      float adjacent = destination.X - origin.X;

      float direction = (float)Math.Asin(Math.Abs(opposite) / hypotenuse);
      direction = MathHelper.ToDegrees(direction);

      if (adjacent < 0 && opposite < 0)
      {
        direction = HalfAngle + direction;
      }
      else if (adjacent < 0)
      {
        direction = HalfAngle - direction;
      }
      else if (opposite < 0)
      {
        direction = FullAngle - direction;
      }

      return direction;
    }

    public static bool CanDamage(Projectile proj) => proj.damage > 0;

    public static bool CanDamage(Item item) => item.damage > 0;

    public static string KnockbackTooltip(float knockback)
    {
      if (knockback <= 0f) return "No knockback";
      else if (knockback <= 1.5f) return "Extremely weak knockback";
      else if (knockback <= 3f) return "Very weak knockback";
      else if (knockback <= 4f) return "Weak knockback";
      else if (knockback <= 6f) return "Average knockback";
      else if (knockback <= 7f) return "Strong knockback";
      else if (knockback <= 11f) return "Extremely strong knockback";
      else return "Insane knockback";
    }

    public static bool IsEqualWithThreshold(Vector2 questionedPoint, Vector2 referencePoint, float threshold)
    {
      return questionedPoint.X <= (referencePoint.X + threshold) &&
             questionedPoint.Y <= (referencePoint.Y + threshold) &&
             questionedPoint.X >= (referencePoint.X - threshold) &&
             questionedPoint.Y >= (referencePoint.Y - threshold);
    }

    public static bool IsEqualWithThreshold(float questionedNumber, float referenceNumber, float threshold)
    {
      return questionedNumber <= (referenceNumber + threshold) &&
             questionedNumber >= (referenceNumber - threshold);
    }

    public static void NormalizeAngleDegrees(ref float angleDegrees)
    {
      if (angleDegrees >= FullAngle) angleDegrees -= FullAngle;
      else if (angleDegrees < 0) angleDegrees += FullAngle;
    }

    public static bool IsSameClientOwner(Projectile proj) => Main.myPlayer == proj.owner;

    public static bool IsSameClientOwner(Player player) => Main.myPlayer == player.whoAmI;

    public static bool IsSameClientOwner(int playerIndex) => Main.myPlayer == playerIndex;

    public static bool IsNotMultiplayerClient() => Main.netMode != NetmodeID.MultiplayerClient;

    public static bool IsBydoAccessory(ModItem modItem)
    {
      return modItem is BydoEmbryo ||
             modItem is NeedleBydo;
    }

    public static int RoundOffToWhole(float num) => (int)Math.Round(num, 0, MidpointRounding.AwayFromZero);

    public static void FlipAngleDirection(ref float angleDegrees, int direction)
    {
      if (direction < 0)
      {
        angleDegrees += 180f;
        NormalizeAngleDegrees(ref angleDegrees);
      }
    }

    public static int NewNPC(float X, float Y, int Type, int Start = 0, int ai0 = 0,
                             int ai1 = 0, int ai2 = 0, int ai3 = 0, int Target = 255)
    {
      int npcIndex = NPC.NewNPC((int)X, (int)Y, Type, Start, ai0, ai1, ai2, ai3, Target);
      Main.npc[npcIndex].Bottom = new Vector2(X, Y);
      return npcIndex;
    }

    public static int UnderworldTilesYLocation => Main.maxTilesY - 200;

    public static int SkyTilesYLocation => RoundOffToWhole((float)Main.worldSurface * .35f);

    public static float AngularCycle(float value, float min, float max)
    {
      float delta = max - min;
      float result = (value - min) % delta;

      if (result < 0) result += delta;

      return result + min;
    }

    public static float AngularRotate(float currentAngle, float targetAngle,
                                      float min, float max, float speed)
    {
      float diff = AngularCycle(targetAngle - currentAngle, min, max);

      if (diff < -speed) return currentAngle - speed;
      else if (diff > speed) return currentAngle + speed;
      else return targetAngle;
    }

    public static float ApproachValue(float currentValue, float targetValue, float speed)
    {
      if (currentValue < targetValue)
      {
        currentValue += speed;
        if (currentValue > targetValue)
          return targetValue;
      }
      else
      {
        currentValue -= speed;
        if (currentValue < targetValue)
          return targetValue;
      }

      return currentValue;
    }

    public static void ProjectileDestroy(Projectile proj)
    {
      try
      {
        proj.Kill();
      }
      catch
      {
        proj.active = false;
      }
    }

    public static bool AchievementLibUnlock(string achievement, Player player = null)
    {
      //Mod achievementLib = ModLoader.GetMod("AchievementLib");
      //if (achievementLib != null)
      //{
      //  if (player == null) achievementLib.Call("UnlockGLobal", InternalModName, achievement);
      //  else achievementLib.Call("UnlockLocal", InternalModName, achievement, player);

      //  return true;
      //}

      return false;
    }

    public static int? FindEquippedAccessory(Player player, int accType)
    {
      for (int i = LowerAccessorySlot; i <= HigherAccessorySlot; i++)
      {
        if (player.armor[i].type == accType) return i;
      }

      return null;
    }

    public static int SpawnItem(Item item, Vector2 position, Vector2 velocity)
    {
      int iInd = Item.NewItem(position, item.type);
      item.Center = position;
      item.velocity = velocity;
      Main.item[iInd] = item;
      Main.item[iInd].Center = position;
      Main.item[iInd].velocity = velocity;

      return iInd;
    }
  }
}
