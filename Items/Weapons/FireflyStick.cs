using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using System;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.Projectiles;
using Terraria.DataStructures;

namespace MythosOfMoonlight.Items.Weapons
{
    public class FireflyStick : ModItem
    {
        public int fireflyCount = 0;
        public int ownedFireflyCount = 0;
        public int closeFireflyCount = 0;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Firefly Stick");
            /* Tooltip.SetDefault("Launches loyal fireflies to the cursor and back\n" +
                "Increase their number by catching more fireflies"); */
            Item.ResearchUnlockCount = 1;
            Item.staff[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.holdStyle = ItemHoldStyleID.HoldFront;
            Item.width = 44;
            Item.mana = 0;
            Item.height = 50;
            Item.mana = 2;
            Item.useAnimation = 8;
            Item.useTime = 8;
            Item.knockBack = 2f;
            Item.damage = 8;
            Item.rare = ItemRarityID.Green;
            Item.DamageType = DamageClass.Magic;
            Item.noMelee = true;
            Item.noUseGraphic = false;
            Item.useTurn = false;
            Item.autoReuse = true;
            Item.shootSpeed = 0f;
            Item.shoot = ModContent.ProjectileType<FireflyMinion>();
        }
        public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
        {
            reduce = 2;
            mult = 0;
        }
        public override void HoldStyle(Player player, Rectangle heldItemFrame)
        {
            if (Main.myPlayer == player.whoAmI)
            {
                //player.direction = Main.MouseWorld.X >= player.Center.X ? 1 : -1;
                float itemRotation = -MathHelper.ToRadians(40) * player.gravDir;
                player.itemRotation = itemRotation * player.direction;
                Vector2 itemPosition = player.MountedCenter + new Vector2(0, 10 * player.gravDir) + Utils.ToRotationVector2(player.direction > 0 ? 0 : (float)Math.PI) * 3f;
                player.itemLocation = itemPosition;
            }
            fireflyCount = 0;
            for (int i = 0; i < player.inventory.Length; i++)
            {
                if (!player.inventory[i].IsAir)
                {
                    if (player.inventory[i].type == ItemID.Firefly)
                    {
                        fireflyCount += player.inventory[i].stack;
                    }
                }
            }
            fireflyCount /= 5;
            fireflyCount++;
            if (fireflyCount > 5) fireflyCount = 5;
            ownedFireflyCount = 0;
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                if (Main.projectile[i].owner == player.whoAmI)
                {
                    if (Main.projectile[i].active)
                    {
                        if (Main.projectile[i].type == Item.shoot)
                        {
                            ownedFireflyCount++;
                            if (ownedFireflyCount > fireflyCount) Main.projectile[i].Kill();
                        }
                    }
                }
            }
            if (ownedFireflyCount < fireflyCount)
            {
                int j = fireflyCount - ownedFireflyCount;
                for (int i = 1; i <= j; i++)
                {
                    int p = Projectile.NewProjectile(null, player.Center, Vector2.Zero, Item.shoot, Item.damage, Item.knockBack, player.whoAmI);
                    Main.projectile[p].CritChance = Item.crit;
                    Main.projectile[p].ArmorPenetration += 10;
                }
            }
        }
        public override void HoldItem(Player player)
        {
            Lighting.AddLight(player.MountedCenter + new Vector2(0, 20 * player.gravDir) + Utils.ToRotationVector2(player.direction > 0 ? 0 : (float)Math.PI) * 3f, new Vector3(.45f, .45f, .15f));
        }
        public override bool? UseItem(Player player)
        {
            if (player.CheckMana(5, false))
                for (int i = 0; i < Main.projectile.Length; i++)
                {
                    if (Main.projectile[i].owner == player.whoAmI)
                    {
                        if (Main.projectile[i].active)
                        {
                            if (Main.projectile[i].type == Item.shoot)
                            {
                                if (Main.projectile[i].ai[0] == 0)
                                {
                                    Main.projectile[i].ai[0] = 1;
                                    break;
                                }
                            }
                        }
                    }
                }
            player.manaRegenDelay = (int)player.maxRegenDelay;
            return base.UseItem(player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Wood, 20)
                .AddIngredient(ItemID.FallenStar)
                .AddIngredient(ItemID.FireflyinaBottle)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
