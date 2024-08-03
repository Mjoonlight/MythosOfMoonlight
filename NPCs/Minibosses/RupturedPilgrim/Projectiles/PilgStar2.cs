using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MythosOfMoonlight.Common.Crossmod;
using MythosOfMoonlight.Dusts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;


namespace MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim.Projectiles
{
    public class PilgStar2 : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/NPCs/Minibosses/RupturedPilgrim/Projectiles/PilgStar";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Starine Shaft");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            Projectile.AddElement(CrossModHelper.Celestial);
            Projectile.AddElement(CrossModHelper.Arcane);
        }
        public override void SetDefaults()
        {
            Projectile.damage = 10;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 360;
            Projectile.friendly = false;
            Projectile.tileCollide = true;
            Projectile.hostile = true;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 16; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<StarineDust>(), 2f);
                Main.dust[dust].scale = 2f;
                Main.dust[dust].velocity = Main.rand.NextVector2Unit() * 1.2f;
                Main.dust[dust].velocity.Y = -1.5f;
                Main.dust[dust].noGravity = true;
            }
            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
        }
        public override bool? CanDamage()
        {
            return ShouldUpdatePosition();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D drawTexture = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle sourceRectangle = new(0, 0, drawTexture.Width, drawTexture.Height);
            Main.EntitySpriteDraw(drawTexture, Projectile.Center - Main.screenPosition, sourceRectangle, Color.White * Projectile.ai[2], Projectile.rotation, drawTexture.Size() / 2, 1, SpriteEffects.None, 0);

            //3hi31mg
            var off = new Vector2(Projectile.width / 2, Projectile.height / 2);
            var clr = new Color(255, 255, 255, 255); // full white
            var texture = TextureAssets.Projectile[Projectile.type].Value;
            //var frame = new Rectangle(0, Projectile.frame, Projectile.width, Projectile.height);
            var orig = Projectile.Size / 2f;
            var trailLength = ProjectileID.Sets.TrailCacheLength[Projectile.type];

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 1; i < trailLength; i++)
            {
                float scale = MathHelper.Lerp(0.70f, 1f, (float)(trailLength - i) / trailLength);
                var fadeMult = 1f / trailLength;
                SpriteEffects flipType = Projectile.spriteDirection == -1 /* or 1, idfk */ ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i] - Main.screenPosition + off, null, clr * (1f - fadeMult * i) * Projectile.ai[2] * 0.75f, Projectile.oldRot[i], orig, scale, flipType, 0f);
            }
            Texture2D tex2 = Helper.GetTex("MythosOfMoonlight/Assets/Textures/Extra/crosslight");
            Main.spriteBatch.Draw(tex2, Projectile.Center - Main.screenPosition, null, Color.Cyan * glareAlpha, 0, tex2.Size() / 2, glareAlpha * 0.2f, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(tex2, Projectile.Center - Main.screenPosition, null, Color.White * glareAlpha, 0, tex2.Size() / 2, glareAlpha * 0.2f, SpriteEffects.None, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
        float glareAlpha;
        public override bool ShouldUpdatePosition()
        {
            return Projectile.ai[0] > 40 + Projectile.ai[1];
        }
        public override void AI()
        {
            if (!ShouldUpdatePosition())
            {
                if (Projectile.localAI[0] == 1)
                {
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.active && npc.type == ModContent.NPCType<RupturedPilgrim>())
                        {
                            Projectile.Center = Vector2.Lerp(Projectile.Center, npc.Center + new Vector2(Projectile.localAI[1], Projectile.localAI[2]).RotatedBy(Helper.FromAToB(npc.Center, Main.LocalPlayer.Center).ToRotation()), 0.2f);
                        }
                    }
                    Projectile.velocity = Helper.FromAToB(Projectile.Center, Main.LocalPlayer.Center) * Projectile.oldVelocity.Length();
                }
            }
            Projectile.rotation += MathHelper.ToRadians(5);
            Projectile.ai[2] = MathHelper.Lerp(Projectile.ai[2], 1, 0.05f);
            Projectile.ai[0]++;
            if (Projectile.ai[0] == 20 + Projectile.ai[1])
            {
                SoundStyle style = SoundID.Item21;
                style.Volume = 0.5f;
                SoundEngine.PlaySound(style, Projectile.Center);
                glareAlpha = 2;
            }
            if (glareAlpha > 0)
                glareAlpha -= 0.05f;
        }
    }
    public class PilgStar3 : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/NPCs/Minibosses/RupturedPilgrim/Projectiles/PilgStar";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Starine Shaft");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            Projectile.AddElement(CrossModHelper.Celestial);
            Projectile.AddElement(CrossModHelper.Arcane);
        }
        public override void SetDefaults()
        {
            Projectile.damage = 10;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.aiStyle = 2;
            Projectile.timeLeft = 240;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 16; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<StarineDust>(), 2f);
                Main.dust[dust].scale = 2f;
                Main.dust[dust].velocity = Main.rand.NextVector2Unit() * 1.2f;
                Main.dust[dust].velocity.Y = -1.5f;
                Main.dust[dust].noGravity = true;
            }
            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
        }
        public override bool? CanDamage()
        {
            return ShouldUpdatePosition();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D drawTexture = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle sourceRectangle = new(0, 0, drawTexture.Width, drawTexture.Height);
            Main.EntitySpriteDraw(drawTexture, Projectile.Center - Main.screenPosition, sourceRectangle, Color.White * Projectile.ai[2], Main.GameUpdateCount * 0.02f, drawTexture.Size() / 2, 1, SpriteEffects.None, 0);

            //3hi31mg
            var off = new Vector2(Projectile.width / 2, Projectile.height / 2);
            var clr = new Color(255, 255, 255, 255); // full white
            var texture = TextureAssets.Projectile[Projectile.type].Value;
            //var frame = new Rectangle(0, Projectile.frame, Projectile.width, Projectile.height);
            var orig = Projectile.Size / 2f;
            var trailLength = ProjectileID.Sets.TrailCacheLength[Projectile.type];

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 1; i < trailLength; i++)
            {
                float scale = MathHelper.Lerp(0.70f, 1f, (float)(trailLength - i) / trailLength);
                var fadeMult = 1f / trailLength;
                SpriteEffects flipType = Projectile.spriteDirection == -1 /* or 1, idfk */ ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i] - Main.screenPosition + off, null, clr * (1f - fadeMult * i) * Projectile.ai[2] * 0.75f, Main.GameUpdateCount * 0.002f, orig, scale, flipType, 0f);
            }
            Texture2D tex2 = Helper.GetTex("MythosOfMoonlight/Assets/Textures/Extra/crosslight");
            Main.spriteBatch.Draw(tex2, Projectile.Center - Main.screenPosition, null, Color.Cyan * glareAlpha, 0, tex2.Size() / 2, glareAlpha * 0.2f, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(tex2, Projectile.Center - Main.screenPosition, null, Color.White * glareAlpha, 0, tex2.Size() / 2, glareAlpha * 0.2f, SpriteEffects.None, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
        float glareAlpha;
        public override void AI()
        {
            Projectile.ai[2] = MathHelper.Lerp(Projectile.ai[2], 1, 0.05f);
            Projectile.ai[0]++;
        }
    }
}
