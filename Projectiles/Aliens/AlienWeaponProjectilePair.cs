namespace ChensGradiusMod.Projectiles.Aliens
{
    public class AlienWeaponProjectilePair : GeneralAlien
    {
        public readonly string weaponName = null;
        public readonly string projectileName = null;
        public readonly int? weaponType = null;
        public readonly int? projectileType = null;

        public AlienWeaponProjectilePair(string mod, string weap, string proj) : base(mod)
        {
            weaponName = weap;
            projectileName = proj;
        }

        public AlienWeaponProjectilePair(int weap, int proj) : base("Terraria")
        {
            weaponType = weap;
            projectileType = proj;
        }

        public bool CheckType(int weapType, int projType)
        {
            if (modName == "Terraria" && weaponType != null && projectileType != null)
            {
                return weapType == weaponType && projType == projectileType;
            }
            else if (modInstance != null && weaponName != null && projectileName != null)
            {
                return weapType == modInstance.ItemType(weaponName) &&
                       projType == modInstance.ProjectileType(projectileName);
            }
            else return false;
        }
    }
}