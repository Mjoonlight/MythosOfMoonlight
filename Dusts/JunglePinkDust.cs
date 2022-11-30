
﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Dusts
{
    public class JunglePinkDust : ModDust
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

            dust.rotation += 0.1f;
            dust.scale -= 0.05f;
            dust.position += dust.velocity;
            if (dust.scale <= 0)
                dust.active = false;

            return false;
        }
        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return new Color(255, 255, 255, 100) * dust.scale;
        }
    }
}