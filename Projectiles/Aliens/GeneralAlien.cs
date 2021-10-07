﻿using Terraria.ModLoader;

namespace ChensGradiusMod.Projectiles.Aliens
{
    public abstract class GeneralAlien
    {
        public readonly string modName;

        protected readonly Mod modInstance;

        protected GeneralAlien(string mod)
        {
            modName = mod;
            modInstance = ModLoader.GetMod(modName);
        }
    }
}