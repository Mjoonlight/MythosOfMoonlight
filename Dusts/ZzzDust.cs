using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Dusts
{
    public class ZzzDust : ModDust
    {
        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.velocity *= 0.99f;
            dust.position.Y -= 0.5f;
            if (dust.velocity.Length() < .5f)
                dust.scale -= 0.01f;
            if (dust.scale <= 0)
                dust.active = false;
            dust.frame = new Rectangle(0, 0, 8, 8);
            return false;
        }
        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return Color.White * (dust.scale);
        }
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = false;
            base.OnSpawn(dust);
        }
    }
}
