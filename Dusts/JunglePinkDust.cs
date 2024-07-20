
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Dusts
{
    public class JunglePinkDust : ModDust
    {
        public override string Texture => Helper.Empty;
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = false;
            dust.noLight = true;
            dust.frame = new Rectangle(0, 8 * Main.rand.Next(3), 8, 8);
            dust.rotation = Main.rand.NextFloat(MathHelper.Pi);
            // dust.scale *= 2f;
        }
        public override bool Update(Dust dust)
        {
            Lighting.AddLight(dust.position, 0.3f, 0, 0.1f);
            return base.Update(dust);
        }
        public override bool MidUpdate(Dust dust)
        {
            if (!dust.noGravity)
            {
                dust.velocity.Y += 0.1f;
            }

            dust.rotation += 0.1f;
            if (dust.customData != null)
            {
                if (dust.customData.Equals(1))
                {
                    dust.scale -= 0.025f;
                }
                else
                    dust.scale -= 0.001f;
            }
            dust.position += dust.velocity;
            if (dust.scale <= 0)
                dust.active = false;

            return false;
        }
        public static void DrawAll(SpriteBatch sb)
        {
            foreach (Dust d in Main.dust)
            {
                if (d.active && d.type == ModContent.DustType<JunglePinkDust>())
                {
                    Texture2D tex = ModContent.Request<Texture2D>("MythosOfMoonlight/Dusts/JunglePinkDust_White").Value;
                    sb.Draw(tex, d.position - Main.screenPosition, d.frame, Color.White, d.rotation, new Vector2(4, 4), d.scale, SpriteEffects.None, 0);
                }
            }
        }
    }
}