using ChensGradiusMod.Gores;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.NPCs
{
  public abstract class GradiusEnemy : ModNPC
  {
    public enum Types { Small, Large, Boss };

    public override void SetDefaults()
    {
      npc.friendly = false;

      switch (EnemyType)
      {
        case Types.Small:
          npc.HitSound = SoundID.NPCHit4;
          npc.DeathSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Enemies/Gradius2Death");
          break;
        case Types.Large:
        case Types.Boss:
          npc.HitSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Enemies/Gradius2Hit");
          npc.DeathSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Enemies/Gradius2Destroy");
          break;
      }

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

    protected virtual Types EnemyType => Types.Small;

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
