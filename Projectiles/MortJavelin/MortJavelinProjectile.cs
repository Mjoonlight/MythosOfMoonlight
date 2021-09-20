using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Projectiles.MortJavelin
{
    public class MortJavelinProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mortiflora Javelin");
            Main.projFrames[projectile.type] = 1;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 40;
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

        private float velocity = 12f;
        private void NormalAI()
        {
            TargetWhoAmI++;
            timeSinceLastBounced++;

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
                if ((npc.friendly || !npc.active || potentialDistance > 48 * BLOCK_LENGTH) 
                    || potentialDistance >= closestDistance)
                    continue;
                bool collidesWith = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);
                if (collidesWith)
                {
                    closestTarget = npc;
                    closestDistance = potentialDistance;
                }
            }
            if (closestDistance <= 16 * BLOCK_LENGTH)
                projectile.tileCollide = false;
            return closestTarget;
        }

        private void Bounce()
        {
            target = GetTarget();

            if (target != null)
            {
                // if ()
                // {
                Main.PlaySound(SoundID.Item71);
                var direction = -(projectile.Center - target.Center);
                direction.Normalize();
                projectile.velocity = direction * (velocity * 2);

                //doost
                Dust dust;
                Vector2 position = projectile.Center;
                for (int i = 0; i < 17; i++)
                {
                    dust = Main.dust[Terraria.Dust.NewDust(position, 20, 20, 1, 0f, 0f, 0, new Color(255, 0, 0), 1f)];
                    dust.fadeIn = 0f;
                }

                // }

                // else
                // {
                //     hitTarget = true;
                // }
            }

            else
            {
                Main.PlaySound(SoundID.NPCHit2);
                projectile.velocity = new Vector2(projectile.velocity.X, -projectile.velocity.Y);
            }

            // if (Math.Abs((int)projectile.velocity.Y) == 0)
            // {
            //    projectile.Kill();
            // }
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
                    if (timeSinceLastBounced > 2)
                        projectile.Kill();
                    else
                        Bounce();
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

        int timeSinceLastBounced = 0;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //3hi31mg
            var clr = new Color(255, 255, 255, 255); // full white
            var drawPos = projectile.Center - Main.screenPosition;
            var off = new Vector2(projectile.width / 2f, projectile.height / 2f);
            var origTexture = Main.projectileTexture[projectile.type];
            var texture = mod.GetTexture("Projectiles/MortJavelin/MortJavelinGlow");
            int frameHeight = texture.Height / Main.projFrames[projectile.type];
            var frame = new Rectangle(0, frameHeight * projectile.frame, texture.Width, frameHeight - 2);
            var orig = frame.Size() / 2f;

            Main.spriteBatch.Draw(origTexture, drawPos, frame, clr, projectile.rotation, orig, projectile.scale, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, drawPos, frame, clr, projectile.rotation, orig, projectile.scale, SpriteEffects.None, 0f);

            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[projectile.type];
            if (target != null)
            {
                var red = new Color(255, 0, 0, 155);
                for (int i = 1; i < ProjectileID.Sets.TrailCacheLength[projectile.type]; i++)
                {
                    Main.spriteBatch.Draw(texture, projectile.oldPos[i] - Main.screenPosition, frame, red * (1f - fadeMult * i), projectile.oldRot[i], orig, projectile.scale, SpriteEffects.None, 0f);
                }
            }
            return false;
        }

        private NPC target;
        public override void AI()
        {
            var off = new Vector2(projectile.width / 2f, projectile.height / 2f) - new Vector2(0, 9).RotatedBy(projectile.rotation);
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[projectile.type];
            var dust = Terraria.Dust.NewDustPerfect(projectile.position + off + (projectile.velocity * 2), 235, new Vector2(), 0, new Color(255, 255, 255), 1f);

            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
            Vector2 position = projectile.Center + new Vector2(-2, 32).RotatedBy(projectile.rotation);
            dust = Terraria.Dust.NewDustPerfect(position, 1, new Vector2(0f, 0f), 0, new Color(177, 255, 0), 1f);
            dust.noGravity = true;
            dust.fadeIn = 0.9f;

            NormalAI();
        }
    }
}