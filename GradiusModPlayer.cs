using ChensGradiusMod.Projectiles.Forces;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace ChensGradiusMod
{
  public class GradiusModPlayer : ModPlayer
  {
    private const int MaxFlightPathCount = 60;
    private const int MaxProducedProjectileBuffer = 300;

    public bool forceBase;
    public Projectile forceProjectile;

    public List<Vector2> optionFlightPath = new List<Vector2>();
    public List<int> optionAlreadyProducedProjectiles = new List<int>();
    public bool optionOne;
    public bool optionTwo;
    public bool optionThree;
    public bool optionFour;

    public GradiusModPlayer() => UpdateDead();

    public override void ResetEffects()
    {
      forceBase = true;
      optionOne = false;
      optionTwo = false;
      optionThree = false;
      optionFour = false;
    }

    public override void UpdateDead()
    {
      ResetEffects();
      ResetOptionVariables();
    }

    public override void OnEnterWorld(Player player)
    {
      int pInd = Projectile.NewProjectile(player.Center, player.velocity, mod.ProjectileType<ForceBase>(), 0, 0f, player.whoAmI);
      forceProjectile = Main.projectile[pInd];
    }

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
      if (forceBase && ChensGradiusMod.forceActionKey.JustReleased &&
          forceProjectile.modProjectile is ForceBase fbProj)
      {
        switch (fbProj.mode)
        {
          case (int)ForceBase.States.Attached:
            fbProj.mode = (int)ForceBase.States.Launched;
            break;
          case (int)ForceBase.States.Detached:
            fbProj.mode = (int)ForceBase.States.Pulled;
            break;
          case (int)ForceBase.States.Launched:
          case (int)ForceBase.States.Pulled:
            break;
        }
      }
    }

    public override void PreUpdate()
    {
      if (HasAnyOptions())
      {
        if (optionFlightPath.Count > 0)
        {
          for (int h = 0; h < optionAlreadyProducedProjectiles.Count; h++)
          {
            Projectile p = Main.projectile[optionAlreadyProducedProjectiles[h]];
            if (!p.active) optionAlreadyProducedProjectiles.RemoveAt(h--);
          }

          if (!(optionFlightPath[0].X == player.Center.X && optionFlightPath[0].Y == player.Center.Y))
          {
            if (optionFlightPath.Count >= MaxFlightPathCount) optionFlightPath.RemoveAt(optionFlightPath.Count - 1);
            optionFlightPath.Insert(0, player.Center);
          }
        }
        else optionFlightPath.Insert(0, player.Center);
      }
      else ResetOptionVariables();
    }

    public override void PostUpdate()
    {
      if (HasAnyOptions()) GradiusHelper.FreeListData(optionAlreadyProducedProjectiles, MaxProducedProjectileBuffer);
    }

    private void ResetOptionVariables()
    {
      optionFlightPath.Clear();
      optionFlightPath = new List<Vector2>();
      optionAlreadyProducedProjectiles.Clear();
      optionAlreadyProducedProjectiles = new List<int>();
    }

    private bool HasAnyOptions() => optionOne || optionTwo || optionThree || optionFour;
  }
}