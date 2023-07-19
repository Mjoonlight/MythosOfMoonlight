using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MythosOfMoonlight.Items.ShadowHex
{
    [AutoloadEquip(EquipType.Head)]
    public class ShadowHexHead : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Shadow-Hexer Skull");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 32;
            Item.value = 15000;
            Item.rare = ItemRarityID.Green;
            Item.vanity = true;
        }
    }
    public class ShadowHexLayer : PlayerDrawLayer
    {
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, "ShadowHexHead", EquipType.Head);
        }

        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.Head);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;

            Texture2D texture = ModContent.Request<Texture2D>("MythosOfMoonlight/Items/ShadowHex/ShadowHexHead_Head").Value;

            Vector2 headPosition = drawInfo.helmetOffset +
                new Vector2(
                    (int)(drawInfo.Position.X - (float)(drawInfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawInfo.drawPlayer.width / 2)),
                    (int)(drawInfo.Position.Y + (float)drawInfo.drawPlayer.height - (float)drawInfo.drawPlayer.bodyFrame.Height + 4f))
                + drawInfo.drawPlayer.headPosition
                + drawInfo.headVect;

            headPosition -= Main.screenPosition;

            Vector2 origin = drawInfo.headVect;

            Rectangle headFrame = drawPlayer.bodyFrame;

            DrawData data = new(texture, headPosition, headFrame, drawInfo.colorArmorHead, drawInfo.drawPlayer.headRotation, origin, 1f, drawInfo.playerEffect, 0)
            {
                shader = drawInfo.cHead
            };

            drawInfo.DrawDataCache.Add(data);

            texture = ModContent.Request<Texture2D>("MythosOfMoonlight/Items/ShadowHex/ShadowHexHead_Glow").Value;

            data = new(texture, headPosition, headFrame, Color.White, drawInfo.drawPlayer.headRotation, origin, 1f, drawInfo.playerEffect, 0)
            {
                shader = drawInfo.cHead
            };

            drawInfo.DrawDataCache.Add(data);
        }
    }
}