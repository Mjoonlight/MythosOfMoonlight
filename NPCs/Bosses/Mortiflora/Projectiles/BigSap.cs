using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MythosOfMoonlight.NPCs.Bosses.Mortiflora.Projectiles
{
    public class BigSap : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sap");
            Main.projFrames[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = 1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.timeLeft = 9999;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            if (Projectile.frame == 1)
            {
                for (int playerCount = 0; playerCount < 255; playerCount++)
                {
                    Player player = Main.player[playerCount];
                    if (player.active && Vector2.Distance(Projectile.Center, player.Center) < 26)
                        player.AddBuff(BuffID.Slow, 240);
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (!(Projectile.frame == 1))
            {
                Projectile.rotation = 0;
                Projectile.position += Projectile.velocity + new Vector2(0, 6);
                Projectile.velocity = new Vector2();
                Projectile.aiStyle = -1;
                Projectile.timeLeft = 360;
                Projectile.frame = 1;
                Projectile.damage = 0;
                Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            }
            return false;
        }
        public override void Kill(int timeLeft)
        {
            int damage = 4;
            if (Main.expertMode)
                damage = 5;
            for (int i = 0; i < 4; i++)
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-10, -6)), ModContent.ProjectileType<LittleSap>(), damage, 0f, Main.myPlayer);
        }
    }
}