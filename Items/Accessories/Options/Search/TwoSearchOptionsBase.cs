using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.Items.Accessories.Options.Search
{
    public abstract class TwoSearchOptionsBase : SearchOptionBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 9));
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            TwoOptionsMethods.SetDefaults(item);
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(item.Center, 1.5f, 1.2647f, 0f);
        }

        public override string Texture => $"ChensGradiusMod/Sprites/Two{ProjectileType}" +
                                          $"Options{OptionPosition[0]}{OptionPosition[1]}";

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TwoOptionsMethods.UpdateAccessory(player, OptionPosition, ProjectileType, ProjectileName,
                                              StoreProjectileCounts, ResetProjectileCounts,
                                              CreateOption, CreationOrderingBypass);
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            return ModeChecks(ModPlayer(player)) &&
                   OptionsPredecessorRequirement(ModPlayer(player), OptionPosition[0]);
        }

        protected override void UpgradeUsualRecipe(ModRecipe recipe)
        {
            TwoOptionsMethods.UpgradeUsualRecipe(mod, ProjectileType, OptionPosition[0], recipe);
        }

        protected new virtual string[] ProjectileName { get; } = { "1", "2" };

        protected new virtual int[] OptionPosition { get; } = { 1, 2 };

        protected override string OptionTooltip =>
          "Deploys two Options type Search.\n" +
          "Some projectiles you create are copied by the drones.\n" +
          "The drones will follow your flight path.\n" +
          "Hold the Option Action Key to perform a different movement behavior!";

        protected override int ComputeItemValue(int multiplier)
        {
            return TwoOptionsMethods.ComputeItemValue(OptionPosition, base.ComputeItemValue);
        }

        public override void AddRecipes()
        {
            TwoOptionsMethods.AddRecipes(mod, this, UpgradeUsualRecipe);
        }
    }
}