using MythosOfMoonlight.Projectiles.MortJavelin;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using MythosOfMoonlight.NPCs.Enemies.EntropicTotem.EntropicTotemProjectile;
using MythosOfMoonlight.Dusts;
using Microsoft.Xna.Framework.Graphics;

namespace MythosOfMoonlight.Items.Weapons.Melee
{
    public class TotemsHead : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Totem Head");
            Tooltip.SetDefault("Creates a rotating spark on contact with enemies.");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Shroomerang);
            Item.Size = new Vector2(18, 42);
            Item.damage = 12;
            Item.shoot = ModContent.ProjectileType<TotemsHeadP>();
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] == 0;
        }
    }
    public class TotemsHeadP : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/Items/Weapons/Melee/TotemsHead";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Shroomerang);
            Projectile.Size = new Vector2(18, 42);
        }
        int b;
        public override void AI()
        {
            var dustType = ModContent.DustType<EntropicTotemProjectileDust>();
            Dust.NewDustPerfect(Projectile.Center, dustType, Projectile.velocity, Scale: 0.9f);
        }
        /*public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Main.spriteBatch.Reload(BlendState.Additive);
            Texture2D glow = ModContent.Request<Texture2D>("MythosOfMoonlight/Items/Weapons/Melee/TotemsHead_Gloww").Value;
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                Main.EntitySpriteDraw(glow, Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2f, Projectile.height / 2f), new Rectangle(0, 0, glow.Width, glow.Height), Color.DarkSlateGray * (1f - fadeMult * i), Projectile.oldRot[i], glow.Size() / 2, Projectile.scale * (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type], SpriteEffects.None, 0);
            }

            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return true;
        }*/
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (b == 0)
            {
                Projectile a = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.One, ModContent.ProjectileType<EntropicTotemProjectile2>(), Projectile.damage / 3, Projectile.knockBack, Projectile.owner);
                a.ai[0] = 10f;
                b = 1;
            }
        }
    }
    public class EntropicTotemProjectile2 : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/NPCs/Enemies/EntropicTotem/EntropicTotemProjectile/EntropicTotemProjectile";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Totem Bullet");
        }
        public const int MAX_TIMELEFT = 35;
        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 10;
            Projectile.aiStyle = 0;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.damage = 42;
            Projectile.tileCollide = false;
            Projectile.timeLeft = MAX_TIMELEFT;
        }
        float RotationalIncrement => MathHelper.ToRadians(Projectile.ai[0]);
        public override void AI()
        {
            var dustType = ModContent.DustType<EntropicTotemProjectileDust>();
            Dust.NewDustPerfect(Projectile.Center, dustType, -Projectile.velocity);

            Projectile.velocity = Projectile.velocity.RotatedBy(RotationalIncrement);
        }
    }
}
