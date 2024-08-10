using System;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.GameContent;
using MythosOfMoonlight.Common.Graphics.Misc;
using MythosOfMoonlight.Tiles.Furniture.Pilgrim;
using MythosOfMoonlight.Tiles;
using System.Collections.Generic;
using MythosOfMoonlight.Items.Weapons.Melee;
using Terraria.DataStructures;
using MythosOfMoonlight.Projectiles;
using MythosOfMoonlight.Common.Systems;
using Terraria.Audio;
using MythosOfMoonlight.Dusts;
using MythosOfMoonlight.Projectiles.VFXProjectiles;
using MythosOfMoonlight.Common.Crossmod;

namespace MythosOfMoonlight.Items.PurpleComet.Galactite
{
    public class ShinyHammer : ModItem
    {
        public override void SetDefaults()
        {
            Item.knockBack = 10f;
            Item.width = Item.height = 80;
            Item.crit = 30;
            Item.damage = 22;
            Item.useAnimation = 40;
            Item.useTime = 40;
            Item.noUseGraphic = true;
            Item.autoReuse = false;
            Item.noMelee = true;
            Item.channel = true;
            Item.DamageType = DamageClass.Melee;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = 2;
            Item.shootSpeed = 1f;
            Item.shoot = ModContent.ProjectileType<GravitronP>();
        }
        public override bool? CanAutoReuseItem(Player player)
        {
            return false;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, Vector2.UnitX * player.direction, type, damage, knockback, player.whoAmI, 0, -player.direction);
            return false;
        }
    }
    public class GravitronP : HeldSword
    {
        public override string Texture => "MythosOfMoonlight/Items/PurpleComet/Galactite/ShinyHammer";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DontCancelChannelOnKill[Type] = true;
            Projectile.AddElement(CrossModHelper.Celestial);
            ProjectileID.Sets.TrailCacheLength[Type] = 30;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetExtraDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            swingTime = 75;
            holdOffset = 30;
            Projectile.tileCollide = false;
            modifyCooldown = true;
            Projectile.localNPCHitCooldown = 10;
            IgnoreDefaultBehaviour = true;
            glowAlpha = 1f;
            glowBlend = BlendState.AlphaBlend;
        }
        public override float Ease(float x)
        {
            float c1 = 1.70158f;
            float c2 = c1 * 1.525f;

            return x < 0.5f
              ? (MathF.Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2)) / 2
              : (MathF.Pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2;
        }
        float lerpProg = 1, swingProgress, rotation;
        bool hitNPC;
        public override void PreExtraDraw(float progress)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 tip = player.Center + (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * (Projectile.height + holdOffset * 0.25f);

        }
        public override void ExtraAI()
        {
            Player player = Main.player[Projectile.owner];
            if (lerpProg != 1 && lerpProg != -1)
                lerpProg = MathHelper.SmoothStep(lerpProg, 1, 0.1f);
            if ((swingProgress > 0.35f && swingProgress < 0.75f) || hitNPC)
                if (Projectile.ai[0] == 0 && TRay.CastLength(Projectile.Center, Vector2.UnitY, 100, true) < 15)
                {
                    Projectile.ai[0] = 1;
                    CameraSystem.ScreenShakeAmount = 5;
                    Projectile.timeLeft = 14;
                    SoundEngine.PlaySound(SoundID.Item70.WithVolumeScale(0.67f), Projectile.Center);
                    SoundEngine.PlaySound(new SoundStyle("MythosOfMoonlight/Assets/Sounds/estrellaImpact") { Pitch = 0.3f, PitchVariance = 0.15f, MaxInstances = 3 }, Projectile.Center);
                    //for (int i = 0; i < 5; i++)
                    //  Projectile.NewProjectile(Projectile.GetSource_FromAI(), TRay.Cast(Projectile.Center - Vector2.UnitY * 30, Vector2.UnitY, 500, true), Vector2.Zero, ModContent.ProjectileType<LimestoneDebris>(), Projectile.damage / 15, Projectile.knockBack, Projectile.owner);

                    Projectile.NewProjectile(null, Projectile.Center + new Vector2(Projectile.width / 4 * player.direction, 25), Vector2.Zero, ModContent.ProjectileType<GravitronImpactVFX>(), Projectile.damage, 0);
                    int rand = Main.rand.Next(3, 6);
                    for (int i = 0; i < 20; i++)
                    {
                        Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GemAmethyst, Projectile.velocity.X * Main.rand.NextFloat(-3, 7), -Main.rand.NextFloat(1, 7), Scale: Main.rand.NextFloat(0.5f, 0.9f)).noGravity = true;
                        if (i > rand)
                        {
                            //Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<Starry>(), new Vector2(Projectile.velocity.X * Main.rand.NextFloat(-1, 7), -Main.rand.NextFloat(2, 8)), 0, Color.White, Main.rand.NextFloat(0.12f, 0.2f));
                        }
                    }

                    lerpProg = -1;
                }
            int direction = (int)Projectile.ai[1];
            if (lerpProg >= 0)
                swingProgress = MathHelper.Lerp(swingProgress, Ease(Utils.GetLerpValue(0f, swingTime, Projectile.timeLeft)), lerpProg);
            float defRot = Projectile.velocity.ToRotation();
            float start = defRot - (MathHelper.PiOver2 + MathHelper.PiOver4);
            float end = defRot + (MathHelper.PiOver2 + MathHelper.PiOver4);
            if (lerpProg >= 0)
                rotation = MathHelper.Lerp(rotation, direction == 1 ? start + MathHelper.Pi * 3 / 2 * swingProgress : end - MathHelper.Pi * 3 / 2 * swingProgress, lerpProg);
            Vector2 position = player.GetFrontHandPosition(stretch, rotation - MathHelper.PiOver2) +
                rotation.ToRotationVector2() * holdOffset * ScaleFunction(swingProgress);
            Projectile.Center = position;
            Projectile.rotation = (position - player.Center).ToRotation() + MathHelper.PiOver4;
            player.ChangeDir(Projectile.velocity.X < 0 ? -1 : 1);
            player.heldProj = Projectile.whoAmI;
            player.SetCompositeArmFront(true, stretch, rotation - MathHelper.PiOver2);
            player.SetCompositeArmBack(true, stretch, rotation - MathHelper.PiOver2 - MathHelper.PiOver4);
            if (swingProgress > 0.87f)
            {
                Vector2 tip = player.Center + (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * (Projectile.height);
                for (int i = 0; i < 2; i++)
                {
                    Vector2 off = Main.rand.NextVector2CircularEdge(40, 40);
                    Dust d = Dust.NewDustPerfect(tip + off * 1.5f, ModContent.DustType<LineDustFollowPoint>(), Helper.FromAToB(tip + off, tip) * Main.rand.NextFloat(7, 15), 0, Color.DarkViolet * 0.35f, Main.rand.NextFloat(0.025f, 0.15f));

                    Dust d2 = Dust.NewDustPerfect(tip + off * 2.5f, ModContent.DustType<LineDustFollowPoint>(), Helper.FromAToB(tip + off, tip) * Main.rand.NextFloat(15, 25), 0, Color.DarkViolet * 0.25f, Main.rand.NextFloat(0.05f, 0.2f));
                    d.customData = tip;
                    d2.customData = tip;
                }
            }
        }
        public override bool? CanDamage()
        {
            return Projectile.ai[0] < 1 && swingProgress > 0.35f && swingProgress < 0.65f;
        }
        public override void OnHit(NPC target, NPC.HitInfo hit, int damageDone)
        {
            CameraSystem.ScreenShakeAmount = 2;
            //lerpProg = -.1f;
            hitNPC = true;
        }
    }
    public class GravitronOrb : ModProjectile
    {
        public override string Texture => Helper.ExtraDir + "purpVortex";
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Bullet);
            Projectile.aiStyle = -1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Texture2D tex2 = Helper.GetTex(Helper.ExtraDir + "purpCircle");
            Texture2D tex3 = Helper.GetTex(Helper.ExtraDir + "darkPurpCircle");
            Main.spriteBatch.Draw(tex3, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, tex2.Size() / 2, Projectile.scale * 0.075f, SpriteEffects.None, 0);
            Main.spriteBatch.Reload(BlendState.Additive);
            Main.spriteBatch.Draw(tex2, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, tex2.Size() / 2, Projectile.scale * 0.075f, SpriteEffects.None, 0);
            Main.spriteBatch.Reload(effect: MythosOfMoonlight.PullingForce);
            MythosOfMoonlight.PullingForce.Parameters["uOpacity"].SetValue(1f);
            MythosOfMoonlight.PullingForce.Parameters["uIntensity"].SetValue(1f);
            MythosOfMoonlight.PullingForce.Parameters["uOffset"].SetValue(0.5f);
            MythosOfMoonlight.PullingForce.Parameters["uSpeed"].SetValue(4f);
            MythosOfMoonlight.PullingForce.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * 4f);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, tex.Size() / 2, Projectile.scale * 0.025f, SpriteEffects.None, 0);
            Main.spriteBatch.Reload(effect: null);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(2);
            Projectile.timeLeft = 10;
        }
    }
}
