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
using Terraria.Audio;

namespace MythosOfMoonlight.Projectiles.IridicProjectiles
{
    public class FragmentBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fragment Bullet");
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.timeLeft = 600;
            Projectile.netUpdate = true;
            Projectile.netUpdate2 = true;
            Projectile.netImportant = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
<<<<<<< Updated upstream
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(.4f, 0, .6f));
=======
            Projectile.alpha = 255;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(.4f, .4f, .4f));
            if (Projectile.alpha > 0) Projectile.alpha -= 15;
>>>>>>> Stashed changes
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
        public override Color? GetAlpha(Color lightColor)
        {
<<<<<<< Updated upstream
            return Color.Lerp(lightColor, Color.White, .4f);
=======
            float p = (255 - (float)Projectile.alpha) / 255f;
            return Color.Lerp(lightColor, Color.White, .5f * p);
>>>>>>> Stashed changes
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            for (int i = 1; i <= 3; i++)
            {
                Vector2 vel = -Utils.SafeNormalize(Projectile.oldVelocity, Vector2.Zero).RotatedBy(Main.rand.NextFloat(-.66f, .66f)) * Main.rand.NextFloat(1f, 2f);
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<PurpurineDust>(), vel.X, vel.Y, 0, default, Main.rand.NextFloat(.6f, 1.2f));
                dust.noGravity = true;
                dust.velocity = vel;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 1; i <= 3; i++)
            {
                Vector2 vel = -Utils.SafeNormalize(Projectile.oldVelocity, Vector2.Zero).RotatedBy(Main.rand.NextFloat(-.66f, .66f)) * Main.rand.NextFloat(1f, 2f);
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<PurpurineDust>(), vel.X, vel.Y, 0, default, Main.rand.NextFloat(.6f, 1.2f));
                dust.noGravity = true;
                dust.velocity = vel;
            }
            return base.OnTileCollide(oldVelocity);
        }
    }
}
