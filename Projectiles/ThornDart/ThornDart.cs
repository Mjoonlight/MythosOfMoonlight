using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace MythosOfMoonlight.Projectiles.ThornDart
{
    public class ThornDart : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("ThornDart");
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Seed);
            AIType = ProjectileID.CursedDart;
            Projectile.aiStyle = ProjectileID.CursedDart;
        }

        public override void PostAI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Dust dust;
            var off = new Vector2(12, 4);
            if (Main.rand.NextFloat() < .65f)
            {
                dust = Dust.NewDustPerfect(Projectile.position + off, 0, new Vector2(), 0, new Color(177, 255, 0), 1f);
                dust.noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 10);
            for (int i = 0; i < Main.rand.Next(3, 4); i++)
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.position, new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedByRandom(30), ModContent.ProjectileType<Orbe.Orbe>(), 0, 0, Projectile.owner, Projectile.ai[0]);
            Projectile.Kill();
        }
    }
}