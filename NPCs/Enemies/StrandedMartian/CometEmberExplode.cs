using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.Dusts;

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
            Projectile.width = 70;
            Projectile.height = 70;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 2;
            Projectile.netUpdate = true;
            Projectile.netUpdate2 = true;
            Projectile.netImportant = true;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            Projectile.Kill();
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 1; i <= 40; i++)
            {
                Vector2 shoot = Main.rand.NextFloat(0, 6.28f).ToRotationVector2() * Main.rand.NextFloat(0, 6);
                Dust dust = Dust.NewDustDirect(Projectile.Center, 1, 1, ModContent.DustType<PurpurineDust>());
                dust.scale = Main.rand.NextFloat(.4f, 1.6f);
                dust.noGravity = Main.rand.NextFloat(1) < 0.75f;
                dust.velocity = shoot;
            }
        }
    }
}
