using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChensGradiusMod
{
	public class ChensGradiusMod : Mod
	{
    public static ModHotKey forceActionKey;
    public static ModHotKey optionActionKey;

    public ChensGradiusMod() { }

    public override void Load()
    {
      forceActionKey = RegisterHotKey("Force Action Toggle", "Mouse3");
      optionActionKey = RegisterHotKey("Option Action Key", "Mouse2");
    }

    public override void Unload()
    {
      forceActionKey = null;
      optionActionKey = null;
    }

    public override void AddRecipeGroups()
    {
      RecipeGroup group = new RecipeGroup(() => Language.GetTextValue("Ebonstone Block or Crimstone Block"), new int[]
      {
        ItemID.CrimstoneBlock,
        ItemID.EbonstoneBlock
      });
      RecipeGroup.RegisterGroup("ChensGradiusMod:EvilStones", group);

      group = new RecipeGroup(() => Language.GetTextValue("Tissue Sample or Shadow Scale"), new int[]
      {
        ItemID.ShadowScale,
        ItemID.TissueSample
      });
      RecipeGroup.RegisterGroup("ChensGradiusMod:EvilDrops", group);

      group = new RecipeGroup(() => Language.GetTextValue("Gold Bar or Platinum Bar"), new int[]
      {
        ItemID.PlatinumBar,
        ItemID.GoldBar
      });
      RecipeGroup.RegisterGroup("ChensGradiusMod:GoldTierBar", group);

      group = new RecipeGroup(() => Language.GetTextValue("Cobalt Bar or Palladium Bar"), new int[]
      {
        ItemID.PalladiumBar,
        ItemID.CobaltBar
      });
      RecipeGroup.RegisterGroup("ChensGradiusMod:CobaltTierBar", group);

      group = new RecipeGroup(() => Language.GetTextValue("Copper Bar or Tin Bar"), new int[]
      {
        ItemID.TinBar,
        ItemID.CopperBar
      });
      RecipeGroup.RegisterGroup("ChensGradiusMod:TinTierBar", group);

      group = new RecipeGroup(() => Language.GetTextValue("Any Mechanical Boss Soul"), new int[]
      {
        ItemID.SoulofMight,
        ItemID.SoulofSight,
        ItemID.SoulofFright,
      });
      RecipeGroup.RegisterGroup("ChensGradiusMod:MechSoul", group);

      group = new RecipeGroup(() => Language.GetTextValue("Silver Bar or Tungsten Bar"), new int[]
      {
        ItemID.TungstenBar,
        ItemID.SilverBar
      });
      RecipeGroup.RegisterGroup("ChensGradiusMod:SilverTierBar", group);
    }
  }
}