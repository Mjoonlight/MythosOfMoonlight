using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using System.Collections.Generic;
using MythosOfMoonlight.Common.Globals;
using MythosOfMoonlight.Dusts;
using Terraria.Audio;
using Terraria.DataStructures;
using MythosOfMoonlight.Common.Utilities;

namespace MythosOfMoonlight.Projectiles
{

    public class StarBitBlue : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 6;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;
            Projectile.timeLeft = 400;
            Projectile.netUpdate = true;
            Projectile.penetrate = -1;
            Projectile.netUpdate2 = true;
            Projectile.netImportant = true;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.frame = Main.rand.Next(6);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }


        public override bool PreDraw(ref Color lightColor)
        {
            Color baseCol, secondaryCol;
            switch (Projectile.frame)
            {
                case 0:
                    baseCol = Color.Cyan;
                    secondaryCol = Color.LightBlue;
                    break;
                case 1:
                    baseCol = Color.OrangeRed;
                    secondaryCol = Color.IndianRed;
                    break;
                case 2:
                    baseCol = Color.Violet;
                    secondaryCol = Color.Purple;
                    break;
                case 3:
                    baseCol = Color.LightGreen;
                    secondaryCol = Color.Green;
                    break;
                case 4:
                    baseCol = Color.LightGoldenrodYellow;
                    secondaryCol = Color.LightYellow;
                    break;
                case 5:
                    baseCol = Color.White;
                    secondaryCol = Color.Gray;
                    break;
                default:
                    baseCol = Color.MediumBlue;
                    secondaryCol = Color.LightBlue;
                    break;
            }
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = Request<Texture2D>("MythosOfMoonlight/Assets/Textures/Extra/FireGlow").Value;
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                var offset = new Vector2(Projectile.width / 2f, Projectile.height / 2f);
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + offset;
                float sizec = Projectile.scale * (Projectile.oldPos.Length - k) / (Projectile.oldPos.Length * 1.4f); //decrease the scale to make the glowy bigger
                Color color = baseCol * (1f - Projectile.alpha) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                for (int i = 0; i < 10; i++)
                {
                    Vector2 realPos = Vector2.Lerp(drawPos, (Projectile.oldPos[(int)MathHelper.Max(0, k - 1)] - Main.screenPosition) + offset, (float)i / 10);
                    Main.EntitySpriteDraw(texture, realPos, null, color * 0.2f, Projectile.oldRot[k], texture.Size() / 2, sizec * 0.7f, SpriteEffects.None, 0);
                }
            }
            SpriteBatch spriteBatch = Main.spriteBatch;
            Vector2 ori = Projectile.Size / 2;
            Texture2D trail = Request<Texture2D>("MythosOfMoonlight/Assets/Textures/Extra/White").Value;
            List<VertexInfo2> vertices = new();
            for (int j = 0; j <= Math.Min(300 - Projectile.timeLeft, 19); j += 1)
            {
                if (Projectile.oldPos[j] != Vector2.Zero)
                {
                    vertices.Add(new VertexInfo2(
                        Projectile.oldPos[j] + ori - Main.screenPosition + (Projectile.oldRot[j] + MathHelper.PiOver2).ToRotationVector2() * 6f,
                        new Vector3(j / (float)(Math.Min(300 - Projectile.timeLeft, 20) + 1), 0, 1 - (j / (float)(Math.Min(300 - Projectile.timeLeft, 19) + 1))),
                        baseCol * (1 - (j / (float)(Math.Min(300 - Projectile.timeLeft, 19) + 1)))));
                    vertices.Add(new VertexInfo2(
                        Projectile.oldPos[j] + ori - Main.screenPosition + (Projectile.oldRot[j] - MathHelper.PiOver2).ToRotationVector2() * 6f,
                        new Vector3(j / (float)(Math.Min(20 - Projectile.timeLeft, 20) + 1), .25f, 1 - (j / (float)(Math.Min(300 - Projectile.timeLeft, 19) + 1))),
                        secondaryCol * (1 - (j / (float)(Math.Min(300 - Projectile.timeLeft, 19) + 1)))));
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
            return true;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.extraUpdates = 2;
            // just some homing code 
        }

        public override void Kill(int timeLeft)
        {

            SoundEngine.PlaySound(SoundID.MaxMana, Projectile.Center);
            for (int i = 0; i < 60; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(0.5f, 0.5f);
                var d = Dust.NewDustPerfect(Projectile.Center, DustID.PurpleTorch, speed * 5, Scale: 1f);
                ;
                d.noGravity = true;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return base.OnTileCollide(oldVelocity);
        }
    }
}

