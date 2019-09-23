using Microsoft.Xna.Framework.Audio;
using Terraria.ModLoader;

namespace ChensGradiusMod.Sounds.Forces
{
  public abstract class ForceSoundBase : ModSound
  {
    public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
    {
      soundInstance = sound.CreateInstance();
      soundInstance.Volume = volume * .4f;
      soundInstance.Pan = pan;
      return soundInstance;
    }
  }
}