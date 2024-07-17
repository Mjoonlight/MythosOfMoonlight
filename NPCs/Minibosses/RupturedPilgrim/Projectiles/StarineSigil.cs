using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using MythosOfMoonlight.Dusts;
using Terraria.Audio;
using System;
using System.IO;

namespace MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim.Projectiles
{
    public class StarineSigil : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Starine Sigil");
            MythosOfMoonlight.projectileFinalDrawList.Add(Type);
            Projectile.AddElement(CrossModHelper.Celestial);
            Projectile.AddElement(CrossModHelper.Arcane);
        }
        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 250;
        }
        int projDamage = 12;
        public override void OnKill(int timeLeft)
        {
            float offsett = Main.rand.NextFloat(MathHelper.Pi * 2);
            /*for (int i = 0; i < 3; i++)
            {
                float angle = Helper.CircleDividedEqually(i, 3) + offsett;
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.One.RotatedBy(angle) * 3, ModContent.ProjectileType<StarineShaft>(), 10, 0, ai2: 1);
            }*/
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
        int ProjectileTimer
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        float offset;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(offset);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            offset = reader.ReadSingle();
        }
        static double RandRadian => Main.rand.NextDouble() * (MathHelper.PiOver2 / 3f) - (MathHelper.PiOver2 / 6f);
        public override bool PreDraw(ref Color lightColor)
        {
            /*Projectile.rotation += .1f;
            Projectile.alpha += 50;
            Texture2D drawTexture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D drawTextureGlow = ModContent.Request<Texture2D>(Projectile.ModProjectile.Texture + "_Glow").Value;
            Texture2D drawTexture1 = ModContent.Request<Texture2D>(Projectile.ModProjectile.Texture + "_Extra1").Value;
            Texture2D drawTexture2 = ModContent.Request<Texture2D>(Projectile.ModProjectile.Texture + "_Extra2").Value;
            Rectangle sourceRectangle = new(0, 0, drawTexture.Width, drawTexture.Height);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(drawTexture, Projectile.Center - Main.screenPosition, sourceRectangle, new Color(Projectile.alpha, Projectile.alpha, Projectile.alpha, Projectile.alpha), Projectile.rotation, drawTexture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(drawTexture1, Projectile.Center - Main.screenPosition, sourceRectangle, new Color(Projectile.alpha, Projectile.alpha, Projectile.alpha, Projectile.alpha), -Projectile.rotation, drawTexture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(drawTexture2, Projectile.Center - Main.screenPosition, sourceRectangle, new Color(Projectile.alpha, Projectile.alpha, Projectile.alpha, Projectile.alpha), -Projectile.rotation, drawTexture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);

            if (!Main.dayTime)
            {
                Main.EntitySpriteDraw(drawTextureGlow, Projectile.Center - Main.screenPosition, sourceRectangle, new Color(Projectile.alpha, Projectile.alpha, Projectile.alpha, Projectile.alpha), Projectile.rotation, drawTexture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);*/

            Texture2D tex = Helper.GetExtraTex("Extra/star_02");
            Texture2D tex2 = Helper.GetExtraTex("Extra/cone7");
            Vector2 scale = new Vector2(1f + scaleOff, 0.25f - scaleOff * 0.5f);
            Main.spriteBatch.Reload(BlendState.Additive);

            Vector4 col = (new Color(44, 137, 215) * Projectile.ai[2]).ToVector4();
            Main.spriteBatch.Draw(tex2, Projectile.Center - Main.screenPosition, null, new Color(44, 137, 215) * Projectile.ai[2] * 0.4f, -MathHelper.PiOver2, new Vector2(0, tex2.Height / 2), new Vector2(1, 2) * Projectile.ai[2], SpriteEffects.None, 0);

            Main.spriteBatch.Reload(MythosOfMoonlight.SpriteRotation);
            MythosOfMoonlight.SpriteRotation.Parameters["rotation"].SetValue(MathHelper.ToRadians(Main.GlobalTimeWrappedHourly * 125));
            MythosOfMoonlight.SpriteRotation.Parameters["scale"].SetValue(scale * 0.45f * Projectile.ai[2]);
            col.W = Projectile.ai[2] * 0.15f;
            MythosOfMoonlight.SpriteRotation.Parameters["uColor"].SetValue(col * 0.65f);
            for (int i = 0; i < 80; i++)
            {
                float s = -MathHelper.SmoothStep(0, Projectile.ai[2], (float)i / 80) + 1;
                Vector2 pos = Projectile.Center + new Vector2(0, i * s);
                Main.spriteBatch.Draw(tex, pos - Main.screenPosition, null, Color.White, 0, tex.Size() / 2, s, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(tex, pos - Main.screenPosition, null, Color.White, 0, tex.Size() / 2, s, SpriteEffects.FlipHorizontally, 0);
            }
            Main.spriteBatch.Reload(effect: null);
            Main.spriteBatch.Reload(MythosOfMoonlight.SpriteRotation);
            MythosOfMoonlight.SpriteRotation.Parameters["rotation"].SetValue(MathHelper.ToRadians(Main.GlobalTimeWrappedHourly * 125));
            MythosOfMoonlight.SpriteRotation.Parameters["scale"].SetValue(scale * 0.45f * Projectile.ai[2]);
            MythosOfMoonlight.SpriteRotation.Parameters["uColor"].SetValue(col * 0.25f);
            for (int i = 0; i < 80; i++)
            {
                float s = -MathHelper.SmoothStep(0, Projectile.ai[2], (float)i / 80) + 1;
                Vector2 pos = Projectile.Center + new Vector2(0, i * s);
                Main.spriteBatch.Draw(tex, pos + new Vector2(MathF.Sin(i + Main.GlobalTimeWrappedHourly * 10), MathF.Cos(i + Main.GlobalTimeWrappedHourly * 10)) * MathHelper.Lerp(0, 30, (MathF.Sin(Main.GlobalTimeWrappedHourly * 5.5f) + 1) * 0.5f) - Main.screenPosition, null, Color.White, 0, tex.Size() / 2, s, SpriteEffects.FlipHorizontally, 0);
            }
            Main.spriteBatch.Reload(effect: null);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
        float Timer;
        float scaleOff;
        public override void AI()
        {
            Timer++;
            scaleOff = MathHelper.Lerp(scaleOff, MathF.Sin(Timer * .05f) * 0.05f, 0.25f);
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.type == ModContent.NPCType<RupturedPilgrim>() && npc.immortal)
                {
                    Projectile.Kill();
                }
            }
            if (Timer < 30)
                Projectile.ai[2] = MathHelper.SmoothStep(0, 1, Timer / 30);
            else if (Timer > 240)
                Projectile.ai[2] = MathHelper.SmoothStep(1, 0.4f, (Timer - (240)) / 10);

            if (ProjectileTimer >= 0)
                offset += 0.2f;
            int intOffset = (int)Math.Round(offset, 0);
            Projectile.velocity.X = offset * Projectile.ai[0] * 0.1f;
            Projectile.scale = 1 + (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 0.5f) * 0.25f);
            if (!Main.expertMode)
            {
                if (++ProjectileTimer >= 15 + intOffset)
                {
                    ProjectileTimer = 0;
                    for (int i = 0; i < 15; i++)
                    {
                        int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<StarineDust>(), 2f);
                        Main.dust[dust].scale = 2f;
                        Main.dust[dust].velocity = Main.rand.NextVector2Unit() * 1.2f;
                        Main.dust[dust].noGravity = true;
                    }
                    //for (int i = 0; i < 3; i++)
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + new Vector2(Main.rand.Next(-35, 35), 0), new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-6, -4)), ModContent.ProjectileType<StarineShaft>(), projDamage, 0);
                    SoundStyle style = SoundID.Item21;
                    style.Volume = 0.5f;
                    SoundEngine.PlaySound(style, Projectile.Center);
                    //if (ProjectileTimer == 60)
                    //  Projectile.Kill();
                }
            }
            else
            {
                //                    if (ProjectileTimer == 55)
                //                      Projectile.Kill();
                if (++ProjectileTimer >= 10 + intOffset)
                {
                    ProjectileTimer = 0;
                    for (int i = 0; i < 15; i++)
                    {
                        int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<StarineDust>(), 2f);
                        Main.dust[dust].scale = 2f;
                        Main.dust[dust].velocity = Main.rand.NextVector2Unit() * 1.2f;
                        Main.dust[dust].noGravity = true;
                    }
                    //for (int i = 0; i < 3; i++)
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + new Vector2(Main.rand.Next(-35, 35), 0), new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-6, -4)), ModContent.ProjectileType<StarineShaft>(), projDamage, 0);
                    SoundStyle style = SoundID.Item21;
                    style.Volume = 0.5f;
                    SoundEngine.PlaySound(style, Projectile.Center);
                }

            }
        }
    }
}