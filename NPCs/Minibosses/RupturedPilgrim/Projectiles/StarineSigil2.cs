using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MythosOfMoonlight.Dusts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim.Projectiles
{
    public class StarineSigil2 : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/NPCs/Minibosses/RupturedPilgrim/Projectiles/StarineSigil";
        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 100;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.rotation += .1f;
            Projectile.alpha += 50;
            Texture2D drawTexture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D drawTextureGlow = ModContent.Request<Texture2D>(Projectile.ModProjectile.Texture + "_Glow").Value;
            Texture2D drawTexture1 = ModContent.Request<Texture2D>(Projectile.ModProjectile.Texture + "_Extra1").Value;
            Texture2D drawTexture2 = ModContent.Request<Texture2D>(Projectile.ModProjectile.Texture + "_Extra2").Value;
            Rectangle sourceRectangle = new(0, 0, drawTexture.Width, drawTexture.Height);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(drawTexture, Projectile.Center - Main.screenPosition, sourceRectangle, new Color(Projectile.alpha, Projectile.alpha, Projectile.alpha, Projectile.alpha), Projectile.rotation, drawTexture.Size() / 2, MathHelper.Clamp(Projectile.scale * 2, 0, 1), SpriteEffects.None, 0);
            Main.EntitySpriteDraw(drawTexture1, Projectile.Center - Main.screenPosition, sourceRectangle, new Color(Projectile.alpha, Projectile.alpha, Projectile.alpha, Projectile.alpha), -Projectile.rotation, drawTexture.Size() / 2, MathHelper.Clamp(Projectile.scale * 5, 0, 1), SpriteEffects.None, 0);
            Main.EntitySpriteDraw(drawTexture2, Projectile.Center - Main.screenPosition, sourceRectangle, new Color(Projectile.alpha, Projectile.alpha, Projectile.alpha, Projectile.alpha), -Projectile.rotation, drawTexture.Size() / 2, MathHelper.Clamp(Projectile.scale, 0, 1), SpriteEffects.None, 0);

            if (!Main.dayTime)
            {
                Main.EntitySpriteDraw(drawTextureGlow, Projectile.Center - Main.screenPosition, sourceRectangle, new Color(Projectile.alpha, Projectile.alpha, Projectile.alpha, Projectile.alpha), Projectile.rotation, drawTexture.Size() / 2, MathHelper.Clamp(Projectile.scale * 2, 0, 1), SpriteEffects.None, 0);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            float offset = Helper.FromAToB(Projectile.Center, Main.LocalPlayer.Center).ToRotation();
            for (int i = 0; i < 5; i++)
            {
                float angle = Helper.CircleDividedEqually(i, 5) + offset;
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.UnitX.RotatedBy(angle) * 3, ModContent.ProjectileType<StarineShaft>(), 10, 0, ai2: 1);
            }
            SoundStyle style = SoundID.Item82;
            SoundEngine.PlaySound(style, Projectile.Center);
            for (int i = 0; i < 30; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<StarineDust>(), 2f);
                Main.dust[dust].scale = 2f;
                Main.dust[dust].velocity = Main.rand.NextVector2Circular(5, 5);
                Main.dust[dust].noGravity = true;
            }
            for (int i = 0; i < 60; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<StarineDust>(), 2f);
                Main.dust[dust].scale = 2f;
                Main.dust[dust].velocity = Main.rand.NextVector2Circular(5, 5);
                Main.dust[dust].noGravity = true;
            }
        }
        public override void AI()
        {
            float progress = Utils.GetLerpValue(0, 100, Projectile.timeLeft);
            Projectile.scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 1.25f, 0, 1 + (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 0.5f) * 0.25f));
        }
    }
}
