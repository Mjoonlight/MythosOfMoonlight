using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using MythosOfMoonlight.Dusts;
using Terraria.Audio;
using System;
using static Humanizer.In;

namespace MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim.Projectiles
{
    internal class StarineSlush : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/NPCs/Minibosses/RupturedPilgrim/Projectiles/PilgrimExplosion_Extra";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starine Goop");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
        }
        public override void SetDefaults()
        {
            Projectile.damage = 10;
            Projectile.width = 488;
            Projectile.height = 486;
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 200;
            Projectile.hostile = true;
            Projectile.scale = 0;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 16; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<StarineDust>(), 2f);
                Main.dust[dust].scale = 2f;
                Main.dust[dust].velocity = Main.rand.NextVector2Unit() * 1.2f;
                Main.dust[dust].velocity.Y = -1.5f;
                Main.dust[dust].noGravity = true;
            }
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<PilgrimExplosion>(), 12, .1f, Main.myPlayer);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
        }
        public override void AI()
        {
            if (Projectile.scale < 0.15f)
                Projectile.scale += 0.01f;
            //float progress = Utils.GetLerpValue(0, 200, Projectile.timeLeft);
            //Projectile.scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 0.1f, 0, 0.15f);
            if (Projectile.timeLeft > 150)
                Projectile.velocity = (Main.LocalPlayer.Center - Vector2.UnitY * 300 - Projectile.Center) / 20f;
            if (Projectile.timeLeft == 150)
                Projectile.velocity = -Vector2.UnitY * 5f;
            if (Projectile.timeLeft < 130 && Projectile.timeLeft > 120)
                Projectile.velocity *= 0.98f;
            if (Projectile.timeLeft < 120)
            {
                Projectile.aiStyle = 2;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D drawTexture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Reload(BlendState.Additive);
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Main.spriteBatch.Draw(drawTexture, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, null, Color.Cyan * (1f - fadeMult * i), 0, drawTexture.Size() / 2, Projectile.scale * (1f - fadeMult * i), SpriteEffects.None, 0);
                //Main.spriteBatch.Draw(drawTexture, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, null, Color.White * (1f - fadeMult * i), 0, Projectile.Size / 2, Projectile.scale * (1f - fadeMult * i), SpriteEffects.None, 0);
            }
            Main.EntitySpriteDraw(drawTexture, Projectile.Center - Main.screenPosition, null, Color.Cyan, 0f, drawTexture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(drawTexture, Projectile.Center - Main.screenPosition, null, Color.White * 0.7f, 0f, drawTexture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
    }
}
