using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.Dusts;
using Microsoft.Xna.Framework.Graphics;

namespace MythosOfMoonlight.NPCs.Enemies.StrandedMartian
{
    public class CometEmberExplode : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Comet Ember Explode");
        }
        public override void SetDefaults()
        {
            projectile.width = 70;
            projectile.height = 70;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.timeLeft = 2;
            projectile.netUpdate = true;
            projectile.netUpdate2 = true;
            projectile.netImportant = true;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            projectile.Kill();
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 1; i <= 40; i++)
            {
                Vector2 shoot = Main.rand.NextFloat(0, 6.28f).ToRotationVector2() * Main.rand.NextFloat(0, 6);
                Dust dust = Dust.NewDustDirect(projectile.Center, 1, 1, ModContent.DustType<PurpurineDust>());
                dust.scale = Main.rand.NextFloat(.4f, 1.6f);
                dust.noGravity = Main.rand.NextFloat(1) < 0.75f;
                dust.velocity = shoot;
            }
        }
    }
}
