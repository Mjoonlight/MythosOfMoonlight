using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Projectiles.SoulCandle
{
    public class SoulFlames : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;

        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;

            Projectile.timeLeft = 200;
            Projectile.scale = 1f;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return !target.friendly;
        }
        public override bool ShouldUpdatePosition()
        {
            return Projectile.ai[2] > 0.5f;
        }
        public override void AI()
        {
            if (Projectile.ai[2] > Main.rand.NextFloat(0.25f))
                if (++Projectile.frameCounter % 5 == 0)
                {
                    if (++Projectile.frame >= Main.projFrames[Projectile.type])
                        Projectile.frame = 0;
                }
            Projectile.velocity *= 0.95f;
            if (Projectile.velocity.Length() < 0.01f)
            {
                Projectile.ai[2] = MathHelper.Max(Projectile.ai[2] - 0.1f, 0);
                if (Projectile.ai[2] <= 0)
                    Projectile.Kill();
            }
            else
                Projectile.ai[2] = MathHelper.Min(Projectile.ai[2] + 0.1f, 1);
        }
        public override Color? GetAlpha(Color lightColor) => Color.White * Projectile.ai[2];
    }
}
