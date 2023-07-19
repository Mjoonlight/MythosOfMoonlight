using System;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.ID;
using MythosOfMoonlight.Projectiles;
using Terraria.GameContent;
using Terraria.Audio;

namespace MythosOfMoonlight.Items.Weapons.Melee
{
    public class Hoe : ModItem
    {
        public override void SetDefaults()
        {
            Item.knockBack = 10f;
            Item.width = Item.height = 58;
            Item.crit = 10;
            Item.damage = 10;
            Item.useAnimation = 32;
            Item.useTime = 32;
            Item.noUseGraphic = true;
            Item.autoReuse = false;
            Item.noMelee = true;
            Item.channel = true;
            //Item.reuseDelay = 45;
            Item.DamageType = DamageClass.Melee;
            //Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Green;
            Item.shootSpeed = 1f;
            Item.shoot = ModContent.ProjectileType<HoeP>();

            Item.value = Item.buyPrice(0, 1, 50, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0, -player.direction);
            return false;
        }
    }
    public class HoeP : ModProjectile
    {
        public int swingTime = 20;
        public float holdOffset = 35f;
        public override void SetDefaults()
        {
            swingTime = 32;
            Projectile.Size = new(52, 42);
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = swingTime;
            Projectile.timeLeft = swingTime;
        }
        public float Ease(float x)
        {
            return (float)(x == 0
  ? 0
  : x == 1
  ? 1
  : x < 0.5 ? Math.Pow(2, 20 * x - 10) / 2
  : (2 - Math.Pow(2, -20 * x + 10)) / 2);
        }
        public Player.CompositeArmStretchAmount stretch = Player.CompositeArmStretchAmount.Full;
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            float swingProgress = Ease(Utils.GetLerpValue(0f, swingTime, Projectile.timeLeft));
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 orig = texture.Size() / 2;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), lightColor, Projectile.rotation + (player.direction == 1 ? 0 : MathHelper.PiOver2 * 3), orig, Projectile.scale, player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
            return false;
        }
        public override void AI()
        {

            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead || player.CCed || player.noItems)
            {
                return;
            }
            if (Projectile.ai[1] != 0)
            {
                int direction = (int)Projectile.ai[1];
                float swingProgress = Ease(Utils.GetLerpValue(0f, swingTime, Projectile.timeLeft));
                //if (swingProgress > 0.35f && swingProgress < 0.75f)
                if (Projectile.ai[0] == 0 && TRay.CastLength(Projectile.Center, Vector2.UnitY, 100) < 7)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), TRay.Cast(player.Center - Vector2.UnitY * 20, Vector2.UnitY, 300), player.direction * Vector2.UnitX, ModContent.ProjectileType<HoeP2>(), Projectile.damage / 2, 0, Projectile.owner);
                    Projectile.ai[0] = 1;
                }
                float defRot = Projectile.velocity.ToRotation();
                float start = defRot - (MathHelper.PiOver2 + MathHelper.PiOver4);
                float end = defRot + (MathHelper.PiOver2 + MathHelper.PiOver4);
                float rotation = direction == 1 ? start + MathHelper.Pi * 3 / 2 * swingProgress : end - MathHelper.Pi * 3 / 2 * swingProgress;
                Vector2 position = player.GetFrontHandPosition(stretch, rotation - MathHelper.PiOver2) +
                    rotation.ToRotationVector2() * holdOffset * ScaleFunction(swingProgress);
                Projectile.Center = position;
                Projectile.rotation = (position - player.Center).ToRotation() + MathHelper.PiOver4;
                //player.ChangeDir(Projectile.velocity.X < 0 ? -1 : 1);
                player.heldProj = Projectile.whoAmI;
                player.SetCompositeArmFront(true, stretch, rotation - (MathHelper.PiOver4 / 3.5f * player.direction) - MathHelper.PiOver2);
            }
            if (player.itemTime < 2)
            {
                player.itemTime = 2;
                player.itemAnimation = 2;
            }
        }
        public virtual float ScaleFunction(float progress)
        {
            return 0.7f + (float)Math.Sin(progress * Math.PI) * 0.5f;
        }
        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(SoundID.Item1);
        }
    }
}
