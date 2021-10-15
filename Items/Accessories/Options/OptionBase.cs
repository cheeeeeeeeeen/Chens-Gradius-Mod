using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.Items.Accessories.Options
{
    public abstract class OptionBase : ParentGradiusAccessory
    {
        private readonly int[] cloneProjectileCounts = new int[4] { 0, 0, 0, 0 };

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(OptionTooltip);
            ItemID.Sets.ItemNoGravity[item.type] = true;
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(10, 5));
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.value = ComputeItemValue(OptionPosition);
            AssignItemDimensions(item, 44, 52, true);
        }

        public override string Texture => $"ChensGradiusMod/Sprites/{ProjectileType}Inv{OptionPosition}";

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            StoreProjectileCounts(player);
            CreateOption(player, OptionPosition, ProjectileType + ProjectileName);
            CreationOrderingBypass(player, OptionPosition);
            ResetProjectileCounts(player);
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(item.Center, 1f, .498f, 0f);
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            return ModeChecks(ModPlayer(player)) &&
                   OptionsPredecessorRequirement(ModPlayer(player), OptionPosition);
        }

        protected virtual string ProjectileType => "";

        protected virtual string ProjectileName => "OptionObject";

        protected virtual string OptionName => "Option";

        protected virtual int OptionPosition => 0;

        protected virtual string OptionTooltip =>
          "Deploys an Option.\n" +
          "Some projectiles you create are copied by the drone.\n" +
          "The drone will follow your flight path.\n" +
          "This advanced drone uses Wreek technology,\n" +
          "infusing both technology and psychic elements together.";

        protected virtual void UpgradeUsualStations(ModRecipe recipe)
        {
        }

        protected virtual void UpgradeUsualRecipe(ModRecipe recipe)
        {
            switch (OptionPosition)
            {
                case 1:
                    recipe.AddIngredient(mod, "OptionOne");
                    recipe.AddIngredient(ItemID.Gel, 50);
                    recipe.AddIngredient(ItemID.Bone, 75);
                    recipe.AddRecipeGroup("ChensGradiusMod:GoldTierBar", 10);
                    recipe.AddIngredient(ItemID.Wire, 150);
                    goto case -1;
                case 2:
                    recipe.AddIngredient(mod, "OptionTwo");
                    recipe.AddIngredient(ItemID.SoulofLight, 5);
                    recipe.AddIngredient(ItemID.SoulofNight, 3);
                    recipe.AddRecipeGroup("ChensGradiusMod:CobaltTierBar", 10);
                    recipe.AddRecipeGroup("ChensGradiusMod:TinTierBar", 40);
                    recipe.AddIngredient(ItemID.Wire, 200);
                    goto case -1;
                case 3:
                    recipe.AddIngredient(mod, "OptionThree");
                    recipe.AddRecipeGroup("ChensGradiusMod:MechSoul", 8);
                    recipe.AddIngredient(ItemID.HallowedBar, 12);
                    recipe.AddRecipeGroup("ChensGradiusMod:SilverTierBar", 50);
                    recipe.AddIngredient(ItemID.Wire, 250);
                    goto case -1;
                case 4:
                    recipe.AddIngredient(mod, "OptionFour");
                    recipe.AddIngredient(ItemID.Ectoplasm, 15);
                    recipe.AddIngredient(ItemID.LunarTabletFragment, 15);
                    recipe.AddIngredient(ItemID.ChlorophyteBar, 20);
                    recipe.AddIngredient(ItemID.Wire, 300);
                    goto case -1;
                case -1:
                    break;
            }
        }

        protected virtual int ComputeItemValue(int multiplier)
        {
            int value = Item.buyPrice(gold: 5) * multiplier;
            if (ProjectileType != "") value += RoundOffToWhole(item.value * .25f);

            return value;
        }

        protected virtual bool ModeChecks(GradiusModPlayer gmPlayer, bool includeSelf = true)
        {
            bool result = true;
            if (includeSelf) result &= gmPlayer.normalOption;

            result &= !gmPlayer.aimOption
                   && !gmPlayer.recurveOption
                   && !gmPlayer.rotateOption
                   && !gmPlayer.freezeOption
                   && !gmPlayer.chargeMultiple
                   && !gmPlayer.searchOption;

            return result;
        }

        protected void CreateOption(Player player, int optionPosition, string projectileName)
        {
            if (OptionCheckSelfAndPredecessors(ModPlayer(player), optionPosition) &&
                ModeChecks(ModPlayer(player)) && IsOptionNotDeployed(player, projectileName))
            {
                Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f,
                                         mod.ProjectileType(projectileName), 0, 0f,
                                         player.whoAmI, 0f, 0f);
                player.ownedProjectileCounts[mod.ProjectileType(projectileName)]++;
            }
        }

        protected void CreationOrderingBypass(Player player, int position)
        {
            switch (position)
            {
                case 1:
                    CreateOption(player, 2, ProjectileType + OptionName + "TwoObject");
                    goto case 2;
                case 2:
                    CreateOption(player, 3, ProjectileType + OptionName + "ThreeObject");
                    goto case 3;
                case 3:
                    CreateOption(player, 4, ProjectileType + OptionName + "FourObject");
                    goto case 4;
                case 4:
                    break;
            }
        }

        protected void ResetProjectileCounts(Player player)
        {
            player.ownedProjectileCounts[mod.ProjectileType(ProjectileType + OptionName + "OneObject")] =
              cloneProjectileCounts[0];
            player.ownedProjectileCounts[mod.ProjectileType(ProjectileType + OptionName + "TwoObject")] =
              cloneProjectileCounts[1];
            player.ownedProjectileCounts[mod.ProjectileType(ProjectileType + OptionName + "ThreeObject")] =
              cloneProjectileCounts[2];
            player.ownedProjectileCounts[mod.ProjectileType(ProjectileType + OptionName + "FourObject")] =
              cloneProjectileCounts[3];
        }

        protected void StoreProjectileCounts(Player player)
        {
            cloneProjectileCounts[0] =
              player.ownedProjectileCounts[mod.ProjectileType(ProjectileType + OptionName + "OneObject")];
            cloneProjectileCounts[1] =
              player.ownedProjectileCounts[mod.ProjectileType(ProjectileType + OptionName + "TwoObject")];
            cloneProjectileCounts[2] =
              player.ownedProjectileCounts[mod.ProjectileType(ProjectileType + OptionName + "ThreeObject")];
            cloneProjectileCounts[3] =
              player.ownedProjectileCounts[mod.ProjectileType(ProjectileType + OptionName + "FourObject")];
        }

        private bool IsOptionNotDeployed(Player player, string projectileName)
        {
            return player.ownedProjectileCounts[mod.ProjectileType(projectileName)] <= 0 &&
                   IsSameClientOwner(player);
        }
    }
}