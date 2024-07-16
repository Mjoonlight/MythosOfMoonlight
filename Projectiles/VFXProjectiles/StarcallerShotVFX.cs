using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using MythosOfMoonlight.Dusts;

namespace MythosOfMoonlight.Projectiles.VFXProjectiles
{
    public class StarcallerShotVFX : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/Textures/Extra/MagicBurst";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.height = 1;
            Projectile.width = 1;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
        }
        public override bool ShouldUpdatePosition() => false;
        public override void OnSpawn(IEntitySource source)
        {
            if (Projectile.ai[2] == 0)
            {
                for (int i = 0; i < 20; i++)
                {
                    int dust = Dust.NewDust(Projectile.Center, 1, 1, ModContent.DustType<StarineDust>(), 2f);
                    Main.dust[dust].scale = Main.rand.NextFloat(1, 2);
                    Main.dust[dust].velocity = Projectile.velocity.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(1, 5);
                    Main.dust[dust].noGravity = true;
                }
                for (int i = 0; i < 10; i++)
                {
                    int dust = Dust.NewDust(Projectile.Center, 1, 1, ModContent.DustType<StarineDust>(), 2f);
                    Main.dust[dust].scale = Main.rand.NextFloat(1, 2);
                    Main.dust[dust].velocity = Projectile.velocity.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(5, 10);
                    Main.dust[dust].noGravity = true;
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Texture2D tex2 = Helper.GetExtraTex("Extra/star_02");
            Main.spriteBatch.Reload(BlendState.Additive);
            float alpha = MathHelper.Lerp(1, 0, Projectile.ai[0]);
            Color col = (Color.Lerp(Color.White, new Color(44, 137, 215), MathHelper.Clamp(Projectile.ai[0] * 2, 0, 1)));
            //for (int i = 0; i < 2; i++)
            //  Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * 324, 353, 324), col * alpha, Projectile.rotation, new Vector2(353, 324) / 2, Projectile.ai[0] * 0.35f, SpriteEffects.None, 0);

            Main.spriteBatch.Reload(MythosOfMoonlight.SpriteRotation);
            Vector2 scale = new Vector2(0.25f, 1f);
            MythosOfMoonlight.SpriteRotation.Parameters["scale"].SetValue(scale * 0.5f * Projectile.ai[0]);
            MythosOfMoonlight.SpriteRotation.Parameters["uColor"].SetValue(col.ToVector4() * alpha);
            MythosOfMoonlight.SpriteRotation.Parameters["rotation"].SetValue(MathHelper.PiOver2);
            for (int i = 0; i < 3; i++)
                Main.spriteBatch.Draw(tex2, Projectile.Center + Projectile.velocity * Projectile.ai[0] * 25 - Main.screenPosition, null, Color.White, Projectile.rotation, tex2.Size() / 2, MathHelper.Clamp(Projectile.ai[0], 0, 0.5f), SpriteEffects.None, 0);
            Main.spriteBatch.Reload(effect: null);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            Projectile.ai[0] += 0.1f;
            if (Projectile.ai[0] > 1)
                Projectile.Kill();
        }
    }
}
