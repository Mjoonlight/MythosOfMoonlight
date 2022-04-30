using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MythosOfMoonlight.NPCs.Enemies.RupturedPilgrim.Projectiles
{
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
