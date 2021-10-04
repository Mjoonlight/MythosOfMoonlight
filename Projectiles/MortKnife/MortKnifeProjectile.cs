using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Projectiles.MortKnife
{
    public class MortKnifeProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("MortKnife");
            Main.projFrames[projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 18;
            projectile.friendly = true;
            projectile.timeLeft = 30;
            projectile.penetrate = 2;
            projectile.melee = true;
        }

        bool hit = false;
        const float BLOCK_LENGTH = 16;
        public override void AI()
        {
            var owner = Main.player[projectile.owner];
            var distance = (projectile.Center - owner.Center);
            projectile.rotation = distance.ToRotation();
            projectile.position += owner.velocity;

            projectile.frameCounter++;
            if (projectile.frameCounter == 1)
            {
                projectile.velocity = projectile.velocity.RotatedByRandom(MathHelper.Pi/6f);
            }

            if (projectile.frameCounter == 15 && !hit)
            {
                Reflect();
            }

            else if (distance.LengthSquared() < 256 && hit)
            {
                projectile.timeLeft = 0;
            }
        }
        void Reflect()
        {
            if (!hit)
            {
                projectile.velocity = projectile.oldVelocity.Length() * Vector2.UnitX.RotatedBy(projectile.rotation - MathHelper.Pi);
                hit = true;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.ai[0]++;
            if (projectile.ai[0] == 1)
                Reflect();
            else
                projectile.timeLeft = 0;
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Reflect();
        }
    }
}