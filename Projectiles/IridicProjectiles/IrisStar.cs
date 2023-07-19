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
using Terraria.Audio;
using MythosOfMoonlight.Common.Globals;

namespace MythosOfMoonlight.Projectiles.IridicProjectiles
{
    public class IrisStar : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Iris Ember");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.timeLeft = 300;
            Projectile.netUpdate = true;
            Projectile.netUpdate2 = true;
            Projectile.netImportant = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Vector2 ori = Projectile.Size / 2;
            Texture2D trail = ModContent.Request<Texture2D>("MythosOfMoonlight/Textures/Extra/White").Value;
            List<VertexInfo2> vertices = new();
            for (int j = 0; j <= Math.Min(300 - Projectile.timeLeft, 19); j += 1)
            {
                if (Projectile.oldPos[j] != Vector2.Zero)
                {
                    vertices.Add(new VertexInfo2(
                        Projectile.oldPos[j] + ori - Main.screenPosition + (Projectile.oldRot[j] + MathHelper.PiOver2).ToRotationVector2() * 6f,
                        new Vector3(j / (float)(Math.Min(300 - Projectile.timeLeft, 19) + 1), 0, 1 - (j / (float)(Math.Min(300 - Projectile.timeLeft, 19) + 1))),
                        Color.Purple * (1 - (j / (float)(Math.Min(300 - Projectile.timeLeft, 19) + 1)))));
                    vertices.Add(new VertexInfo2(
                        Projectile.oldPos[j] + ori - Main.screenPosition + (Projectile.oldRot[j] - MathHelper.PiOver2).ToRotationVector2() * 6f,
                        new Vector3(j / (float)(Math.Min(20 - Projectile.timeLeft, 19) + 1), .25f, 1 - (j / (float)(Math.Min(300 - Projectile.timeLeft, 19) + 1))),
                        Color.Purple * (1 - (j / (float)(Math.Min(300 - Projectile.timeLeft, 19) + 1)))));
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
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Lerp(lightColor, Color.White, .66f);
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.timeLeft < (Projectile.DamageType == DamageClass.Summon ? 300 : 300)) Projectile.GetGlobalProjectile<MoMGlobalProj>().HomingActions(Projectile, .125f, 20f, 300f);
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            SoundEngine.PlaySound(SoundID.Item25, Projectile.Center);
            for (int i = 1; i <= 7; i++)
            {
                Vector2 vel = -Utils.SafeNormalize(Projectile.oldVelocity, Vector2.Zero).RotatedBy(Main.rand.NextFloat(-1f, 1f)) * Main.rand.NextFloat(1f, 2f);
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<PurpurineDust>(), vel.X, vel.Y, 0, default, Main.rand.NextFloat(.6f, 1.8f));
                dust.noGravity = true;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Item25, Projectile.Center);
            for (int i = 1; i <= 7; i++)
            {
                Vector2 vel = -Utils.SafeNormalize(Projectile.oldVelocity, Vector2.Zero).RotatedBy(Main.rand.NextFloat(-1f, 1f)) * Main.rand.NextFloat(1f, 2f);
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<PurpurineDust>(), vel.X, vel.Y, 0, default, Main.rand.NextFloat(.6f, 1.8f));
                dust.noGravity = true;
            }
            return base.OnTileCollide(oldVelocity);
        }
    }
}
