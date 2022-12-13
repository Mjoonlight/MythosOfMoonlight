using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using static MonoMod.Cil.RuntimeILReferenceBag.FastDelegateInvokers;

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
            return isActive;
        }
        float intensity;
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (maxDepth >= 3.40282347E+38f && minDepth < 3.40282347E+38f)
            {
                Vector2 Pos = new(Main.screenWidth / 2, Main.screenHeight / 2);
                Texture2D Tex = ModContent.Request<Texture2D>("MythosOfMoonlight/Textures/Sky").Value;
                spriteBatch.Draw(Tex, Pos, null, Color.White * Intensity * 0.5f, 0f, new Vector2(Tex.Width >> 1, Tex.Height >> 1), 1f, SpriteEffects.None, 1f);
            }
        }
        public override Color OnTileColor(Color inColor)
        {
            Vector4 value = inColor.ToVector4();
            return new Color(Vector4.Lerp(value, Vector4.One, Intensity * 0.2f));
        }
    }
}
