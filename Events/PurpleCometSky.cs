using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

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
                Texture2D starTex = ModContent.Request<Texture2D>("MythosOfMoonlight/Textures/Extra/flare").Value;
                Texture2D starTex2 = ModContent.Request<Texture2D>("MythosOfMoonlight/Textures/Extra/star_04").Value;
                Texture2D comet = ModContent.Request<Texture2D>("MythosOfMoonlight/Textures/Extra/comet_tail2").Value;
                Texture2D comet2 = ModContent.Request<Texture2D>("MythosOfMoonlight/Textures/Extra/cone5").Value;
                Vector2 Pos = new(Main.screenWidth / 2, Main.screenHeight / 2);
                //int cometX = (int)(Main.time / 32400.0 * (double)(scen.totalWidth + (float)(comet.Width * 2))) - comet.Width;
                Vector2 cometP = Vector2.Lerp(new Vector2(Main.screenWidth + 300, -100), new Vector2(-500, Main.screenHeight + 100), (float)Main.time / 32400);
                //new(Main.screenWidth / 4, MathHelper.Lerp(-200, Main.screenHeight + comet.Height * 0.5f, (float)Main.time / 32400));
                if (Main.screenWidth > Tex.Width || Main.screenHeight > Tex.Height)
                    spriteBatch.Draw(Tex, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), null, Color.White * Intensity * 0.5f, 0, Vector2.Zero, SpriteEffects.None, 0);
                else
                    spriteBatch.Draw(Tex, Pos, null, Color.White * Intensity * 0.5f, 0f, new Vector2(Tex.Width >> 1, Tex.Height >> 1), 1f, SpriteEffects.None, 1f);
                spriteBatch.Reload(BlendState.Additive);
                glow += Main.rand.NextFloat(-.1f, .1f);
                glow = MathHelper.Clamp(glow, 0, 1);
                spriteBatch.Draw(comet, cometP, null, Color.White * Intensity * 0.65f * ((((float)Math.Sin((double)Main.GlobalTimeWrappedHourly) + 1) * 0.5f) + 0.4f), MathHelper.ToRadians(245), new Vector2(comet.Width / 2, 0.15f), 1, SpriteEffects.None, 0f);
                spriteBatch.Draw(comet, cometP, null, Color.White * Intensity * 0.85f, MathHelper.ToRadians(245), new Vector2(comet.Width / 2, 0.25f), 0.95f, SpriteEffects.None, 0f);
                //spriteBatch.Draw(comet2, cometP - Vector2.UnitY * 30, null, Color.White * Intensity * 0.75f, MathHelper.ToRadians(-90), comet2.Size() / 2, 0.5f, SpriteEffects.None, 0f);

                Vector2 starOffset = new Vector2(35, -14f);

                spriteBatch.Draw(starTex, cometP + starOffset, null, Color.White * Intensity * ((((float)Math.Sin((double)Main.GlobalTimeWrappedHourly) + 1) * 0.7f) + 0.4f + glow), MathHelper.ToRadians(90), starTex.Size() / 2, 0.5f, SpriteEffects.None, 0f);
                spriteBatch.Draw(starTex, cometP + starOffset, null, Color.White * Intensity, MathHelper.ToRadians(90), starTex.Size() / 2, 0.5f, SpriteEffects.None, 0f);

                spriteBatch.Draw(starTex, cometP + starOffset, null, Color.White * Intensity * ((((float)Math.Sin((double)Main.GlobalTimeWrappedHourly) + 1) * 0.7f) + 0.4f + glow), MathHelper.ToRadians(0), starTex.Size() / 2, 0.75f, SpriteEffects.None, 0f);

                spriteBatch.Draw(starTex, cometP + starOffset, null, Color.White * Intensity * 0.5f * ((((float)Math.Sin((double)Main.GlobalTimeWrappedHourly) + 1) * 0.5f) + 0.4f + glow), MathHelper.ToRadians(45), starTex2.Size() / 2, 0.45f, SpriteEffects.None, 0f);

                spriteBatch.Draw(starTex, cometP + starOffset, null, Color.White * Intensity * 0.5f * ((((float)Math.Sin((double)Main.GlobalTimeWrappedHourly) + 1) * 0.5f) + 0.4f + glow), MathHelper.ToRadians(-45), starTex2.Size() / 2, 0.45f, SpriteEffects.None, 0f);

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
