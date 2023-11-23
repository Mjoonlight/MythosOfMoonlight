using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;

namespace MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim.Projectiles
{
    public class RingTelegraph : ModProjectile
    {
        public override string Texture => Helper.Empty;
        public override void SetDefaults()
        {
            Projectile.height = 300;
            Projectile.width = 300;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
        }
        public override bool ShouldUpdatePosition() => false;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Helper.GetTex("MythosOfMoonlight/Textures/Extra/circle_02");
            Main.spriteBatch.Reload(BlendState.Additive);
            float scale = MathHelper.Lerp(1, 0, Projectile.ai[0]);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.Cyan * Projectile.ai[0], Projectile.rotation, tex.Size() / 2, scale, SpriteEffects.None, 0);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
        public override void AI()
        {
            Projectile.ai[0] += 0.05f;
            if (Projectile.ai[0] > 1)
                Projectile.Kill();
        }
    }
}
