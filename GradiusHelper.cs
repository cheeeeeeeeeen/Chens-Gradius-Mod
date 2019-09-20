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
  }
}
