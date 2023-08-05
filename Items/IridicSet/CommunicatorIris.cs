using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MythosOfMoonlight.Items.IridicSet
{
    public class CommunicatorIris : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Communicator-Iris");
            /* Tooltip.SetDefault("Minions become intertwined with Iridic flares, \n" +
                "occasionally releasing them when hitting an enemy. "); */
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D tex = Helper.GetTex(Texture + "_Glow");
            spriteBatch.Draw(tex, Item.Center - Main.screenPosition, null, Color.White, rotation, tex.Size() / 2, scale, SpriteEffects.None, 0);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MoMPlayer>().CommunicatorEquip = true;
        }
    }
}
