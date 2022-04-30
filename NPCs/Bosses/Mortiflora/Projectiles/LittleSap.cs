using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace MythosOfMoonlight.NPCs.Bosses.Mortiflora.Projectiles
{
    public class LittleSap : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sap");
            Main.projFrames[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
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
                    if (player.active && Vector2.Distance(Projectile.Center, player.Center) < 22)
                        player.AddBuff(BuffID.Slow, 60);
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.frame != 1)
            {
                Projectile.rotation = 0;
                Projectile.position += Projectile.velocity + new Vector2(0, 4);
                Projectile.velocity = new Vector2();
                Projectile.aiStyle = -1;
                Projectile.timeLeft = 900;
                Projectile.frame = 1;
                Projectile.damage = 0;
                Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            }
            return false;
        }
    }
}