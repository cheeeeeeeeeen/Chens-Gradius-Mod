using ChensGradiusMod.Projectiles.Aliens;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod
{
    public enum RuleTypes { WeaponProjectilePair, Weapon, Projectile };

    public static class OptionRules
    {
        private static readonly List<AlienWeapon> BannedWeapons = new List<AlienWeapon>()
        {
            new AlienWeapon(ItemID.RainbowGun),
            new AlienWeapon(ItemID.Vilethorn),
            new AlienWeapon(ItemID.DD2BetsyBow),
            new AlienWeapon(ItemID.Minishark),
            new AlienWeapon(ItemID.MushroomSpear),
            new AlienWeapon(ItemID.Megashark),
            new AlienWeapon(ItemID.Gatligator),
            new AlienWeapon(ItemID.ChainGun),
            new AlienWeapon(ItemID.SDMG)
        };

        private static readonly List<AlienProjectile> BannedProjectiles = new List<AlienProjectile>()
        {
            new AlienProjectile(ProjectileID.RocketII),
            new AlienProjectile(ProjectileID.RocketIV),
            new AlienProjectile(ProjectileID.Dynamite),
            new AlienProjectile(ProjectileID.BouncyDynamite),
            new AlienProjectile(ProjectileID.StickyDynamite),
            new AlienProjectile(ProjectileID.Bomb),
            new AlienProjectile(ProjectileID.BombFish),
            new AlienProjectile(ProjectileID.BouncyBomb),
            new AlienProjectile(ProjectileID.StickyBomb)
        };

        private static readonly List<AlienWeaponProjectilePair> BannedWeaponProjectilePairs = new List<AlienWeaponProjectilePair>()
        {
            new AlienWeaponProjectilePair(ItemID.VortexBeater, ProjectileID.VortexBeater)
        };

        private static readonly List<AlienWeapon> AllowedWeapons = new List<AlienWeapon>();

        private static readonly List<AlienProjectile> AllowedProjectiles = new List<AlienProjectile>();

        private static readonly List<AlienWeaponProjectilePair> AllowedWeaponProjectilePairs = new List<AlienWeaponProjectilePair>()
        {
            new AlienWeaponProjectilePair(ItemID.StarCannon, ProjectileID.FallingStar),
            new AlienWeaponProjectilePair(ItemID.OnyxBlaster, ProjectileID.BlackBolt),
            new AlienWeaponProjectilePair(ItemID.Blowpipe, ProjectileID.Seed),
            new AlienWeaponProjectilePair(ItemID.Blowgun, ProjectileID.Seed),
            new AlienWeaponProjectilePair(ItemID.Marrow, ProjectileID.BoneArrow),
            new AlienWeaponProjectilePair(ItemID.MoltenFury, ProjectileID.FireArrow),
            new AlienWeaponProjectilePair(ItemID.BoneGlove, ProjectileID.BoneGloveProj),
            new AlienWeaponProjectilePair(ItemID.ChlorophyteSaber, ProjectileID.SporeCloud),
            new AlienWeaponProjectilePair(ItemID.IceBow, ProjectileID.FrostArrow),
            new AlienWeaponProjectilePair(ItemID.ChargedBlasterCannon, ProjectileID.ChargedBlasterOrb),
            new AlienWeaponProjectilePair(ItemID.MonkStaffT2, ProjectileID.MonkStaffT2Ghast),
            new AlienWeaponProjectilePair(ItemID.MonkStaffT3, ProjectileID.MonkStaffT3_AltShot),
            new AlienWeaponProjectilePair(ItemID.ChlorophyteClaymore, ProjectileID.ChlorophyteOrb),
            new AlienWeaponProjectilePair(ItemID.PulseBow, ProjectileID.PulseBolt),
            new AlienWeaponProjectilePair(ItemID.FireworksLauncher, ProjectileID.RocketFireworkRed),
            new AlienWeaponProjectilePair(ItemID.FireworksLauncher, ProjectileID.RocketFireworkGreen),
            new AlienWeaponProjectilePair(ItemID.FireworksLauncher, ProjectileID.RocketFireworkBlue),
            new AlienWeaponProjectilePair(ItemID.FireworksLauncher, ProjectileID.RocketFireworkYellow),
            new AlienWeaponProjectilePair(ItemID.VortexBeater, ProjectileID.VortexBeaterRocket),
            new AlienWeaponProjectilePair(ItemID.SniperRifle, ProjectileID.BulletHighVelocity)
        };

        private static readonly List<AlienDamageType> SupportedDamageTypes = new List<AlienDamageType>()
        {
            new AlienDamageType("CalamityMod", "CalamityGlobalProjectile", "rogue")
        };

        public static bool StandardFilter(Projectile p)
        {
            return RequiredFilter(p) && !p.hostile && p.friendly && !p.npcProj && CanDamage(p)
                   && IsAbleToCrit(p) && !p.minion && !p.trap;
        }

        public static bool RequiredFilter(Projectile p)
        {
            return p.active && IsNotAYoyo(p);
        }

        public static bool AddWeaponRule(string mode, string modName, string weapName)
        {
            List<AlienWeapon> listToUse;

            switch (mode)
            {
                case "Allow":
                    listToUse = AllowedWeapons;
                    break;

                case "Ban":
                default:
                    listToUse = BannedWeapons;
                    break;
            }

            if (!listToUse.Exists(aw => modName == aw.modName && weapName == aw.weaponName))
            {
                listToUse.Add(new AlienWeapon(modName, weapName));
                return true;
            }

            return false;
        }

        public static bool AddWeaponRule(string mode, int wType)
        {
            List<AlienWeapon> listToUse;

            switch (mode)
            {
                case "Allow":
                    listToUse = AllowedWeapons;
                    break;

                case "Ban":
                default:
                    listToUse = BannedWeapons;
                    break;
            }

            if (!listToUse.Exists(aw => aw.modName == "Terraria" && wType == aw.weaponType))
            {
                listToUse.Add(new AlienWeapon(wType));
                return true;
            }

            return false;
        }

        public static bool AddProjectileRule(string mode, string modName, string projName)
        {
            List<AlienProjectile> listToUse;

            switch (mode)
            {
                case "Allow":
                    listToUse = AllowedProjectiles;
                    break;

                case "Ban":
                default:
                    listToUse = BannedProjectiles;
                    break;
            }

            if (!listToUse.Exists(ap => modName == ap.modName && projName == ap.projectileName))
            {
                listToUse.Add(new AlienProjectile(modName, projName));
                return true;
            }

            return false;
        }

        public static bool AddProjectileRule(string mode, int pType)
        {
            List<AlienProjectile> listToUse;

            switch (mode)
            {
                case "Allow":
                    listToUse = AllowedProjectiles;
                    break;

                case "Ban":
                default:
                    listToUse = BannedProjectiles;
                    break;
            }

            if (!listToUse.Exists(ap => ap.modName == "Terraria" && pType == ap.projectileType))
            {
                listToUse.Add(new AlienProjectile(pType));
                return true;
            }

            return false;
        }

        public static bool AddWeaponProjectilePairRule(string mode, string modName, string weapName, string projName)
        {
            List<AlienWeaponProjectilePair> listToUse;

            switch (mode)
            {
                case "Allow":
                    listToUse = AllowedWeaponProjectilePairs;
                    break;

                case "Ban":
                default:
                    listToUse = BannedWeaponProjectilePairs;
                    break;
            }

            if (!listToUse.Exists(awpp => modName == awpp.modName && weapName == awpp.weaponName
                                          && projName == awpp.projectileName))
            {
                listToUse.Add(new AlienWeaponProjectilePair(modName, weapName, projName));
                return true;
            }

            return false;
        }

        public static bool AddWeaponProjectilePairRule(string mode, int wType, int pType)
        {
            List<AlienWeaponProjectilePair> listToUse;

            switch (mode)
            {
                case "Allow":
                    listToUse = AllowedWeaponProjectilePairs;
                    break;

                case "Ban":
                default:
                    listToUse = BannedWeaponProjectilePairs;
                    break;
            }

            if (!listToUse.Exists(awpp => awpp.modName == "Terraria" && wType == awpp.weaponType
                                          && pType == awpp.projectileType))
            {
                listToUse.Add(new AlienWeaponProjectilePair(wType, pType));
                return true;
            }

            return false;
        }

        public static bool ImportDamageType(string modName, string internalName, string damageType)
        {
            if (!SupportedDamageTypes.Exists(sdt => modName == sdt.modName
                                                    && internalName == sdt.internalName
                                                    && damageType == sdt.damageType))
            {
                AlienDamageType alienDamageType = new AlienDamageType(modName, internalName, damageType);
                SupportedDamageTypes.Add(alienDamageType);
                return true;
            }

            return false;
        }

        public static bool ImportDamageType(string modName, string damageType)
        {
            if (!SupportedDamageTypes.Exists(sdt => modName == sdt.modName
                                                    && damageType == sdt.damageType))
            {
                AlienDamageType alienDamageType = new AlienDamageType(modName, damageType);
                SupportedDamageTypes.Add(alienDamageType);
                return true;
            }

            return false;
        }

        public static bool IsBanned(Item w, Projectile p) => IsBanned(w.type, p.type);

        public static bool IsBanned(int wType, int pType)
        {
            bool result = false;
            foreach (AlienWeapon aw in BannedWeapons)
            {
                if (aw.CheckType(wType))
                {
                    result = true;
                    break;
                }
            }
            if (result) return true;

            result = false;
            foreach (AlienProjectile ap in BannedProjectiles)
            {
                if (ap.CheckType(pType))
                {
                    result = true;
                    break;
                }
            }
            if (result) return true;

            result = false;
            foreach (AlienWeaponProjectilePair awpp in BannedWeaponProjectilePairs)
            {
                if (awpp.CheckType(wType, pType))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public static bool IsAllowed(Item w, Projectile p) => IsAllowed(w.type, p.type);

        public static bool IsAllowed(int wType, int pType)
        {
            bool result = false;
            foreach (AlienWeapon aw in AllowedWeapons)
            {
                if (aw.CheckType(wType))
                {
                    result = true;
                    break;
                }
            }
            if (result) return true;

            result = false;
            foreach (AlienProjectile ap in AllowedProjectiles)
            {
                if (ap.CheckType(pType))
                {
                    result = true;
                    break;
                }
            }
            if (result) return true;

            result = false;
            foreach (AlienWeaponProjectilePair awpp in AllowedWeaponProjectilePairs)
            {
                if (awpp.CheckType(wType, pType))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private static bool IsNotAYoyo(Projectile p) => p.aiStyle != 99;

        private static bool IsAbleToCrit(Projectile p) => p.melee || p.ranged || p.thrown || p.magic
                                                          || IsDamageTypeSupported(p.whoAmI);

        private static bool IsDamageTypeSupported(int pWhoAmI)
        {
            foreach (AlienDamageType aDamageType in SupportedDamageTypes)
            {
                if (aDamageType.IsMatchingDamageType(pWhoAmI)) return true;
            }

            return false;
        }
    }
}