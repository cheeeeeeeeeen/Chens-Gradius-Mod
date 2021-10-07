using Terraria;
using Terraria.ID;

namespace ChensGradiusMod.Items.Accessories.Options.Recurve
{
    public class RecurveOptionThreeFour : TwoRecurveOptionsBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Options type Recurve (3rd & 4th)");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            item.rare = ItemRarityID.Cyan; // 9
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ModPlayer(player).optionThree = true;
            ModPlayer(player).optionFour = true;
            ModPlayer(player).recurveOption = true;

            base.UpdateAccessory(player, hideVisual);
        }

        protected override string[] ProjectileName { get; } = { "OptionThreeObject",
                                                            "OptionFourObject" };

        protected override int[] OptionPosition { get; } = { 3, 4 };
    }
}