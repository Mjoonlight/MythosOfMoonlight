using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MythosOfMoonlight.NPCs.Enemies.CenturyFlower.CenturyFlowerSpore
{
    public class CenturyFlowerSpore : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/NPCs/Enemies/CenturyFlower/CenturyFlowerSpore/CenturyFlowerSpore1";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Century Flower Spore");
            Main.projFrames[projectile.type] = 2;
        }

        const int MAX_TIMELEFT = 270;
        public override void SetDefaults()
        {
            projectile.height = 64;
            projectile.width = 80;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.damage = 0;
            projectile.tileCollide = false;
            projectile.timeLeft = MAX_TIMELEFT;
            projectile.frame = Main.rand.Next(0, 2);
            projectile.localNPCHitCooldown = 1;
            projectile.usesLocalNPCImmunity = true;
        }


        /*
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            var texture = ModContent.GetTexture("MythosOfMoonlight/NPCs/Enemies/CenturyFlower/CenturyFlowerSpore/CenturyFlowerSpore2");
            var rect = new Rectangle(0, projectile.height * projectile.frame, projectile.width, projectile.height);
            var alphadColor = new Color(lightColor.R, lightColor.G, lightColor.B, 255);
            Main.NewText(alphadColor);
            Main.spriteBatch.Draw(texture, projectile.position - Main.screenPosition, rect, alphadColor, -projectile.rotation, default, projectile.scale, SpriteEffects.None, 1f);
            return true;
        }
        */

        void CheckCollision()
        {
            for (int i = 0; i < Main.player.Length; i++)
            {
                var current = Main.player[i];
                if (current.Hitbox.Intersects(projectile.Hitbox))
                {
                    current.AddBuff(BuffID.Suffocation, 60);
                }
            }
        }
        public override void AI()
        {
            projectile.knockBack = 0;
            projectile.velocity = Vector2.Lerp(projectile.velocity, Vector2.Zero, 0.05f);
            projectile.rotation += .01f;

            var currentTime = (float)(MAX_TIMELEFT - projectile.timeLeft);
            projectile.alpha = (int)(currentTime / MAX_TIMELEFT * 255);
            projectile.scale = currentTime / MAX_TIMELEFT + .1f;
            CheckCollision();
        }
    }
}