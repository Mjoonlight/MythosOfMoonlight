using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace MythosOfMoonlight.NPCs.Enemies.CenturyFlower.CenturyFlowerSpore
{
    public class CenturyFlowerSpore : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/NPCs/Enemies/CenturyFlower/CenturyFlowerSpore/CenturyFlowerSpore1";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Century Flower Spore");
            Main.projFrames[Projectile.type] = 2;
        }

        const int MAX_TIMELEFT = 270;
        public override void SetDefaults()
        {
            Projectile.height = 64;
            Projectile.width = 80;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.damage = 0;
            Projectile.tileCollide = false;
            Projectile.timeLeft = MAX_TIMELEFT;
            Projectile.frame = Main.rand.Next(0, 2);
            Projectile.localNPCHitCooldown = 1;
            Projectile.usesLocalNPCImmunity = true;
        }


        /*
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            var texture = ModContent.Request<Texture2D>("MythosOfMoonlight/NPCs/Enemies/CenturyFlower/CenturyFlowerSpore/CenturyFlowerSpore2");
            var rect = new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height);
            var alphadColor = new Color(lightColor.R, lightColor.G, lightColor.B, 255);
            Main.NewText(alphadColor);
            Main.spriteBatch.Draw(texture, Projectile.position - Main.screenPosition, rect, alphadColor, -Projectile.rotation, default, Projectile.scale, SpriteEffects.None, 1f);
            return true;
        }
        */

        void CheckCollision()
        {
            for (int i = 0; i < Main.player.Length; i++)
            {
                var current = Main.player[i];
                if (current.Hitbox.Intersects(Projectile.Hitbox))
                {
                    current.AddBuff(BuffID.Suffocation, 60);
                }
            }
        }
        public override void AI()
        {
            Projectile.knockBack = 0;
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Zero, 0.05f);
            Projectile.rotation += .01f;

            var currentTime = (float)(MAX_TIMELEFT - Projectile.timeLeft);
            Projectile.alpha = (int)(currentTime / MAX_TIMELEFT * 255);
            Projectile.scale = currentTime / MAX_TIMELEFT + .1f;
            CheckCollision();
        }
    }
}