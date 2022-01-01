using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace MythosOfMoonlight
{
    public static class Helper
    {
        public static void SpawnDust(Vector2 position, Vector2 size, int type, Vector2 velocity = default, int amount = 1)
        {
            for (int i = 0; i < amount; i++)
            {
                Dust.NewDust(position, (int)size.X, (int)size.Y, type, velocity.X, velocity.Y);
            }
        }
        public static void SpawnGore(NPC npc, string gore, int amount = 1, int type = -1)
        {
            var mod = npc.modNPC.mod;
            var position = npc.Center;
            if (type != -1)
            {
                gore += type;
            }
            for (int i = 0; i < amount; i++)
            {
                Gore.NewGore(position + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), Vector2.Zero, mod.GetGoreSlot(gore));
            }
        }
        public static float HorizontalDistance(Vector2 one, Vector2 two) => System.Math.Abs(one.X - two.X);
        public static Vector2 GetPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            float cx = 3 * (p1.X - p0.X);
            float cy = 3 * (p1.Y - p0.Y);
            float bx = 3 * (p2.X - p1.X) - cx;
            float by = 3 * (p2.Y - p1.Y) - cy;
            float ax = p3.X - p0.X - cx - bx;
            float ay = p3.Y - p0.Y - cy - by;
            float cube = t * t * t;
            float square = t * t;

            float resX = (ax * cube) + (bx * square) + (cx * t) + p0.X;
            float resY = (ay * cube) + (by * square) + (cy * t) + p0.Y;

            return new Vector2(resX, resY);
        }
        public struct Range
        {
            float min, max;
            public Range(float min, float max)
            {
                this.min = min;
                this.max = max;
            }
            public static implicit operator float(Range range) => range.FValue;
            public float FValue => Main.rand.NextFloat(min, max);
            public int IValue => Main.rand.Next((int)min, (int)max);
        }
        public static void FireProjectilesInArc(Vector2 origin, Vector2 centerDirection, float radians, int type, float speed, int damage, float knockback, int amount)
        {
            var centeredDir = centerDirection.RotatedBy(-radians / 2f); //-MathHelper.ToRadians(degrees / 2f));
            for (float i = 1; i <= amount; i++)
            {
                var direction = centeredDir.RotatedBy(i / amount * radians);
                Projectile.NewProjectile(origin, direction * speed, type, damage, knockback);
            }

            centeredDir = centerDirection.RotatedBy(-radians / 1.5f); //-MathHelper.ToRadians(degrees / 2f));
            for (float i = 1; i <= amount; i++)
            {
                var direction = centeredDir.RotatedBy(i / amount * radians);
                Projectile.NewProjectile(origin, direction * speed, type, damage, knockback);
            }

            centeredDir = centerDirection.RotatedBy(-radians); //-MathHelper.ToRadians(degrees / 2f));
            for (float i = 1; i <= amount; i++)
            {
                var direction = centeredDir.RotatedBy(i / amount * radians);
                Projectile.NewProjectile(origin, direction * speed, type, damage, knockback);
            }
        }
        public static bool InRange(float value, float min, float max) => value < max && value > min;
        public static bool InRange(double value, double min, double max) => value < max && value > min;
    }
    public class MythosOfMoonlight : Mod
	{
        public override void Load()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Filters.Scene["PurpleComet"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0.88f, 0.48f, 1.02f).UseOpacity(.8f), EffectPriority.VeryHigh);
                SkyManager.Instance["PurpleComet"] = new Events.PurpleCometSky();
            }
        }
        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            if (PurpleCometEvent.PurpleComet)
            {
                music = GetSoundSlot(SoundType.Music, "Sounds/Music/PurpleComet");
                priority = MusicPriority.Event;
            }
        }
    }
}