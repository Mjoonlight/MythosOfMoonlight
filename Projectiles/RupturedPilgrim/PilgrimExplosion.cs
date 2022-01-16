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
namespace MythosOfMoonlight
{
    public class PilgrimExplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Explosion");
            Main.projFrames[projectile.type] = 8;
		}
		public override void SetDefaults()
		{
			projectile.width = 26;
			projectile.height = 60;
			projectile.aiStyle = 0;
			projectile.friendly = false;
			projectile.tileCollide = true;
			projectile.hostile = true;
		}
        public override void AI()
        {
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