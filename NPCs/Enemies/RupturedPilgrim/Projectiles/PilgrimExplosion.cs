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
namespace MythosOfMoonlight.NPCs.Enemies.RupturedPilgrim.Projectiles
{
    public class PilgrimExplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starine Storm");
            Main.projFrames[projectile.type] = 8;
		}
		public override void SetDefaults()
		{
			projectile.width = 90;
			projectile.height = 132;
			projectile.aiStyle = -1;
			projectile.friendly = false;
			projectile.tileCollide = true;
			projectile.hostile = true;
		}
        public override void AI()
        {
            projectile.velocity = Vector2.Zero;
            if (++projectile.frameCounter >= 5) {
                projectile.frameCounter = 0;
                projectile.frame++;
            }
            if (projectile.frame == 7) {
                projectile.Kill();
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}