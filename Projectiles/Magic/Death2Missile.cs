﻿using ChensGradiusMod.Items.Weapons.Magic;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.Projectiles.Magic
{
    public class Death2Missile : ModProjectile
    {
        private const float DetectRange = 1000f;
        private const int HomeInTime = 30;
        private const float Acceleration = .4f;
        private const float MaxSpeed = 30f;

        private int homeTick = 0;
        private float currentSpeed = Death2Weapon.MissileSpeed;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Death Missile");
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.tileCollide = true;
            projectile.magic = true;
            projectile.timeLeft = 300 + HomeInTime;
            projectile.light = .4f;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
            if (homeTick++ >= HomeInTime)
            {
                int npcInd = FindTarget(projectile.Center, projectile.Center, DetectRange);
                if (npcInd >= 0)
                {
                    NPC target = Main.npc[npcInd];
                    projectile.rotation = AngularLerp(projectile.rotation.ToRotationVector2(), projectile.Center, target.Center, .1f);
                }
            }
            else
            {
                projectile.rotation = (new Vector2(projectile.ai[0], projectile.ai[1]) - projectile.Center).ToRotation();
            }
            projectile.velocity = projectile.rotation.ToRotationVector2() * currentSpeed;
            currentSpeed = Math.Min(MaxSpeed, currentSpeed + Acceleration);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item14, projectile.Center);
            for (int i = 0; i < 3; i++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Fire, 0f, 0f, 0, default, 1f);
            }
        }

        public override string Texture => "ChensGradiusMod/Sprites/DeathMissile";
    }
}