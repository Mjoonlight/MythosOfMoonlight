using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.Dusts;
using Microsoft.Xna.Framework.Graphics;

namespace MythosOfMoonlight.Projectiles
{
    public class TestTentacle/*Proj*/ : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/Projectiles/TestTentacleProj";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Test Tentacle");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 100;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 300;
            projectile.netUpdate = true;
            projectile.netUpdate2 = true;
            projectile.netImportant = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 15;
            projectile.extraUpdates = 2;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void AI()
        {
            Lighting.AddLight(projectile.Center, 1, 1, 1);
            if (projectile.timeLeft == 299)
            {
                projectile.ai[0] = projectile.velocity.ToRotation();
            }
            projectile.rotation += MathHelper.ToRadians(.8f * (float)Math.Cos(MathHelper.ToRadians(1200 - projectile.timeLeft)));
            drawColor = new Color(0, 230, 230) * (projectile.timeLeft > 60 ? 1 : (float)((float)projectile.timeLeft / 60f));
        }
        public Color drawColor;
        public Vector2 OldPoint(int i)
        {
            return projectile.Center - Main.screenPosition + (projectile.oldRot[i] * (float)(90f - (float)i) / 90f + projectile.ai[0]).ToRotationVector2() * (.12f * (float)i * (float)(1f - (float)Math.Pow(.975f, i)) / .025f);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (projectile.timeLeft < 299)
            {
                List<VertexInfo2> vertices = new List<VertexInfo2>();
                for (int i = 1; i <= 99; i += 1)
                {
                    if (projectile.oldRot[i] != 0)
                    {
                        vertices.Add(new VertexInfo2(OldPoint(i) + new Vector2((float)Math.Min((100 - i) * 3, 30) * (float)((100f - i) / 100f), 0).RotatedBy((OldPoint(i) - OldPoint(i - 1)).ToRotation() - MathHelper.ToRadians(-90)), new Vector3(i / 100f, 0, 1 - (i / 100f)), drawColor));
                        vertices.Add(new VertexInfo2(OldPoint(i) + new Vector2((float)Math.Min((100 - i) * 3, 30) * (float)((100f - i) / 100f), 0).RotatedBy((OldPoint(i) - OldPoint(i - 1)).ToRotation() - MathHelper.ToRadians(90)), new Vector3(i / 100f, 1, 1 - (i / 100f)), drawColor));
                    }
                }
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.GetTexture("MythosOfMoonlight/Textures/Extra/Ex1");
                if (vertices.Count >= 3)
                {
                    Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);
                }
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (projectile.timeLeft > 60 && projectile.timeLeft < 210)
            {
                for (int i = 0; i <= 98; i += 1)
                {
                    if (OldPoint(i) != Vector2.Zero && OldPoint(i + 1) != Vector2.Zero)
                    {
                        float point = 0f;
                        if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), OldPoint(i) + Main.screenPosition, OldPoint(i + 1) + Main.screenPosition, (int)((float)Math.Min((100 - i) * 5, 30) * (float)((100f - i) / 100f)), ref point))
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
