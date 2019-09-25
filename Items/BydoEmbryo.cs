using System.Collections.Generic;
using ChensGradiusMod.Projectiles.Forces;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items
{
  public class BydoEmbryo : ParentGradiusAccessory
  {
    public override void SetStaticDefaults()
    {
      Tooltip.SetDefault("Slightly increases life regeneration.\n" +
                         "Deploys the Force.\n" +
                         "Damage and Knockback are based on the held weapon.\n" +
                         "Any enemy projectile that comes in contact are destroyed.\n" +
                         "Press the Force Action hotkey to launch or pull it!\n" +
                         "You feel as if it is ominously loyal to you.");
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.width = 28;
      item.height = 28;
      item.rare = 6;
    }

    public override string Texture => "ChensGradiusMod/Sprites/ForceBaseAccessory";

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      player.lifeRegen += 1;
      ModPlayer(player).forceBase = true;
      if (IsForceNotDeployed(player))
      {
        float xSpawn;
        if (player.direction == 1) xSpawn = Main.screenPosition.X - 36;
        else xSpawn = Main.screenPosition.X + Main.screenWidth + 36;

        int pInd = Projectile.NewProjectile(xSpawn, player.Center.Y, 0f, 0f,
                                            mod.ProjectileType<ForceBase>(),
                                            ForceBase.dmg, ForceBase.kb, player.whoAmI);
        ModPlayer(player).forceProjectile = Main.projectile[pInd];
        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Forces/ForceSpawn"),
                       Main.projectile[pInd].Center);
      }
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
      Player clientPlayer = Main.player[Main.myPlayer];

      string tooltipText = $"{ModPlayer(clientPlayer).forceProjectile.damage} damage";
      TooltipLine newtip = new TooltipLine(mod, "ForceDamage", tooltipText);
      tooltips.Insert(1, newtip);

      tooltipText = GradiusHelper.KnockbackTooltip(ModPlayer(clientPlayer).forceProjectile.knockBack);
      newtip = new TooltipLine(mod, "ForceKnockback", tooltipText);
      tooltips.Insert(2, newtip);
    }

    private bool IsForceNotDeployed(Player player)
    {
      return player.ownedProjectileCounts[mod.ProjectileType<ForceBase>()] <= 0 &&
             player.whoAmI == Main.myPlayer;
    }
  }
}
