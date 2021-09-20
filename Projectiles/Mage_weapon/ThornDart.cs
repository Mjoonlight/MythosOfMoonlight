using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace MythosOfMoonlight.Projectiles.Mage_weapon
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
		public override void Kill(int timeLeft) {
			Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
		}
	}   
}
