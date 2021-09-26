using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MythosOfMoonlight.Projectiles.ThornDart.Orbe;

namespace MythosOfMoonlight.Projectiles.ThornDart
{
	public class ThornDart : ModProjectile
	{
        public override void SetStaticDefaults() {
			DisplayName.SetDefault("ThornDart");
        }
		public override void SetDefaults() {
			projectile.CloneDefaults(ProjectileID.Seed);
			aiType = ProjectileID.CursedDart;
			projectile.aiStyle = ProjectileID.CursedDart;
		}

		public override void PostAI()
        {
			projectile.rotation = projectile.velocity.ToRotation();
			Dust dust;
			var off = new Vector2(12, 4);
			if (Main.rand.NextFloat() < .65f)
			{
				dust = Dust.NewDustPerfect(projectile.position + off, 0, new Vector2(), 0, new Color(177, 255, 0), 1f);
				dust.noGravity = true;
			}

		}

		public override void Kill(int timeLeft) {
			Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.AddBuff(BuffID.Poisoned, 5);
			for (int i = 0; i < Main.rand.Next(3, 4); i++)
				Projectile.NewProjectile(projectile.position, new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedByRandom(30), ModContent.ProjectileType<Orbe.Orbe>(), 0, 0, projectile.owner, projectile.ai[0]);
			Kill(0);
        }
	}   
}