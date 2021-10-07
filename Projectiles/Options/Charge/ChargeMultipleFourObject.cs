using ChensGradiusMod.Items.Accessories.Options.Charge;
using Terraria.ModLoader;

namespace ChensGradiusMod.Projectiles.Options.Charge
{
    public class ChargeMultipleFourObject : ChargeMultipleBaseObject
    {
        public override int Position => 4;

        public override bool PlayerHasAccessory() => ModOwner.optionFour;

        protected override int OptionAccessoryType => ModContent.ItemType<ChargeMultipleFour>();
    }
}