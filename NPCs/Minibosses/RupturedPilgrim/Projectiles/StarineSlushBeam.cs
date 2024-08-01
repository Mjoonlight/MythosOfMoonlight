using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using MythosOfMoonlight.Dusts;
using Terraria.Audio;
using MythosOfMoonlight.Common.Systems;

namespace MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim.Projectiles
{
    public class StarineSlushBeam : ModProjectile
    {
        public override string Texture => Helper.Empty;
        const int max = 80;
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
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.rotation.ToRotationVector2() * 1024, 60, ref a) && Projectile.scale > 0.5f;
        }
        float visualOffset;
        float visual1, visual2;
        float Timer;
        float scaleOff;
        public override void AI()
        {
            Timer++;
            scaleOff = MathHelper.Lerp(scaleOff, MathF.Sin(Timer * .05f) * 0.05f, 0.25f);
            Projectile.velocity = Vector2.UnitY;
            for (int i = 0; i < 5; i++)
            {
                int dust = Dust.NewDust(Projectile.position - new Vector2(30, 0), 60, (int)Projectile.ai[0], ModContent.DustType<StarineDust>(), 2f);
                Main.dust[dust].scale = 2f;
                Main.dust[dust].velocity = Main.rand.NextVector2Circular(5, 5);
                Main.dust[dust].noGravity = true;
            }
            if (++Projectile.ai[1] == 70)
                for (int i = -1; i < 2; i++)
                {
                    if (i == 0)
                        continue;
                    Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + (Projectile.ai[0] - 30) * Vector2.UnitY, Vector2.UnitX * i, ModContent.ProjectileType<StarineShockwave>(), 17, .1f, Main.myPlayer).ai[1] = i;
                }
            if (Projectile.ai[1] == 10)
                for (int i = -1; i < 2; i++)
                {
                    if (i == 0)
                        continue;
                    Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + (Projectile.ai[0] - 30) * Vector2.UnitY, Vector2.UnitX * i, ModContent.ProjectileType<StarineShockwave>(), 17, .1f, Main.myPlayer).ai[1] = i;
                }
            if (Projectile.ai[0] == 0)
            {
                float len = TRay.CastLength(Projectile.Center, Projectile.velocity, 1024);
                for (int i = 0; i < 80; i++)
                {
                    Dust dust = Dust.NewDustPerfect(Projectile.Center + new Vector2(0, len), ModContent.DustType<StarineDust>(), Main.rand.NextVector2Circular(15, 15));
                    dust.scale = 2f;
                    dust.noGravity = true;
                }
                Projectile.ai[0] = len;
            }
            if (visualOffset == 0)
                visualOffset = Main.rand.NextFloat(0.75f, 1);
            visual1 += 30 * visualOffset;
            visual2 += 35 * visualOffset;
            Projectile.rotation = Projectile.velocity.ToRotation();
            float progress = Utils.GetLerpValue(0, max, Projectile.timeLeft);
            Projectile.scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
        }
        SoundStyle beam = new SoundStyle("MythosOfMoonlight/Assets/Sounds/electricity")
        {
            PitchVariance = 0.15f,
        };
        public override void OnSpawn(IEntitySource source)
        {
            CameraSystem.ScreenShakeAmount = 7f;
            SoundEngine.PlaySound(beam, Projectile.Center);
        }
        public override bool PreDraw(ref Color lightColor)
        {

            if (Projectile.ai[0] > 0)
            {
                Main.spriteBatch.Reload(BlendState.Additive);
                Texture2D tex = Helper.GetTex("MythosOfMoonlight/Assets/Textures/Extra/Ex3");
                Texture2D tex2 = Helper.GetTex("MythosOfMoonlight/Assets/Textures/Extra/spark_07");
                Texture2D tex3 = Helper.GetTex("MythosOfMoonlight/Assets/Textures/Extra/vortex");
                //Texture2D tex3 = Helper.GetTex("MythosOfMoonlight/Assets/Textures/Extra/spark_06");
                Vector2 pos = Projectile.Center;
                Vector2 scale = new Vector2(1f, Projectile.scale);
                for (int i = 0; i < Projectile.ai[0]; i++)
                {
                    Main.spriteBatch.Draw(tex, pos - Main.screenPosition, null, Color.Cyan, Projectile.rotation, new Vector2(0, tex.Height / 2), scale, SpriteEffects.None, 0);
                    pos += Projectile.rotation.ToRotationVector2();
                }
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearWrap, DepthStencilState.Default, Main.Rasterizer, null, Main.GameViewMatrix.ZoomMatrix);
                Vector2 pos2 = Projectile.Center;

                for (int i = 0; i < 2 - (Projectile.ai[0] < 512 ? 1 : 0); i++)
                {
                    int len = 512;
                    if (i == 1 && Projectile.ai[0] < 1024 && Projectile.ai[0] - 512 > 0)
                        len = (int)Projectile.ai[0] - 512;
                    if (i == 0 && Projectile.ai[0] < 512)
                        len = (int)Projectile.ai[0];
                    Main.spriteBatch.Draw(tex2, pos2 - Main.screenPosition, new Rectangle((int)-visual1, 0, len, 512), Color.Cyan, Projectile.rotation, new Vector2(0, tex2.Height / 2), scale, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(tex2, pos2 - Main.screenPosition, new Rectangle((int)-visual1, 0, len, 512), Color.White, Projectile.rotation, new Vector2(0, tex2.Height / 2), scale, SpriteEffects.None, 0);

                    Main.spriteBatch.Draw(tex2, pos2 - Main.screenPosition, new Rectangle((int)-visual2, 0, len, 512), Color.Cyan, Projectile.rotation, new Vector2(0, tex2.Height / 2), scale, SpriteEffects.FlipVertically, 0);
                    Main.spriteBatch.Draw(tex2, pos2 - Main.screenPosition, new Rectangle((int)-visual2, 0, len, 512), Color.White, Projectile.rotation, new Vector2(0, tex2.Height / 2), scale, SpriteEffects.FlipVertically, 0);
                    pos2 += Projectile.rotation.ToRotationVector2() * (Projectile.ai[0] >= 512 && i != 0 ? Projectile.ai[0] - 512 : 512);
                }
                Main.spriteBatch.Draw(tex3, Projectile.Center + Projectile.ai[0] * Vector2.UnitY - Main.screenPosition, null, Color.Cyan, Main.GameUpdateCount * 0.003f, tex3.Size() / 2, scale.Y * 0.15f, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(tex3, Projectile.Center + Projectile.ai[0] * Vector2.UnitY - Main.screenPosition, null, Color.White, Main.GameUpdateCount * 0.03f, tex3.Size() / 2, scale.Y * 0.15f, SpriteEffects.None, 0);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.Default, Main.Rasterizer, null, Main.GameViewMatrix.ZoomMatrix);
            }

            Texture2D s_tex = Helper.GetExtraTex("Extra/star_02");
            Texture2D s_tex2 = Helper.GetExtraTex("Extra/cone7");
            Vector2 s_scale = new Vector2(1f + scaleOff, 0.25f - scaleOff * 0.5f);
            Main.spriteBatch.Reload(BlendState.Additive);

            Vector4 col = (new Color(44, 137, 215) * Projectile.scale).ToVector4();
            Main.spriteBatch.Draw(s_tex2, Projectile.Center - Main.screenPosition, null, new Color(44, 137, 215) * Projectile.scale * 0.75f, MathHelper.PiOver2, new Vector2(0, s_tex2.Height / 2), new Vector2(1, 2) * Projectile.scale, SpriteEffects.None, 0);

            Main.spriteBatch.Reload(MythosOfMoonlight.SpriteRotation);
            MythosOfMoonlight.SpriteRotation.Parameters["rotation"].SetValue(MathHelper.ToRadians(Main.GlobalTimeWrappedHourly * 125));
            MythosOfMoonlight.SpriteRotation.Parameters["scale"].SetValue(s_scale * 0.45f * Projectile.scale);
            col.W = Projectile.scale * 0.15f;
            MythosOfMoonlight.SpriteRotation.Parameters["uColor"].SetValue(col);
            for (int i = 0; i < 80; i++)
            {
                float s = MathHelper.SmoothStep(Projectile.scale, 0, (float)i / 80);
                Vector2 pos = Projectile.Center - new Vector2(0, -20) - new Vector2(0, i * s);
                Main.spriteBatch.Draw(s_tex, pos - Main.screenPosition, null, Color.White, 0, s_tex.Size() / 2, s, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(s_tex, pos - Main.screenPosition, null, Color.White, 0, s_tex.Size() / 2, s, SpriteEffects.FlipHorizontally, 0);
            }
            Main.spriteBatch.Reload(effect: null);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);

            return false;
        }
    }
    public class StarineSlushTelegraph : ModProjectile
    {
        public override string Texture => Helper.Empty;
        const int max = 40;
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
        public override void AI()
        {
            Projectile.velocity = Vector2.UnitY;
            for (int i = 0; i < 5; i++)
            {
                int dust = Dust.NewDust(Projectile.position - new Vector2(10, 0), 10, (int)Projectile.ai[0], ModContent.DustType<StarineDust>(), 2f);
                Main.dust[dust].scale = 0.5f + MathHelper.Lerp(1.5f, 0, (float)Projectile.timeLeft / max);
                Main.dust[dust].velocity = Main.rand.NextVector2Circular(5, 5);
                Main.dust[dust].noGravity = true;
            }
            if (Projectile.ai[0] == 0)
            {
                Projectile.ai[0] = TRay.CastLength(Projectile.Center, Projectile.velocity, 1024);
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            float progress = Utils.GetLerpValue(0, max, Projectile.timeLeft);
            Projectile.scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
        }
    }
}
