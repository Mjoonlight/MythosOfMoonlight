using Microsoft.Win32;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using static Terraria.Main;

namespace MythosOfMoonlight.Events
{
    public class PurpleCometSky : CustomSky
    {
        public bool isActive;
        public float Intensity;
        public override void Activate(Vector2 position, params object[] args)
        {
            isActive = true;
        }
        public override void Deactivate(params object[] args)
        {
            isActive = false;
        }
        public override void Reset()
        {
            isActive = false;
        }
        public override void Update(GameTime gameTime)
        {
            if (isActive)
            {
                Intensity = Math.Min(1f, 0.01f + Intensity);
            }
            else
            {
                Intensity = Math.Max(0f, Intensity - 0.01f);
            }

        }
        public override bool IsActive()
        {
            return Intensity > 0;
        }
        float glow;
        //float intensity;
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (maxDepth >= 3.40282347E+38f && minDepth < 3.40282347E+38f)
            {
                Texture2D Tex = ModContent.Request<Texture2D>("MythosOfMoonlight/Textures/Sky").Value;
                Texture2D starTex = ModContent.Request<Texture2D>("MythosOfMoonlight/Textures/Extra/star_05").Value;
                Texture2D starTex2 = ModContent.Request<Texture2D>("MythosOfMoonlight/Textures/Extra/star_04").Value;
                Texture2D comet = ModContent.Request<Texture2D>("MythosOfMoonlight/Textures/Extra/comet_tail").Value;
                Texture2D comet2 = ModContent.Request<Texture2D>("MythosOfMoonlight/Textures/Extra/cone5").Value;
                Vector2 Pos = new(Main.screenWidth / 2, Main.screenHeight / 2);
                //int cometX = (int)(Main.time / 32400.0 * (double)(scen.totalWidth + (float)(comet.Width * 2))) - comet.Width;
                Vector2 cometP = new(Main.screenWidth / 4, MathHelper.Lerp(-200, Main.screenHeight + comet.Height * 0.5f, (float)Main.time / 32400));
                spriteBatch.Draw(Tex, Pos, null, Color.White * Intensity * 0.5f, 0f, new Vector2(Tex.Width >> 1, Tex.Height >> 1), 1f, SpriteEffects.None, 1f);
                spriteBatch.Reload(BlendState.Additive);
                /*Effect effect = MythosOfMoonlight.ScreenDistort;
                effect.Parameters["screenPosition"].SetValue(Main.screenPosition);
                effect.Parameters["noiseTex"].SetValue(ModContent.Request<Texture2D>("MythosOfMoonlight/Textures/Extra/seamlessNoise").Value);
                effect.Parameters["distortionMultiplier"].SetValue(0.75f * Intensity);
                effect.Parameters["screenSize"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight) * -36);
                effect.Parameters["alpha"].SetValue(Intensity);
                effect.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly);
                effect.CurrentTechnique.Passes[0].Apply();*/
                glow += Main.rand.NextFloat(-.05f, .05f);
                glow = MathHelper.Clamp(glow, 0, 1);
                spriteBatch.Draw(comet, cometP, null, Color.White * Intensity * 0.75f * ((((float)Math.Sin((double)Main.GlobalTimeWrappedHourly) + 1) * 0.5f) + 0.4f), MathHelper.ToRadians(180), comet.Size() / 2, 0.85f, SpriteEffects.None, 0f);
                spriteBatch.Draw(comet, cometP, null, Color.White * Intensity * 0.75f, MathHelper.ToRadians(180), comet.Size() / 2, 0.75f, SpriteEffects.None, 0f);
                //spriteBatch.Draw(comet2, cometP - Vector2.UnitY * 30, null, Color.White * Intensity * 0.75f, MathHelper.ToRadians(-90), comet2.Size() / 2, 0.5f, SpriteEffects.None, 0f);

                Vector2 starPos = new Vector2(-5, comet.Height * 0.3f);
                spriteBatch.Draw(starTex, cometP + starPos, null, Color.White * Intensity * 0.75f * ((((float)Math.Sin((double)Main.GlobalTimeWrappedHourly) + 1) * 0.7f) + 0.4f + glow), MathHelper.ToRadians(45) + Main.GameUpdateCount * 0.0015f, starTex.Size() / 2, 0.45f, SpriteEffects.None, 0f);
                spriteBatch.Draw(starTex, cometP + starPos, null, Color.White * Intensity, MathHelper.ToRadians(45) + Main.GameUpdateCount * 0.0015f, starTex.Size() / 2, 0.45f, SpriteEffects.None, 0f);
                spriteBatch.Draw(starTex2, cometP + starPos, null, Color.White * Intensity * 0.75f * ((((float)Math.Sin((double)Main.GlobalTimeWrappedHourly) + 1) * 0.5f) + 0.4f + glow), Main.GameUpdateCount * 0.002f, starTex2.Size() / 2, 0.45f, SpriteEffects.None, 0f);

            }
            spriteBatch.Reload(BlendState.AlphaBlend);
        }

        public override Color OnTileColor(Color inColor)
        {
            Vector4 value = inColor.ToVector4();
            return new Color(Vector4.Lerp(value, Vector4.One, Intensity * 0.2f));
        }
    }
}
