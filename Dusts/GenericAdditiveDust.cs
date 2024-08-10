using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MythosOfMoonlight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MythosOfMoonlight.Dusts
{
    public class GenericAdditiveDust : ModDust
    {
        public override string Texture => Helper.Empty;
        public override void OnSpawn(Dust dust)
        {
            dust.alpha = 255;
            dust.noLight = true;
            dust.noGravity = true;
            //if (dust.scale <= 1f && dust.scale >= 0.8f)
            //  dust.scale = 0.25f;
            base.OnSpawn(dust);

        }
        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.scale -= 0.005f;
            dust.velocity *= 0.95f;
            if (dust.scale <= 0)
                dust.active = false;
            return false;
        }
        public static void DrawAll(SpriteBatch sb)
        {
            foreach (Dust d in Main.dust)
            {
                if (d.type == ModContent.DustType<GenericAdditiveDust>() && d.active)
                {
                    Texture2D tex = ModContent.Request<Texture2D>("MythosOfMoonlight/Assets/Textures/Extra/explosion_1").Value;
                    if (d.customData != null)
                        sb.Draw(tex, d.position - Main.screenPosition, null, Color.White * d.scale * 5, 0, tex.Size() / 2, d.scale * 0.85f, SpriteEffects.None, 0);
                    sb.Draw(tex, d.position - Main.screenPosition, null, d.color * (d.customData != null ? ((int)d.customData == 2 ? d.scale * 10 : 1) : 1), 0, tex.Size() / 2, d.scale, SpriteEffects.None, 0);
                }
            }
        }
    }
    public class SparkleDust : ModDust
    {
        public override string Texture => Helper.Empty;
        public override void OnSpawn(Dust dust)
        {
            dust.alpha = 255;
            dust.noLight = true;
            dust.noGravity = true;
            //if (dust.scale <= 1f && dust.scale >= 0.8f)
            //  dust.scale = 0.25f;
            base.OnSpawn(dust);

        }
        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.scale -= 0.005f;
            dust.velocity *= 0.95f;
            if (dust.scale <= 0)
                dust.active = false;
            return false;
        }
        public static void DrawAll(SpriteBatch sb)
        {
            foreach (Dust d in Main.dust)
            {
                if (d.type == ModContent.DustType<SparkleDust>() && d.active)
                {
                    Texture2D tex = ModContent.Request<Texture2D>("MythosOfMoonlight/Assets/Textures/Extra/crosslight").Value;
                    if (d.customData != null)
                        sb.Draw(tex, d.position - Main.screenPosition, null, Color.White * d.scale * 5, 0, tex.Size() / 2, d.scale * 0.85f, SpriteEffects.None, 0);
                    sb.Draw(tex, d.position - Main.screenPosition, null, d.color * (d.customData != null ? ((int)d.customData == 2 ? d.scale * 10 : 1) : 1), 0, tex.Size() / 2, d.scale, SpriteEffects.None, 0);
                }
            }
        }
    }
    public class LineDustFollowPoint : ModDust
    {
        public override string Texture => Helper.Empty;
        public override void OnSpawn(Dust dust)
        {
            dust.alpha = 255;
            dust.noLight = true;
            dust.noGravity = true;
            //if (dust.scale <= 1f && dust.scale >= 0.8f)
            //  dust.scale = 0.25f;
            base.OnSpawn(dust);

        }
        public override bool Update(Dust dust)
        {
            if (dust.fadeIn == 0)
                dust.fadeIn = dust.scale;
            dust.position += dust.velocity;
            dust.scale -= 0.005f;
            dust.rotation = dust.velocity.ToRotation() - MathHelper.PiOver2;
            if (dust.customData != null && dust.customData.GetType() == typeof(Vector2))
            {
                dust.velocity = Helper.FromAToB(dust.position, (Vector2)dust.customData, false) / 10;
                if (dust.position.Distance((Vector2)dust.customData) < 50)
                    dust.scale -= 0.02f;
            }
            if (dust.scale <= 0)
                dust.active = false;
            return false;
        }
        public static void DrawAll(SpriteBatch sb)
        {
            foreach (Dust d in Main.dust)
            {
                if (d.type == ModContent.DustType<LineDustFollowPoint>() && d.active)
                {
                    Texture2D tex = ModContent.Request<Texture2D>("MythosOfMoonlight/Assets/Textures/Extra/trace_01").Value;
                    sb.Draw(tex, d.position - Main.screenPosition, null, d.color * (d.scale * 10), d.rotation, tex.Size() / 2, new Vector2(d.scale, d.fadeIn), SpriteEffects.None, 0);
                }
            }
        }
    }
}
