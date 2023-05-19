using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim.Projectiles
{
    public class StarineShockwave : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 6;
        }
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override bool ShouldUpdatePosition() => false;
        public override void Kill(int timeLeft)
        {
            if (Projectile.ai[0] < 5)
            {
                Projectile a = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.width * Projectile.velocity + (TRay.Cast(Projectile.Center - Vector2.UnitY * 100, Vector2.UnitY, 1000) - 30 * Vector2.UnitY), Projectile.velocity, Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.ai[0] + 1, Projectile.ai[1]);
                a.ai[0] = Projectile.ai[0] + 1;
                a.ai[1] = Projectile.ai[1];
            }
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AI()
        {
            Projectile.velocity.Normalize();
            if (++Projectile.frameCounter >= 3)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 6)
                {
                    Projectile.Kill();
                }
            }
        }
    }
}
