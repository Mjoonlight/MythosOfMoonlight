using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MythosOfMoonlight.NPCs.Bosses.Mortiflora.Projectiles
{
	public class BigSap : ModProjectile
	{
        public override void SetStaticDefaults() {
			DisplayName.SetDefault("Sap");
			Main.projFrames[projectile.type] = 2;
        }
		public override void SetDefaults() {
			projectile.width = 20;
			projectile.height = 20;
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
						if (Vector2.Distance(projectile.Center, Main.player[playerCount].Center) < 26) {
							Main.player[playerCount].AddBuff(BuffID.Slow, 240);
						}
					}
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity) {
			if (!(projectile.frame == 1)) {
				projectile.rotation = 0;
				projectile.position += projectile.velocity + new Vector2(0, 6);
				projectile.velocity = new Vector2();
				projectile.aiStyle = -1;
				projectile.timeLeft = 360;
				projectile.frame = 1;
				projectile.damage = 0;
				Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
			}
			return false;
		}
		public override void Kill(int timeLeft) {
			int damage = 4;
			if (Main.expertMode) damage = 5;
			for (int i = 0; i < 4; i++) 
				Projectile.NewProjectile(projectile.Center, new Vector2(Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-10, -6)), ModContent.ProjectileType<LittleSap>(), damage, 0f, Main.myPlayer);
		}
	}   
}