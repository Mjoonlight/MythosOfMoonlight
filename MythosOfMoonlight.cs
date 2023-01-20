using Microsoft.Xna.Framework;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using MythosOfMoonlight.Dusts;
using System;
using MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim;
using ReLogic.Content;
using System.Reflection;
using Terraria.GameContent;
using MythosOfMoonlight.Items.BossSummons;

namespace MythosOfMoonlight
{
    public static class TRay
    {
        public static Vector2 Cast(Vector2 start, Vector2 direction, float length)
        {
            direction = direction.SafeNormalize(Vector2.UnitY);
            Vector2 output = start;
            for (int i = 0; i < length; i++)
            {
                if (Collision.CanHitLine(output, 0, 0, output + direction, 0, 0))
                {
                    output += direction;
                }
                else
                {
                    break;
                }
            }
            return output;
        }
        public static float CastLength(Vector2 start, Vector2 direction, float length)
        {
            Vector2 end = Cast(start, direction, length);
            return (end - start).Length();
        }
    }
    public static class Helper
    {
        public static void SineMovement(this Projectile projectile, Vector2 initialCenter, Vector2 initialVel, float frequencyMultiplier, float amplitude)
        {
            projectile.ai[1]++;
            float wave = (float)Math.Sin(projectile.ai[1] * frequencyMultiplier);
            Vector2 vector = new Vector2(initialVel.X, initialVel.Y).RotatedBy(MathHelper.ToRadians(90));
            vector.Normalize();
            wave *= projectile.ai[0];
            wave *= amplitude;
            Vector2 offset = vector * wave;
            projectile.Center = initialCenter + (projectile.velocity * projectile.ai[1]);
            projectile.Center = projectile.Center + offset;
        }
        public static float CircleDividedEqually(float i, float max)
        {
            return 2f * (float)Math.PI / max * i;
        }
        public static Vector2 FromAToB(Vector2 a, Vector2 b, bool normalize = true, bool reverse = false)
        {
            Vector2 baseVel = b - a;
            if (normalize)
                baseVel.Normalize();
            if (reverse)
            {
                Vector2 baseVelReverse = a - b;
                if (normalize)
                    baseVelReverse.Normalize();
                return baseVelReverse;
            }
            return baseVel;
        }
        public static int HostileProjDmg(int normal, int expert, int master)
        {
            int d = Main.masterMode ? master / 6 : (Main.expertMode ? expert / 4 : normal / 2);
            return d;
        }
        public static void Reload(this SpriteBatch spriteBatch, SpriteSortMode sortMode = SpriteSortMode.Deferred)
        {
            if ((bool)spriteBatch.GetType().GetField("beginCalled", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch))
            {
                spriteBatch.End();
            }
            BlendState blendState = (BlendState)spriteBatch.GetType().GetField("blendState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            SamplerState samplerState = (SamplerState)spriteBatch.GetType().GetField("samplerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            DepthStencilState depthStencilState = (DepthStencilState)spriteBatch.GetType().GetField("depthStencilState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            RasterizerState rasterizerState = (RasterizerState)spriteBatch.GetType().GetField("rasterizerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Effect effect = (Effect)spriteBatch.GetType().GetField("customEffect", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Matrix matrix = (Matrix)spriteBatch.GetType().GetField("transformMatrix", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, matrix);
        }
        public static void Reload(this SpriteBatch spriteBatch, BlendState blendState = default)
        {
            if ((bool)spriteBatch.GetType().GetField("beginCalled", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch))
            {
                spriteBatch.End();
            }
            SpriteSortMode sortMode = SpriteSortMode.Deferred;
            SamplerState samplerState = (SamplerState)spriteBatch.GetType().GetField("samplerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            DepthStencilState depthStencilState = (DepthStencilState)spriteBatch.GetType().GetField("depthStencilState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            RasterizerState rasterizerState = (RasterizerState)spriteBatch.GetType().GetField("rasterizerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Effect effect = (Effect)spriteBatch.GetType().GetField("customEffect", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Matrix matrix = (Matrix)spriteBatch.GetType().GetField("transformMatrix", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, matrix);
        }
        public static void Reload(this SpriteBatch spriteBatch, Effect effect = null)
        {
            if ((bool)spriteBatch.GetType().GetField("beginCalled", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch))
            {
                spriteBatch.End();
            }
            SpriteSortMode sortMode = SpriteSortMode.Deferred;
            BlendState blendState = (BlendState)spriteBatch.GetType().GetField("blendState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            SamplerState samplerState = (SamplerState)spriteBatch.GetType().GetField("samplerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            DepthStencilState depthStencilState = (DepthStencilState)spriteBatch.GetType().GetField("depthStencilState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            RasterizerState rasterizerState = (RasterizerState)spriteBatch.GetType().GetField("rasterizerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Matrix matrix = (Matrix)spriteBatch.GetType().GetField("transformMatrix", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, matrix);
        }
        public static Rectangle GetFrame(this NPC NPC) => new(0, NPC.frame.Y, NPC.width, NPC.height);
        public static float RandomRotation() => Main.rand.NextFloat() * MathHelper.TwoPi;
        public static float Squared(float flt) => flt * flt;
        public static Player PlayerTarget(this NPC NPC) => Main.player[NPC.target];
        public static Vector2 CoordToTile(Vector2 coordinates)
        {
            return new Vector2((int)(coordinates.X / 16f), (int)(coordinates.Y / 16f));
        }
        public static bool TileAtWorldPosition(Vector2 coords) => TileAtTilePosition(CoordToTile(coords));
        public static bool TileAtTilePosition(Vector2 coords)
        {
            return Framing.GetTileSafely(coords).HasTile;
        }
        public static bool PositionComparison(Vector2 center, Vector2 other, float minDist) // compares two vectors to see if the distsance between them exceeds a certain value or not. returns true if it does, false if it doesn't
            => (center - other).LengthSquared() < Squared(minDist);
        public static bool WarpAroundPlayer(this NPC NPC, Vector2 center, float sqrMinDistFromCenter, float radius, int attempts = -1) // when attempts == -1, attempts to find an open spot to teleport to until it does so successfully
        {
            Vector2 finalPos = center + Main.rand.NextVector2Circular(radius, radius);
            for (int i = 0; TileAtWorldPosition(finalPos) || (center - finalPos).LengthSquared() < sqrMinDistFromCenter; i++)
            {
                finalPos = center + Main.rand.NextVector2Circular(radius, radius);
            }
            NPC.Center = finalPos;
            return NPC.Center == finalPos;
        }
        public static Texture2D GetTex(string fullPath, bool immediate = false)
        {
            return Request<Texture2D>(fullPath, immediate ? AssetRequestMode.ImmediateLoad : AssetRequestMode.AsyncLoad).Value;
        }
        public static Vector2 GetWarpPosition(this NPC NPC, Vector2 center, float sqrMinDistFromCenter, float radius)
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
            for (int x = intX; x < intX + xRange; x++)
            {
                var tile = Framing.GetTileSafely(new Vector2(x, intY));
                if (tile.HasTile)
                {
                    return tile;
                }
            }
            return default;
        }
        public static Tile GetTileInHorizontalRange(float x, float y, int xRange) // start is tile coords
        {
            return GetTileInHorizontalRange(new Vector2(x, y), xRange);
        }
        public static void SpawnDust(Vector2 position, Vector2 size, int type, Vector2 velocity = default, int amount = 1, Action<Dust> dustModification = null)
        {
            for (int i = 0; i < amount; i++)
            {
                var dust = Main.dust[Dust.NewDust(position, (int)size.X, (int)size.Y, type, velocity.X, velocity.Y)];
                dustModification?.Invoke(dust);
            }
        }
        public static void SpawnGore(NPC NPC, string gore, int amount = 1, int type = -1, Vector2 vel = default)
        {
            var position = NPC.Center;
            if (type != -1)
            {
                gore += type;
            }
            for (int i = 0; i < amount; i++)
            {
                Gore.NewGore(NPC.GetSource_OnHit(NPC), position + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), vel, Find<ModGore>(gore).Type);
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
            private readonly float min;
            private readonly float max;

            public Range(float min, float max)
            {
                this.min = min;
                this.max = max;
            }
            public static implicit operator float(Range range) => range.FValue;
            public float FValue => Main.rand.NextFloat(min, max);
            public int IValue => Main.rand.Next((int)min, (int)max);
        }
        public static void FireProjectilesInArc(Entity entity, Vector2 origin, Vector2 centerDirection, float radians, int type, float speed, int damage, float knockback, int amount)
        {
            var centeredDir = centerDirection.RotatedBy(-radians / 2f); //-MathHelper.ToRadians(degrees / 2f));
            for (float i = 1; i <= amount; i++)
            {
                var direction = centeredDir.RotatedBy(i / amount * radians);
                Projectile.NewProjectile(entity.GetSource_FromThis(), origin, direction * speed, type, damage, knockback);
            }

            centeredDir = centerDirection.RotatedBy(-radians / 1.5f); //-MathHelper.ToRadians(degrees / 2f));
            for (float i = 1; i <= amount; i++)
            {
                var direction = centeredDir.RotatedBy(i / amount * radians);
                Projectile.NewProjectile(entity.GetSource_FromThis(), origin, direction * speed, type, damage, knockback);
            }

            centeredDir = centerDirection.RotatedBy(-radians); //-MathHelper.ToRadians(degrees / 2f));
            for (float i = 1; i <= amount; i++)
            {
                var direction = centeredDir.RotatedBy(i / amount * radians);
                Projectile.NewProjectile(entity.GetSource_FromThis(), origin, direction * speed, type, damage, knockback);
            }
        }
        public static bool InRange(float value, float min, float max) => value < max && value > min;
        public static bool InRange(double value, double min, double max) => value < max && value > min;
    }
    public class MythosOfMoonlight : Mod
    {
        public static RenderTarget2D OrigRender;
        public static RenderTarget2D DustTrail1;
        public static Effect PurpleCometEffect;//, ScreenDistort;
        public static MythosOfMoonlight Instance { get; set; }
        public MythosOfMoonlight()
        {
            Instance = this;
        }
        public override void Load()
        {
            if (!Main.dedServ)
            {
                PurpleCometEffect = Instance.Assets.Request<Effect>("Effects/PurpleComet", AssetRequestMode.ImmediateLoad).Value;
                //ScreenDistort = Instance.Assets.Request<Effect>("Effects/DistortMove", AssetRequestMode.ImmediateLoad).Value;
                Filters.Scene["PurpleComet"] = new Filter(new ScreenShaderData(new Ref<Effect>(PurpleCometEffect), "ModdersToolkitShaderPass"), EffectPriority.VeryHigh);
                SkyManager.Instance["PurpleComet"] = new Events.PurpleCometSky();
            }
            On.Terraria.Graphics.Effects.FilterManager.EndCapture += FilterManager_EndCapture;
            On.Terraria.Main.LoadWorlds += new On.Terraria.Main.hook_LoadWorlds(Main_LoadWorlds);
            On.Terraria.Player.SetTalkNPC += Player_SetTalkNPC;
            Main.OnResolutionChanged += Main_OnResolutionChanged;
        }
        private void Player_SetTalkNPC(On.Terraria.Player.orig_SetTalkNPC orig, Player self, int npcIndex, bool fromNet)
        {
            if (npcIndex == NPCType<Starine_Symbol>())
            {
                self.currentShoppingSettings.HappinessReport = "";
            }
            orig.Invoke(self, npcIndex, fromNet);
        }

        private void Main_OnResolutionChanged(Vector2 obj)
        {
            if (OrigRender == null)
            {
                OrigRender = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);
                DustTrail1 = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);
            }
        }

        private void Main_LoadWorlds(On.Terraria.Main.orig_LoadWorlds orig)
        {
            orig.Invoke();
            if (OrigRender == null)
            {
                GraphicsDevice gd = Main.graphics.GraphicsDevice;
                OrigRender = new RenderTarget2D(gd, gd.PresentationParameters.BackBufferWidth, gd.PresentationParameters.BackBufferHeight, false, gd.PresentationParameters.BackBufferFormat, 0);
                DustTrail1 = new RenderTarget2D(gd, gd.PresentationParameters.BackBufferWidth, gd.PresentationParameters.BackBufferHeight, false, gd.PresentationParameters.BackBufferFormat, 0);
            }
        }

        private void FilterManager_EndCapture(On.Terraria.Graphics.Effects.FilterManager.orig_EndCapture orig, FilterManager self, RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Color clearColor)
        {
            GraphicsDevice gd = Main.graphics.GraphicsDevice;
            if (Main.myPlayer >= 0)
            {
                if (OrigRender == null)
                {
                    OrigRender = new RenderTarget2D(gd, gd.PresentationParameters.BackBufferWidth, gd.PresentationParameters.BackBufferHeight, false, gd.PresentationParameters.BackBufferFormat, 0);
                    DustTrail1 = new RenderTarget2D(gd, gd.PresentationParameters.BackBufferWidth, gd.PresentationParameters.BackBufferHeight, false, gd.PresentationParameters.BackBufferFormat, 0);
                }
                DustTrail(gd);
            }
            orig.Invoke(self, finalTexture, screenTarget1, screenTarget2, clearColor);
        }
        private void DustTrail(GraphicsDevice graphicsDevice)
        {
            graphicsDevice.SetRenderTarget(Main.screenTargetSwap);
            graphicsDevice.Clear(Color.Transparent);
            Main.spriteBatch.Begin(0, BlendState.AlphaBlend);
            Main.spriteBatch.Draw(Main.screenTarget, Vector2.Zero, Color.White);
            Main.spriteBatch.End();
            graphicsDevice.SetRenderTarget(OrigRender);
            graphicsDevice.Clear(Color.Transparent);
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend);
            Main.spriteBatch.Draw(Main.screenTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, 0, 0f);
            Main.spriteBatch.End();
            graphicsDevice.SetRenderTarget(DustTrail1);
            graphicsDevice.Clear(Color.Transparent);
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            foreach (Dust dust in Main.dust)
            {
                if (dust.type == ModContent.DustType<StarineDust>() && dust.velocity.Length() > 0 && dust.active)
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        Main.spriteBatch.Draw(Request<Texture2D>("MythosOfMoonlight/Dusts/StarineDust").Value, dust.position - dust.velocity * i - Main.screenPosition, dust.frame, Color.White * (float)Math.Sqrt((float)Math.Abs((float)(5f - (float)i) / 5f)), dust.rotation, new Vector2(4, 4), dust.scale * (float)((float)(5f - (float)i) / 5f), SpriteEffects.None, 0f);
                    }
                }
            }
            Main.spriteBatch.End();
            graphicsDevice.SetRenderTarget(Main.screenTarget);
            graphicsDevice.Clear(Color.Transparent);
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            Main.spriteBatch.Draw(OrigRender, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, 0, 0f);
            Main.spriteBatch.Draw(DustTrail1, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, 0, 0f);
            Main.spriteBatch.End();
        }
        /*public override void UpdateMusic(ref int music, ref MusicPriority priority) // Put this in a ModSceneEffect thing
        {
            if (PurpleCometEvent.PurpleComet && Main.LocalPlayer.ZoneOverworldHeight)
            {
                music = GetSoundSlot(SoundType.Music, "Sounds/Music/PurpleComet");
                priority = MusicPriority.Event;
            }
        }*/
    }
}