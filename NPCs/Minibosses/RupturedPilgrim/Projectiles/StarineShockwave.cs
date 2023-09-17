using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim.Projectiles
{
    public class StarineShockwave : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 6;
            Projectile.AddElement(CrossModHelper.Celestial);
            Projectile.AddElement(CrossModHelper.Arcane);
        }
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox.Width = 45;
            hitbox.Height = 30;
        }
        public override bool? CanDamage()
        {
            return Projectile.frame == 1 || Projectile.frame == 2;
        }
        public override bool ShouldUpdatePosition() => false;
        public override void Kill(int timeLeft)
        {
            if (Projectile.ai[0] < 6)
            {
                SoundStyle style = SoundID.DD2_BetsyFireballImpact;
                style.Volume = 0.5f;
                SoundEngine.PlaySound(style, Projectile.Center);
                Projectile a = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.width * Projectile.velocity + (TRay.Cast(Projectile.Center - Vector2.UnitY * 50, Vector2.UnitY, 500, true) - 30 * Vector2.UnitY), Projectile.velocity, Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.ai[0] + 1, Projectile.ai[1]);
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
