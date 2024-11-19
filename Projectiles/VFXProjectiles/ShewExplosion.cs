using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using static System.Net.Mime.MediaTypeNames;
using static tModPorter.ProgressUpdate;

namespace MythosOfMoonlight.Projectiles.VFXProjectiles
{
    public class ShewExplosion : ModProjectile
    {
        public override string Texture => Helper.Placeholder;
        public override void SetDefaults()
        {
            Projectile.height = 1;
            Projectile.width = 1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 200;
            Projectile.tileCollide = false;
        }
        public override bool ShouldUpdatePosition() => false;
        int seed;
        public override void OnSpawn(IEntitySource source)
        {
            seed = Main.rand.Next(int.MaxValue);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (seed == 0) return false;
            Texture2D tex = Helper.GetTex("MythosOfMoonlight/Assets/Textures/Extra/slash");
            Texture2D tex2 = Helper.GetTex("MythosOfMoonlight/Assets/Textures/Extra/circlething");
            UnifiedRandom rand = new UnifiedRandom(seed);
            float max = 40;
            Main.spriteBatch.Reload(BlendState.Additive);
            float ringScale = MathHelper.Lerp(1, 0, MathHelper.Clamp(Projectile.ai[2] * 6.5f, 0, 1));
            if (ringScale > 0.01f)
            {
                for (float i = 0; i < max; i++)
                {
                    float angle = Helper.CircleDividedEqually(i, max);
                    float scale = rand.NextFloat(0.2f, 1f);
                    Vector2 offset = new Vector2(Main.rand.NextFloat(50, 100) * ringScale * scale, 0).RotatedBy(angle);
                    for (float j = 0; j < 2; j++)
                        Main.spriteBatch.Draw(tex, Projectile.Center + offset - Main.screenPosition, null, Color.White * ringScale, angle, tex.Size() / 2, new Vector2(MathHelper.Clamp(Projectile.ai[2] * 6.5f, 0, 1), ringScale) * scale * 0.2f * 2, SpriteEffects.None, 0);
                }
            }

            float alpha = MathHelper.Lerp(0.5f, 0, Projectile.ai[1]) * 2;
            for (float i = 0; i < max; i++)
            {
                float angle = Helper.CircleDividedEqually(i, max);
                float scale = rand.NextFloat(0.2f, 1f);
                Vector2 offset = new Vector2(Main.rand.NextFloat(50) * Projectile.ai[1] * scale, 0).RotatedBy(angle);
                for (float j = 0; j < 2; j++)
                    Main.spriteBatch.Draw(tex, Projectile.Center + offset - Main.screenPosition, null, Color.White * alpha, angle, tex.Size() / 2, new Vector2(Projectile.ai[1], alpha) * scale * 2, SpriteEffects.None, 0);
            }
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
        public override void AI()
        {
            Projectile.ai[2] = MathHelper.Lerp(Projectile.ai[2], 0.2f, 0.1f);
            Projectile.ai[0] = MathHelper.Lerp(Projectile.ai[0], Projectile.ai[0] + Projectile.ai[2] + (Projectile.ai[0] * 0.075f), 0.15f);
            Projectile.ai[1] = MathHelper.SmoothStep(0, 1.5f, Projectile.ai[0]);

            if (Projectile.timeLeft >= 190 && Projectile.timeLeft < 194)
            {
                UnifiedRandom rand = new UnifiedRandom(seed);
                float max = 10 + ((Projectile.timeLeft - 190) * 10);
                for (int i = ((Projectile.timeLeft - 190) * 10); i < max; i++)
                {
                    float angle = Helper.CircleDividedEqually(i, max);
                    float scale = rand.NextFloat(1f - (Projectile.timeLeft - 190) * 0.2f);
                    Vector2 offset = new Vector2(Main.rand.NextFloat(50) * scale, 0).RotatedBy(angle);
                    int jMax = rand.Next(3, 5);
                    for (int j = 0; j < jMax; j++)
                        Dust.NewDustPerfect(Projectile.Center + offset * 0.5f, DustID.GemDiamond, Helper.FromAToB(Projectile.Center, Projectile.Center + offset).RotatedByRandom(MathHelper.PiOver4 * (j == 0 ? 0 : 1)) * (scale * 20)).noGravity = true;
                }
            }

            if (Projectile.ai[1] > 1.15f)
                Projectile.Kill();
        }
    }
    public class ShewRainExplosion : ModProjectile
    {
        public override string Texture => Helper.Placeholder;
        public override void SetDefaults()
        {
            Projectile.height = 1;
            Projectile.width = 1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 200;
            Projectile.tileCollide = false;
        }
        public override bool ShouldUpdatePosition() => false;
        int seed;
        public override void OnSpawn(IEntitySource source)
        {
            seed = Main.rand.Next(int.MaxValue);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (seed == 0) return false;
            Texture2D tex = Helper.GetTex("MythosOfMoonlight/Assets/Textures/Extra/slash");
            Main.spriteBatch.Reload(BlendState.Additive);
            float alpha = MathHelper.Lerp(0.5f, 0, Projectile.ai[1]) * 2;
            UnifiedRandom rand = new UnifiedRandom(seed);
            float max = 40;
            for (float i = 0; i < max; i++)
            {
                float angle = Helper.CircleDividedEqually(i, max);
                float scale = rand.NextFloat(0.1f, 0.5f);
                Vector2 offset = new Vector2(Main.rand.NextFloat(25) * Projectile.ai[1] * scale, 0).RotatedBy(angle);
                for (float j = 0; j < 2; j++)
                    Main.spriteBatch.Draw(tex, Projectile.Center + offset - Main.screenPosition, null, Color.White * alpha, angle, tex.Size() / 2, new Vector2(Projectile.ai[1], alpha) * scale, SpriteEffects.None, 0);
            }
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
        public override void AI()
        {
            Projectile.ai[2] = MathHelper.Lerp(Projectile.ai[2], 0.2f, 0.1f);
            Projectile.ai[0] = MathHelper.Lerp(Projectile.ai[0], Projectile.ai[0] + Projectile.ai[2] + (Projectile.ai[0] * 0.075f), 0.15f);
            Projectile.ai[1] = MathHelper.SmoothStep(0, 1.5f, Projectile.ai[0]);

            if (Projectile.ai[1] > 1.15f)
                Projectile.Kill();
        }
    }
}
