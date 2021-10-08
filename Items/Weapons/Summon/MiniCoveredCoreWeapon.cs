using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using ChensGradiusMod.Projectiles.Summon;
using Microsoft.Xna.Framework;

namespace ChensGradiusMod.Items.Weapons.Summon
{
    public class MiniCoveredCoreWeapon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Uncanny Spherical Remote");
            Tooltip.SetDefault("Summons a mini Covered Core sentry to launch missiles");
        }

        public override void SetDefaults()
        {
            item.damage = 100;
            item.mana = 20;
            item.width = 42;
            item.height = 40;
            item.useTime = (item.useAnimation = 25);
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.noMelee = true;
            item.knockBack = 4.5f;
            item.value = Item.buyPrice(0, 1, 0, 0);
            item.rare = ItemRarityID.Lime;
            item.UseSound = SoundID.Item113;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<MiniCoveredCore>();
            item.shootSpeed = 10f;
            item.summon = true;
            item.sentry = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int projectileIndex = Projectile.NewProjectile(Main.MouseWorld, Vector2.Zero, type, damage, knockBack, player.whoAmI, 0f, 0f);
            Main.projectile[projectileIndex].spriteDirection = -player.direction;
            player.UpdateMaxTurrets();
            return false;
        }

        public override string Texture => "ChensGradiusMod/Sprites/PlaceholderWeapon";
    }
}