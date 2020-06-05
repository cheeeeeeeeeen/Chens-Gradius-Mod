using System.ComponentModel;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.Config;

namespace ChensGradiusMod
{
  public class GradiusModConfig : ModConfig
  {
    public override ConfigScope Mode => ConfigScope.ServerSide;

    [Label("Bacterion Contact Damage to NPCs")]
    [Tooltip("Adjust the damage done to town NPCs when Bacterion enemies make contact with them. Adjustment is by percentage. 1 is 100%.")]
    [Range(0f, 1f)]
    [Increment(.01f)]
    [DrawTicks]
    [DefaultValue(.3f)]
    public float bacterionContactDamageMultiplier;

    [Label("Bacterion Bullet Damage to NPCs")]
    [Tooltip("Adjust the damage done to town NPCs when Bacterion bullets hit them. Adjustment is by percentage. 1 is 100%.")]
    [Range(0f, 1f)]
    [Increment(.01f)]
    [DrawTicks]
    [DefaultValue(.3f)]
    public float bacterionBulletDamageMultiplier;

    [Label("Bacterion Spawn Rate")]
    [Tooltip("Adjust the base spawn rate of the Bacterion enemies. Adjustment is by percentage. 1 is 100%. 100% is the default.")]
    [Range(.1f, 2f)]
    [Increment(.01f)]
    [DrawTicks]
    [DefaultValue(1f)]
    public float bacterionSpawnRateMultiplier;

    public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
    {
      switch (Main.netMode)
      {
        case NetmodeID.Server:
          if (whoAmI == Main.myPlayer)
          {
            return true;
          }
          else
          {
            message = "You are unauthorized to make changes. Only the server may change the configuration.";
            return false;
          }

        case NetmodeID.SinglePlayer:
          return true;

        default:
          message = "Something went wrong. You should not be seeing this message.";
          return false;
      }
    }
  }
}