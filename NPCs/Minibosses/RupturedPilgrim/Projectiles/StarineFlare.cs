using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MythosOfMoonlight.Common.Crossmod;
using Terraria;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim.Projectiles
{
    public class StarineFlare : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/Assets/Textures/Extra/blank";
        public override void SetStaticDefaults()
        {
            Projectile.AddElement(CrossModHelper.Celestial);
            Projectile.AddElement(CrossModHelper.Arcane);
        }
        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }
        public override bool? CanDamage()
        {
            return Projectile.ai[0] < 230 && Projectile.ai[1] > 0;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.Reload(BlendState.Additive);
            Texture2D tex = Helper.GetTex("MythosOfMoonlight/Assets/Textures/Extra/star_03");
            float flicker = 1;
            flicker += Main.rand.NextFloat(-0.5f, 0.5f);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.Cyan * (0.3f + Projectile.ai[1]) * flicker, 0, tex.Size() / 2, 0.35f * (Projectile.ai[1] / 2 + 0.5f), SpriteEffects.None, 0);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White * (0.3f + Projectile.ai[1]), 0, tex.Size() / 2, 0.35f * (Projectile.ai[1] / 2 + 0.5f), SpriteEffects.None, 0);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
        public override void AI()
        {
            Projectile.velocity *= 0.9f;
            Projectile.ai[0]--;
            if (Projectile.ai[0] < 230 && Projectile.ai[0] > 0)
            {
                if (Projectile.ai[1] < 1)
                    Projectile.ai[1] += 0.1f * (Projectile.ai[1] == 0 ? 1 : Projectile.ai[1]);
                if (Projectile.ai[1] >= 1)
                    Projectile.ai[1] -= 0.05f;
            }
            if (Projectile.ai[0] < -30)
                Projectile.ai[1] -= 0.1f;
            if (Projectile.ai[1] < -0.3f)
                Projectile.Kill();
        }
    }
}
