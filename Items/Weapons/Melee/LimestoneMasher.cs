using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.Projectiles;
using Terraria.Audio;
using MythosOfMoonlight.Common.Systems;
using Terraria.Localization;

namespace MythosOfMoonlight.Items.Weapons.Melee
{
    public class LimestoneMasher : ModItem
    {
        public override void SetDefaults()
        {
            Item.knockBack = 10f;
            Item.width = Item.height = 80;
            Item.crit = 30;
            Item.damage = 15;
            Item.useAnimation = 40;
            Item.useTime = 40;
            Item.noUseGraphic = true;
            Item.autoReuse = false;
            Item.noMelee = true;
            Item.channel = true;
            Item.DamageType = DamageClass.Melee;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = 5;
            Item.shootSpeed = 1f;
            Item.shoot = ModContent.ProjectileType<LimestoneMasherP>();
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
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FossilOre, 20)
                .AddIngredient(ItemID.StoneBlock, 50)
                .AddRecipeGroup(RecipeGroup.RegisterGroup("GoldHammers", new RecipeGroup(() => $"{Lang.GetItemNameValue(ItemID.PlatinumHammer)} / {Lang.GetItemNameValue(ItemID.GoldHammer)}", ItemID.PlatinumHammer, ItemID.GoldHammer)))
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
    public class LimestoneMasherP : HeldSword
    {
        public override string Texture => "MythosOfMoonlight/Items/Weapons/Melee/LimestoneMasher";
        public override void SetExtraDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            swingTime = 65;
            holdOffset = 30;
            Projectile.tileCollide = false;
            modifyCooldown = true;
            Projectile.localNPCHitCooldown = 10;
            IgnoreDefaultBehaviour = true;
        }
        public override float Ease(float x)
        {
            return x == 0
  ? 0
  : x == 1
  ? 1
  : x < 0.5 ? MathF.Pow(2, 20 * x - 10) / 2
  : (2 - MathF.Pow(2, -20 * x + 10)) / 2;
        }
        float lerpProg = 1, swingProgress, rotation;
        bool hitNPC;
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
                    SoundEngine.PlaySound(SoundID.Item70, Projectile.Center);
                    for (int i = 0; i < 5; i++)
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), TRay.Cast(Projectile.Center - Vector2.UnitY * 30, Vector2.UnitY, 500, true), Vector2.Zero, ModContent.ProjectileType<LimestoneDebris>(), Projectile.damage / 15, Projectile.knockBack, Projectile.owner);

                    int rand = Main.rand.Next(3, 6);
                    for (int i = 0; i < 8; i++)
                    {
                        Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Marble, Projectile.velocity.X * Main.rand.NextFloat(), Projectile.velocity.X * -Main.rand.NextFloat());
                        if (i > rand)
                        {
                            Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.DesertPot, Projectile.velocity.X * Main.rand.NextFloat(), Projectile.velocity.X * -Main.rand.NextFloat());
                            Gore.NewGorePerfect(null, Projectile.Center, new Vector2(Main.rand.NextFloat(1, 6f) * player.direction, Main.rand.NextFloat(-3, -1)), Main.rand.Next(61, 64));
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
        }
        public override bool? CanDamage()
        {
            return Projectile.ai[0] < 1 && swingProgress > 0.35f && swingProgress < 0.65f;
        }
        public override void OnHit(NPC target, NPC.HitInfo hit, int damageDone)
        {
            CameraSystem.ScreenShakeAmount = 2;
            lerpProg = -.1f;
            hitNPC = true;
        }
    }
}
