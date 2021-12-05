using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace MythosOfMoonlight.NPCs.Bosses.Mortiflora.Projectiles
{
	public class MortifloraBones2 : ModProjectile
	{
		public override string Texture => "MythosOfMoonlight/NPCs/Bosses/Mortiflora/Projectiles/MortifloraBones";
        public override void SetStaticDefaults() {
			DisplayName.SetDefault("Dessicated Bones");
			Main.projFrames[projectile.type] = 4;
        }
		public override void SetDefaults() {
			projectile.width = 17;
			projectile.height = 17;
			projectile.aiStyle = 1;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.timeLeft = 9999;
			projectile.ignoreWater = true;
			projectile.penetrate = 1;
			projectile.frame = Main.rand.Next(4);
		}
		public override bool OnTileCollide(Vector2 oldVelocity) {
			Main.PlaySound(SoundID.NPCHit2);
			projectile.penetrate--;
			if (projectile.penetrate <= 0) {
				projectile.Kill();
			}
			else {
				Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
				Main.PlaySound(SoundID.Item10, projectile.position);
				if (projectile.velocity.X != oldVelocity.X) {
					projectile.velocity.X = -oldVelocity.X * 0.5f;
				}
				if (projectile.velocity.Y != oldVelocity.Y) {
					projectile.velocity.Y = -oldVelocity.Y * 0.5f;
				}
			}
			return false;
		}
		public override void Kill(int timeLeft) {
			Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
		}
	}   
}