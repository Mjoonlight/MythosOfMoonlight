using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Projectiles
{
    public class LimestoneDebris : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 10;
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
            Projectile.Size = new Vector2(30, 32);
            Projectile.aiStyle = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 500;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.frame = Main.rand.Next(1, 9);
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            if (Projectile.frame > 2)
            {
                hitbox.OffsetSize(-15, -15);
                hitbox.Offset(7, 7);
            }
        }
        public override void OnKill(int timeLeft)
        {
            for (int num613 = 0; num613 < (Projectile.frame > 2 ? 5 : 10); num613++)
            {
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Marble, Projectile.velocity.X * Main.rand.NextFloat(), Projectile.velocity.X * -Main.rand.NextFloat());
                if (num613 > 5)
                    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.DesertPot, Projectile.velocity.X * Main.rand.NextFloat(), Projectile.velocity.X * -Main.rand.NextFloat());
            }
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.ai[0]++;
            if (Projectile.ai[0] == 1)
            {
                if (player.whoAmI == Main.myPlayer)
                {
                    Projectile.velocity = new Vector2(Main.rand.NextFloat(2, 14f) * player.direction, Main.rand.NextFloat(-20, -5));
                }
            }
            if (Projectile.ai[0] > 1 && Projectile.ai[0] < 20)
            {
                Projectile.velocity.Y = MathHelper.Lerp(Projectile.velocity.Y, 15, 0.025f);
                if (Projectile.frame < 8)
                    Projectile.rotation += MathHelper.ToRadians(Projectile.ai[0] * 0.25f);
                else
                    Projectile.rotation = Projectile.velocity.ToRotation();
            }
            if (Projectile.ai[0] > 20)
            {
                Projectile.tileCollide = true;
                Projectile.velocity.Y = MathHelper.Lerp(Projectile.velocity.Y, 15, 0.1f);
                if (Projectile.frame < 8)
                    Projectile.rotation += MathHelper.ToRadians(5);
                else
                    Projectile.rotation = Projectile.velocity.ToRotation();
            }
        }
    }
}
