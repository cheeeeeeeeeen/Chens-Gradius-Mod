using Terraria;
using Terraria.DataStructures;

namespace ChensGradiusMod.Items.Accessories.Options.Rotate
{
  public abstract class TwoRotateOptionsBase : RotateOptionBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();
      Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 9));
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.width = 38;
      item.height = 30;
    }

    public override string Texture => $"ChensGradiusMod/Sprites/Two{ProjectileType}" +
                                      $"Options{OptionPosition[0]}{OptionPosition[1]}";

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      StoreProjectileCounts(player);
      for (int i = 0; i < 2; i++)
      {
        CreateOption(player, OptionPosition[i], ProjectileType + ProjectileName[i]);
        CreationOrderingBypass(player, OptionPosition[i]);
      }
      ResetProjectileCounts(player);
    }

    public override void PostUpdate()
    {
      Lighting.AddLight(item.Center, .747f, 1.5f, 0f);
    }

    public override bool CanEquipAccessory(Player player, int slot)
    {
      return ModeChecks(player) &&
             GradiusHelper.OptionsPredecessorRequirement(ModPlayer(player),
                                                         OptionPosition[0]);
    }

    protected new virtual string[] ProjectileName { get; } = { "1", "2" };

    protected new virtual int[] OptionPosition { get; } = { 1, 2 };

    protected override string OptionTooltip =>
     "Deploys two Options type Rotate.\n" +
     "Some projectiles you create are copied by the drones.\n" +
     "The drones will follow your flight path.\n" +
     "Hold the Option Action Key to have the drones revolve you!\n";
  }
}
