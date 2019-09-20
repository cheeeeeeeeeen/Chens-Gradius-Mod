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
  }
}
