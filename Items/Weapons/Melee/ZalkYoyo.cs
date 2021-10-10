using ChensGradiusMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;
using static ChensGradiusMod.Items.GradiusGlobalItem;

namespace ChensGradiusMod.Items.Weapons.Melee
{
    public class ZalkYoyo : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zelos Influence");
            Tooltip.SetDefault("Hitting enemies will spawn temporary friendly Zalks to offer offensive power\n\"While men exist, so will I.\"");
            ItemID.Sets.Yoyo[item.type] = true;
            ItemID.Sets.GamepadExtraRange[item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 26;
            item.melee = true;
            item.damage = 180;
            item.knockBack = 8f;
            item.useTime = 25;
            item.useAnimation = 20;
            item.autoReuse = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.UseSound = SoundID.Item1;
            item.channel = true;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.shoot = ModContent.ProjectileType<ZalkYoyoProjectile>();
            item.shootSpeed = 16f;
            item.rare = (int)GradiusRarity.BigCore;
            item.autoReuse = true;
            item.value = Item.sellPrice(0, 30, 0, 0);
        }

        public override string Texture => "ChensGradiusMod/Sprites/ZalkYoyoItem";

        public override bool AllowPrefix(int pre)
        {
            if (universalPrefixes.Contains(pre)) return true;
            else return false;
        }
    }
}