using ChensGradiusMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;
using static ChensGradiusMod.Items.GradiusGlobalItem;

namespace ChensGradiusMod.Items.Weapons.Magic
{
    public class Death2Weapon : ModItem
    {
        public const float MissileSpeed = .5f;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("DES Advanced Cannon");
            Tooltip.SetDefault("Left click to shoot a highly concentrated energy laser\nRight click to fire a barrage of missiles");
        }

        public override void SetDefaults()
        {
            item.damage = 200;
            item.noMelee = true;
            item.magic = true;
            item.mana = 30;
            item.rare = (int)GradiusRarity.BigCore;
            item.useAnimation = item.useTime = 20;
            item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Enemies/BigCoreShoot");
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shoot = ModContent.ProjectileType<Death2Laser>();
            item.value = Item.sellPrice(gold: 30);
            item.knockBack = 0f;
            item.shootSpeed = MissileSpeed;
            item.autoReuse = false;
            AssignItemDimensions(item, 96, 64);
        }

        public override string Texture => "ChensGradiusMod/Sprites/DeathWeapon";

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Enemies/Gradius3Hit");
                item.knockBack = 8f;
                item.shoot = ModContent.ProjectileType<Death2Missile>();
            }
            else
            {
                item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Enemies/BigCoreShoot");
                item.knockBack = 0f;
                item.shoot = ModContent.ProjectileType<Death2Laser>();
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2)
            {
                Vector2 velocity = new Vector2(speedX, speedY);
                velocity.Normalize();
                position += velocity * 80;

                Vector2 firstAnchor = position + (velocity * 1000);
                float angle = velocity.ToRotation() + MathHelper.PiOver2;
                Vector2 targetPosition = firstAnchor + (angle.ToRotationVector2() * 100);
                Projectile.NewProjectile(position + (angle.ToRotationVector2() * 2), velocity, type, damage, knockBack, player.whoAmI, targetPosition.X, targetPosition.Y);

                targetPosition = firstAnchor + (angle.ToRotationVector2() * 300);
                Projectile.NewProjectile(position + (angle.ToRotationVector2() * 6), velocity, type, damage, knockBack, player.whoAmI, targetPosition.X, targetPosition.Y);

                angle = velocity.ToRotation() + (3f * MathHelper.PiOver2);
                targetPosition = firstAnchor + (angle.ToRotationVector2() * 100);
                Projectile.NewProjectile(position + (angle.ToRotationVector2() * 2), velocity, type, damage, knockBack, player.whoAmI, targetPosition.X, targetPosition.Y);

                targetPosition = firstAnchor + (angle.ToRotationVector2() * 300);
                Projectile.NewProjectile(position + (angle.ToRotationVector2() * 6), velocity, type, damage, knockBack, player.whoAmI, targetPosition.X, targetPosition.Y);

                return false;
            }
            else return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }
    }
}