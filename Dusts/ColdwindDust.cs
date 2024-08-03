using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria;

namespace MythosOfMoonlight.Dusts
{

    public class ColdwindDust : ModDust
    {
        public override string Texture => Helper.Empty;
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
            if (dust.customData == null)
                dust.customData = dust.scale;

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
                if (d.type == ModContent.DustType<ColdwindDust>() && d.active)
                {
                    Texture2D tex = ModContent.Request<Texture2D>("MythosOfMoonlight/Dusts/Starry").Value;
                    float progress = Utils.GetLerpValue(0f, (float)d.customData, d.scale);
                    float alpha = MathHelper.Clamp(MathF.Sin(progress * MathHelper.Pi) * 0.1f, 0, .2f);
                    DrawData a = new(tex, d.position - Main.screenPosition, null, d.color * alpha, d.rotation, tex.Size() / 2, d.scale, SpriteEffects.None, 0);
                    a.Draw(sb);
                    a.rotation = Main.GameUpdateCount * 0.02f;
                    a.Draw(sb);
                    //Helper.DrawWithDye(sb, a, ItemID.TwilightDye, null);
                }
            }
        }
    }
}
