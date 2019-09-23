using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;

namespace ChensGradiusMod
{
  public static class GradiusHelper
  {
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
      if (pos == 1)      return true;
      else if (pos == 2) return gp.optionOne;
      else if (pos == 3) return gp.optionOne && gp.optionTwo;
      else if (pos == 4) return gp.optionOne && gp.optionTwo && gp.optionThree;
      else               return false;
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
  }
}
