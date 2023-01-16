using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using MythosOfMoonlight.Dusts;
using Terraria.Audio;

namespace MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim.Projectiles
{
    public class StarineSigil : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starine Sigil");
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
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(drawTexture, Projectile.Center - Main.screenPosition, sourceRectangle, new Color(Projectile.alpha, Projectile.alpha, Projectile.alpha, Projectile.alpha), Projectile.rotation, drawTexture.Size() / 2, 1, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(drawTexture1, Projectile.Center - Main.screenPosition, sourceRectangle, new Color(Projectile.alpha, Projectile.alpha, Projectile.alpha, Projectile.alpha), -Projectile.rotation, drawTexture.Size() / 2, 1, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(drawTexture2, Projectile.Center - Main.screenPosition, sourceRectangle, new Color(Projectile.alpha, Projectile.alpha, Projectile.alpha, Projectile.alpha), -Projectile.rotation, drawTexture.Size() / 2, 1, SpriteEffects.None, 0);

            if (!Main.dayTime)
            {
                Main.EntitySpriteDraw(drawTextureGlow, Projectile.Center - Main.screenPosition, sourceRectangle, new Color(Projectile.alpha, Projectile.alpha, Projectile.alpha, Projectile.alpha), Projectile.rotation, drawTexture.Size() / 2, 1, SpriteEffects.None, 0);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 45;
        }
        int projDamage = 8;
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 3; i++)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Main.rand.NextVector2Unit() * 5, ModContent.ProjectileType<StarineShaft>(), 10, 0);
            }
            SoundEngine.PlaySound(SoundID.Item9, Projectile.Center);
            for (int i = 0; i < 30; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<StarineDust>(), 2f);
                Main.dust[dust].scale = 2f;
                Main.dust[dust].velocity = Main.rand.NextVector2Unit() * 1.2f;
                Main.dust[dust].noGravity = true;
            }
            for (int i = 0; i < 60; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<StarineDust>(), 2f);
                Main.dust[dust].scale = 2f;
                Main.dust[dust].velocity = Main.rand.NextVector2Unit() * 4f;
                Main.dust[dust].noGravity = true;
            }
        }
        int ProjectileTimer
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        static double RandRadian => Main.rand.NextDouble() * (MathHelper.PiOver2 / 3f) - (MathHelper.PiOver2 / 6f);
        public override void AI()
        {
            if (!Main.expertMode)
            {
                if (++ProjectileTimer % 20 == 0)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, 10.5f * Utils.RotatedBy(Projectile.DirectionTo(Main.player[Main.myPlayer].Center), RandRadian * .5f), ModContent.ProjectileType<StarineShaft>(), projDamage, 0);
                    SoundEngine.PlaySound(SoundID.Item9, Projectile.Center);
                    if (ProjectileTimer == 60)
                        Projectile.Kill();
                }
            }
            else
            {
                if (++ProjectileTimer >= 15)
                {
                    if (ProjectileTimer == 55)
                        Projectile.Kill();
                    else if (ProjectileTimer % 15 == 0)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, 10.5f * Utils.RotatedBy(Projectile.DirectionTo(Main.player[Main.myPlayer].Center), RandRadian * .5f), ModContent.ProjectileType<StarineShaft>(), projDamage, 0);
                        SoundEngine.PlaySound(SoundID.Item9, Projectile.Center);
                    }
                }
            }
        }
    }
}