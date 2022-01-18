using System;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.Dusts;
using MythosOfMoonlight.Items.PurpleComet.Critters;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs.Critters.PurpleComet
{
    public class CometPeepLeader : CometPeep
    {
        public override string Texture => "MythosOfMoonlight/NPCs/Critters/PurpleComet/CometPeep";
        bool firstFrame;
        public override bool PreAI()
        {
            isCasual = false;
            return true;
        }
        public override void PostAI()
        {
            if (!firstFrame)
            {
                firstFrame = true;
                int normalType = ModContent.NPCType<CometPeep>();
                float posYFix = 11.0844980469f;
                for (int i = 1; i < Main.rand.Next(2, 4); i++)
                {
                    var newNPC = NPC.NewNPC((int)npc.Center.X + npc.width * i * 2 * -npc.direction, (int)(npc.Center.Y + posYFix), normalType);
                    Main.npc[newNPC].direction = npc.direction;
                    Main.npc[newNPC].velocity = npc.velocity;
                    var peep = Main.npc[newNPC].modNPC as CometPeep;
                    peep.isCasual = false;
                    Main.NewText(Main.npc[newNPC].position.Y - npc.position.Y);
                }
            }
        }
    }
}
