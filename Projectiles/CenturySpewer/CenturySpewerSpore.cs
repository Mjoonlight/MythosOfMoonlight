using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace MythosOfMoonlight.Projectiles.CenturySpewer.CenturySpewerSpore
{
    public class CenturySpewerSpore : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/NPCs/Enemies/CenturyFlower/CenturyFlowerSpore/CenturyFlowerSpore1";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Century Spewer Spore");
            Main.projFrames[projectile.type] = 2;
        }

        const int MAX_TIMELEFT = 270;
        public override void SetDefaults()
        {
            projectile.damage = 5;
            projectile.height = 64;
            projectile.penetrate = -1;  
            projectile.width = 80;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.damage = 0;
            projectile.tileCollide = false;
            projectile.timeLeft = MAX_TIMELEFT;
            projectile.frame = Main.rand.Next(0, 2);
            projectile.localNPCHitCooldown = 1;
        }

        public override void AI()
        {
            projectile.knockBack = 0;
            projectile.velocity = Vector2.Lerp(projectile.velocity, Vector2.Zero, 0.05f);
            projectile.rotation += .01f;

            var currentTime = (float)(MAX_TIMELEFT - projectile.timeLeft);
            projectile.alpha = (int)(currentTime / MAX_TIMELEFT * 255);
            projectile.scale = currentTime / MAX_TIMELEFT + .1f;
        }
    }
}