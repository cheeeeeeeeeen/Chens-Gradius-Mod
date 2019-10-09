using ChensGradiusMod.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items
{
  public class GradiusGlobalItem : GlobalItem
  {
    public static Rectangle?[] meleeHitbox = new Rectangle?[Main.maxPlayers];

    public override void UseItemHitbox(Item item, Player player, ref Rectangle hitbox, ref bool noHitbox)
    {
      bool moaiExists = false;

      for (int i = 0; i < Main.maxNPCs; i++)
      {
        NPC selectedNpc = Main.npc[i];
        if (selectedNpc.modNPC is Moai)
        {
          moaiExists = true;
          meleeHitbox[player.whoAmI] = hitbox;
          break;
        }
      }

      if (!moaiExists) meleeHitbox = new Rectangle?[Main.maxPlayers];
    }
  }
}