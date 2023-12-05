using Microsoft.Xna.Framework;
using MythosOfMoonlight.Projectiles.SoulCandle;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Items.Weapons.Magic
{
    public class SoulCandle : ModItem
    {
        public override void SetStaticDefaults()
        {


            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }

        public override void SetDefaults()
        {
            Item.damage = 19;
            Item.DamageType = DamageClass.Magic;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.knockBack = 3;
            Item.value = 30000;
            Item.rare = 3;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ModContent.ProjectileType<SoulCandleHeld>();
            Item.shootSpeed = 1f;
            Item.noMelee = true;
            //Item.UseSound = SoundID.DD2_EtherianPortalSpawnEnemy;

            Item.channel = true;
            Item.noUseGraphic = true;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.WaterCandle).AddIngredient(ItemID.Bone, 10).AddTile(TileID.Anvils).Register();
        }
    }
}