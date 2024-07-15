using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Items.Weapons.Ranged
{
    public class Borer : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("4-Bore");
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 20;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.shoot = ModContent.ProjectileType<BorerHeld>();
            Item.shootSpeed = 1f;
            Item.rare = ItemRarityID.Green;
            Item.knockBack = 5f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item38;
            Item.noUseGraphic = true;
            Item.useAmmo = AmmoID.Bullet;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.channel = true;
            Item.value = Item.buyPrice(0, 8, 0, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            velocity.Normalize();
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<BorerHeld>(), damage, knockback, player.whoAmI);
            return false;
        }
    }
    public class BorerHeld : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/Items/Weapons/Ranged/Borer";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DontCancelChannelOnKill[Type] = true;
        }

        public virtual float Ease(float f)
        {
            return 1 - (float)Math.Pow(2, 10 * f - 10);
        }
        public virtual float ScaleFunction(float progress)
        {
            return 0.7f + (float)Math.Sin(progress * Math.PI) * 0.5f;
        }
        float holdOffset;
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.Size = new(88, 26);
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.timeLeft = 50;
            holdOffset = 20;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead || player.CCed || player.noItems)
            {
                return;
            }
            if (Projectile.ai[2] > 0)
                Projectile.ai[2] -= 0.2f;
            if (Projectile.timeLeft == 46)
            {
                bool success = false;
                for (int j = 54; j < 58; j++)
                {
                    if (player.inventory[j].ammo == player.HeldItem.useAmmo && player.inventory[j].stack > 0)
                    {
                        if (player.inventory[j].maxStack > 1)
                            player.inventory[j].stack--;
                        success = true;
                        break;
                    }
                }
                if (!success)
                {
                    Projectile.Kill();
                    return;
                }

                Vector2 velocity = Projectile.velocity * 10;
                Vector2 position = player.Center + new Vector2(0, -6).RotatedBy(velocity.ToRotation()) * player.direction;
                position += velocity * 3;
                for (int i = 0; i < 8; i++)
                {
                    Dust.NewDustPerfect(position + velocity * 3, DustID.Torch, (velocity * Main.rand.NextFloat(0.1f, 1)).RotatedByRandom(MathHelper.PiOver4), Scale: 1.5f);
                    Dust.NewDustPerfect(position + velocity * 3, DustID.Smoke, (velocity * Main.rand.NextFloat(0.1f, 1)).RotatedByRandom(MathHelper.PiOver4), Scale: 1.5f);
                }
                Projectile.NewProjectile(null, position, velocity, ModContent.ProjectileType<BorerP>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
            if (Projectile.timeLeft > 40 && Projectile.timeLeft <= 45)
            {
                holdOffset = MathHelper.Lerp(holdOffset, 6, 0.3f);
                Projectile.ai[0] = MathHelper.Lerp(Projectile.ai[0], MathHelper.ToRadians(29f * -Projectile.direction), 0.3f);
            }
            else if (Projectile.timeLeft < 35)
            {
                Projectile.ai[1] += 0.002f;
                holdOffset = MathHelper.Lerp(holdOffset, 20, 0.1f);
                Projectile.ai[0] = MathHelper.Lerp(Projectile.ai[0], 0, 0.05f + Projectile.ai[1]);
            }
            if (Projectile.timeLeft == 15)
            {
                Helper.SpawnGore(player.Center, "MythosOfMoonlight/BorerG", vel: -Vector2.UnitY);
            }
            if (Projectile.timeLeft == 1)
            {
                if (player.active && player.channel && !player.dead && !player.CCed && !player.noItems)// && player.autoReuseAllWeapons)
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        SoundEngine.PlaySound(SoundID.Item38, player.Center);
                        Vector2 dir = Vector2.Normalize(Main.MouseWorld - player.Center);
                        Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), player.Center, dir, Projectile.type, Projectile.damage, Projectile.knockBack, player.whoAmI, 0);
                        proj.Center = Projectile.Center;
                        proj.velocity = dir;
                    }
                }
            }

            Projectile.direction = Projectile.velocity.X > 0 ? 1 : -1;
            Vector2 pos = player.RotatedRelativePoint(player.MountedCenter);
            player.ChangeDir(Projectile.velocity.X < 0 ? -1 : 1);
            player.itemRotation = (Projectile.velocity.ToRotation() + Projectile.ai[0]) * player.direction;
            pos += (Projectile.velocity.RotatedBy(Projectile.ai[0])) * holdOffset;
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.velocity.ToRotation() - MathHelper.PiOver2);

            Projectile.rotation = (pos - player.Center).ToRotation() + Projectile.ai[0] * Projectile.spriteDirection;
            player.itemTime = 2;
            Projectile.Center = pos - Vector2.UnitY * 2;
            player.heldProj = Projectile.whoAmI;
            player.itemAnimation = 2;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            SpriteEffects effects = Projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;
            if (Projectile.timeLeft < 50)
                Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, tex.Size() / 2, Projectile.scale, effects, 0);
            return false;
        }
    }
    public class BorerP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("4-Bore");
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 36;
        }
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 500 * 6;
            Projectile.extraUpdates = 6;
            Projectile.Size = new(12, 12);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Helper.GetTex(Texture + "_Trail");
            var fadeMult = 1f / Projectile.oldPos.Length;
            float colorF = MathHelper.Lerp(1, 0, Projectile.ai[2] / 36);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float mult = (1f - fadeMult * i);
                if (i > 0)
                    for (int j = 0; j < 7; j++)
                    {
                        Vector2 p = Vector2.Lerp(Projectile.oldPos[i - 1] + Projectile.Size / 2, Projectile.oldPos[i] + Projectile.Size / 2, (float)j / 7);
                        Main.spriteBatch.Draw(tex, p - Main.screenPosition, null, Color.White * 0.4f * colorF * mult, Projectile.rotation, Projectile.Size / 2, Projectile.scale * mult, SpriteEffects.None, 0);
                    }
            }
            return Projectile.ai[2] == 0;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.life <= 0)
                if (Projectile.ai[1]++ < 6)
                    Projectile.penetrate++;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            Projectile.tileCollide = false;
            for (int i = 0; i < 20; i++)
            {
                Vector2 vel = Main.rand.NextVector2Circular(8, 8);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, vel.X, vel.Y, 150, default(Color), Main.rand.NextFloat(0.5f, 2));
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, vel.X, vel.Y, 150, default(Color), Main.rand.NextFloat(0.5f, 2));
            }
            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            Projectile.velocity = Vector2.Zero;
            if (Projectile.ai[2] == 0)
            {
                Projectile.ai[2] = 1;
                Projectile.timeLeft = 36;
            }
            return false;
        }
        public override bool PreKill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                Vector2 vel = Main.rand.NextVector2Circular(8, 8);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, vel.X, vel.Y, 150, default(Color), Main.rand.NextFloat(0.5f, 2));
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, vel.X, vel.Y, 150, default(Color), Main.rand.NextFloat(0.5f, 2));
            }
            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            Projectile.velocity = Vector2.Zero;
            if (Projectile.ai[2] == 0)
            {
                Projectile.ai[2] = 1;
                Projectile.timeLeft = 36;
                return false;
            }
            return Projectile.ai[2] > 36;
        }
        public override void AI()
        {
            if (Projectile.ai[2] > 0)
                Projectile.ai[2]++;
            if (Projectile.ai[2] > 36)
                Projectile.Kill();
            if (Projectile.timeLeft % 8 == 0 && Projectile.ai[2] <= 0)
                Dust.NewDustPerfect(Projectile.Center - new Vector2(6, 0).RotatedBy(Projectile.rotation - MathHelper.PiOver2), DustID.Torch, Projectile.velocity.RotatedByRandom(MathHelper.PiOver4), Scale: 2).noGravity = true;
            if (Main.rand.NextBool(15) && Projectile.ai[2] <= 0)
                Dust.NewDustPerfect(Projectile.Center - new Vector2(6, 0).RotatedBy(Projectile.rotation - MathHelper.PiOver2), DustID.Smoke, Projectile.velocity.RotatedByRandom(MathHelper.PiOver4), Scale: Main.rand.NextFloat(1, 2));
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
    }
}
