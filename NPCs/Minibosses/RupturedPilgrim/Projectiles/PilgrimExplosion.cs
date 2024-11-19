using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using MythosOfMoonlight.Common.Crossmod;
using MythosOfMoonlight.Common.Systems;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.Utilities;
using System;

namespace MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim.Projectiles
{
    public class PilgrimExplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Starine Storm");
            Main.projFrames[Projectile.type] = 8;
            Projectile.AddElement(CrossModHelper.Celestial);
            Projectile.AddElement(CrossModHelper.Arcane);
        }
        int max = 35;
        public override void SetDefaults()
        {
            Projectile.width = 90;
            Projectile.height = 132;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = max;
        }
        public override void AI()
        {
            Projectile.velocity = Vector2.Zero;
            if (++Projectile.frameCounter >= 5)
            {
                if (Projectile.frame == 1)
                    CameraSystem.ScreenShakeAmount = 7f;
                Projectile.frameCounter = 0;
                Projectile.frame++;
            }
            if (Projectile.frame == 7)
                Projectile.Kill();

            Projectile.scale = MathHelper.SmoothStep(2, 1, Utils.GetLerpValue(0, max, Projectile.timeLeft * 1.5f, true));
        }
        public override void OnSpawn(IEntitySource source)
        {
            seed = Main.rand.Next(int.MaxValue - 1);
        }
        int seed;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Helper.GetTex("MythosOfMoonlight/Assets/Textures/Extra/cone4");
            Texture2D tex2 = Helper.GetTex("MythosOfMoonlight/Assets/Textures/Extra/slash");
            Main.spriteBatch.Reload(BlendState.Additive);
            UnifiedRandom rand = new UnifiedRandom(seed);
            float max = 40;
            float alpha = MathHelper.Lerp(0.5f, 0, (Projectile.scale - 1)) * 2;
            for (float i = 0; i < max; i++)
            {
                float angle = Helper.CircleDividedEqually(i, max);
                float scale = rand.NextFloat(0.125f, .35f);
                for (float j = 0; j < 2; j++)
                    Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, new Color(44, 137, 215) * alpha * 0.25f, angle, new Vector2(0, tex.Height / 2), new Vector2((Projectile.scale - 1) * 0.5f, alpha) * scale * 6, SpriteEffects.None, 0);

                for (float j = 0; j < 2; j++)
                    Main.spriteBatch.Draw(tex2, Projectile.Center - Main.screenPosition, null, new Color(44, 137, 215) * alpha * 0.25f, angle, new Vector2(0, tex2.Height / 2), new Vector2((Projectile.scale - 1) * 0.5f, alpha) * scale * 5 * 2, SpriteEffects.None, 0);
            }
            Main.spriteBatch.Reload(BlendState.AlphaBlend);

            if (Projectile.frame > 1)
            {
                Texture2D drawTextureGlow = ModContent.Request<Texture2D>(Projectile.ModProjectile.Texture + "_Extra").Value;
                Rectangle sourceRectangle = new(0, 0, drawTextureGlow.Width, drawTextureGlow.Height);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                var scale = (Projectile.frame * 10 + Projectile.frameCounter) / 35f;
                var color = Color.Lerp(lightColor, new Color(lightColor.R, lightColor.G, lightColor.B, 0), scale);
                Main.EntitySpriteDraw(drawTextureGlow, Projectile.Center - Main.screenPosition, sourceRectangle, color, 0f, new Vector2(drawTextureGlow.Width, drawTextureGlow.Height) / 2, scale, SpriteEffects.None, 0);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = -(Main.expertMode ? 4 : 2); i < (Main.expertMode ? 5 : 3); i++)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(0, -6).RotatedBy(MathHelper.ToRadians(i * Main.rand.NextFloat(14, 17) + Main.rand.NextFloat(-17, 17))), ModContent.ProjectileType<StarineShaft>(), 20, 0);
            }
            SoundEngine.PlaySound(SoundID.Item9, Projectile.Center);
        }
    }
}