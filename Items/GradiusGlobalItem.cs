using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.Items
{
    public class GradiusGlobalItem : GlobalItem
    {
        public static Rectangle?[] meleeHitbox = new Rectangle?[Main.maxPlayers];

        public override void UseItemHitbox(Item item, Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            if (item.melee && CanDamage(item) && !item.noMelee)
            {
                meleeHitbox[player.whoAmI] = hitbox;
            }
        }
    }
}