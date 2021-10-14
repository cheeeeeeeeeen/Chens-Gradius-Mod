using ChensGradiusMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ChensGradiusMod.Items.GradiusGlobalItem;

namespace ChensGradiusMod.Items.Weapons.Ranged
{
    public class GarunLauncher : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bacterian Frigate");
            Tooltip.SetDefault("Spacecraft carrier made more portable\nDeploy Garuns that shoot towards hostiles");
        }

        public override void SetDefaults()
        {
            item.damage = 120;
            item.ranged = true;
            item.width = 76;
            item.height = 50;
            item.useAnimation = item.useTime = 30;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 2;
            item.value = Item.sellPrice(gold: 30);
            item.rare = (int)GradiusRarity.BigCore;
            item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/VictoryViper/ShootMissile");
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<AlliedGarun>();
            item.shootSpeed = 10f;
            item.useAmmo = AmmoID.Rocket;
        }

        public override string Texture => "ChensGradiusMod/Sprites/GarunLauncher";

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-40, 0);
        }
    }
}