using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.BaseClasses.BaseProj
{
    public abstract class FriendlyProj : ModProjectile
    {
        public float RotOffset;
        public int TrailInterval = 0;
        public float TrailFade = 1f;
        public int MaxTime;
        public bool DirectionToVel = false;
        public bool DrawTrail = false;
        public bool IsAdditive = false;
        public bool IsTrailAdditive = false;
        /// <summary>
        /// For quickly set static defaults which share among all projectiles with the same internal name that inherits this class.
        /// </summary>
        /// <param name="name">the display name</param>
        /// <param name="trailLength">how many points will be recorded for trail drawing things or sth else</param>
        /// <param name="trailMode">the trail mode</param>
        public void StaticDefaults(int frame = 1, int trailLength = 0, int trailMode = 0)
        {
            if (frame > 1)
            {
                Main.projFrames[Type] = frame;
            }
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = trailLength;
            if (trailLength > 0)
            {
                ProjectileID.Sets.TrailingMode[Projectile.type] = trailMode;
            }
        }
        /// <summary>
        /// For quickly set defaults for projectiles that inherits this class.
        /// </summary>
        /// <param name="width">the width of hitbox</param>
        /// <param name="height">the height of hitbox</param>
        /// <param name="timeLeft">the max timeleft on spawn</param>
        /// <param name="tileCollide">whether or not be killed or do sth else on tile collide</param>
        /// <param name="extraUpdate">extra updates per frame</param>
        /// <param name="rotOffset">how many degrees the sprite should rotate to fit actual rotation</param>
        /// <param name="trailInterval">the interval between every 2 drawn points</param>
        /// <param name="trailFade">how quickly should the trail fade out</param>
        /// <param name="DirToVel">whether the projectile syncs its rotation to the velocity</param>
        /// <param name="additive">whether the projectile will be drawn in additive blending</param>
        public void Defaults(int width, int height, int timeLeft = -1, int penetrate = 1, bool tileCollide = true, int extraUpdate = 0, float rotOffset = 0, int trailInterval = 1, float trailFade = 1f, bool DirToVel = true, bool additive = false, bool isTrailAdditive = false)
        {
            Projectile.width = width;
            Projectile.height = height;
            if (timeLeft > 0)
            {
                Projectile.timeLeft = timeLeft;
                MaxTime = timeLeft;
            }
            Projectile.penetrate = penetrate;
            Projectile.tileCollide = tileCollide;
            Projectile.extraUpdates = extraUpdate;
            RotOffset = rotOffset;
            Projectile.netUpdate = true;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            DirectionToVel = DirToVel;
            IsAdditive = additive;
            IsTrailAdditive = isTrailAdditive;
            if (ProjectileID.Sets.TrailCacheLength[Projectile.type] > 0)
            {
                TrailInterval = trailInterval;
                TrailFade = trailFade;
                DrawTrail = true;
            }
        }
        public override void PostAI()
        {
            if (DirectionToVel) Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(RotOffset);
        }
        public void Glow(Vector3 lightColor)
        {
            Lighting.AddLight(Projectile.Center, lightColor);
        }
        public void SpriteBatchTrail(Color trailColor, bool flipH = false, bool flipV = false)
        {
            Main.instance.LoadProjectile(Projectile.type);
            SpriteEffects effect = SpriteEffects.None;
            if (flipH) effect = Projectile.velocity.X >= 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            if (flipV) effect = Projectile.velocity.X >= 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, IsTrailAdditive ? BlendState.Additive : BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            if (ProjectileID.Sets.TrailCacheLength[Projectile.type] > 0)
            {
                if (DrawTrail)
                {
                    if (Projectile.timeLeft < MaxTime - ProjectileID.Sets.TrailCacheLength[Projectile.type])
                    {
                        for (int i = 1; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i += TrailInterval)
                        {
                            Vector2 pos = Projectile.oldPos[i] + new Vector2(Projectile.width / 2f, Projectile.height / 2f) - Main.screenPosition;
                            float scaleLerp = (float)Math.Pow(TrailFade, ((float)Projectile.oldPos.Length - i) / Projectile.oldPos.Length);
                            float scale = MathHelper.Lerp(1, scaleLerp, TrailFade);
                            Color color = trailColor * (float)((float)((float)Projectile.oldPos.Length - i) / Projectile.oldPos.Length) * ((float)(255 - (float)Projectile.alpha) / 255f);
                            spriteBatch.Draw(
                                TextureAssets.Projectile[Projectile.type].Value,
                                pos,
                                null,
                                color,
                                ProjectileID.Sets.TrailingMode[Projectile.type] == 2 ? Projectile.oldRot[i] : Projectile.rotation,
                                TextureAssets.Projectile[Projectile.type].Value.Size() / 2,
                                scale,
                                effect,
                                0
                                );
                        }
                    }
                }
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public void Draw(Color lightColor, bool flipH = false, bool flipV = false)
        {
            SpriteEffects effect = SpriteEffects.None;
            if (flipH) effect = Projectile.velocity.X >= 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            if (flipV) effect = Projectile.velocity.X >= 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
            Main.instance.LoadProjectile(Projectile.type);
            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, IsAdditive ? BlendState.Additive : BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            spriteBatch.Draw(
                TextureAssets.Projectile[Projectile.type].Value,
                Projectile.Center - Main.screenPosition,
                null,
                Projectile.GetAlpha(lightColor),
                Projectile.rotation,
                new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width / 2f, TextureAssets.Projectile[Projectile.type].Value.Height / 2f),
                Projectile.scale,
                effect,
                0f
                );
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public void PrimitiveTrail(Color trailColor, float alpha = 1f, bool fade = false)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Vector2 ori = Projectile.Size / 2;
            Texture2D trail = Helper.GetTex("White");
            List<VertexInfo2> vertices = new();
            for (int j = 1; j <= Math.Min(MaxTime - Projectile.timeLeft, ProjectileID.Sets.TrailCacheLength[Projectile.type] - 1); j += 1)
            {
                if (Projectile.oldPos[j] != Vector2.Zero)
                {
                    vertices.Add(new VertexInfo2(
                        Projectile.oldPos[j] + ori - Main.screenPosition + ((Projectile.oldPos[j - 1] - Projectile.oldPos[j]).ToRotation() + MathHelper.PiOver2).ToRotationVector2() * (Projectile.width / 2) * (fade ? (1 - j / (float)(Math.Min(MaxTime - Projectile.timeLeft, ProjectileID.Sets.TrailCacheLength[Projectile.type] - 1) + 1)) : 1),
                        new Vector3(j / (float)(Math.Min(MaxTime - Projectile.timeLeft, ProjectileID.Sets.TrailCacheLength[Projectile.type] - 1) + 1), 0, 1 - (j / (float)(Math.Min(MaxTime - Projectile.timeLeft, ProjectileID.Sets.TrailCacheLength[Projectile.type] - 1) + 1))),
                        trailColor * (1 - (j / (float)(Math.Min(MaxTime - Projectile.timeLeft, ProjectileID.Sets.TrailCacheLength[Projectile.type] - 1) + 1))) * alpha));
                    vertices.Add(new VertexInfo2(
                        Projectile.oldPos[j] + ori - Main.screenPosition + ((Projectile.oldPos[j - 1] - Projectile.oldPos[j]).ToRotation() - MathHelper.PiOver2).ToRotationVector2() * Projectile.width / 2 * (fade ? (1 - j / (float)(Math.Min(MaxTime - Projectile.timeLeft, ProjectileID.Sets.TrailCacheLength[Projectile.type] - 1) + 1)) : 1),
                        new Vector3(j / (float)(Math.Min(MaxTime - Projectile.timeLeft, ProjectileID.Sets.TrailCacheLength[Projectile.type] - 1) + 1), .25f, 1 - (j / (float)(Math.Min(MaxTime - Projectile.timeLeft, ProjectileID.Sets.TrailCacheLength[Projectile.type] - 1) + 1))),
                        trailColor * (1 - (j / (float)(Math.Min(MaxTime - Projectile.timeLeft, ProjectileID.Sets.TrailCacheLength[Projectile.type] - 1) + 1))) * alpha));
                }
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Main.graphics.GraphicsDevice.Textures[0] = trail;
            if (vertices.Count >= 3)
            {
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public void Stroke(Color strokeColor, SpriteEffects effect, int num = 8, float radius = 5, float rotSpeed = 6f, float alpha = 1f, float size = 1f)
        {
            Main.instance.LoadProjectile(Projectile.type);
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D origTex = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 orig = origTex.Size() / 2;
            float rot = MathHelper.ToRadians(rotSpeed * Common.Systems.TimerSystem.TimerNoPause);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            for (int i = 1; i <= num; i++)
            {
                Vector2 pos = Projectile.Center - Main.screenPosition + (rot + MathHelper.TwoPi * i / num).ToRotationVector2() * radius;
                spriteBatch.Draw(
                origTex,
                pos,
                null,
                strokeColor * alpha,
                Projectile.rotation,
                orig,
                Projectile.scale * size,
                effect,
                0f
                );
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}
