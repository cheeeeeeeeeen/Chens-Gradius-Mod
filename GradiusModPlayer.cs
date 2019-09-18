using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod
{
  public class GradiusModPlayer : ModPlayer
  {
    private const int MaxFlightPathCount = 30;

    public List<Vector2> optionFlightPath = new List<Vector2>();
    public int optionOneUUID;
    public int optionTwoUUID;
    public bool optionOne;
    public bool optionTwo;

    public GradiusModPlayer() => UpdateDead();

    public override void ResetEffects()
    {
      optionOneUUID = -1;
      optionTwoUUID = -1;
      optionOne = false;
      optionTwo = false;
    }

    public override void UpdateDead()
    {
      ResetEffects();
      ResetOptionFlightPath();
    }

    public override void PreUpdate()
    {
      if (optionOne)
      {
        if (optionFlightPath.Count > 0)
        {
          if (!(optionFlightPath[0].X == player.position.X && optionFlightPath[0].Y == player.position.Y))
          {
            if (optionFlightPath.Count >= MaxFlightPathCount) optionFlightPath.RemoveAt(optionFlightPath.Count - 1);
            optionFlightPath.Insert(0, player.position);
          }
        }
        else optionFlightPath.Insert(0, player.position);
      }
      else ResetOptionFlightPath();
    }

    public override bool Shoot(Item item, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
    {
      Projectile optionOne = Main.projectile[optionOneUUID];
      Projectile optionTwo = Main.projectile[optionTwoUUID];
      Projectile.NewProjectile(optionOne.Center.X, optionOne.Center.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0f, 0f);
      Projectile.NewProjectile(optionTwo.Center.X, optionTwo.Center.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0f, 0f);
      return base.Shoot(item, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
    }

    private void ResetOptionFlightPath() => optionFlightPath.Clear();
  }
}