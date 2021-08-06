using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;

namespace MythosOfMoonlight.NPCs.Bosses.Mortiflora.Projectiles
{
	public class MortifloraRedWave : ModProjectile
	{
        public override void SetStaticDefaults() {
			DisplayName.SetDefault("Red Wave");
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }
		public override void SetDefaults() {
			aiType = ProjectileID.Bullet;
			projectile.width = 30;
			projectile.height = 30;
			projectile.aiStyle = 1;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.timeLeft = 720;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.extraUpdates = 2;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {
			var clr = new Color(255, 255, 255, 255); // full white
			var drawPos = projectile.Center - Main.screenPosition;
			var off = new Vector2(projectile.width / 2f, projectile.height / 2f);
			var texture = Main.projectileTexture[projectile.type];
			int frameHeight = texture.Height / Main.projFrames[projectile.type];
			var frame = new Rectangle(0, frameHeight * projectile.frame, texture.Width, frameHeight - 2);
			var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[projectile.type];
			var orig = frame.Size() / 2f;
			for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[projectile.type]; i++) {
				Main.spriteBatch.Draw(texture, projectile.oldPos[i] + off - Main.screenPosition, frame, clr * (1f - fadeMult * i), projectile.oldRot[i], orig, projectile.scale, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(texture, drawPos, frame, clr, projectile.rotation, orig, projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void AI() {
			projectile.velocity *= 1.01f;
		}
	}   
}