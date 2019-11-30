using Terraria;

namespace ChensGradiusMod.Items.Accessories.Options.Search
{
  public class SearchOptionOneTwo : TwoSearchOptionsBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();

      DisplayName.SetDefault("Options type Search (1st & 2nd)");
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.rare = 7;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      ModPlayer(player).optionOne = true;
      ModPlayer(player).optionTwo = true;
      ModPlayer(player).searchOption.Value = true;

      base.UpdateAccessory(player, hideVisual);
    }

    public override bool CanEquipAccessory(Player player, int slot)
    {
      return ModeChecks(ModPlayer(player), false);
    }

    protected override string[] ProjectileName { get; } = { "OptionOneObject",
                                                            "OptionTwoObject" };

    protected override int[] OptionPosition { get; } = { 1, 2 };
  }
}