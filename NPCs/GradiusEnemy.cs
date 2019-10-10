using ChensGradiusMod.Gores;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.NPCs
{
  public abstract class GradiusEnemy : ModNPC
  {
    public override void SetDefaults()
    {
      npc.friendly = false;
      npc.HitSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Enemies/Gradius2Hit");
      npc.DeathSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Enemies/Gradius2Destroy");
      npc.aiStyle = -1;
    }

    public override void HitEffect(int hitDirection, double damage)
    {
      if (npc.life <= 0)
      {
        Gore.NewGorePerfect(GradiusExplode.CenterSpawn(npc.Center), Vector2.Zero,
                            mod.GetGoreSlot("Gores/GradiusExplode"));
      }
    }

    protected virtual Rectangle[] InvulnerableHitboxes
    {
      get { return new Rectangle[0]; }
    }

    protected void ReduceDamage(ref int damage, ref float knockback, ref bool crit)
    {
      damage = 1;
      crit = false;
      knockback = 0f;
    }

    protected void ImmuneToBuffs()
    {
      for (int i = 0; i < npc.buffImmune.Length; i++)
      {
        npc.buffImmune[i] = true;
      }
    }
  }
}
