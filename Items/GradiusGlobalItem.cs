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
      meleeHitbox[player.whoAmI] = hitbox;
    }

    public override void PostUpdate(Item item)
    {
      base.PostUpdate(item);
    }
  }
}