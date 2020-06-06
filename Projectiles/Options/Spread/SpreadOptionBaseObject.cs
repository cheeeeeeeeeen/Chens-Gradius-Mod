using ChensGradiusMod.Projectiles.Options.Aim;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.Projectiles.Options.Spread
{
  public class SpreadOptionBaseObject : OptionBaseObject
  {
    private const int FireRate = 4;
    private const int SpreadDuplicateLife = 30;
    private const float SpreadDuplicateDamageMultiplier = .25f;

    private readonly Vector2[] CardinalDirections = new Vector2[]
    {
      new Vector2(-1, -1),
      new Vector2(-1, 0),
      new Vector2(-1, 1),
      new Vector2(1, -1),
      new Vector2(1, 1),
      new Vector2(1, 0),
      new Vector2(0, -1),
      new Vector2(0, 1)
    };

    private int fireCounter = 0;

    public override string Texture => "ChensGradiusMod/Sprites/SpreadSheet";

    protected override int SpawnDuplicateProjectile(Projectile p)
    {
      if (ModOwner.isSpreading)
      {
        if (++fireCounter >= FireRate)
        {
          fireCounter = 0;
          Vector2 pPosition = ComputeOffset(Main.player[p.owner].Center, p.Center);
          for (int i = 0; i < CardinalDirections.Length; i++)
          {
            Vector2 toward = pPosition + CardinalDirections[i];
            Vector2 offsetVelocity = AimOptionBaseObject.ComputeVelocityOffset(p, pPosition, toward);
            int ind = Projectile.NewProjectile(pPosition, offsetVelocity, p.type,
                                               RoundOffToWhole(p.damage * SpreadDuplicateDamageMultiplier),
                                               p.knockBack, projectile.owner, 0f, 0f);
            if (i >= CardinalDirections.Length - 1) return ind;
            else ManualAdditionToProducedProjectiles(ind);
          }
        }

        return -1;
      }
      else return base.SpawnDuplicateProjectile(p);
    }

    protected override void SetDuplicateDefaults(Projectile p)
    {
      base.SetDuplicateDefaults(p);
      if (ModOwner.isSpreading) p.timeLeft = Math.Min(SpreadDuplicateLife, p.timeLeft);
    }

    private void ManualAdditionToProducedProjectiles(int index)
    {
      if (index >= 0)
      {
        ModOwner.optionAlreadyProducedProjectiles.Add(index);
        SetDuplicateDefaults(Main.projectile[index]);
      }
    }
  }
}