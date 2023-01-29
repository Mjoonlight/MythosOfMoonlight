using System;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.ID;
using MythosOfMoonlight.Projectiles;
using Terraria.GameContent;
using System.Runtime.InteropServices;

namespace MythosOfMoonlight.Items.Galactite
{
    public class Estrella : ModItem
    {
        public override void SetDefaults()
        {
            Item.knockBack = 10f;
            Item.width = Item.height = 76;
            Item.crit = 45;
            Item.damage = 34;
            Item.useAnimation = 32;
            Item.useTime = 32;
            Item.noUseGraphic = true;
            Item.autoReuse = false;
            Item.noMelee = true;
            Item.channel = true;
            //Item.reuseDelay = 45;
            Item.DamageType = DamageClass.Melee;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.LightRed;
            Item.shootSpeed = 1f;
            Item.shoot = ModContent.ProjectileType<EstrellaP>();
        }
        int dir = 1;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            dir = -dir;
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0, dir);
            return false;
        }
    }
    public class EstrellaP : HeldSword
    {
        public override string Texture => "MythosOfMoonlight/Items/Galactite/Estrella";
        public override string GlowTexture => "MythosOfMoonlight/Items/Galactite/Estrella_Glow";
        public override void SetExtraDefaults()
        {
            swingTime = 50;
            Projectile.Size = new(66);
            glowAlpha = 1f;
            BlendState _blendState = new BlendState();
            _blendState.AlphaSourceBlend = Blend.SourceAlpha;
            _blendState.AlphaDestinationBlend = Blend.InverseSourceAlpha;

            _blendState.ColorSourceBlend = Blend.SourceAlpha;
            _blendState.ColorDestinationBlend = Blend.InverseSourceAlpha;
            glowBlend = _blendState;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - Vector2.UnitY * 900, Vector2.UnitY * Main.rand.NextFloat(10, 20f), ModContent.ProjectileType<EstrellaP2>(), Projectile.damage, 0, Projectile.owner);
        }
    }
    public class EstrellaP2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 5;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 32;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
        }
        float alpha = 1;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            for (int i = 1; i < 5; i++)
            {
                float _scale = MathHelper.Lerp(1f, 0.95f, (float)(5 - i) / 5);
                var fadeMult = 1f / 5;
                Main.spriteBatch.Draw(tex, Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2, null, Color.Pink * (1f - fadeMult * i) * 0.5f * alpha, Projectile.oldRot[i], Projectile.Size / 2, _scale, SpriteEffects.None, 0f);
            }
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * alpha;
        }
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(3);
            if (Projectile.timeLeft < 20)
                alpha -= 0.05f;
        }
    }
}