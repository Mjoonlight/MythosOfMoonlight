using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using MythosOfMoonlight.Dusts;
using Microsoft.Xna.Framework;
using Terraria.Utilities;
using Microsoft.Xna.Framework.Graphics;

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
            Item.damage = 1;
            Item.DamageType = DamageClass.Ranged;
            Item.noUseGraphic = false;
            Item.noMelee = true;
            Item.knockBack = 1.5f;
            Item.width = 46;
            Item.height = 22;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.reuseDelay = 10;
            Item.UseSound = SoundID.Item102;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAmmo = AmmoID.Bullet;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 0, 0, 1);
            Item.rare = ItemRarityID.Green;
            Item.shoot = ModContent.ProjectileType<PlantGunP>();
            Item.shootSpeed = 4f;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, 0);
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D tex = Helper.GetTex(Texture + "_Glow");
            spriteBatch.Draw(tex, Item.Center - Main.screenPosition, null, Color.White, rotation, tex.Size() / 2, scale, SpriteEffects.None, 0);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position += new Vector2(Item.width, 0).RotatedBy(velocity.ToRotation());
            type = ModContent.ProjectileType<PlantGunP>();
        }
    }
    public class PlantGunP : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/Textures/Extra/blank";
        public override void SetStaticDefaults()
        {
            Projectile.AddElement(CrossModHelper.Nature);
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Bullet);
            Projectile.light = 0f;
            Projectile.extraUpdates = 3;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 2; i++)
            {
                Vector2 vel = Main.rand.NextVector2Unit();
                float f = Main.rand.NextFloat(1, 5) * Main.rand.NextFloatDirection();
                Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center + -vel * 16, vel * 20, ModContent.ProjectileType<PlantGunP2>(), Projectile.damage, Projectile.knockBack, Projectile.owner, f, Main.rand.NextFloat(0.8f, 0.9f), -f);
            }
            //target.AddBuff(ModContent.BuffType<PlantGunB>(), 300);
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 vel = Main.rand.NextVector2Unit();
                float f = Main.rand.NextFloat(1, 5) * Main.rand.NextFloatDirection();
                Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center + -vel * 16, vel * 20, ModContent.ProjectileType<PlantGunP2>(), Projectile.damage, Projectile.knockBack, Projectile.owner, f, Main.rand.NextFloat(0.8f, 0.9f), -f);
            }
        }
        bool canFire;
        public override void AI()
        {
            if (Projectile.timeLeft % 30 == Projectile.ai[2])
            {
                float f = Main.rand.NextFloat(1, 5) * Main.rand.NextFloatDirection();
                if (Projectile.ai[2] < 15)
                    Projectile.ai[2] = Main.rand.Next(15, 30);
                else
                    Projectile.ai[2] = Main.rand.Next(15);

                Vector2 vel = Main.rand.NextVector2Unit();

                if (canFire)
                    Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center + -vel * 16, vel * 20, ModContent.ProjectileType<PlantGunP2>(), Projectile.damage, Projectile.knockBack, Projectile.owner, f, Main.rand.NextFloat(0.8f, 0.9f), -f);

                canFire = true;
            }

            if (--Projectile.ai[1] <= 0)
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && !npc.friendly)
                    {
                        if (npc.Center.X.CloseTo(Projectile.Center.X, npc.width + 40) && npc.Center.Y.CloseTo(Projectile.Center.Y, npc.height + 40))
                        {
                            float f = Main.rand.NextFloat(1, 5) * Main.rand.NextFloatDirection();
                            Vector2 vel = Helper.FromAToB(Projectile.Center, npc.Center);
                            Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center + -vel * 16, vel * 20, ModContent.ProjectileType<PlantGunP2>(), Projectile.damage, Projectile.knockBack, Projectile.owner, f, Main.rand.NextFloat(0.8f, 0.9f), -f);
                            Projectile.ai[1] = Main.rand.Next(20, 50);
                            break;
                        }
                    }
                }

            for (float i = 0; i < 1; i += 0.25f)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center + Projectile.velocity * i, ModContent.DustType<JunglePinkDust>(), Vector2.Zero, Scale: 2);
                d.noGravity = true;
                d.customData = 2;
            }
            //Dust.NewDustPerfect(Projectile.Center, DustID.GreenTorch, Vector2.Zero, Scale: 2).noGravity = true;
        }
    }
    public class PlantGunP2 : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/Textures/Extra/blank";
        public override void SetStaticDefaults()
        {
            Projectile.AddElement(CrossModHelper.Nature);
            ProjectileID.Sets.TrailCacheLength[Type] = 30;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //target.AddBuff(ModContent.BuffType<PlantGunB>(), 300);
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
            Projectile.aiStyle = 1;
            Projectile.penetrate = 2;
        }
        public override void AI()
        {
            if (Projectile.oldPos.Length > 0 && Projectile.velocity.Length() > 1)
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    float s = MathHelper.Lerp(0.01f, 1.8f, (float)i / Projectile.oldPos.Length);

                    Dust d = Dust.NewDustPerfect(Projectile.oldPos[i] + Projectile.Size / 2, ModContent.DustType<JunglePinkDust>(), Vector2.Zero, Scale: s);
                    d.noGravity = true;
                    d.customData = 1;

                    if (i > 0)
                        for (float j = 0; j < 1; j += 0.25f)
                        {
                            Dust d2 = Dust.NewDustPerfect(Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[i - 1], j) + Projectile.Size / 2, ModContent.DustType<JunglePinkDust>(), Vector2.Zero, Scale: s);
                            d2.noGravity = true;
                            d2.customData = 1;
                        }
                }
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Projectile.ai[0])) * Projectile.ai[1];
            Projectile.ai[0] = MathHelper.Lerp(Projectile.ai[0], 0, 0.1f);
            //Dust.NewDustPerfect(Projectile.Center, DustID.GreenTorch, Vector2.Zero).noGravity = true;
        }
    }
    public class PlantGunB : ModBuff
    {
        public override string Texture => "MythosOfMoonlight/Textures/Extra/blank";
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
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
