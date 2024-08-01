using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria;
using Terraria.ModLoader;
using MythosOfMoonlight.Dusts;

namespace MythosOfMoonlight.NPCs.Minibosses.StarveiledProj
{
    public class ScholarPortal : ModProjectile
    {
        public override string Texture => Helper.Empty;
        public override void SetDefaults()
        {
            Projectile.height = 100;
            Projectile.width = 100;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 450;
        }
        public override bool ShouldUpdatePosition() => false;
        /*public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Helper.GetTex("MythosOfMoonlight/Assets/Textures/Extra/star_01");
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.Black * Projectile.ai[0], Projectile.rotation, tex.Size() / 2, new Vector2(Projectile.ai[0] , Projectile.ai[0] * 0.6f), SpriteEffects.None, 0);
            return false;
        }*/
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            float prog = Utils.GetLerpValue(0, 250, Projectile.timeLeft);
            if (Projectile.timeLeft > 300)
            {
                Vector2 pos = Projectile.Center + (200 - Main.rand.NextFloat(50)) * Main.rand.NextVector2Unit();
                Dust a = Dust.NewDustPerfect(pos, ModContent.DustType<Starry2>(), Helper.FromAToB(pos, Projectile.Center) * Main.rand.NextFloat(3, 6), newColor: Color.White, Scale: Main.rand.NextFloat(0.15f, 0.35f));
                a.noGravity = false;
            }
            Projectile.ai[0] = MathHelper.Clamp((float)Math.Sin(prog * MathHelper.Pi), 0, 1) + ((float)Math.Sin(Main.GlobalTimeWrappedHourly) * 0.1f);
        }
    }
}
