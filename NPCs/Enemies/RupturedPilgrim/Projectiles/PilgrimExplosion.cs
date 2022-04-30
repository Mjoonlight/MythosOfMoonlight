using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace MythosOfMoonlight.NPCs.Enemies.RupturedPilgrim.Projectiles
{
    public class PilgrimExplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starine Storm");
            Main.projFrames[Projectile.type] = 8;
        }
        public override void SetDefaults()
        {
            Projectile.width = 90;
            Projectile.height = 132;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.tileCollide = true;
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
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

                var scale = (Projectile.frame * 5 + Projectile.frameCounter) / 35f;
                var color = Color.Lerp(lightColor, new Color(lightColor.R, lightColor.G, lightColor.B, 0), scale);
                Main.EntitySpriteDraw(drawTextureGlow, Projectile.Center - Main.screenPosition, sourceRectangle, color, 0f, new Vector2(drawTextureGlow.Width, drawTextureGlow.Height) / 2, scale, SpriteEffects.None, 0);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            }
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 1; i < (Main.expertMode ? 9 : 6); i++)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(0, -6).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-45, 45))), ModContent.ProjectileType<StarineShaft>(), 10, 0);
            }
            SoundEngine.PlaySound(SoundID.Item9, Projectile.Center);
        }
    }
}