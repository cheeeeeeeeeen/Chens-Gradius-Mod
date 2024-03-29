﻿using ChensGradiusMod.Projectiles.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
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
            item.melee = true;
            item.damage = 250;
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
            AssignItemDimensions(item, 30, 26, false);
        }

        public override string Texture => "ChensGradiusMod/Sprites/ZalkYoyoItem";

        public override int ChoosePrefix(UnifiedRandom rand)
        {
            int newPrefixIndex = rand.Next(0, universalPrefixes.Count);
            return universalPrefixes[newPrefixIndex];
        }
    }
}