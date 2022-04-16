using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.GameContent;
namespace MythosOfMoonlight.NPCs.Enemies.RupturedPilgrim.Projectiles
{
    public class PilgrimExplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starine Storm");
            Main.projFrames[projectile.type] = 8;
		}
		public override void SetDefaults()
		{
			projectile.width = 90;
			projectile.height = 132;
			projectile.aiStyle = -1;
			projectile.friendly = false;
			projectile.tileCollide = true;
			projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
		}
        public override void AI()
        {
            projectile.velocity = Vector2.Zero;
            if (++projectile.frameCounter >= 5) {
                projectile.frameCounter = 0;
                projectile.frame++;
            }
            if (projectile.frame == 7) {
                projectile.Kill();
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (projectile.frame > 1) 
            {
                Texture2D drawTextureGlow = ModContent.GetTexture("MythosOfMoonlight/NPCs/Enemies/RupturedPilgrim/Projectiles/PilgrimExplosion_Extra");
                Rectangle sourceRectangle = new Rectangle(0, 0, drawTextureGlow.Width, drawTextureGlow.Height);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

                var scale = (projectile.frame * 5 + projectile.frameCounter) / 35f;
                var color = Color.Lerp(lightColor, new Color(lightColor.R, lightColor.G, lightColor.B, 0), scale);
                Main.spriteBatch.Draw(drawTextureGlow, projectile.Center - Main.screenPosition, sourceRectangle, color, 0f, new Vector2(drawTextureGlow.Width, drawTextureGlow.Height) / 2, scale, SpriteEffects.None, 0f);
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
                Projectile.NewProjectile(projectile.Center, new Vector2(0,-6).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-45,45))), ModContent.ProjectileType<StarineShaft>(), 10, 0);
            }
            Main.PlaySound(SoundID.Item9, projectile.Center);
        }
    }
}