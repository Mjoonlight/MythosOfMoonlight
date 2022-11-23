using Microsoft.Xna.Framework;
using MythosOfMoonlight.BiomeManager;
using MythosOfMoonlight.Dusts;
using MythosOfMoonlight.Items.PurpleComet.Critters;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs.Critters.PurpleComet
{
    public class SparkleSkittler : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sparkle Skittler");
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.CountsAsCritter[Type] = true;
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new(0) { Velocity = 1 };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 5;
            NPC.width = 26;
            NPC.height = 20;
            NPC.defense = 0;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.catchItem = (short)ModContent.ItemType<SparkleSkittlerItem>();
            NPC.dontCountMe = true;
            NPC.npcSlots = 0;
            NPC.dontTakeDamageFromHostiles = false; 
            SpawnModBiomes = new int[]
            {
                ModContent.GetInstance<PurpleCometBiome>().Type
            };
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new BestiaryPortraitBackgroundProviderPreferenceInfoElement(ModContent.GetInstance<PurpleCometBiome>().ModBiomeBestiaryInfoElement),
                new FlavorTextBestiaryInfoElement("By absorbing natural cosmic energy around it, a Sparkle Skittler can make quick dashes away from potential predators, although it doesn¡¯t always have control over which direction it goes.")
            });
        }
        const float SPEED = 3.5f;
        const int TRANSITION_CHANCE = 99;
        int State
        {
            get => (int)NPC.ai[0];
            set => NPC.ai[0] = value;
        }
        public override void AI()
        {
            Collision.StepUp(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed, ref NPC.gfxOffY, 1, false, 0);
            switch (State)
            {
                case 0:
                    if (NPC.direction == 0)
                    {
                        NPC.direction = Main.rand.NextBool(2) ? 1 : -1;
                    }
                    var dustPosOffset = new Vector2(1 * NPC.direction, 5);
                    var dustPosition = NPC.Center + dustPosOffset;
                    var dust = Dust.NewDustPerfect(dustPosition, ModContent.DustType<PurpurineDust>(), default, 0, default, 1f); // 10, 9
                    dust.noGravity = true;
                    dust.velocity = Vector2.Zero;

                    if (Main.rand.NextBool(TRANSITION_CHANCE))
                    {
                        NPC.velocity.X = 0;
                        State = 1;
                    }
                    else if (NPC.velocity.X == 0)
                        NPC.direction = -NPC.direction;
                    NPC.velocity.X = NPC.direction * SPEED;
                    break;
                case 1:
                    NPC.velocity.X = 0;
                    if (Main.rand.NextBool(TRANSITION_CHANCE))
                    {
                        NPC.direction = 0;
                        State = 0;
                    }
                    break;
            }
            NPC.spriteDirection = NPC.direction;
        }
        const int FRAME_RATE = 3;
        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 2; i++)
            {
                int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<PurpurineDust>(), 2 * hitDirection, -1.5f);
                Main.dust[dust].scale = 1f;
            }
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<PurpurineDust>(), 2 * hitDirection, -1.5f);
                    Main.dust[dust].scale = 1f;
                }

                if (Main.netMode == NetmodeID.Server)
                    return;

                for (int i = 0; i < 2; i++)
                    Gore.NewGore(NPC.GetSource_OnHit(NPC), NPC.Center + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), Vector2.Zero, ModContent.Find<ModGore>("MythosOfMoonlight/Purpurine").Type);
            }
        }
        public override void FindFrame(int frameHeight)
        {
            switch (State)
            {
                case 0:
                    if (NPC.frameCounter + 1 < FRAME_RATE * 4)
                    {
                        NPC.frameCounter++;
                        if (NPC.frameCounter < FRAME_RATE)
                            NPC.frameCounter = FRAME_RATE;
                    }
                    else
                        NPC.frameCounter = FRAME_RATE;
                    break;
                case 1:
                    NPC.frameCounter = 0;
                    break;
            }
            NPC.frame.Y = (int)NPC.frameCounter / FRAME_RATE * frameHeight;
        }
        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return !PurpleCometEvent.PurpleComet ? 0 : 0.17f;
        }
    }
}