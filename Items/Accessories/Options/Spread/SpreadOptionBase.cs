using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.Items.Accessories.Options.Spread
{
    public abstract class SpreadOptionBase : OptionBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 6));
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            AssignItemDimensions(item, 44, 52, true);
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(item.Center, .7f, .7f, .7f);
        }

        protected override string ProjectileType => "Spread";

        protected override string OptionTooltip =>
          "Deploys an Option type Spread.\n" +
          "Some projectiles you create are copied by the drone.\n" +
          "The drone will follow your flight path.\n" +
          "Hold the Option Action Key to allow the option to shoot in all directions!";

        protected override void UpgradeUsualStations(ModRecipe recipe)
        {
            recipe.AddTile(TileID.Sawmill);
        }

        protected override bool ModeChecks(GradiusModPlayer gmPlayer, bool includeSelf = true)
        {
            bool result = true;
            if (includeSelf) result &= gmPlayer.spreadOption;

            result &= !gmPlayer.normalOption
                   && !gmPlayer.recurveOption
                   && !gmPlayer.rotateOption
                   && !gmPlayer.freezeOption
                   && !gmPlayer.chargeMultiple
                   && !gmPlayer.searchOption
                   && !gmPlayer.aimOption;

            return result;
        }
    }
}