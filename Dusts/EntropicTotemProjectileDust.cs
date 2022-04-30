using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Dusts
{
    class EntropicTotemProjectileDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = true;
            dust.scale = 2f;
        }
        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return Color.White;
        }
    }
}
