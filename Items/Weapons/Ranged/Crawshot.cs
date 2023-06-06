using Microsoft.Xna.Framework;
using RealmOne.Items.Weapons.PreHM.Throwing;
using RealmOne.Projectiles.Bullet;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
namespace RealmOne.Items.Weapons
{
	public class Crawshot : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crawshot"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("Fires a bullet from each barrel with a bit of innacuracy");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

		}

		public override void SetDefaults()
		{
			Item.damage = 10;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 15;
			Item.height = 15;
			Item.useTime = 26;
			Item.useAnimation = 26;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 0.87f;
			Item.value = Item.sellPrice(silver:65);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item11;
			Item.autoReuse = true;
			Item.useAmmo = AmmoID.Bullet;
			Item.noMelee = true;
			Item.shootSpeed = 55f;

			Item.shoot = ProjectileID.Bullet;

		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 25f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
                position += muzzleOffset;

            float numberProjectiles = 3 + Main.rand.Next(1); // 3, 4, or 5 shots
			float rotation = MathHelper.ToRadians(7);
			position += Vector2.Normalize(velocity) * -2f;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .5f; // Watch out for dividing by 0 if there is only 1 projectile.
				Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
			}

            for (int i = 0; i < 60; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(0.5f, 0.5f);
                var d = Dust.NewDustPerfect(Main.LocalPlayer.Center, DustID.Water, speed * 4, Scale: 1f);
                ;
                d.noGravity = true;
            }
            return false;
		}

        public override bool IsQuestFish()
        {
            return true; 
        }
        
        public override bool IsAnglerQuestAvailable()
        {
            return !Main.hardMode; 
        }

        public override void AnglerQuestChat(ref string description, ref string catchLocation)
        {
            description = "I'm no expert with guns and weaponry, but I'm positive I saw a lil' crustacean with a couple of gun barrels stuck to its mouth! Can you catch this so i dont have to worry about being shot by a lobster?";
            catchLocation = "Caught in the ocean.";
        }

        public override Vector2? HoldoutOffset()
		{
			var offset = new Vector2(0, 0);
			return offset;
		}
	}
class CrawshotChestSpawn: ModSystem
	{
        public override void PostWorldGen()
        {
            int[] waterchest = { ItemType<Crawshot>() };
            int waterchestchoice = 0;
            for (int WchestIndex = 0; WchestIndex < 1000; WchestIndex++)

            {

                Chest Wchest = Main.chest[WchestIndex];
                if (Wchest != null && Main.tile[Wchest.x, Wchest.y].TileType == TileID.Containers && Main.tile[Wchest.x, Wchest.y].TileFrameX == 17 * 36)
                {

                    for (int WinventoryIndex = 0; WinventoryIndex < 40; WinventoryIndex++)
                    {

                        if (Wchest.item[WinventoryIndex].type == ItemID.None)
                        {

                            Wchest.item[WinventoryIndex].SetDefaults(waterchest[waterchestchoice]);

                            Wchest.item[WinventoryIndex].stack = WorldGen.genRand.Next(0, 1);

                            waterchestchoice = (waterchestchoice + 1) % waterchest.Length;
                            //Wchest.item[WinventoryIndex].SetDefaults(Main.rand.Next(WinventoryIndex));
                            break;
                        }
                    }
                }
            }
        }
    }
}