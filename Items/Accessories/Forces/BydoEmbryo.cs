﻿using ChensGradiusMod.Projectiles.Forces;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.Items.Accessories.Forces
{
    public class BydoEmbryo : ParentGradiusAccessory
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Slightly increases life regeneration.\n" +
                               "Deploys the Force.\n" +
                               "Damage and Knockback are based on the held weapon.\n" +
                               "Any enemy projectile that comes in contact are destroyed.\n" +
                               "Press the Force Action hotkey to launch or pull it!\n" +
                               "You feel as if it is ominously loyal to you.");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            item.width = 24;
            item.height = 24;
            item.rare = ItemRarityID.Green; // 3
        }

        public override string Texture => "ChensGradiusMod/Sprites/ForceBaseAccessory";

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.lifeRegen += 1;
            ModPlayer(player).forceBase = true;
            DeployForce(player);
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(item.Center, 1.125f, .5602f, 0f);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player clientPlayer = Main.LocalPlayer;
            if (IsForceAlreadyDeployed(clientPlayer))
            {
                string tooltipText = $"{ModPlayer(clientPlayer).forceProjectile.damage} damage";
                TooltipLine newtip = new TooltipLine(mod, "ForceDamage", tooltipText);
                tooltips.Insert(1, newtip);

                tooltipText = KnockbackTooltip(ModPlayer(clientPlayer).forceProjectile.knockBack);
                newtip = new TooltipLine(mod, "ForceKnockback", tooltipText);
                tooltips.Insert(2, newtip);
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Gel, 200);
            recipe.AddRecipeGroup("Wood", 400);
            recipe.AddRecipeGroup("IronBar", 70);
            recipe.AddRecipeGroup("ChensGradiusMod:EvilStones", 100);
            recipe.AddRecipeGroup("ChensGradiusMod:EvilDrops", 40);
            recipe.AddIngredient(ItemID.Bunny);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        protected void DeployForce(Player player)
        {
            if (IsSameClientOwner(player) && !IsForceAlreadyDeployed(player))
            {
                float xSpawn;
                if (player.direction == 1) xSpawn = Main.screenPosition.X - 36;
                else xSpawn = Main.screenPosition.X + Main.screenWidth + 36;

                int pInd = Projectile.NewProjectile(xSpawn, player.Center.Y, 0f, 0f,
                                                    ThisProjectileType(),
                                                    ForceBase.Dmg, ForceBase.Kb, player.whoAmI);
                ModPlayer(player).forceProjectile = Main.projectile[pInd];
            }
        }

        protected virtual int ThisProjectileType() => ModContent.ProjectileType<ForceBase>();

        private bool IsForceAlreadyDeployed(Player player)
        {
            return ModPlayer(player).forceProjectile != null &&
                   ModPlayer(player).forceProjectile.active;
        }
    }
}