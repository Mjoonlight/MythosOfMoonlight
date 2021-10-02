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

		public override void AI()
		{
			var target = Main.player[projectile.owner];
			var distance = projectile.position - (target.position + target.oldVelocity);
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
			//3hi31mg
			var clr = new Color(255, 255, 255, 255); // full white
			var drawPos = projectile.Center - Main.screenPosition;
			var off = new Vector2(projectile.width / 2f, projectile.height / 2f);
			var origTexture = Main.projectileTexture[projectile.type];
			var texture = mod.GetTexture("Projectiles/ThornDart/Orbe/Orbe");
			int frameHeight = texture.Height / Main.projFrames[projectile.type];
			var frame = new Rectangle(0, frameHeight * projectile.frame, texture.Width, frameHeight - 2);
			var orig = frame.Size() / 2f;

			for (int i = 0; i < 14; i++)
			{
				var rand = Main.rand.NextFloat(-.25f, .25f) * projectile.width;
				Main.spriteBatch.Draw(origTexture, projectile.Center - Main.screenPosition + Vector2.UnitX.RotatedByRandom(180.0) * rand, frame, clr * (i / 14f), 0f, orig, Main.rand.NextFloat(), SpriteEffects.None, 0f);
			}

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