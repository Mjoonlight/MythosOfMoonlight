using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.Dusts;
using Microsoft.Xna.Framework.Graphics;

namespace MythosOfMoonlight.Projectiles.IridicProjectiles
{
    public class FragmentBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Fragment Bullet");
            Projectile.AddElement(CrossModHelper.Celestial);
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = 3;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.localNPCHitCooldown = 15;
            Projectile.aiStyle = -1;
            Projectile.usesLocalNPCImmunity = true;
            //Projectile.alpha = 255;
        }
        /*public override void OnSpawn(IEntitySource source)
        {
            Projectile.ai[0] = TRay.CastLength(Projectile.Center, Projectile.velocity, 1100);
            Projectile.velocity.Normalize();
        }*/
        public override void AI()
        {
            if (Main.player[Projectile.owner].ownedProjectileCounts[Projectile.type] > 3)
                Projectile.Kill();
            /*if (Projectile.timeLeft == 599)
            {
                Projectile.ai[0] = TRay.CastLength(Projectile.Center, Projectile.velocity, 1100);
            }*/

            MathHelper.Clamp(Projectile.ai[0], 0, 1100);
            if (Projectile.timeLeft == 598)
            {
                if (Projectile.ai[0] < 40)
                {
                    Projectile.Kill();
                    return;
                }

                if (Projectile.ai[0] < 1099)
                {
                    Projectile.ai[1] = 1;
                    Projectile.damage = 0;

                    for (int i = 0; i < 5; i++)
                    {
                        Helper.SpawnDust(Projectile.Center + Projectile.velocity * Projectile.ai[0], Projectile.Size, ModContent.DustType<PurpurineDust>());
                    }
                }
            }
            //Main.NewText(Projectile.ai[0]);
            Projectile.scale -= 0.05f;
            if (Projectile.scale <= 0)
                Projectile.Kill();
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float a = 0;
            if (Projectile.ai[0] == 0 || Projectile.ai[1] == 1)
                return false;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.velocity * (Projectile.ai[0] + targetHitbox.Width), 1, ref a) && Projectile.ai[1] == 0 && Projectile.ai[0] != 0;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.Reload(BlendState.Additive);
            if (Projectile.timeLeft < 599)
            {
                for (int i = 0; i < Projectile.ai[0]; i++)
                {
                    Texture2D tex = Helper.GetTex("MythosOfMoonlight/Textures/Extra/Ex3");
                    Main.spriteBatch.Draw(tex, Projectile.Center + i * Projectile.velocity - Main.screenPosition, null, Color.Lerp(Color.White, Color.Purple, Projectile.scale) * Projectile.scale, Projectile.velocity.ToRotation(), new Vector2(0, tex.Height / 2), new Vector2(1, Projectile.scale / 10), SpriteEffects.None, 0);
                }
            }
            //Utils.DrawLine(Main.spriteBatch, Projectile.Center, Projectile.Center + Projectile.velocity * Projectile.ai[0], Color.Lerp(Color.White, Color.Purple, Projectile.scale) * Projectile.scale, Color.Lerp(Color.Purple, Color.White, Projectile.scale) * Projectile.scale, Projectile.width * Projectile.scale);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        /*public override Color? GetAlpha(Color lightColor)
        {
            float p = (255 - (float)Projectile.alpha) / 255f;
            return Color.Lerp(lightColor, Color.White, .5f * p);
        }*/
        public override bool? CanDamage()
        {
            return Projectile.ai[1] == 0 && Projectile.ai[0] != 0;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[1] == 0)
            {
                if (target.life <= 0)
                    Projectile.Kill();
                Projectile.ai[0] = Helper.FromAToB(Projectile.Center, target.Center + new Vector2(0, target.width / 2).RotatedBy(-Projectile.velocity.ToRotation()), false).Length();
                for (int i = 1; i <= 3; i++)
                {
                    Vector2 vel = -Utils.SafeNormalize(Projectile.oldVelocity, Vector2.Zero).RotatedBy(Main.rand.NextFloat(-.66f, .66f)) * Main.rand.NextFloat(1f, 2f);
                    Dust dust = Dust.NewDustDirect(target.Center, Projectile.width, Projectile.height, ModContent.DustType<PurpurineDust>(), vel.X, vel.Y, 0, default, Main.rand.NextFloat(.6f, 1.2f));
                    dust.noGravity = true;
                    dust.velocity = vel;
                }
            }

            Projectile.ai[1] = 1;
        }
        /*public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 1; i <= 3; i++)
            {
                Vector2 vel = -Utils.SafeNormalize(Projectile.oldVelocity, Vector2.Zero).RotatedBy(Main.rand.NextFloat(-.66f, .66f)) * Main.rand.NextFloat(1f, 2f);
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<PurpurineDust>(), vel.X, vel.Y, 0, default, Main.rand.NextFloat(.6f, 1.2f));
                dust.noGravity = true;
                dust.velocity = vel;
            }
            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
            return base.OnTileCollide(oldVelocity);
        }*/
    }
}
