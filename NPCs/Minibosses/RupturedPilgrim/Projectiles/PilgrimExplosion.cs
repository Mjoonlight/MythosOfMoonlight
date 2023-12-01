using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim.Projectiles
{
    public class PilgrimExplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Starine Storm");
            Main.projFrames[Projectile.type] = 8;
            Projectile.AddElement(CrossModHelper.Celestial);
            Projectile.AddElement(CrossModHelper.Arcane);
        }
        public override void SetDefaults()
        {
            Projectile.width = 90;
            Projectile.height = 132;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Projectile.velocity = Vector2.Zero;
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
            }
            if (Projectile.frame == 7)
                Projectile.Kill();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.frame > 1)
            {
                Texture2D drawTextureGlow = ModContent.Request<Texture2D>(Projectile.ModProjectile.Texture + "_Extra").Value;
                Rectangle sourceRectangle = new(0, 0, drawTextureGlow.Width, drawTextureGlow.Height);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                var scale = (Projectile.frame * 10 + Projectile.frameCounter) / 35f;
                var color = Color.Lerp(lightColor, new Color(lightColor.R, lightColor.G, lightColor.B, 0), scale);
                Main.EntitySpriteDraw(drawTextureGlow, Projectile.Center - Main.screenPosition, sourceRectangle, color, 0f, new Vector2(drawTextureGlow.Width, drawTextureGlow.Height) / 2, scale, SpriteEffects.None, 0);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = -(Main.expertMode ? 4 : 2); i < (Main.expertMode ? 5 : 3); i++)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(0, -6).RotatedBy(MathHelper.ToRadians(i * Main.rand.NextFloat(14, 17) + Main.rand.NextFloat(-17, 17))), ModContent.ProjectileType<StarineShaft>(), 20, 0);
            }
            SoundEngine.PlaySound(SoundID.Item9, Projectile.Center);
        }
    }
}