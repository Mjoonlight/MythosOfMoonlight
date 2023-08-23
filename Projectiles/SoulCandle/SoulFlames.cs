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

            Projectile.timeLeft = 600;
            Projectile.scale = 1f;
            Projectile.penetrate = 1;
            Projectile.aiStyle = 9;
        }

     
        public override void Kill(int timeLeft)
        {


           

            Player player = Main.player[Projectile.owner];
            for (int i = 0; i < 3; i++)
            {
             

            }

           
        }
       
        public override bool? CanHitNPC(NPC target)
        {
            return !target.friendly;
        }
        public override void AI()
        {
            if (++Projectile.frameCounter >= 12f)
            {
                Projectile.frameCounter = 0;

                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
            Projectile.velocity *= 0.95f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = ModContent.Request<Texture2D>("MythosOfMoonlight/Textures/Extra/FireGlow").Value;
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                var offset = new Vector2(Projectile.width / 2f, Projectile.height / 2f);
                var frame = texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
                float sizec = Projectile.scale * (Projectile.oldPos.Length - k) / (Projectile.oldPos.Length * 1f);
                Color ProjColor = new Color(244, 204, 39) * (1f - Projectile.alpha) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + offset;

                Main.EntitySpriteDraw(texture, drawPos, frame, ProjColor, Projectile.oldRot[k], frame.Size() / 2f, sizec, SpriteEffects.None, 0);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return true;
        }

       
    }
}
