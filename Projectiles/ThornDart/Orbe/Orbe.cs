using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace MythosOfMoonlight.Projectiles.ThornDart.Orbe
{
	public class Orbe : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("ThornDart");
		}
		public override void SetDefaults()
		{
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