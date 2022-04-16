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
        public static Rectangle GetFrame(this NPC npc) => new Rectangle(0, npc.frame.Y, npc.width, npc.height);
        public static float RandomRotation() => Main.rand.NextFloat() * MathHelper.TwoPi;
        public static float Squared(float flt) => flt * flt;
        public static Player PlayerTarget(this NPC npc) => Main.player[npc.target];
        public static Vector2 CoordToTile(Vector2 coordinates)
        {
            return new Vector2((int)(coordinates.X / 16f), (int)(coordinates.Y / 16f));
        }
        public static bool TileAtWorldPosition(Vector2 coords) => TileAtTilePosition(CoordToTile(coords));
        public static bool TileAtTilePosition(Vector2 coords)
        {
            return Framing.GetTileSafely(coords).active();
        }
        public static bool PositionComparison(Vector2 center, Vector2 other, float minDist) // compares two vectors to see if the distsance between them exceeds a certain value or not. returns true if it does, false if it doesn't
            => (center - other).LengthSquared() < Squared(minDist);
        public static bool WarpAroundPlayer(this NPC npc, Vector2 center, float sqrMinDistFromCenter, float radius, int attempts = -1) // when attempts == -1, attempts to find an open spot to teleport to until it does so successfully
        {
            Vector2 finalPos = center + Main.rand.NextVector2Circular(radius, radius);
            for (int i = 0; TileAtWorldPosition(finalPos) || (center - finalPos).LengthSquared() < sqrMinDistFromCenter; i++)
            {
                finalPos = center + Main.rand.NextVector2Circular(radius, radius);
            }
            npc.Center = finalPos;
            return npc.Center == finalPos;
        }
        public static Vector2 GetWarpPosition(this NPC npc, Vector2 center, float sqrMinDistFromCenter, float radius)
        {
            Vector2 finalPos = center + Main.rand.NextVector2Circular(radius, radius);
            for (int i = 0; TileAtWorldPosition(finalPos) || (center - finalPos).LengthSquared() < sqrMinDistFromCenter; i++)
            {
                finalPos = center + Main.rand.NextVector2Circular(radius, radius);
            }
            return finalPos;
        }
        public static Tile GetTileInHorizontalRange(Vector2 start, int xRange) // start is tile coords
        {
            int intX = (int)start.X, intY = (int)start.Y; 
            for (int x = (int)intX; x < intX + xRange; x++)
            {
                var tile = Framing.GetTileSafely(new Vector2(x, intY));
                if (tile.active()) 
                {
                    return tile;
                }
            } return null;
        }
        public static Tile GetTileInHorizontalRange(float x, float y, int xRange) // start is tile coords
        {
            return GetTileInHorizontalRange(new Vector2(x, y), xRange);
        }
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
            if (PurpleCometEvent.PurpleComet && Main.LocalPlayer.ZoneOverworldHeight)
            {
                music = GetSoundSlot(SoundType.Music, "Sounds/Music/PurpleComet");
                priority = MusicPriority.Event;
            }
        }
        public override void Close()
        {
            var slots = new int[] {
                 GetSoundSlot(SoundType.Music, "Sounds/Music/PurpleComet"),
              };

            foreach (var slot in slots)
            {
                if (Main.music.IndexInRange(slot) && Main.music[slot]?.IsPlaying == true)
                {
                    Main.music[slot].Stop(Microsoft.Xna.Framework.Audio.AudioStopOptions.Immediate);
                }
            }

            base.Close();
        }
    }
}