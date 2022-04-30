using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace MythosOfMoonlight.Gores.Enemies
{
    public class Purpurine : ModGore
    {
        public override void OnSpawn(Gore gore, IEntitySource source)
        {
            gore.behindTiles = true;
            gore.timeLeft = Gore.goreTime * 3;
            gore.scale = 0.8f;
        }
        public override bool Update(Gore gore)
        {
            gore.GetAlpha(Color.White);
            gore.rotation += 0.5f;
            gore.position += gore.velocity;
            gore.velocity.Y += 0.04f;
            gore.scale -= 0.007f;
            if (gore.scale < 0.1)
            {
                gore.scale = 0.1f;
                gore.alpha = 255;
            }
            return false;
        }
        public override Color? GetAlpha(Gore gore, Color lightColor) => new Color(255, 255, 255, 255);
    }
}