using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MythosOfMoonlight.Projectiles.ThornDart.Orbe
{
	public class Orbe : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orbe");
			Main.projFrames[projectile.type] = 1;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
		}
		public override void SetDefaults()
		{
			projectile.width = projectile.height = 22;
			projectile.timeLeft = 120;
			projectile.maxPenetrate = 999;
			projectile.tileCollide = false;
			projectile.damage = 3;
			projectile.knockBack = 0;
			projectile.aiStyle = -1;
		}

		Vector2 distance;
		public override void AI()
		{
			var target = Main.player[projectile.owner];
			distance = projectile.position - (target.position + target.oldVelocity);
			var unsqDist = distance.Length();
			var speed = MathHelper.Max(3, distance.Length() / 20);
			projectile.velocity = Vector2.Lerp(projectile.velocity, -distance.SafeNormalize(Vector2.UnitX)*speed, 0.1f);
			if (distance.LengthSquared() < 256) 
			{
				var heal = (int)projectile.ai[0];
				target.statLife += heal;
				target.HealEffect(heal);
				projectile.timeLeft = 0;
			}
        }

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			Texture2D texture = ModContent.GetTexture("MythosOfMoonlight/Projectiles/ThornDart/Orbe/Orbe_Glowy");
			Rectangle rect = new Rectangle(0, 0, texture.Width, texture.Height);
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, rect, new Color(5, 240, 0), projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			return true;
		}


		public override Color? GetAlpha(Color lightColor) => new Color(255, 255, 255, 0);

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {

			target.statLife += 3;
			Kill(0);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			
        }
    }
}