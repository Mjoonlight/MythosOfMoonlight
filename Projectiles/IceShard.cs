using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;
using Terraria.Audio;

namespace MythosOfMoonlight.Projectiles
{
    public class IceShard : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White * Projectile.scale * 0.7f;
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.aiStyle = 1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 500;
            Projectile.scale = 0;
            Projectile.tileCollide = true;
            Projectile.hide = true;
        }
        public override void Kill(int timeLeft)
        {
            for (int num613 = 0; num613 < 15; num613++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Frost, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 150, default(Color), 0.8f);
            }
            SoundEngine.PlaySound(SoundID.Item48, Projectile.Center);
        }
        public override bool ShouldUpdatePosition()
        {
            return Projectile.timeLeft < 450;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.frame = Main.rand.Next(3);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Main.rand.NextBool(4))
                target.AddBuff(BuffID.Frostburn2, 200);
            target.AddBuff(BuffID.Chilled, 300);
        }
        public override void AI()
        {
            if (Projectile.scale < 1)
            {

                Projectile.scale += 0.05f;
            }
            if (Projectile.timeLeft > 450)
                Projectile.velocity = Projectile.oldVelocity;
            //Dust a = Dust.NewDustPerfect(Projectile.Center, DustID.Frost);
            //a.noGravity = true;
            //a.scale = 0.25f;
            Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0 ? 1 : -1;
            Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.direction == 1 ? MathHelper.PiOver4 : -MathHelper.PiOver4);
        }
    }
}
