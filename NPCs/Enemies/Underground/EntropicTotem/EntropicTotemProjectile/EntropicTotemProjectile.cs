using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using MythosOfMoonlight.Dusts;

namespace MythosOfMoonlight.NPCs.Enemies.Underground.EntropicTotem.EntropicTotemProjectile
{
    public class EntropicTotemProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Totem Bullet");
        }
        public const int MAX_TIMELEFT = 240;
        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 10;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.damage = 42;
            Projectile.tileCollide = false;
            Projectile.timeLeft = MAX_TIMELEFT;
        }
        float RotationalIncrement => MathHelper.ToRadians(Projectile.ai[0]);
        int Parent => (int)Projectile.ai[1];
        public override void AI()
        {
            Projectile.damage = Main.expertMode ? Main.npc[Parent].damage / 2 : Main.npc[Parent].damage;
            var dustType = ModContent.DustType<EntropicTotemProjectileDust>();
            var dust = Dust.NewDustPerfect(Projectile.Center, dustType, -Projectile.velocity, Scale: 2f);

            Projectile.velocity = Projectile.velocity.RotatedBy(RotationalIncrement);
            Projectile.position += Main.npc[Parent].position - Main.npc[Parent].oldPosition; // move to NPC's position constantly
            if (!Main.npc[Parent].active)
                Projectile.timeLeft = 0;
        }
    }
}
