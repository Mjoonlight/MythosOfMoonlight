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

namespace MythosOfMoonlight.Projectiles
{

    public class StarBitBlue : ModProjectile
    {
        public override void SetStaticDefaults()
        {
       
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide =false ;
            Projectile.ignoreWater = false;
            Projectile.timeLeft = 400;
            Projectile.netUpdate = true;
            Projectile.netUpdate2 = true;
            Projectile.netImportant = true;
        }


        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
       

        public override bool PreDraw(ref Color lightColor)
        {

            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = Request<Texture2D>("MythosOfMoonlight/Textures/Extra/FireGlow").Value;
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                var offset = new Vector2(Projectile.width / 2f, Projectile.height / 2f);
                var frame = texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + offset;
                float sizec = Projectile.scale * (Projectile.oldPos.Length - k) / (Projectile.oldPos.Length * 1.4f); //decrease the scale to make the glowy bigger
                Color color = new Color(24, 146, 235) * (1f - Projectile.alpha) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, frame, color, Projectile.oldRot[k], frame.Size() / 2f, sizec, SpriteEffects.None, 0);
            }
            SpriteBatch spriteBatch = Main.spriteBatch;
            Vector2 ori = Projectile.Size / 2;
            Texture2D trail = Request<Texture2D>("MythosOfMoonlight/Textures/Extra/White").Value;
            List<VertexInfo2> vertices = new();
            for (int j = 0; j <= Math.Min(300 - Projectile.timeLeft, 19); j += 1)
            {
                if (Projectile.oldPos[j] != Vector2.Zero)
                {
                    vertices.Add(new VertexInfo2(
                        Projectile.oldPos[j] + ori - Main.screenPosition + (Projectile.oldRot[j] + MathHelper.PiOver2).ToRotationVector2() * 6f,
                        new Vector3(j / (float)(Math.Min(300 - Projectile.timeLeft, 20) + 1), 0, 1 - (j / (float)(Math.Min(300 - Projectile.timeLeft, 19) + 1))),
                        Color.MediumBlue    * (1 - (j / (float)(Math.Min(300 - Projectile.timeLeft, 19) + 1)))));
                    vertices.Add(new VertexInfo2(
                        Projectile.oldPos[j] + ori - Main.screenPosition + (Projectile.oldRot[j] - MathHelper.PiOver2).ToRotationVector2() * 6f,
                        new Vector3(j / (float)(Math.Min(20 - Projectile.timeLeft, 20) + 1), .25f, 1 - (j / (float)(Math.Min(300 - Projectile.timeLeft, 19) + 1))),
                        Color.LightBlue * (1 - (j / (float)(Math.Min(300 - Projectile.timeLeft, 19) + 1)))));
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
            if (Projectile.timeLeft < (Projectile.DamageType == DamageClass.Summon ? 300 : 300)) Projectile.GetGlobalProjectile<MoMGlobalProj>().HomingActions(Projectile, .125f, 20f, 300f);
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

