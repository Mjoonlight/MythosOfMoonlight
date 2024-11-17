using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MythosOfMoonlight.Common.Crossmod;
using MythosOfMoonlight.Dusts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs.Minibosses.StarveiledProj
{
    public class ScholarArrow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 40;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            Projectile.AddElement(CrossModHelper.Celestial);
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WoodenArrowHostile);
            Projectile.Size = new(18, 48);
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 1000;
            Projectile.extraUpdates = 1;
            Projectile.tileCollide = false;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox) => hitbox.Height = 18;
        public override void OnKill(int timeLeft)
        {
            // Impact VFX, sparkle dust + small light explosion thing idk
        }
        public override bool PreDraw(ref Color lightColor)
        {
            animationOffset -= 0.05f;
            if (animationOffset <= 0)
                animationOffset = 1;
            animationOffset = MathHelper.Clamp(animationOffset, float.Epsilon, 1 - float.Epsilon);
            if (Projectile.timeLeft >= 999) return false;
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Texture2D bloom = Helper.GetTex(Texture + "_Bloom");
            List<VertexPositionColorTexture> vertices = new List<VertexPositionColorTexture>(ProjectileID.Sets.TrailCacheLength[Type]);
            for (int i = 0; i < Projectile.oldPos.Length - 1; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero || i == 0) continue;
                float mult = 1f - (1f / Projectile.oldPos.Length) * i;
                mult *= mult;

                float __off = animationOffset;
                if (__off > 1) __off = -__off + 1;
                float _off = __off + (float)i / Projectile.oldPos.Length;

                if (mult > 0)
                {
                    vertices.Add(Helper.AsVertex(Projectile.oldPos[i] + (Projectile.velocity.ToRotation()).ToRotationVector2() * 6 - Main.screenPosition + Projectile.Size / 2 + new Vector2(16 * mult, 0).RotatedBy(Helper.FromAToB(Projectile.oldPos[i], Projectile.oldPos[i + 1]).ToRotation() + MathHelper.PiOver2), Color.Orchid * mult * bloomAlpha, new Vector2(_off, 0)));
                    vertices.Add(Helper.AsVertex(Projectile.oldPos[i] + (Projectile.velocity.ToRotation()).ToRotationVector2() * 6 - Main.screenPosition + Projectile.Size / 2 + new Vector2(16 * mult, 0).RotatedBy(Helper.FromAToB(Projectile.oldPos[i], Projectile.oldPos[i + 1]).ToRotation() - MathHelper.PiOver2), Color.Orchid * mult * bloomAlpha, new Vector2(_off, 1)));
                }
            }
            Main.spriteBatch.SaveCurrent();
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            if (vertices.Count > 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    Helper.DrawTexturedPrimitives(vertices.ToArray(), PrimitiveType.TriangleStrip, Helper.GetExtraTex("Extra/trail_01"), false);
                    Helper.DrawTexturedPrimitives(vertices.ToArray(), PrimitiveType.TriangleStrip, Helper.GetExtraTex("Extra/Ex1_backup"), false);
                }
            }
            Main.spriteBatch.ApplySaved();
            Main.spriteBatch.Reload(BlendState.Additive);
            Main.EntitySpriteDraw(bloom, Projectile.Center - Main.screenPosition, null, Color.White * bloomAlpha, Projectile.rotation, bloom.Size() / 2, Projectile.scale, SpriteEffects.None);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, tex.Size() / 2, Projectile.scale, SpriteEffects.None);
            return false;
        }
        float bloomAlpha = 1, animationOffset;
        public override void AI()
        {
            if (Projectile.timeLeft < 930)
                Projectile.velocity.Y = MathHelper.Lerp(Projectile.velocity.Y, 15, 0.0001f);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.timeLeft % 8 == 0)
                Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<SparkleDust>(), -Projectile.velocity.RotatedByRandom(MathHelper.PiOver4 * 0.25f) * 0.4f, 0, Color.Lerp(Color.Orchid, Color.Thistle, Main.rand.NextFloat()), Main.rand.NextFloat(0.05f, 0.1f));
            if (Projectile.timeLeft % 8 == 4)
                Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<SparkleDust>(), Projectile.velocity.RotatedByRandom(MathHelper.PiOver4 * 0.25f) * 0.4f, 0, Color.Lerp(Color.Orchid, Color.Thistle, Main.rand.NextFloat()), Main.rand.NextFloat(0.05f, 0.1f));
        }
    }
    public class FastScholarArrow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 300;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 4000;
            Projectile.AddElement(CrossModHelper.Celestial);
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WoodenArrowHostile);
            Projectile.Size = new(18, 48);
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 3000;
            Projectile.tileCollide = false;
            Projectile.scale = 1.3f;
            Projectile.extraUpdates = 10;
        }
        public override void OnSpawn(IEntitySource source)
        {
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox) => hitbox.Height = 18;
        public override void OnKill(int timeLeft)
        {
            // Impact VFX, sparkle dust + small light explosion thing idk
        }
        public override bool PreDraw(ref Color lightColor)
        {
            animationOffset -= 0.05f;
            if (animationOffset <= 0)
                animationOffset = 1;
            animationOffset = MathHelper.Clamp(animationOffset, float.Epsilon, 1 - float.Epsilon);

            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Texture2D bloom = Helper.GetTex(Texture + "_Bloom");
            List<VertexPositionColorTexture> vertices = new List<VertexPositionColorTexture>(ProjectileID.Sets.TrailCacheLength[Type]);
            for (int i = 0; i < Projectile.oldPos.Length - 1; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero) continue;
                float mult = 1f - (1f / Projectile.oldPos.Length) * i;
                mult *= mult;

                float __off = animationOffset;
                if (__off > 1) __off = -__off + 1;
                float _off = __off + (float)i / Projectile.oldPos.Length;

                if (mult > 0)
                {
                    vertices.Add(Helper.AsVertex(Projectile.oldPos[i] + (Projectile.rotation - MathHelper.PiOver2).ToRotationVector2() * 6 - Main.screenPosition + Projectile.Size / 2 + new Vector2(17 * mult, 0).RotatedBy(Helper.FromAToB(Projectile.oldPos[i], Projectile.oldPos[i + 1]).ToRotation() + MathHelper.PiOver2), Color.Lerp(Color.White, Color.Orchid, Projectile.ai[2]) * mult * bloomAlpha, new Vector2(_off, 0)));
                    vertices.Add(Helper.AsVertex(Projectile.oldPos[i] + (Projectile.rotation - MathHelper.PiOver2).ToRotationVector2() * 6 - Main.screenPosition + Projectile.Size / 2 + new Vector2(17 * mult, 0).RotatedBy(Helper.FromAToB(Projectile.oldPos[i], Projectile.oldPos[i + 1]).ToRotation() - MathHelper.PiOver2), Color.Lerp(Color.White, Color.Orchid, Projectile.ai[2]) * mult * bloomAlpha, new Vector2(_off, 1)));
                }
            }
            Main.spriteBatch.SaveCurrent();
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            if (vertices.Count > 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    Helper.DrawTexturedPrimitives(vertices.ToArray(), PrimitiveType.TriangleStrip, Helper.GetExtraTex("Extra/trail_01"), false);
                    Helper.DrawTexturedPrimitives(vertices.ToArray(), PrimitiveType.TriangleStrip, Helper.GetExtraTex("Extra/wavyLaser2"), false);
                }
            }
            Main.spriteBatch.ApplySaved();
            Main.spriteBatch.Reload(BlendState.Additive);
            Main.EntitySpriteDraw(bloom, Projectile.Center - Main.screenPosition, null, Color.White * bloomAlpha, Projectile.rotation, bloom.Size() / 2, Projectile.scale, SpriteEffects.None);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, tex.Size() / 2, Projectile.scale, SpriteEffects.None);
            return false;
        }
        float bloomAlpha = 1, animationOffset;
        public override void AI()
        {
            if (Projectile.ai[2] == 0)
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust.NewDustPerfect(Projectile.Center + Projectile.velocity, ModContent.DustType<SparkleDust>(), Main.rand.NextVector2Circular(10, 10) + Projectile.velocity, 0, Color.Lerp(Color.Orchid, Color.Thistle, Main.rand.NextFloat()), Main.rand.NextFloat(0.05f, 0.1f));

                    Dust.NewDustPerfect(Projectile.Center + Projectile.velocity, ModContent.DustType<SparkleDust>(), Main.rand.NextVector2Circular(.1f, .1f), 0, Color.Lerp(Color.Orchid, Color.Thistle, Main.rand.NextFloat()), Main.rand.NextFloat(0.05f, 0.15f));
                }
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.ai[2] = MathHelper.Lerp(Projectile.ai[2], 1, 0.15f);
            if (Projectile.timeLeft % 8 == 0)
                Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<SparkleDust>(), -Projectile.velocity.RotatedByRandom(MathHelper.PiOver4 * 0.25f) * 0.4f, 0, Color.Lerp(Color.Orchid, Color.Thistle, Main.rand.NextFloat()), Main.rand.NextFloat(0.05f, 0.1f));
            if (Projectile.timeLeft % 8 == 4)
                Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<SparkleDust>(), Projectile.velocity.RotatedByRandom(MathHelper.PiOver4 * 0.25f) * 0.4f, 0, Color.Lerp(Color.Orchid, Color.Thistle, Main.rand.NextFloat()), Main.rand.NextFloat(0.05f, 0.1f));
        }
    }
}
