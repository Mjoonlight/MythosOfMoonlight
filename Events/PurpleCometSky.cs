using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Events
{
    public class PurpleCometSky : CustomSky
    {
        public bool isActive;
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
            
        }
        public override bool IsActive()
        {
            return isActive;
        }
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (maxDepth >= 3.40282347E+38f && minDepth < 3.40282347E+38f)
            {
                Texture2D aa = ModContent.GetTexture("MythosOfMoonlight/Textures/Sky");
                spriteBatch.Draw(aa, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White * intensity);
            }
        }
    }
}
