using ChensGradiusMod.Projectiles.Options.Miscellaneous;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.Items.Accessories.Options.Miscellaneous
{
    public class OptionSeed : ParentGradiusAccessory
    {
        private const string ProjectileName = "OptionSeedObject";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Option Seed");
            Tooltip.SetDefault("Deploys an Option Seed.\n" +
                               "An incomplete, but surprisingly working, Option drone.\n" +
                               "The drone will shoot bullets or arrows based on the ammo slots.\n" +
                               "This incomplete drone can be upgraded into an Option,\n" +
                               "allowing it to be at its full potential.");
            ItemID.Sets.ItemNoGravity[item.type] = true;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            item.width = 20;
            item.height = 26;
            item.rare = ItemRarityID.Blue; // 1
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ModPlayer(player).optionSeed = true;

            if (IsSameClientOwner(player))
            {
                if (player.ownedProjectileCounts[mod.ProjectileType(ProjectileName)] <= 0)
                {
                    int pInd = Projectile.NewProjectile(player.Center.X + OptionSeedObject.SeedDistance * player.direction,
                                                        player.Center.Y, 0f, 0f, mod.ProjectileType(ProjectileName), 0, 0f,
                                                        player.whoAmI, 0f, 0f);
                    ModOwner(player).seedProjectile = Main.projectile[pInd];
                }

                ModOwner(player).seedRotateDirection = (sbyte)-hideVisual.ToDirectionInt();
            }
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(item.Center, .5f, .249f, 0f);
        }

        public override string Texture => "ChensGradiusMod/Sprites/SeedInv";

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("Wood", 400);
            recipe.AddRecipeGroup("IronBar", 10);
            recipe.AddIngredient(ItemID.Daybloom, 1);
            recipe.AddIngredient(ItemID.Moonglow, 1);
            recipe.AddIngredient(ItemID.Deathweed, 1);
            recipe.AddIngredient(ItemID.Mushroom, 1);
            recipe.AddRecipeGroup("ChensGradiusMod:EvilMushroom", 1);
            recipe.AddRecipeGroup("PresurePlate", 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        private GradiusModPlayer ModOwner(Player p) => p.GetModPlayer<GradiusModPlayer>();
    }
}