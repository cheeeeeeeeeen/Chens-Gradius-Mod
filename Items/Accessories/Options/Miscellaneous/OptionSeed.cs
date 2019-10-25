using System.Collections.Generic;
using ChensGradiusMod.Projectiles.Options.Miscellaneous;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options.Miscellaneous
{
  public class OptionSeed : ParentGradiusAccessory
  {
    private int? pInd = null;
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

      if (GradiusHelper.IsSameClientOwner(player) && player.ownedProjectileCounts[mod.ProjectileType(projectileName)] <= 0)
      {
        pInd = Projectile.NewProjectile(player.Center.X + OptionSeedObject.SeedDistance * player.direction,
                                        player.Center.Y, 0f, 0f, mod.ProjectileType(projectileName), 0, 0f,
                                        player.whoAmI, 0f, 0f);
        ModOwner(player).seedProjectile = OptionObject;
      }

      if (pInd != null && OptionObject.active && OptionObject.modProjectile is OptionSeedObject optSeed)
      {
        optSeed.rotateDirection = -hideVisual.ToDirectionInt();
        if (optSeed.isSpawning)
        {
          optSeed.isSpawning = false;
          if (player.direction < 0) optSeed.currentAngle = GradiusHelper.HalfAngle;
        }
      }
      else pInd = null;
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
      base.ModifyTooltips(tooltips);
    }

    public override string Texture => "ChensGradiusMod/Sprites/OptionSeedSheet";

    private Projectile OptionObject => Main.projectile[(int)pInd];

    private GradiusModPlayer ModOwner(Player p) => p.GetModPlayer<GradiusModPlayer>();
  }
}
