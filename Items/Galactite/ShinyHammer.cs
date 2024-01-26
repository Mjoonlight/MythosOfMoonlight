using System;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.GameContent;
using MythosOfMoonlight.Misc;
using MythosOfMoonlight.Tiles.Furniture.Pilgrim;
using MythosOfMoonlight.Tiles;
using System.Collections.Generic;

namespace MythosOfMoonlight.Items.Galactite
{
    public class ShinyHammer : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Gravitron");
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.PaladinsHammer);
            Item.damage = 20;
            Item.shoot = ModContent.ProjectileType<ShinyHammerP>();
        }
    }
    public class ShinyHammerP : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/Items/Galactite/ShinyHammer";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 11;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            Projectile.AddElement(CrossModHelper.Celestial);
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Shuriken);
            Projectile.timeLeft = 500;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            for (int i = 1; i < 5; i++)
            {
                float _scale = MathHelper.Lerp(1f, 0.95f, (float)(5 - i) / 5);
                var fadeMult = 1f / 5;
                Main.spriteBatch.Draw(tex, Projectile.oldPos[i] - Main.screenPosition + tex.Size() / 2, null, Color.Pink * (1f - fadeMult * i) * 0.5f, Projectile.oldRot[i], Projectile.Size / 2, _scale, SpriteEffects.None, 0f);
            }
            return true;
        }
        public override void AI()
        {
            if (Projectile.timeLeft > 460)
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Zero, 0.05f);
            else
            {
                Projectile.aiStyle = 0;
                Vector2 move = Vector2.Zero;
                float distance = 5050f;
                bool target = false;
                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].active && !Main.npc[k].friendly && !Main.npc[k].dontTakeDamage)
                    {
                        Vector2 newMove = Main.npc[k].Center - Projectile.Center;
                        float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                        if (distanceTo < distance)
                        {
                            move = newMove;
                            distance = distanceTo;
                            target = true;
                        }
                    }
                }
                if (++Projectile.ai[0] % 5 == 0 && target && Projectile.timeLeft > 45)
                {
                    AdjustMagnitude(ref move);
                    Projectile.velocity = (11f * Projectile.velocity + move) / 11f;
                    AdjustMagnitude(ref Projectile.velocity);
                }
                if (Projectile.timeLeft < 45)
                {
                    Projectile.velocity *= 0.95f;
                }
            }
        }
        private static void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 11f)
            {
                vector *= 11f / magnitude;
            }
        }
    }
}
