using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using MythosOfMoonlight.Dusts;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace MythosOfMoonlight.NPCs.Enemies.Underground.EntropicTotem.EntropicTotemProjectile
{
    public class EntropicTotemProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 25;
            ProjectileID.Sets.TrailingMode[Type] = 2;
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
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Helper.GetExtraTex("Extra/flare_01");
            Texture2D tex2 = Helper.GetExtraTex("Extra/explosion_1");
            Main.spriteBatch.Reload(BlendState.Additive);
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < Projectile.oldPos.Length - (Projectile.timeLeft < 25 ? 25 - Projectile.timeLeft : 0); i++)
            {
                float mult = (1f - fadeMult * i);
                if (i > 0)
                    for (float j = 0; j < 5; j++)
                    {
                        Vector2 pos = Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[i - 1], (float)(j / 5));
                        Main.spriteBatch.Draw(tex2, pos + Projectile.Size / 2 - Main.screenPosition, null, Color.Pink * mult, Projectile.oldRot[i], tex2.Size() / 2, mult * 0.026f, SpriteEffects.None, 0);
                        Main.spriteBatch.Draw(tex2, pos + Projectile.Size / 2 - Main.screenPosition, null, Color.White * 0.15f * mult, Projectile.oldRot[i], tex2.Size() / 2, mult * 0.025f, SpriteEffects.None, 0);
                    }
            }
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, tex.Size() / 2, 0.05f, SpriteEffects.None, 0);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
        float RotationalIncrement => MathHelper.ToRadians(Projectile.ai[0]);
        int Parent => (int)Projectile.ai[1];
        public override void AI()
        {
            Projectile.damage = Main.expertMode ? Main.npc[Parent].damage / 2 : Main.npc[Parent].damage;
            var dustType = ModContent.DustType<EntropicTotemProjectileDust>();
            if (Main.rand.NextBool(5)) Dust.NewDustPerfect(Projectile.Center, dustType, -Projectile.velocity, Scale: 2f);

            Projectile.velocity = Projectile.velocity.RotatedBy(RotationalIncrement);
            Projectile.position += Main.npc[Parent].position - Main.npc[Parent].oldPosition; // move to NPC's position constantly
            if (!Main.npc[Parent].active)
                Projectile.timeLeft = 0;
        }
    }
}
