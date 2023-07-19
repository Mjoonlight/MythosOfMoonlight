using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.Buffs;

namespace MythosOfMoonlight.Projectiles.CenturySpewer
{
    public class CenturySpewerSpore : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/NPCs/Enemies/Overworld/CenturyFlower/CenturyFlowerSpore/CenturyFlowerSpore1";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Century Spewer Spore");
            Main.projFrames[Projectile.type] = 2;
        }

        const int MAX_TIMELEFT = 170;
        public override void SetDefaults()
        {
            Projectile.damage = 0;
            Projectile.height = 64;
            Projectile.penetrate = -1;
            Projectile.width = 80;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.damage = 0;
            Projectile.tileCollide = false;
            Projectile.timeLeft = MAX_TIMELEFT;
            Projectile.frame = Main.rand.Next(0, 2);
            Projectile.localNPCHitCooldown = 1;
        }

        public override void AI()
        {
            Projectile.knockBack = 0;
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Zero, 0.05f);
            Projectile.rotation += .01f;

            var currentTime = (float)(MAX_TIMELEFT - Projectile.timeLeft);
            Projectile.alpha = (int)(currentTime / MAX_TIMELEFT * 255);
            Projectile.scale = currentTime / MAX_TIMELEFT + .1f;
            if (Projectile.owner == Main.myPlayer)
            {
                Rectangle rect1 = new((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].active && !Main.npc[i].friendly && !Main.npc[i].dontTakeDamage && Main.npc[i].lifeMax > 1)
                    {
                        Rectangle rect2 = new((int)Main.npc[i].position.X, (int)Main.npc[i].position.Y, Main.npc[i].width, Main.npc[i].height);
                        if (rect1.Intersects(rect2))
                            Main.npc[i].AddBuff(ModContent.BuffType<NPCsuffocating>(), 2);
                    }
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<NPCsuffocating>(), 2);
        }
    }
}