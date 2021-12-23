using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs.Enemies.EntropicTotem
{
    public class EntropicTotem : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Entropic Totem");
            Main.npcFrameCount[npc.type] = 5;
        }
        public override void SetDefaults()
        {
            npc.width = 62;
            npc.height = 70;
            npc.aiStyle = -1;
            npc.damage = 15;
            npc.defense = 10;
            npc.lifeMax = 260;
            npc.noGravity = true;
            npc.noTileCollide = true;
        }

        const float SPEED = 6f, MINIMUM_DISTANCE = 160f;
        public override void AI()
        {
            npc.TargetClosest();
            var player = Main.player[npc.target];
            var horizontalDistance = Math.Abs(npc.Center.X - player.Center.X);
            if (horizontalDistance >= MINIMUM_DISTANCE)
            {
                var direction = -(npc.position - player.position).SafeNormalize(Vector2.UnitX);
                npc.velocity = Vector2.Lerp(npc.velocity, direction * SPEED, 0.03f);
            }
            npc.rotation = MathHelper.Clamp(npc.velocity.X * .25f, MathHelper.ToRadians(-30), MathHelper.ToRadians(30));
        }
    }
}
