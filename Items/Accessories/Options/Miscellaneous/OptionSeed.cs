using ChensGradiusMod.Projectiles.Options.Miscellaneous;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace ChensGradiusMod.Items.Accessories.Options.Miscellaneous
{
  public class OptionSeed : ParentGradiusAccessory
  {
    private readonly string projectileName = "OptionSeedObject";

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Option Seed");
      Tooltip.SetDefault("Deploys an Option Seed.\n" +
                         "An incomplete, but surprisingly working, Option drone.\n" +
                         "The drone will shoot bullets or arrows based on the ammo slots.\n" +
                         "This incomplete drone can be upgraded into an Option,\n" +
                         "allowing it to be at its full potential.");
      ItemID.Sets.ItemNoGravity[item.type] = true;
      Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(30, 2));
    }

    public override void SetDefaults()
    {
      base.SetDefaults();
      item.width = 20;
      item.height = 16;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      ModPlayer(player).optionSeed = true;

      if (GradiusHelper.IsSameClientOwner(player))
      {
        if (player.ownedProjectileCounts[mod.ProjectileType(projectileName)] <= 0)
        {
          int pInd = Projectile.NewProjectile(player.Center.X + OptionSeedObject.SeedDistance * player.direction,
                                              player.Center.Y, 0f, 0f, mod.ProjectileType(projectileName), 0, 0f,
                                              player.whoAmI, 0f, 0f);
          ModOwner(player).seedProjectile = Main.projectile[pInd];
        }

        ModOwner(player).seedRotateDirection = -hideVisual.ToDirectionInt();
      }
    }

    public override string Texture => "ChensGradiusMod/Sprites/OptionSeedSheet";

    private GradiusModPlayer ModOwner(Player p) => p.GetModPlayer<GradiusModPlayer>();
  }
}
