﻿using ChensGradiusMod.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.Items.Spawners
{
    public class BigBlueBizarreLens : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Big Blue Bizarre Lens");
            Tooltip.SetDefault(ItemTooltip);
            ItemID.Sets.SortingPriorityBossSpawns[item.type] = 0;
        }

        public override void SetDefaults()
        {
            item.maxStack = 20;
            item.value = Item.sellPrice(silver: 150);
            item.rare = ItemRarityID.Purple; // 11
            item.useAnimation = 30;
            item.useTime = 30;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.consumable = true;
            AssignItemDimensions(item, 56, 66, false);
        }

        public override string Texture => "ChensGradiusMod/Sprites/BlueCore";

        public override bool CanUseItem(Player player)
        {
            return CanSpawnAnother || !NPC.AnyNPCs(BossType);
        }

        public override bool UseItem(Player player)
        {
            SummonBoss(mod, player, BossType, 100);
            return true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        protected virtual string ItemTooltip =>
          "A strange-looking machinery...\n" +
          "You do not know anything about it, but you remember something like this.\n" +
          "Summons Big Core Custom";

        protected virtual bool CanSpawnAnother => true;

        protected virtual int BossType => ModContent.NPCType<BigCoreCustom>();

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LunarBar, 40);
            recipe.AddIngredient(ItemID.Lens, 5);
            recipe.AddRecipeGroup("ChensGradiusMod:EvilDrops", 5);
            recipe.AddRecipeGroup("IronBar", 10);
            recipe.AddIngredient(ItemID.Wire, 100);
            recipe.AddIngredient(ItemID.MechanicalWagonPiece);
            recipe.AddIngredient(ItemID.MechanicalBatteryPiece);
            recipe.AddIngredient(ItemID.MechanicalWheelPiece);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}