using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;

namespace MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim.Projectiles
{
    public class TestTentacle2 : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/Textures/Extra/Ex2";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Test Tentacle real");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 100;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        private List<float> rots;

        public int len;
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 160;
            Projectile.netUpdate = true;
            Projectile.netUpdate2 = true;
            Projectile.netImportant = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.extraUpdates = 2;
            rots = new List<float>();
            len = 0;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public Color drawColor;
        public Vector2 OldPoint(int i)
        {
            return Projectile.Center - Main.screenPosition + (Projectile.oldRot[i] * (float)(90f - i) / 90f + Projectile.ai[0]).ToRotationVector2() * (.12f * i * (float)(1f - (float)Math.Pow(.975f, i)) / .025f);
        }
        float value;
        public override void AI()
        {
            if (Projectile.timeLeft == 99)
            {
                if (Projectile.ai[0] == 0)
                    Projectile.ai[0] = 70;
                if (Projectile.ai[1] == 0)
                    Projectile.ai[1] = 0.5f;
            }
            for (int i = 0; i < 3; i++)
            {
                value += Projectile.ai[1];
                if (base.Projectile.timeLeft % 1 == 0)
                {
                    float factor = 1f;
                    Vector2 velocity = base.Projectile.velocity * factor * 4f;
                    Projectile.rotation = 0.3f * (float)Math.Sin((double)(value / 100f)) + velocity.ToRotation();
                    rots.Insert(0, Projectile.rotation);
                    while (rots.Count > Projectile.ai[0])
                    {
                        rots.RemoveAt(rots.Count - 1);
                    }
                }
                if (len < Projectile.ai[0] && Projectile.timeLeft > 80)
                {
                    len++;
                }
                if (len >= 0 && Projectile.timeLeft <= 80)
                {
                    len--;
                }
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for (int i = 1; i < len; i++)
            {
                float factor = (float)i / (float)len;
                float w = 10 * MathHelper.SmoothStep(0.8f, 0.1f, factor);
                if (Collision.CheckAABBvAABBCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center - new Vector2(w, w) + Utils.RotatedBy(new Vector2((float)(5 * i), 0f), rots[i]), new Vector2(w, w) * 2f))
                {
                    return true;
                }
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            List<VertexInfo2> bars = new List<VertexInfo2>();
            for (int i = 1; i < len; i++)
            {
                float factor = (float)i / (float)len;
                Vector2 v0 = Projectile.Center + Utils.RotatedBy(new Vector2((float)(5 * (i - 1)), 0f), rots[i - 1]);
                Vector2 v1 = Projectile.Center + Utils.RotatedBy(new Vector2((float)(5 * i), 0f), rots[i]);
                Vector2 normaldir = v1 - v0;
                normaldir = new Vector2(normaldir.Y, 0f - normaldir.X);
                ((Vector2)(normaldir)).Normalize();
                float w = 10 * MathHelper.SmoothStep(0.8f, 0.1f, factor);
                bars.Add(new VertexInfo2(v1 + w * normaldir, new Vector3(factor, 0f, 0f), drawColor));
                bars.Add(new VertexInfo2(v1 - w * normaldir, new Vector3(factor, 1f, 0f), drawColor));
            }
            if (bars.Count > 2)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
                Matrix projection = Matrix.CreateOrthographicOffCenter(0f, (float)Main.screenWidth, (float)Main.screenHeight, 0f, 0f, 1f);
                Matrix model = Matrix.CreateTranslation(new Vector3(0f - Main.screenPosition.X, 0f - Main.screenPosition.Y, 0f)) * Main.GameViewMatrix.ZoomMatrix;
                MythosOfMoonlight.Tentacle.Parameters[0].SetValue(model * projection);
                MythosOfMoonlight.Tentacle.CurrentTechnique.Passes[0]
                    .Apply();
                ((Game)Main.instance).GraphicsDevice.Textures[0] = (Texture)(object)ModContent.Request<Texture2D>(Texture, (AssetRequestMode)2).Value;
                ((Game)Main.instance).GraphicsDevice.DrawUserPrimitives<VertexInfo2>((PrimitiveType)1, bars.ToArray(), 0, bars.Count - 2);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, (Effect)null, Main.GameViewMatrix.TransformationMatrix);
            }
            return false;
        }
    }
    public class TestTentacleProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Test Tentacle");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 100;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 300;
            Projectile.netUpdate = true;
            Projectile.netUpdate2 = true;
            Projectile.netImportant = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.extraUpdates = 2;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 1, 1, 1);
            if (Projectile.timeLeft == 299)
                Projectile.ai[0] = Projectile.velocity.ToRotation();

            Projectile.rotation += MathHelper.ToRadians(.8f * (float)Math.Cos(MathHelper.ToRadians(1200 - Projectile.timeLeft)));
            drawColor = new Color(0, 230, 230) * (Projectile.timeLeft > 60 ? 1 : (float)(Projectile.timeLeft / 60f));
        }
        public Color drawColor;
        public Vector2 OldPoint(int i)
        {
            return Projectile.Center - Main.screenPosition + (Projectile.oldRot[i] * (float)(90f - i) / 90f + Projectile.ai[0]).ToRotationVector2() * (.12f * i * (float)(1f - (float)Math.Pow(.975f, i)) / .025f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.timeLeft < 299)
            {
                List<VertexInfo2> vertices = new();
                for (int i = 1; i <= 99; i += 1)
                {
                    if (Projectile.oldRot[i] != 0)
                    {
                        vertices.Add(new VertexInfo2(OldPoint(i) + new Vector2(Math.Min((100 - i) * 3, 30) * (float)((100f - i) / 100f), 0).RotatedBy((OldPoint(i) - OldPoint(i - 1)).ToRotation() - MathHelper.ToRadians(-90)), new Vector3(i / 100f, 0, 1 - (i / 100f)), drawColor));
                        vertices.Add(new VertexInfo2(OldPoint(i) + new Vector2(Math.Min((100 - i) * 3, 30) * (float)((100f - i) / 100f), 0).RotatedBy((OldPoint(i) - OldPoint(i - 1)).ToRotation() - MathHelper.ToRadians(90)), new Vector3(i / 100f, 1, 1 - (i / 100f)), drawColor));
                    }
                }
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("MythosOfMoonlight/Textures/Extra/Ex1").Value;
                if (vertices.Count >= 3)
                {
                    Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);
                }
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (Projectile.timeLeft > 60 && Projectile.timeLeft < 210)
            {
                for (int i = 0; i <= 98; i += 1)
                {
                    if (OldPoint(i) != Vector2.Zero && OldPoint(i + 1) != Vector2.Zero)
                    {
                        float point = 0f;
                        if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), OldPoint(i) + Main.screenPosition, OldPoint(i + 1) + Main.screenPosition, (int)(Math.Min((100 - i) * 5, 30) * (float)((100f - i) / 100f)), ref point))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
