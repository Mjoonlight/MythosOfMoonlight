using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.Dusts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace MythosOfMoonlight.Items.Pets
{
    public class BlinkrootI : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void SetDefaults()
        {
            Item.DefaultToVanitypet(ModContent.ProjectileType<Blinkroot>(), ModContent.BuffType<BlinkrootB>());
            Item.rare = ItemRarityID.Cyan;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.NPCHit54;
            Item.useTime = Item.useAnimation = 80;
        }
    }
    public class Blinkroot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            Main.projPet[Projectile.type] = true;

            ProjectileID.Sets.TrailCacheLength[Type] = 5;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BabyFaceMonster);
            Projectile.Size = new Vector2(30, 28);
            AIType = ProjectileID.BabyFaceMonster;
        }

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];

            player.babyFaceMonster = false;

            return true;
        }
        int frameCounter;
        Rectangle frames;
        public override bool PreDraw(ref Color lightColor)
        {
            frames.Width = 30;
            frames.Height = 30;

            Texture2D tex = Helper.GetTex("MythosOfMoonlight/Items/Pets/Blinkroot");
            Texture2D glow = Helper.GetTex("MythosOfMoonlight/Items/Pets/Blinkroot_Glow");
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition + new Vector2(0, 2), frames, lightColor, rotation, frames.Size() / 2, Projectile.scale, Projectile.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            Main.spriteBatch.Draw(glow, Projectile.Center - Main.screenPosition + new Vector2(0, 2), frames, Color.White * ((MathF.Sin(Main.GlobalTimeWrappedHourly * 0.75f) + 1) * 0.5f), rotation, frames.Size() / 2, Projectile.scale, Projectile.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            return false;
        }
        float rotation;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.dead && player.HasBuff(ModContent.BuffType<BlinkrootB>()))
            {
                Projectile.timeLeft = 2;
            }
            Lighting.AddLight(Projectile.Center, ((MathF.Sin(Main.GlobalTimeWrappedHourly * 0.75f) + 1) * 0.5f) * 0.2f, ((MathF.Sin(Main.GlobalTimeWrappedHourly * 0.75f) + 1) * 0.5f) * 0.2f, ((MathF.Sin(Main.GlobalTimeWrappedHourly * 0.75f) + 1) * 0.5f) * 0.2f);
            frameCounter++;
            int height = frames.Height;
            if (Projectile.velocity.Y >= 0f && (double)Projectile.velocity.Y <= 0.8)
            {
                if ((double)Projectile.velocity.X < -0.8 || (double)Projectile.velocity.X > 0.8)
                {
                    if (frameCounter % 5 == 0)
                    {
                        frames.Y += height;
                        if (frames.Y > 4 * height)
                            frames.Y = 0 * height;
                    }
                }
                else frames.Y = 0;
                rotation = MathHelper.Lerp(rotation, 0, 0.2f);
            }
            else
            {
                if (Main.rand.NextBool(4))
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GrassBlades, Scale: Main.rand.NextFloat(0.25f, .5f));
                Projectile.direction = 1;
                rotation += MathHelper.ToRadians(6);
                frames.Y = 0;
            }

        }
    }
    public class BlinkrootB : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.lightPet[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;

            int projType = ModContent.ProjectileType<Blinkroot>();


            if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[projType] <= 0)
            {
                var entitySource = player.GetSource_Buff(buffIndex);

                Projectile.NewProjectile(entitySource, player.Center, Vector2.Zero, projType, 0, 0f, player.whoAmI);
            }
        }
    }
}
