using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Projectiles.VFXProjectiles
{
    internal class QuickFlare : ModProjectile
    {
        public override string Texture => Helper.Empty;
        public override void SetDefaults()
        {
            Projectile.height = 300;
            Projectile.width = 300;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hitinfo, int damage)
        {
            Projectile.ai[1] = 1;
        }
        public override void PostAI()
        {
            if (Projectile.ai[1] == 1)
                Projectile.damage = 0;
        }
        public override bool ShouldUpdatePosition() => false;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Helper.GetTex("MythosOfMoonlight/Assets/Textures/Extra/crosslight");
            Texture2D tex2 = Helper.GetTex("MythosOfMoonlight/Assets/Textures/Extra/star_06");
            Main.spriteBatch.Reload(BlendState.Additive);
            float alpha = MathHelper.Lerp(1, 0, Projectile.ai[0]);
            Main.spriteBatch.Draw(tex2, Projectile.Center - Main.screenPosition, null, Color.White * 0.75f * Projectile.ai[0], Projectile.rotation, tex.Size() / 2, alpha * 2f, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.Cyan * Projectile.ai[0], Projectile.rotation, tex.Size() / 2, alpha * 2, SpriteEffects.None, 0);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
        public override void AI()
        {
            Projectile.ai[0] += 0.05f;
            if (Projectile.ai[0] > 1)
                Projectile.Kill();
        }
    }
    internal class BigQuickFlare : ModProjectile
    {
        public override string Texture => Helper.Empty;
        public override void SetDefaults()
        {
            Projectile.height = 5;
            Projectile.width = 5;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }
        public override bool ShouldUpdatePosition() => false;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Helper.GetTex("MythosOfMoonlight/Assets/Textures/Extra/crosslight");
            Texture2D tex2 = Helper.GetTex("MythosOfMoonlight/Assets/Textures/Extra/star_06");
            Main.spriteBatch.Reload(BlendState.Additive);
            float alpha = MathHelper.Lerp(1, 0, Projectile.ai[0] / 2);
            Main.spriteBatch.Draw(tex2, Projectile.Center - Main.screenPosition, null, Color.White * 0.75f * Projectile.ai[0], Projectile.rotation, tex.Size() / 2, alpha * 2.5f, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.Cyan * Projectile.ai[0], Projectile.rotation, tex.Size() / 2, alpha * 2.5f, SpriteEffects.None, 0);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
        public override void AI()
        {
            Projectile.ai[0] += 0.05f;
            if (Projectile.ai[0] > 2)
                Projectile.Kill();
        }
    }
    internal class EntroQuickFlare : ModProjectile
    {
        public override string Texture => Helper.Empty;
        public override void SetDefaults()
        {
            Projectile.height = 5;
            Projectile.width = 5;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }
        public override bool ShouldUpdatePosition() => false;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Helper.GetTex("MythosOfMoonlight/Assets/Textures/Extra/crosslight");
            Texture2D tex2 = Helper.GetTex("MythosOfMoonlight/Assets/Textures/Extra/star_06");
            Main.spriteBatch.Reload(BlendState.Additive);
            float alpha = MathHelper.Lerp(1, 0, Projectile.ai[0]);
            Main.spriteBatch.Draw(tex2, Projectile.Center - Main.screenPosition, null, Color.White * 0.15f * Projectile.ai[0], Projectile.rotation, tex.Size() / 2, alpha * 1.5f, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.Pink * Projectile.ai[0], Projectile.rotation, tex.Size() / 2, alpha * 1.5f, SpriteEffects.None, 0);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
        public override void AI()
        {
            Projectile.ai[0] += 0.05f;
            if (Projectile.ai[0] > 1)
                Projectile.Kill();
        }
    }
}
