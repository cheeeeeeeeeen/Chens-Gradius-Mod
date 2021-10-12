using ChensGradiusMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ChensGradiusMod.Items.GradiusGlobalItem;

namespace ChensGradiusMod.Items.Weapons.Magic
{
    public class Death2Weapon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("DES Laser Cannon");
            Tooltip.SetDefault("Shoot a highly concentrated energy laser");
        }

        public override void SetDefaults()
        {
            item.damage = 200;
            item.noMelee = true;
            item.magic = true;
            item.mana = 30;
            item.rare = (int)GradiusRarity.BigCore;
            item.width = 96;
            item.height = 64;
            item.useAnimation = item.useTime = 20;
            item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Enemies/BigCoreShoot");
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shoot = ModContent.ProjectileType<Death2Laser>();
            item.value = Item.sellPrice(gold: 30);
            item.knockBack = 0f;
            item.shootSpeed = 20f;
        }

        public override string Texture => "ChensGradiusMod/Sprites/DeathWeapon";

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
    }
}