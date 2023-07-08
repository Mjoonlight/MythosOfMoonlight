using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace MythosOfMoonlight.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class EntropicHead : ModItem
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Item.Size = new Vector2(36, 18);
            Item.rare = ItemRarityID.Green;
            Item.defense = 3;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 1, 50, 0);
        }
    }
    public class EntropicHeadL : PlayerDrawLayer
    {
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, "EntropicHead", EquipType.Head);
        }

        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.Head);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;

            Texture2D texture = ModContent.Request<Texture2D>("MythosOfMoonlight/Items/Armor/EntropicHead_Head").Value;

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

            texture = ModContent.Request<Texture2D>("MythosOfMoonlight/Items/Armor/EntropicHead_HeadGlow").Value;

            data = new(texture, headPosition, headFrame, Color.White, drawInfo.drawPlayer.headRotation, origin, 1f, drawInfo.playerEffect, 0)
            {
                shader = drawInfo.cHead
            };

            drawInfo.DrawDataCache.Add(data);
        }
    }
}
