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
namespace MythosOfMoonlight
{
    public class PilgrimProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starine Sigil");
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D drawTexture = Main.projectileTexture[projectile.type];
            Texture2D drawTexture2 = ModContent.GetTexture("MythosOfMoonlight/Projectiles/RupturedPigrim/PilgrimProj_Glow");
            Texture2D drawTexture3 = ModContent.GetTexture("MythosOfMoonlight/Projectiles/RupturedPigrim/PilgrimProj_Extra");
            Texture2D drawTexture4 = ModContent.GetTexture("MythosOfMoonlight/Projectiles/RupturedPigrim/PilgrimProj_Extra2");
			Rectangle sourceRectangle = new Rectangle(0, 0, drawTexture.Width, drawTexture.Height);
			spriteBatch.Draw(drawTexture, projectile.Center - Main.screenPosition, sourceRectangle, Color.White, -Main.GameUpdateCount * 0.314f, drawTexture.Size() / 2, 1, SpriteEffects.None, 0);
			spriteBatch.Draw(drawTexture3, projectile.Center - Main.screenPosition, sourceRectangle, Color.White, 0, drawTexture.Size() / 2, 1, SpriteEffects.None, 0);
			spriteBatch.Draw(drawTexture4, projectile.Center - Main.screenPosition, sourceRectangle, Color.White, Main.GameUpdateCount * 0.314f, drawTexture.Size() / 2, 1, SpriteEffects.None, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			spriteBatch.Draw(drawTexture2, projectile.Center - Main.screenPosition, sourceRectangle, Color.White, -Main.GameUpdateCount * 0.314f, drawTexture.Size() / 2, 1, SpriteEffects.None, 0);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
        }
		public override void SetDefaults()
		{
			projectile.width = 5;
			projectile.height = 5;
			projectile.aiStyle = 0;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.hostile = true;
		}
        public override void AI()
        {
            if (projectile.ai[0] == 15) {
            Projectile.NewProjectile(projectile.Center, 15.5f * Utils.RotatedBy(projectile.DirectionTo(Main.player[Main.myPlayer].Center), 0), ModContent.ProjectileType<StarineArrow>(), 22, 0);
            }
            if (projectile.ai[0] == 30) {
            Projectile.NewProjectile(projectile.Center, 15.5f * Utils.RotatedBy(projectile.DirectionTo(Main.player[Main.myPlayer].Center), 0), ModContent.ProjectileType<StarineArrow>(), 22, 0);
            }
            if (++projectile.ai[0] == 45) {
            Projectile.NewProjectile(projectile.Center, 15.5f * Utils.RotatedBy(projectile.DirectionTo(Main.player[Main.myPlayer].Center), 0), ModContent.ProjectileType<StarineArrow>(), 22, 0);
            projectile.Kill();
            }
        }
    }
}