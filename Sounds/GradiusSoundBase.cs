using Microsoft.Xna.Framework.Audio;
using Terraria.ModLoader;

namespace ChensGradiusMod.Sounds
{
    public abstract class GradiusSoundBase : ModSound
    {
        protected virtual float VolumePercent => 1f;

        protected virtual bool AnotherInstance => true;

        public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
        {
            if (AnotherInstance) soundInstance = sound.CreateInstance();
            soundInstance.Volume = volume * VolumePercent;
            soundInstance.Pan = pan;
            return soundInstance;
        }
    }
}