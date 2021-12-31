using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MythosOfMoonlight.Dusts;

namespace MythosOfMoonlight.NPCs.Enemies.EntropicTotem.EntropicTotemProjectile
{
    public class EntropicTotemProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Totem Bullet");
        }
        public const int MAX_TIMELEFT = 240;
        public override void SetDefaults()
        {
            projectile.height = projectile.width = 10;
            projectile.aiStyle = -1;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.damage = 42;
            projectile.tileCollide = false;
            projectile.timeLeft = MAX_TIMELEFT;
        }
        float RotationalIncrement => MathHelper.ToRadians(projectile.ai[0]);
        int Parent => (int)projectile.ai[1];
        public override void AI()
        {
            var dustType = ModContent.DustType<EntropicTotemProjectileDust>();
            var dust = Dust.NewDustPerfect(projectile.Center, dustType, -projectile.velocity);

            projectile.velocity = projectile.velocity.RotatedBy(RotationalIncrement);
            projectile.position += Main.npc[Parent].position - Main.npc[Parent].oldPosition; // move to npc's position constantly
            if (!Main.npc[Parent].active)
            {
                projectile.timeLeft = 0;
            }
        }
    }
}
