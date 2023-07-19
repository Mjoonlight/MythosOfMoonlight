using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Dusts
{
    public class FireDust : ModDust
    {
        public override string Texture => "MythosOfMoonlight/Textures/Extra/blank";
        public override void OnSpawn(Dust dust)
        {
            dust.alpha = 255;
            dust.noLight = true;
            dust.noGravity = true;
            if (dust.scale > 0.35f)
                dust.scale = 0.35f;
            dust.customData = Main.rand.Next(1, 3);
            base.OnSpawn(dust);

        }
        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.scale -= 0.0025f;
            dust.velocity *= 0.95f;
            if (dust.scale <= 0)
                dust.active = false;

            return false;
        }
        public static void DrawAll(SpriteBatch sb)
        {
            foreach (Dust d in Main.dust)
            {
                if (d.type == ModContent.DustType<FireDust>() && d.active)
                {
                    Texture2D tex = ModContent.Request<Texture2D>("MythosOfMoonlight/Textures/Extra/explosion").Value;
                    sb.Draw(tex, d.position - Main.screenPosition, null, Color.White * d.scale * 10, 0, tex.Size() / 2, d.scale * 0.85f, SpriteEffects.None, 0);
                    sb.Draw(tex, d.position - Main.screenPosition, null, d.color * d.scale * 10, 0, tex.Size() / 2, d.scale, SpriteEffects.None, 0); ;
                }
            }
        }
    }
}
