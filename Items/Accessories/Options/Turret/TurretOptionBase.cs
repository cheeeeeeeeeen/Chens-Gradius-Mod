using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options.Turret
{
    public abstract class TurretOptionBase : OptionBase
    {
        public override void PostUpdate()
        {
            Lighting.AddLight(item.Center, .1f, .1f, .1f);
        }

        public override string Texture => $"ChensGradiusMod/Sprites/Inv{OptionPosition}";

        protected override string ProjectileType => "Turret";

        protected override string OptionTooltip =>
          "Deploys an Option type Turret.\n" +
          "Some projectiles you create are copied by the drone.\n" +
          "The drone will follow your flight path.\n" +
          "Hold the Option Action Key to lock its position in place!";

        protected override void UpgradeUsualStations(ModRecipe recipe)
        {
            recipe.AddTile(TileID.Solidifier);
        }

        protected override bool ModeChecks(GradiusModPlayer gmPlayer, bool includeSelf = true)
        {
            bool result = true;
            if (includeSelf) result &= gmPlayer.turretOption;

            result &= !gmPlayer.normalOption
                   && !gmPlayer.recurveOption
                   && !gmPlayer.rotateOption
                   && !gmPlayer.aimOption
                   && !gmPlayer.chargeMultiple
                   && !gmPlayer.spreadOption
                   && !gmPlayer.searchOption
                   && !gmPlayer.freezeOption;

            return result;
        }
    }
}