using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace MythosOfMoonlight.Dusts
{
    public class Starry : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.alpha = 255;
            dust.noLight = true;
            dust.rotation = Main.rand.NextFloat(MathHelper.Pi * 2);
            dust.noGravity = true;
            base.OnSpawn(dust);
        }
        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.scale -= 0.01f;//Main.rand.NextFloat(0.01f, 0.035f);
            dust.velocity *= 0.99f;
            if (dust.scale <= 0)
                dust.active = false;
            return false;
        }
        public static void DrawAll(SpriteBatch sb)
        {
            foreach (Dust d in Main.dust)
            {
                if (d.type == ModContent.DustType<Starry>() && d.active)
                {
                    Texture2D tex = ModContent.Request<Texture2D>("MythosOfMoonlight/Dusts/Starry").Value;
                    DrawData a = new(tex, d.position - Main.screenPosition, null, d.color * (d.scale * 4), 0, tex.Size() / 2, d.scale, SpriteEffects.None, 0);
                    a.Draw(sb);
                    //Helper.DrawWithDye(sb, a, ItemID.TwilightDye, null);
                }
            }
        }
    }
}
