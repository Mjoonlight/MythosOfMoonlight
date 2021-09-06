using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Projectiles.MortJavelin
{
    public class MortJavelin : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mortiflora Javelin");
            Main.projFrames[projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            projectile.width = 28;
            projectile.height = 74;
            projectile.friendly = true;
            projectile.timeLeft = 300;
            projectile.penetrate = 3;
            projectile.melee = true;
        }

        private const int MAX_TICKS = 45;

        private void NormalAI()
        {
            TargetWhoAmI++;

            // For a little while, the javelin will travel with the same speed, but after this, the javelin drops velocity very quickly.
            if (TargetWhoAmI >= MAX_TICKS)
            {
                const float velXmult = 0.98f;
                const float velYmult = 0.35f;
                TargetWhoAmI = MAX_TICKS;
                projectile.velocity.X *= velXmult;
                projectile.velocity.Y += velYmult;
            }

            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
        }

        const float BLOCK_LENGTH = 16;
        NPC GetTarget()
        {
            NPC closestTarget = null;
            float closestDistance = 99999;
            foreach (NPC npc in Main.npc)
            {
                float potentialDistance = Vector2.Distance(projectile.position, npc.Center);
                if ((npc.friendly || potentialDistance > 81 * BLOCK_LENGTH) || potentialDistance >= closestDistance)
                    continue;
                closestTarget = npc;
            }
            return closestTarget;
        }

        private void Bounce()
        {
            target = GetTarget();
            bool hitTarget = false;
            if (target != null)
            {
                if (Collision.CanHitLine(projectile.position, projectile.width, projectile.height, target.position, target.width, target.height))
                {
                    var direction = -(projectile.position - target.position);
                    direction.Normalize();
                    projectile.velocity = direction * Magnitude(projectile.velocity.RotatedBy(15));
                }

                else
                {
                    hitTarget = true;
                }
            }

            if (hitTarget)
            {
                projectile.velocity = new Vector2(projectile.velocity.X, -projectile.velocity.Y);
            }

            // if (Math.Abs((int)projectile.velocity.Y) == 0)
            // {
            //    projectile.Kill();
            // }
            TargetWhoAmI = 0;
        }

        private float Magnitude(Vector2 mag)
        {
            return (float)Math.Sqrt(mag.X * mag.X + mag.Y * mag.Y);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // Inflate some target hitboxes if they are beyond 8,8 size
            if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
            {
                targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
            }
            // Return if the hitboxes intersects, which means the javelin collides or not
            return projHitbox.Intersects(targetHitbox);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 28;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            switch (Phase)
            {
                case 0:
                    Phase++;
                    Bounce();
                    break;
                default:
                    projectile.Kill();
                    break;
            }
            return false;
        }

        private int TargetWhoAmI
        {
            get => (int)projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        private int Phase
        {
            get => (int)projectile.ai[1];
            set => projectile.ai[1] = value;
        }

        private NPC target;
        public override void AI()
        {
            NormalAI();
        }
    }
}