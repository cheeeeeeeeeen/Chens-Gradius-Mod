using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace ChensGradiusMod.Items.Accessories.Options
{
  public abstract class OptionBase : ParentGradiusAccessory
  {
    private readonly int[] cloneProjectileCounts = new int[4] { 0, 0, 0, 0 };

    public override void SetStaticDefaults()
    {
      Tooltip.SetDefault(OptionTooltip);
      ItemID.Sets.ItemNoGravity[item.type] = true;
      Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(10, 5));
    }

    public override void SetDefaults()
    {
      base.SetDefaults();
      item.width = 44;
      item.height = 52;
    }

    public override string Texture => $"ChensGradiusMod/Sprites/OptionInv{OptionPosition}";

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      StoreProjectileCounts(player);
      CreateOption(player, OptionPosition, ProjectileType + ProjectileName);
      CreationOrderingBypass(player, OptionPosition);
      ResetProjectileCounts(player);
    }

    public override bool CanEquipAccessory(Player player, int slot)
    {
      return ModeChecks(player, true);
    }

    public override void OnCraft(Recipe recipe)
    {
      GradiusHelper.AchievementLibUnlock("Wreek Weapon", Main.LocalPlayer);
    }

    protected virtual string ProjectileType => "";

    protected virtual string ProjectileName => "OptionObject";

    protected virtual string OptionName => "Option";

    protected virtual int OptionPosition => 0;

    protected virtual string OptionTooltip =>
      "Deploys an Option.\n" +
      "Some projectiles you create are copied by the drone.\n" +
      "The drone will follow your flight path.\n" +
      "This advanced drone uses Wreek technology,\n" +
      "infusing both technology and psychic elements together.";

    protected virtual bool ModeChecks(Player player, bool hideVisual)
    {
      return ModPlayer(player).normalOption &&
             !ModPlayer(player).freezeOption &&
             !ModPlayer(player).rotateOption &&
             !ModPlayer(player).chargeMultiple;
    }

    protected void CreateOption(Player player, int optionPosition, string projectileName)
    {
      if (GradiusHelper.OptionCheckSelfAndPredecessors(ModPlayer(player), optionPosition) &&
          ModeChecks(player, false) && IsOptionNotDeployed(player, projectileName))
      {
        Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f,
                                 mod.ProjectileType(projectileName), 0, 0f,
                                 player.whoAmI, 0f, 0f);
        player.ownedProjectileCounts[mod.ProjectileType(projectileName)]++;
      }
    }

    protected void CreationOrderingBypass(Player player, int position)
    {
      switch (position)
      {
        case 1:
          CreateOption(player, 2, ProjectileType + OptionName + "TwoObject");
          goto case 2;
        case 2:
          CreateOption(player, 3, ProjectileType + OptionName + "ThreeObject");
          goto case 3;
        case 3:
          if (ModPlayer(player).optionFour)
            CreateOption(player, 4, ProjectileType + OptionName + "FourObject");
          goto case 4;
        case 4:
          break;
      }
    }

    protected void ResetProjectileCounts(Player player)
    {
      player.ownedProjectileCounts[mod.ProjectileType(ProjectileType + OptionName + "OneObject")] =
        cloneProjectileCounts[0];
      player.ownedProjectileCounts[mod.ProjectileType(ProjectileType + OptionName + "TwoObject")] =
        cloneProjectileCounts[1];
      player.ownedProjectileCounts[mod.ProjectileType(ProjectileType + OptionName + "ThreeObject")] =
        cloneProjectileCounts[2];
      player.ownedProjectileCounts[mod.ProjectileType(ProjectileType + OptionName + "FourObject")] =
        cloneProjectileCounts[3];
    }

    protected void StoreProjectileCounts(Player player)
    {
      cloneProjectileCounts[0] =
        player.ownedProjectileCounts[mod.ProjectileType(ProjectileType + OptionName + "OneObject")];
      cloneProjectileCounts[1] =
        player.ownedProjectileCounts[mod.ProjectileType(ProjectileType + OptionName + "TwoObject")];
      cloneProjectileCounts[2] =
        player.ownedProjectileCounts[mod.ProjectileType(ProjectileType + OptionName + "ThreeObject")];
      cloneProjectileCounts[3] =
        player.ownedProjectileCounts[mod.ProjectileType(ProjectileType + OptionName + "FourObject")];
    }

    private bool IsOptionNotDeployed(Player player, string projectileName)
    {
      return player.ownedProjectileCounts[mod.ProjectileType(projectileName)] <= 0 &&
             GradiusHelper.IsSameClientOwner(player);
    }
  }
}
