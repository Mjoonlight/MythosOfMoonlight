using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace MythosOfMoonlight.NPCs.Bosses.Mortiflora.Projectiles
{
	public class LittleSap : ModProjectile
	{
        public override void SetStaticDefaults() {
			DisplayName.SetDefault("Sap");
			Main.projFrames[projectile.type] = 2;
        }
		public override void SetDefaults() {
			projectile.width = 14;
			projectile.height = 14;
			projectile.aiStyle = 1;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.timeLeft = 9999;
			projectile.ignoreWater = true;
		}
		public override void AI() {
			if (projectile.frame == 1)
			for (int playerCount = 0; playerCount < 255; playerCount++) {
					if (Main.player[playerCount].active) {
						if (Vector2.Distance(projectile.Center, Main.player[playerCount].Center) < 22) {
							Main.player[playerCount].AddBuff(BuffID.Slow, 60);
						}
					}
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity) {
			if (!(projectile.frame == 1)) {
				projectile.rotation = 0;
				projectile.position += projectile.velocity + new Vector2(0, 4);
				projectile.velocity = new Vector2();
				projectile.aiStyle = -1;
				projectile.timeLeft = 900;
				projectile.frame = 1;
				projectile.damage = 0;
				Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
			}
			return false;
		}
	}   
}