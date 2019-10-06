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
    public const float FullAngle = 360f;

    public static void FreeListData(List<int> list, int buffer)
    {
      if (list.Count > buffer)
      {
        int bufferExcess = list.Count - buffer;
        for (int j = 0; j < bufferExcess; j++)
        {
          Main.projectile[list[0]].Kill();
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

    public static Vector2 MoveToward(Vector2 origin, Vector2 destination, float speed)
    {
      float hypotenuse = Vector2.Distance(origin, destination);
      float opposite = destination.Y - origin.Y;
      float adjacent = destination.X - origin.X;

      float direction = (float)Math.Acos(Math.Abs(adjacent) / hypotenuse);
      // float direction = (float)Math.Asin(Math.Abs(opposite) / hypotenuse);
      // float direction = (float)Math.Atan(Math.Abs(opposite) / adjacent);

      float newX = (float)Math.Cos(direction) * Math.Sign(adjacent) * speed;
      float newY = -(float)Math.Sin(direction) * -Math.Sign(opposite) * speed;

      return new Vector2(newX, newY);
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

    public static void NormalizeAngleDegrees(ref float angleDegrees)
    {
      if (angleDegrees >= FullAngle) angleDegrees -= FullAngle;
      else if (angleDegrees < 0) angleDegrees += FullAngle;
    }

    public static bool IsSameClientOwner(Projectile proj) => Main.myPlayer == proj.owner;

    public static bool IsSameClientOwner(Player player) => Main.myPlayer == player.whoAmI;

    public static bool IsNotMultiplayerClient() => Main.netMode != NetmodeID.MultiplayerClient;

    public static bool IsBydoAccessory(ModItem modItem)
    {
      return modItem is BydoEmbryo ||
             modItem is NeedleBydo;
    }

    public static int RoundOffToWhole(float num)
    {
      string numStr = num.ToString();
      int decimalLength = numStr.Substring(numStr.IndexOf(".") + 1).Length;
      return (int)Math.Round(num, decimalLength, MidpointRounding.AwayFromZero);
    }

    public static void FlipAngleDirection(ref float angleDegrees, int direction)
    {
      if (direction < 0)
      {
        angleDegrees += 180f;
        NormalizeAngleDegrees(ref angleDegrees);
      }
    }
  }
}
