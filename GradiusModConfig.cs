using System.ComponentModel;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod
{
  public class GradiusModConfig : ModConfig
  {
    private static GradiusModConfig _instance = null;

    public override ConfigScope Mode => ConfigScope.ServerSide;

    [Label("Bacterion Contact Damage to NPCs")]
    [Tooltip("Adjust the damage done to town NPCs when Bacterion enemies make contact with them.\nAdjustment is by percentage.\n1 is 100%, " +
             "meaning Bacterions will deal full damage as intended to NPCs. Default is 50%.")]
    [Range(0f, 2f)]
    [Increment(.01f)]
    [DrawTicks]
    [DefaultValue(.5f)]
    public float bacterionContactDamageMultiplierToNpc;

    [Label("Bacterion Bullet Damage to NPCs")]
    [Tooltip("Adjust the damage done to town NPCs when Bacterion bullets hit them.\nAdjustment is by percentage.\n1 is 100%, " +
             "meaning Bacterion Bullets will deal full damage as intended to NPCs. Default is 50%.")]
    [Range(0f, 2f)]
    [Increment(.01f)]
    [DrawTicks]
    [DefaultValue(.5f)]
    public float bacterionBulletDamageMultiplierToNpc;

    [Label("Bacterion Spawn Rate")]
    [Tooltip("Adjust the base spawn rate of the Bacterion enemies.\nAdjustment is by percentage. 1 is 100%.\n100% is the default, " +
             "meaning the spawn rate will work as intended.")]
    [Range(0f, 2f)]
    [Increment(.01f)]
    [DrawTicks]
    [DefaultValue(1f)]
    public float bacterionSpawnRateMultiplier;

    [Label("Option Projectiles Duplication Limit")]
    [Tooltip("Adjusts the limit of how much an option can duplicate projectiles in the world.\n" +
             "If your projectiles are disappearing and you are not experiencing lag, then you may increase this value.\n" +
             "If you are experiencing lag due to the amount of projectiles being duplicated, then decrease this value.")]
    [Range(0, 400)]
    [Increment(1)]
    [DrawTicks]
    [DefaultValue(150)]
    public int projectileDuplicationLimit;

    [Label("Option Damage Multiplier")]
    [Tooltip("Adjust the how much damage each Option would deal.\nAdjustment is by percentage. 1 is 100%.\n1 is the default, " +
             "meaning it does full damage as the original. This also affects knockback.")]
    [Range(0f, 2f)]
    [Increment(.01f)]
    [DrawTicks]
    [DefaultValue(1f)]
    public float optionDamageMultiplier;

    [Label("Bacterion General Contact Damage")]
    [Tooltip("Adjust the damage done to friendlies when Bacterion enemies make contact with them.\nAdjustment is by percentage.\n1 is 100%, " +
             "meaning Bacterions will deal full damage as intended.\nThis setting works together with NPC Damage Adjustment.")]
    [Range(0f, 2f)]
    [Increment(.01f)]
    [DrawTicks]
    [DefaultValue(1f)]
    public float bacterionContactDamageMultiplier;

    [Label("Bacterion General Bullet Damage")]
    [Tooltip("Adjust the damage done to friendlies when Bacterion bullets hit them.\nAdjustment is by percentage.\n1 is 100%, " +
             "meaning Bacterion Bullets will deal full damage as intended.\nThis setting works together with NPC Damage Adjustment.")]
    [Range(0f, 2f)]
    [Increment(.01f)]
    [DrawTicks]
    [DefaultValue(1f)]
    public float bacterionBulletDamageMultiplier;

    [Label("Bacterion General Health")]
    [Tooltip("Adjust the health of the Bacterions.\nAdjustment is by percentage.\n1 is 100%, " +
             "meaning they will have the health value as intended.")]
    [Range(0f, 2f)]
    [Increment(.01f)]
    [DrawTicks]
    [DefaultValue(1f)]
    public float bacterionHealthMultiplier;

    [Label("Bacterion General Armor")]
    [Tooltip("Adjust the armor of the Bacterions.\nAdjustment is by percentage.\n1 is 100%, " +
             "meaning they will have the armor value as intended.")]
    [Range(0f, 2f)]
    [Increment(.01f)]
    [DrawTicks]
    [DefaultValue(1f)]
    public float bacterionArmorMultiplier;

    [Label("Bacterion Damage Reduction")]
    [Tooltip("Adjust the damage reduction stat of the large Bacterions.\nAdjustment is by percentage.\n1 is 100%, " +
             "meaning they will have the damage reduction values as intended.\n" +
             "Only large enemies benefit from this stat.")]
    [Range(0f, 2f)]
    [Increment(.01f)]
    [DrawTicks]
    [DefaultValue(1f)]
    public float bacterionDamageReductionMultiplier;

    [Label("Post Plantera Buff Multiplier")]
    [Tooltip("Adjust the buff multiplier to the Bacterions when Plantera is defeated.\nAdjustment is by percentage.\n1 is 100%, " +
             "meaning they will strengthen according to intended values.\nDamage reduction stat is not affected by the multiplier.")]
    [Range(0f, 2f)]
    [Increment(.01f)]
    [DrawTicks]
    [DefaultValue(1f)]
    public float postPlanteraBuffMultiplier;

    [Label("Post Moon Lord Buff Multiplier")]
    [Tooltip("Adjust the buff multiplier to the Bacterions when Moon Lord is defeated.\nAdjustment is by percentage.\n1 is 100%, " +
             "meaning they will strengthen according to intended values.\nPlantera buffing is replaced with this buff instead of stacking." +
             "\nDamage reduction stat is not affected by the multiplier.")]
    [Range(0f, 2f)]
    [Increment(.01f)]
    [DrawTicks]
    [DefaultValue(1f)]
    public float postMoonLordBuffMultiplier;

    public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
    {
      if (Main.netMode == NetmodeID.SinglePlayer) return true;
      else if (ChensGradiusMod.herosMod != null)
      {
        if ((bool)ChensGradiusMod.herosMod.Call("HasPermission", whoAmI, "UpdateConfig")) return true;
        else
        {
          message = "You are unauthorized to make changes. Insufficient privileges.";
          return false;
        }
      }
      else if (!IsPlayerLocalServerOwner(whoAmI))
      {
        message = "You are unauthorized to make changes. Only the server may change the configuration.";
        return false;
      }

      message = "Unsupported check. Report to Chen if you see this.";
      return false;
    }

    public static GradiusModConfig Instance
    {
      get
      {
        if (_instance == null) _instance = ModContent.GetInstance<GradiusModConfig>();
        return _instance;
      }
    }
  }
}