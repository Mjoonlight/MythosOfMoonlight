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
using MythosOfMoonlight.Common.Graphics.Misc;

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
            Projectile.CloneDefaults(ProjectileID.FennecFox);
            Projectile.Size = new Vector2(30, 28);
            AIType = 0;
            Projectile.aiStyle = -1;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            Player player = Main.player[Projectile.owner];
            fallThrough = player.Center.Y > Projectile.Center.Y + Projectile.height;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref Projectile.stepSpeed, ref Projectile.gfxOffY);
            return base.OnTileCollide(oldVelocity);
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
            if (Projectile.Center.Distance(player.Center) > 4000)
                Projectile.Center = player.Center;
            Lighting.AddLight(Projectile.Center, 0.05f + ((MathF.Sin(Main.GlobalTimeWrappedHourly * 0.75f) + 1) * 0.5f) * 0.2f, 0.05f + ((MathF.Sin(Main.GlobalTimeWrappedHourly * 0.75f) + 1) * 0.5f) * 0.2f, 0.05f + ((MathF.Sin(Main.GlobalTimeWrappedHourly * 0.75f) + 1) * 0.5f) * 0.2f);
            frameCounter++;
            int height = frames.Height;
            Projectile.tileCollide = (Player.GetFloorTile(player.Bottom.ToTileCoordinates().X, player.Bottom.ToTileCoordinates().Y) != null && Player.GetFloorTile(Projectile.Top.ToTileCoordinates().X, Projectile.Top.ToTileCoordinates().Y) == null && (player.Center.X.CloseTo(Projectile.Center.X, 400) && player.Center.Y.CloseTo(Projectile.Center.Y, 200)));
            if ((Projectile.velocity.Y += 0.2f) > 7f) Projectile.velocity.Y = 7f;

            if (!Projectile.tileCollide)
            {
                Projectile.ai[2]++;
                Vector2 to = Helper.FromAToB(Projectile.Center, player.Center - new Vector2(0, 50));
                float s = MathHelper.Clamp(MathHelper.Lerp(4, 10, player.Center.Distance(Projectile.Center) / 150), 4, 10);
                if (Projectile.ai[2] % 40 <= 10)
                {
                    Projectile.velocity.X = MathHelper.Lerp(Projectile.velocity.X, to.X * s, 0.1f);
                }
                if (Projectile.ai[2] % 40 >= 35)
                    Projectile.velocity.X *= 0.9f;
                if (Projectile.velocity.Y > -10)
                    Projectile.velocity.Y += (to.Y * s) * 0.1f;

                if (Main.rand.NextBool(4))
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GrassBlades, Scale: Main.rand.NextFloat(0.25f, .5f));
                rotation += MathHelper.ToRadians(6);
                frames.Y = 0;
            }
            else
            {

                if (Collision.SolidCollision(Projectile.position + new Vector2(0, 22), Projectile.width, 6))
                    Projectile.Center -= Vector2.UnitY;

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
                rotation = Helper.LerpAngle(rotation, 0, 0.3f);
                if (!Collision.CanHit(Projectile.position + new Vector2(Helper.FromAToB(Projectile.position, player.position, false).X, 0), Projectile.width, Projectile.height, player.position, player.width, player.height))
                    Projectile.velocity.Y = -5;


                float range = 6f;
                float speed = 0.2f;
                if (range < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y))
                {
                    range = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);
                    speed = 0.3f;
                }

                float posX = player.Center.X + -50 * -player.direction;
                if (posX < Projectile.position.X + (float)(Projectile.width / 2) - (float)30)
                {
                    if ((double)Projectile.velocity.X > -3.5)
                    {
                        Projectile.velocity.X -= speed;
                    }
                    else
                    {
                        Projectile.velocity.X -= speed * 0.25f;
                    }
                }
                else if (posX > Projectile.position.X + (float)(Projectile.width / 2) + (float)30)
                {
                    if ((double)Projectile.velocity.X < 3.5)
                    {
                        Projectile.velocity.X += speed;
                    }
                    else
                    {
                        Projectile.velocity.X += speed * 0.25f;
                    }
                }
                else
                {
                    Projectile.velocity.X *= 0.9f;
                    if (Projectile.velocity.X >= 0f - speed && Projectile.velocity.X <= speed)
                    {
                        Projectile.velocity.X = 0f;
                    }
                }
            }
            //if (TRay.CastLength(Projectile.Center, Vector2.UnitY, Projectile.height, true) < Projectile.height * 0.75f)
            //Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref Projectile.stepSpeed, ref Projectile.gfxOffY);
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
