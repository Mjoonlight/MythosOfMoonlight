using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.Dusts;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System.Drawing.Drawing2D;
using Terraria.GameContent;
using MythosOfMoonlight.Graphics;
using MythosOfMoonlight.Graphics.Particles;

namespace MythosOfMoonlight.Projectiles.IridicProjectiles
{
    public class FragmentBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fragment Bullet");
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = 3;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            //Projectile.alpha = 255;
        }
        public override void AI()
        {
            Projectile.velocity.Normalize();
            //if (Projectile.alpha > 0) Projectile.alpha -= 15;
            if (Projectile.timeLeft == 599 && TRay.CastLength(Projectile.Center, Projectile.velocity, 1920, true) < 1920)
            {
                for (int i = 0; i < 5; i++)
                    Helper.SpawnDust(TRay.Cast(Projectile.Center, Projectile.velocity, 1920, true), Projectile.Size, ModContent.DustType<PurpurineDust>());
            }
            Projectile.scale -= 0.05f;
            if (Projectile.scale <= 0)
                Projectile.Kill();
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float a = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.velocity * TRay.CastLength(Projectile.Center, Projectile.velocity, 1920, false), 6, ref a);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.Reload(BlendState.Additive);
            Utils.DrawLine(Main.spriteBatch, Projectile.Center, Projectile.Center + Projectile.velocity * TRay.CastLength(Projectile.Center, Projectile.velocity, 1920, true), Color.Lerp(Color.White, Color.Purple, Projectile.scale) * Projectile.scale, Color.Lerp(Color.Purple, Color.White, Projectile.scale) * Projectile.scale, Projectile.width * Projectile.scale);
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
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.damage = 0;
            Projectile.scale -= 0.1f;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            for (int i = 1; i <= 3; i++)
            {
                Vector2 vel = -Utils.SafeNormalize(Projectile.oldVelocity, Vector2.Zero).RotatedBy(Main.rand.NextFloat(-.66f, .66f)) * Main.rand.NextFloat(1f, 2f);
                Dust dust = Dust.NewDustDirect(target.Center, Projectile.width, Projectile.height, ModContent.DustType<PurpurineDust>(), vel.X, vel.Y, 0, default, Main.rand.NextFloat(.6f, 1.2f));
                dust.noGravity = true;
                dust.velocity = vel;
            }
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
