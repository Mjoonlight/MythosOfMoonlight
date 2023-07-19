using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using MythosOfMoonlight.Dusts;
using Microsoft.Xna.Framework;

namespace MythosOfMoonlight.Items.Jungle
{
    public class PlantGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Spitvine");
        }
        public override void SetDefaults()
        {
            Item.damage = 4;
            Item.DamageType = DamageClass.Ranged;
            Item.noUseGraphic = false;
            Item.noMelee = true;
            Item.knockBack = 1.5f;
            Item.width = 46;
            Item.height = 22;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.reuseDelay = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAmmo = AmmoID.Bullet;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 0, 0, 1);
            Item.rare = ItemRarityID.Green;
            Item.shoot = ModContent.ProjectileType<PlantGunP>();
            Item.shootSpeed = 7f;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position += (Item.Size / 2).RotatedBy(velocity.ToRotation());
            type = ModContent.ProjectileType<PlantGunP>();
        }
    }
    public class PlantGunP : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/Textures/Extra/blank";
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Bullet);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 4; i++)
            {
                Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center, new Vector2(-Main.rand.NextFloat(-5, 5), -7), ModContent.ProjectileType<PlantGunP2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
            target.AddBuff(ModContent.BuffType<PlantGunB>(), 300);
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 3; i++)
            {
                Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center, -Projectile.velocity - Main.rand.NextVector2Unit(-Projectile.velocity.ToRotation(), 1), ModContent.ProjectileType<PlantGunP2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
        }
        public override void AI()
        {
            Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<JunglePinkDust>(), Vector2.Zero, Scale: 2).noGravity = true;
            //Dust.NewDustPerfect(Projectile.Center, DustID.GreenTorch, Vector2.Zero, Scale: 2).noGravity = true;
        }
    }
    public class PlantGunP2 : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/Textures/Extra/blank";
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<PlantGunB>(), 300);
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
            Projectile.tileCollide = true;
            Projectile.aiStyle = 1;
            Projectile.penetrate = 2;
        }
        public override void AI()
        {
            Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<JunglePinkDust>(), Vector2.Zero).noGravity = true;
            //Dust.NewDustPerfect(Projectile.Center, DustID.GreenTorch, Vector2.Zero).noGravity = true;
        }
    }
    public class PlantGunB : ModBuff
    {
        public override string Texture => "MythosOfMoonlight/Textures/Extra/blank";
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsAnNPCWhipDebuff[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<PlantGunNPC>().planted = true;
        }
    }
    public class PlantGunNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool planted;
        public override void ResetEffects(NPC npc)
        {
            planted = false;
        }
        public override void PostAI(NPC npc)
        {
            if (planted)
                Lighting.AddLight(npc.Center, new Vector3(.19f, .08f, .11f));
        }
        public override Color? GetAlpha(NPC npc, Color drawColor)
        {
            if (planted)
                return Color.HotPink;
            return base.GetAlpha(npc, drawColor);
        }
    }
}
