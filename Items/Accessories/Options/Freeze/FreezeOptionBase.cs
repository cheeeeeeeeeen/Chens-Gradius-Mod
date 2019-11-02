using Terraria;
using Terraria.DataStructures;

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

      item.width = 48;
      item.height = 60;
    }

    public override void PostUpdate()
    {
      Lighting.AddLight(item.Center, 0f, .749f, 1f);
    }

    public override string Texture => $"ChensGradiusMod/Sprites/FreezeInv{OptionPosition}";

    protected override string ProjectileType => "Freeze";

    protected override string OptionTooltip =>
      "Deploys an Option type Freeze.\n" +
      "Some projectiles you create are copied by the drone.\n" +
      "The drone will follow your flight path.\n" +
      "Hold the Option Action Key to perform a different movement behavior!";

    protected override bool ModeChecks(Player player, bool hideVisual)
    {
      return ModPlayer(player).freezeOption &&
             !ModPlayer(player).rotateOption &&
             !ModPlayer(player).normalOption &&
             !ModPlayer(player).chargeMultiple;
    }
  }
}