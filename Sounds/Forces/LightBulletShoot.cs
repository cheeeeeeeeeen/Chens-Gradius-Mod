using Microsoft.Xna.Framework.Audio;
using Terraria.ModLoader;

namespace ChensGradiusMod.Sounds.Forces
{
  public class LightBulletShoot : ForceSoundBase
  {
    public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
    {
      return base.PlaySound(ref soundInstance, .2f, pan, type);
    }
  }
}