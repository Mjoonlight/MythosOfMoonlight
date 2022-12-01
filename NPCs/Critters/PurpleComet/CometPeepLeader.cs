using System;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.Dusts;
using MythosOfMoonlight.Items.Critters;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs.Critters.PurpleComet
{
    public class CometPeepLeader : CometPeep
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers(0) { Hide = true, });
        }
        public override string Texture => "MythosOfMoonlight/NPCs/Critters/PurpleComet/CometPeep";
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return !PurpleCometEvent.PurpleComet ? 0 : 0.17f;
        }
        public override void PostAI()
        {
            /*firstFrame = true;
            int normalType = ModContent.NPCType<CometPeep>();
            // float posYFix = 11.0844980469f;
            for (int i = 1; i < Main.rand.Next(2, 4); i++)
            {
                var newNPC = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X + NPC.width * i * 2 * -NPC.direction, (int)NPC.Center.Y, normalType);

                Main.npc[newNPC].direction = npc.direction;
                Main.npc[newNPC].velocity = npc.velocity;
                var peep = Main.npc[newNPC].modNPC as CometPeep;
                peep.isCasual = false;
                Main.NewText(Main.npc[newNPC].position.Y - npc.position.Y);

            }*/
            CometPeep leader = Main.npc[NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<CometPeep>())].ModNPC as CometPeep;
            System.Collections.Generic.List<CometPeep> peeps = new System.Collections.Generic.List<CometPeep>();
            for (int a = 0; a < 4; a++)
            {
                float use = (float)(Math.PI / 4) * a;
                Point p = new Point((int)(40 * (float)Math.Cos(use)), (int)(40 * (float)Math.Sin(use)));
                peeps.Add(Main.npc[NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X + p.X, (int)NPC.Center.Y + p.Y, ModContent.NPCType<CometPeep>())].ModNPC as CometPeep);
            }
            int index = 0;
            foreach (CometPeep p in peeps)
            {
                index++;
                p.leader = leader;
                p.NPC.ai[1] = index;
            }
            leader.friends = peeps;
            NPC.active = false;
        }
    }
}
