using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.Projectiles.Melee
{
    public class ZalkYoyoProjectile : ModProjectile
    {
        private const float AngleSpeed = 10f;

        public Projectile[] alliedZalkSlots = new Projectile[4];
        public float currentAngle = 0f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = -1f;
            ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 500f;
            ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 20f;
        }

        public override void SetDefaults()
        {
            projectile.extraUpdates = 0;
            projectile.width = 24;
            projectile.height = 24;
            projectile.aiStyle = 99;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.melee = true;
            projectile.scale = 1f;
            projectile.localNPCHitCooldown = 10;
            projectile.usesLocalNPCImmunity = true;
        }

        public override string Texture => "ChensGradiusMod/Sprites/ZalkYoyoProjectile";

        public override void AI()
        {
            UpdateReferenceAngle();
            DestroyAlliesWhenReturning();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            int slot = GetAvailableSlot();
            if (slot >= 0)
            {
                float xSpawn;
                if (Owner.direction == 1) xSpawn = Main.screenPosition.X - 28;
                else xSpawn = Main.screenPosition.X + Main.screenWidth + 28;
                int newAllyInd = Projectile.NewProjectile(xSpawn, projectile.Center.Y, 0, 0, ModContent.ProjectileType<AlliedZalk>(),
                                                          projectile.damage, projectile.knockBack,
                                                          projectile.owner, projectile.whoAmI, slot);
                if (newAllyInd >= 0)
                {
                    Projectile newProj = Main.projectile[newAllyInd];
                    newProj.spriteDirection = Owner.direction;
                    alliedZalkSlots[slot] = newProj;
                }
            }
        }

        private Player Owner => Main.player[projectile.owner];

        private int GetAvailableSlot()
        {
            for (int i = 0; i < alliedZalkSlots.Length; i++)
            {
                Projectile selectedProj = alliedZalkSlots[i];
                if (selectedProj == null || !selectedProj.active) return i;
            }

            return -1;
        }

        private void UpdateReferenceAngle()
        {
            currentAngle += AngleSpeed * Owner.direction;
            NormalizeAngleDegrees(ref currentAngle);
        }

        private void DestroyAlliesWhenReturning()
        {
            if (projectile.ai[0] < 0)
            {
                for (int i = 0; i < alliedZalkSlots.Length; i++)
                {
                    Projectile selectedProj = alliedZalkSlots[i];
                    if (selectedProj != null && selectedProj.active) selectedProj.Kill();
                }
            }
        }
    }
}