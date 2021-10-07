using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options.Freeze
{
    public abstract class FreezeOptionBase : OptionBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 6));
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            item.width = 44;
            item.height = 58;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(item.Center, 0f, .749f, 1f);
        }

        protected override string ProjectileType => "Freeze";

        protected override string OptionTooltip =>
          "Deploys an Option type Freeze.\n" +
          "Some projectiles you create are copied by the drone.\n" +
          "The drone will follow your flight path.\n" +
          "Hold the Option Action Key to perform a different movement behavior!";

        protected override void UpgradeUsualStations(ModRecipe recipe)
        {
            recipe.AddTile(TileID.IceMachine);
        }

        protected override bool ModeChecks(GradiusModPlayer gmPlayer, bool includeSelf = true)
        {
            bool result = true;
            if (includeSelf) result &= gmPlayer.freezeOption;

            result &= !gmPlayer.normalOption
                   && !gmPlayer.recurveOption
                   && !gmPlayer.rotateOption
                   && !gmPlayer.aimOption
                   && !gmPlayer.chargeMultiple
                   && !gmPlayer.spreadOption
                   && !gmPlayer.searchOption;

            return result;
        }
    }
}