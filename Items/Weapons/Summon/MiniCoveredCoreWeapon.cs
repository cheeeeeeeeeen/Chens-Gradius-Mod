using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using ChensGradiusMod.Projectiles.Summon;
using Microsoft.Xna.Framework;
using static ChensGradiusMod.Items.GradiusGlobalItem;

namespace ChensGradiusMod.Items.Weapons.Summon
{
    public class MiniCoveredCoreWeapon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Uncanny Core Remote Rod");
            Tooltip.SetDefault("Summons a mini Covered Core sentry to launch missiles");
        }

        public override void SetDefaults()
        {
            item.damage = 100;
            item.mana = 20;
            item.width = 50;
            item.height = 50;
            item.useTime = (item.useAnimation = 25);
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.noMelee = true;
            item.knockBack = 4.5f;
            item.value = Item.sellPrice(0, 30, 0, 0);
            item.rare = (int)GradiusRarity.BigCore;
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
            Projectile newProjectile = Main.projectile[projectileIndex];
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(newProjectile.position, newProjectile.width, newProjectile.height, DustID.ApprenticeStorm, 0f, 0f, 0, default, 1.4f);
            }
            Main.projectile[projectileIndex].spriteDirection = -player.direction;
            player.UpdateMaxTurrets();
            return false;
        }

        public override string Texture => "ChensGradiusMod/Sprites/MiniCoveredCoreStaff";
    }
}