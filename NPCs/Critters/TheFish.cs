using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace MythosOfMoonlight.NPCs.Critters
{
    public class TheFish : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Yellow Boxfish");
            Main.npcFrameCount[NPC.type] = 15;
            NPCID.Sets.CountsAsCritter[Type] = true;
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new(0) { Velocity = 1 };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = 16;
            AIType = NPCID.Goldfish;
            NPC.lifeMax = 5;
            NPC.noGravity = true;
            NPC.width = 22;
            NPC.height = 20;
            NPC.defense = 0;
            NPC.catchItem = ModContent.ItemType<Items.Critters.TheFishItem>();
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.dontCountMe = true;
            NPC.npcSlots = 0;
            NPC.dontTakeDamageFromHostiles = false; 
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
                new FlavorTextBestiaryInfoElement("A cubical fish whose entire existence is dedicated to perpetually swimming through the deep blue sea.")
            });
        }
        public override void FindFrame(int frameHeight)
        {

            NPC.frameCounter++;
            if (NPC.wet || NPC.IsABestiaryIconDummy) 
            {
                if (NPC.frameCounter % 5 == 0)
                {
                    if (NPC.frame.Y < 14 * NPC.height)
                        NPC.frame.Y += NPC.height;
                    else
                        NPC.frame.Y = 0 * NPC.height;
                }
            }
            else
            {
                NPC.frame.Y = 0;
            }
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int i = 0; i < 6; i++)
            {
                int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood , 2 * hit.HitDirection, -1.5f);
                Main.dust[dust].scale = 1f;
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Water)
                return SpawnCondition.Ocean.Chance * 0.1f;
            return 0;
        }
    }
}