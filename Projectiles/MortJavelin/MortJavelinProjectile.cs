using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Projectiles.MortJavelin
{
    public class MortJavelinProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Thornbella");
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
        }

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 74;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 3;
            Projectile.DamageType = DamageClass.Melee;
        }

        private const int MAX_TICKS = 45;

        private readonly float velocity = 12f;
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
                Projectile.velocity.X *= velXmult;
                Projectile.velocity.Y += velYmult;
                if (target != null)
                    if (Vector2.DistanceSquared(Projectile.position, target.Center) >= 16 * BLOCK_LENGTH)
                        Projectile.tileCollide = true;

            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
        }

        const float BLOCK_LENGTH = 16;
        const int HITBOX_SIZE = 28;
        NPC GetTarget()
        {
            NPC closestTarget = null;
            float closestDistance = 99999;
            foreach (NPC NPC in Main.npc)
            {
                float potentialDistance = Vector2.Distance(Projectile.position, NPC.Center);
                if (NPC.friendly || !NPC.active || potentialDistance > 48 * BLOCK_LENGTH
                    || potentialDistance >= closestDistance)
                    continue;
                bool collidesWith = Collision.CanHitLine(Projectile.position, HITBOX_SIZE, HITBOX_SIZE, NPC.position, NPC.width, NPC.height);
                if (collidesWith)
                {
                    closestTarget = NPC;
                    closestDistance = potentialDistance;
                }
            }
            // if (closestDistance <= 16 * BLOCK_LENGTH)
            //     Projectile.tileCollide = false;
            return closestTarget;
        }

        private void Bounce()
        {
            target = GetTarget();

            if (target != null)
            {
                // if ()
                // {
                SoundEngine.PlaySound(SoundID.Item71);
                var direction = -(Projectile.Center - target.Center);
                direction.Normalize();
                Projectile.velocity = direction * (velocity * 2);

                //doost
                Dust dust;
                Vector2 position = Projectile.Center;
                for (int i = 0; i < 17; i++)
                {
                    dust = Main.dust[Terraria.Dust.NewDust(position, 20, 20, DustID.LifeDrain, 0f, 0f, 0, new Color(255, 0, 0), 1.1f)];
                    dust.velocity = Main.rand.NextVector2Unit() * 1.2f;
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
                SoundEngine.PlaySound(SoundID.NPCHit2);
                Projectile.velocity = new Vector2(Projectile.velocity.X, -Projectile.velocity.Y);
            }

            // if (Math.Abs((int)Projectile.velocity.Y) == 0)
            // {
            //    Projectile.Kill();
            // }
        }

        private static float Magnitude(Vector2 mag)
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

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
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
                    TargetWhoAmI = 15;
                    Bounce();
                    break;
                default:
                    if (timeSinceLastBounced > 2)
                        Projectile.Kill();
                    else
                        Bounce();
                    break;
            }
            return false;
        }

        private int TargetWhoAmI
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        private int Phase
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        int timeSinceLastBounced = 0;

        public override bool PreDraw(ref Color lightColor)
        {
            //3hi31mg
            var clr = new Color(255, 255, 255, 255); // full white
            var drawPos = Projectile.Center - Main.screenPosition;
            var off = new Vector2(Projectile.width / 2f, Projectile.height / 2f);
            var origTexture = TextureAssets.Projectile[Projectile.type].Value;
            var texture = ModContent.Request<Texture2D>("MythosOfMoonlight/Projectiles/MortJavelin/MortJavelinGlow").Value;
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            var frame = new Rectangle(0, frameHeight * Projectile.frame, texture.Width, frameHeight - 2);
            var orig = frame.Size() / 2f;

            Main.spriteBatch.Draw(origTexture, drawPos, frame, clr, Projectile.rotation, orig, Projectile.scale, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, drawPos, frame, clr, Projectile.rotation, orig, Projectile.scale, SpriteEffects.None, 0f);

            if (target != null)
            {
                var red = new Color(255, 0, 0, 155);
                var trailLength = ProjectileID.Sets.TrailCacheLength[Projectile.type];
                var fadeMult = 1f / trailLength;
                for (int i = 1; i < trailLength; i++)
                {
                    Main.spriteBatch.Draw(origTexture, Projectile.oldPos[i] - Main.screenPosition + off, frame, red * (1f - fadeMult * i), Projectile.oldRot[i], orig, Projectile.scale * (trailLength - i) / trailLength, SpriteEffects.None, 0f);
                }
            }
            return true;
        }

        private NPC target;
        public override void AI()
        {
            var off = new Vector2(Projectile.width / 2f, Projectile.height / 2f) - new Vector2(0, 9).RotatedBy(Projectile.rotation);
            var dust = Dust.NewDustPerfect(Projectile.position + off + (Projectile.velocity * 2), 235, new Vector2(), 0, new Color(255, 255, 255), 1f);

            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
            Vector2 position = Projectile.Center + new Vector2(-2, 32).RotatedBy(Projectile.rotation);
            dust = Dust.NewDustPerfect(position, 1, new Vector2(0f, 0f), 0, new Color(177, 255, 0), 1f);
            dust.noGravity = true;
            dust.fadeIn = 0.9f;

            NormalAI();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 60);
        }
    }
}