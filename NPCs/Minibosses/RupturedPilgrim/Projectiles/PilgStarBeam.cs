using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim.Projectiles
{
    public class PilgStarBeam : ModProjectile
    {
        public override string Texture => Helper.Empty;
        const int max = 60;
        public override void SetDefaults()
        {
            Projectile.Size = Vector2.One;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.timeLeft = max;
        }
        public override bool ShouldUpdatePosition() => false;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float a = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.rotation.ToRotationVector2() * 840, 29, ref a) && Projectile.scale > 0.5f;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            float progress = Utils.GetLerpValue(0, max, Projectile.timeLeft);
            Projectile.scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.Reload(BlendState.Additive);
            Texture2D tex = Helper.GetTex("MythosOfMoonlight/Textures/Extra/Ex3");
            Texture2D tex2 = Helper.GetTex("MythosOfMoonlight/Textures/Extra/star_03");
            Vector2 pos = Projectile.Center;
            Vector2 scale = new Vector2(0.5f, 0.5f * Projectile.scale);
            for (int i = 0; i < 420 * 2; i++)
            {
                Main.spriteBatch.Draw(tex, pos - Main.screenPosition, null, Color.Cyan, Projectile.rotation, new Vector2(0, tex.Height / 2), scale, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(tex, pos - Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(0, tex.Height / 2), scale, SpriteEffects.None, 0);
                pos += Projectile.rotation.ToRotationVector2();
            }
            Main.spriteBatch.Draw(tex2, Projectile.Center - Main.screenPosition, null, Color.Cyan, Main.GameUpdateCount * 0.003f, tex2.Size() / 2, scale.Y, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(tex2, Projectile.Center - Main.screenPosition, null, Color.White, Main.GameUpdateCount * 0.003f, tex2.Size() / 2, scale.Y, SpriteEffects.None, 0);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
    }
}
