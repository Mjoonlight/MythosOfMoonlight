using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Dusts
{
    public class StarineDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = false;
            dust.noLight = true;
            // dust.scale *= 2f;
        }
        public override bool MidUpdate(Dust dust)
        {
            if (!dust.noGravity)
            {
                dust.velocity.Y += 0.1f;
            }

            dust.rotation += 1f;
            dust.scale -= 0.05f;
            dust.position += dust.velocity;
            if (dust.scale <= 0)
                dust.active = false;

            Lighting.AddLight(dust.position, 0.5f * dust.scale, 0.7f * dust.scale, 1f * dust.scale);
            return false;
        }
        public override Color? GetAlpha(Dust dust, Color lightColor)
        => new Color(lightColor.R, lightColor.G, lightColor.B, 25);
    }
    public class StarineDustAlt : ModDust
    {
        public override string Texture => "MythosOfMoonlight/Dusts/StarineDust";
        public override void OnSpawn(Dust dust)
        {
            dust.noLight = true;
            // dust.scale *= 2f;
        }
        public override bool MidUpdate(Dust dust)
        {
            dust.rotation += 1f;
            dust.scale -= 0.001f;
            dust.position += dust.velocity;
            dust.velocity *= 0.9f;
            if (dust.scale <= 0)
                dust.active = false;

            Lighting.AddLight(dust.position, 0.5f * dust.scale, 0.7f * dust.scale, 1f * dust.scale);
            return false;
        }
        public override Color? GetAlpha(Dust dust, Color lightColor)
        => new Color(lightColor.R, lightColor.G, lightColor.B);
    }
}