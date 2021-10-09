using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
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

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item?.modItem?.mod == ChensGradiusMod.gradiusMod)
            {
                TooltipLine line = tooltips.FirstOrDefault(tl => tl.Name == "ItemName" && tl.mod == "Terraria");
                if (line != null)
                {
                    switch (item.rare)
                    {
                        case (int)GradiusRarity.BigCore:
                            if (Main.GlobalTime % 1f < 0.5f) line.overrideColor = new Color?(Color.Lerp(bigCoreStripesColor, bigCoreColor, (Main.GlobalTime % 1f) / 0.5f));
                            else line.overrideColor = new Color?(Color.Lerp(bigCoreColor, bigCoreStripesColor, (Main.GlobalTime % 1f - 0.5f) / 0.5f));
                            break;
                    }
                }
            }
        }

        public enum GradiusRarity : int
        {
            BigCore = 12
        }
    }
}