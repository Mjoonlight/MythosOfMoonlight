﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MythosOfMoonlight.Projectiles.IridicProjectiles;
using rail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Items.Weapons
{
    public class ThawGauntlet : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 30;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 2;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 3f;
            Item.width = 30;
            Item.height = 30;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = false;
            Item.channel = true;
            Item.value = Item.buyPrice(0, 0, 0, 1);
            Item.rare = ItemRarityID.Green;
            Item.shoot = ModContent.ProjectileType<ThawGauntletP>();
        }
        public override bool CanShoot(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<ThawGauntletP>()] < 1 && player.statMana > 5;
        }
    }
    public class ThawGauntletP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(16);
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 30;
            Projectile.netUpdate = true;
            Projectile.netUpdate2 = true;
            Projectile.netImportant = true;
            Projectile.ownerHitCheck = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, tex.Size() / 2, Projectile.scale, Main.player[Projectile.owner].direction == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
            return false;
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
            Projectile.ai[1]++;
            foreach (Player player in Main.player)
            {
                if (player == Main.player[Projectile.owner] && player == Main.LocalPlayer)
                {
                    if (Projectile.ai[1] == 1)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Helper.FromAToB(player.Center, Main.MouseWorld) * 5, ModContent.ProjectileType<ThawGauntletP2>(), Projectile.damage, 0, Projectile.owner);
                    }
                    player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
                    Projectile.timeLeft++;
                    player.direction = Main.MouseWorld.X >= player.Center.X ? 1 : -1;
                    player.itemTime = 2;
                    player.itemAnimation = 2;
                    Projectile.ModProjectile.DrawHeldProjInFrontOfHeldItemAndArms = true;
                    player.heldProj = Projectile.whoAmI;
                    Projectile.Center = player.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
                    Projectile.rotation = (Main.MouseWorld - player.Center).ToRotation();

                    if (!player.channel || player.statMana <= 0 || !player.CheckMana(1)) Projectile.Kill();
                }
            }
        }
    }

    public class ThawGauntletP2 : ModProjectile
    {
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);
            Helper.SpawnDust(Projectile.Center, Projectile.Size, DustID.Ice, Vector2.Zero, 25, new Action<Dust>((target) => { target.noGravity = true; target.scale = Main.rand.NextFloat(0.6f, 0.9f); }
            ));
        }
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(26, 20);
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.aiStyle = -1;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 300;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;

        }
        public override bool ShouldUpdatePosition()
        {
            return Projectile.ai[0] != 0;
        }
        public override bool? CanDamage()
        {
            return Projectile.ai[0] != 0;
        }
        bool didAlpha;
        float alpha, vel = 5;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
            writer.Write(alpha);
            writer.Write(vel);
            writer.Write(didAlpha);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();
            alpha = reader.ReadSingle();
            vel = reader.ReadSingle();
            didAlpha = reader.ReadBoolean();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (alpha > 0)
                alpha -= 0.1f;
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Texture2D glow = Helper.GetTex(Texture + "_Glow");
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, tex.Size() / 2, Projectile.localAI[0], Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            Main.spriteBatch.Reload(BlendState.Additive);
            Main.spriteBatch.Draw(glow, Projectile.Center - Main.screenPosition, null, Color.White * alpha, Projectile.rotation, tex.Size() / 2, Projectile.localAI[0], Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.damage > 1)
                Projectile.damage /= 2;
        }
        int a;
        public override void AI()
        {
            Projectile.ai[1]++;
            foreach (Player player in Main.player)
            {
                if (player == Main.player[Projectile.owner] && player == Main.LocalPlayer)
                {
                    if (Projectile.ai[0] == 0)
                    {
                        Projectile.Center = player.Center + Helper.FromAToB(player.Center, Main.MouseWorld) * 40;
                        Projectile.velocity = Helper.FromAToB(player.Center, Main.MouseWorld) * 5;
                        if (Projectile.localAI[0] < 1)
                            Projectile.localAI[0] += 0.05f;
                        else
                        {
                            if (!didAlpha)
                            {
                                didAlpha = true;
                                alpha = 1f;
                            }
                        }
                        if (!didAlpha)
                            if (Projectile.ai[1] % 5 == 0)
                            {
                                player.CheckMana(2, true, true);
                                player.manaRegenDelay = (int)player.maxRegenDelay;
                            }
                        if (player.ZoneSnow)
                            Projectile.timeLeft = 600;
                        else
                            Projectile.timeLeft = 300;
                    }
                    if (Projectile.ai[0] == 0 && (!player.channel || player.statMana <= 0 || !player.CheckMana(1)))
                    {
                        if (Projectile.localAI[0] < 0.95f)
                            Projectile.Kill();
                        else
                            Projectile.ai[0] = 1;
                    }
                }
            }
            if (Projectile.ai[0] != 0)
            {
                Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0 ? 1 : -1;

                if (TRay.CastLength(Projectile.Center, Vector2.UnitY, 12) <= 10)
                {
                    Projectile.velocity.Y = 0;
                    if (TRay.CastLength(Projectile.Center, Vector2.UnitY, 12) > 2 && TRay.CastLength(Projectile.Center, -Vector2.UnitY, 12) > 2 && a < 3)
                    {
                        Projectile.Center = TRay.Cast(Projectile.Center - Vector2.UnitY * 10, Vector2.UnitY, 100) - new Vector2(0, 10);
                        Tile tile = Framing.GetTileSafely(Projectile.Center.ToTileCoordinates16().ToPoint());
                        if (tile.HasTile && !tile.IsActuated && WorldGen.SolidTile(tile))
                            a++;
                    }
                    else
                    {
                        Main.player[Projectile.owner].statMana += 5;
                        Projectile.timeLeft -= 300;
                    }
                    Helper.SpawnDust(Projectile.Bottom + new Vector2(5 * -Projectile.direction, -4), Vector2.One, DustID.Frost, new Vector2(-Projectile.velocity.X, -2), 2, new Action<Dust>((target) => { target.noGravity = true; target.scale = Main.rand.NextFloat(0.6f, 0.9f); }
                    ));
                    if (Projectile.localAI[1] == 0)
                        Projectile.velocity.X = vel * Projectile.direction;
                    Projectile.localAI[1]++;
                }
                if (TRay.CastLength(Projectile.Center, Vector2.UnitY, 12) > 10)
                {
                    a = 0;
                    if (Projectile.localAI[1] != 0)
                        Projectile.velocity.Y = MathHelper.Lerp(Projectile.velocity.Y, 10, 0.25f);
                    else
                        Projectile.velocity.Y += 0.15f;
                }
                // Projectile.velocity.X = 10 * Projectile.direction;
                vel *= 0.995f;
                Projectile.ai[1]--;
                int i2 = (int)((Projectile.position.X - 8f) / 16f);
                int num219 = (int)(Projectile.position.Y / 16f);
                bool bounce = false;
                bool bounce2 = false;
                bool stepUp = false;
                if (WorldGen.SolidTile(i2, num219))
                {
                    if (WorldGen.SolidTile(i2, num219 + 1))
                    {
                        if (Projectile.ai[1] < 0 && !WorldGen.SolidTile(i2, num219 + 2))
                            Projectile.Center -= Vector2.UnitY * 16;
                        else
                            bounce = true;
                        Projectile.ai[1] = 30;
                    }
                    else
                        bounce = true;
                }
                i2 = (int)((Projectile.position.X + (float)Projectile.width + 8f) / 16f);
                if (WorldGen.SolidTile(i2, num219))
                {
                    if (WorldGen.SolidTile(i2, num219 + 1))
                    {
                        if (Projectile.ai[1] < 0 && !WorldGen.SolidTile(i2, num219 + 2))
                            Projectile.Center -= Vector2.UnitY * 16;
                        else
                            bounce2 = true;
                        Projectile.ai[1] = 30;
                    }
                    else
                        bounce2 = true;
                }
                if (bounce)
                {
                    Projectile.velocity.X = vel;
                }
                else if (bounce2)
                {
                    Projectile.velocity.X = 0f - vel;
                }
            }

        }
    }
}