using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MythosOfMoonlight.Items.CenturySet;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using MythosOfMoonlight.Projectiles;
using MythosOfMoonlight.Dusts;
using MythosOfMoonlight.NPCs.Enemies.Overworld;
using MythosOfMoonlight.NPCs.Enemies.Overworld.CenturyFlower.CenturyFlowerSpore;

namespace MythosOfMoonlight.NPCs.Enemies.Snow
{
    public class BorealFlower : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chilly Century Flower");
            Main.npcFrameCount[NPC.type] = 15;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
                new FlavorTextBestiaryInfoElement("The snowy plains freeze the body and calm the mind. Some creatures find peace in this, strengthening the mind and awakening once slumbering powers, such is the case for this Century Flower.")
            });
        }
        public override void SetDefaults()
        {
            NPC.width = 30;
            NPC.height = 70;
            NPC.damage = 12;
            NPC.lifeMax = 50;
            NPC.defense = 5;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath32;
            NPC.buffImmune[BuffID.Frostburn] = true;
            NPC.aiStyle = -1;
            NPC.netAlways = true;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * bossLifeScale);
        }
        public float AIState
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }
        public float AITimer
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }
        public float AITimer2
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }
        public float AITimer3
        {
            get => NPC.ai[3];
            set => NPC.ai[3] = value;
        }
        const int Move = 0, Plant = 1;
        public override void FindFrame(int frameHeight)
        {
            if (AIState == Move || NPC.IsABestiaryIconDummy)
            {
                if (NPC.velocity.Y > 1 || NPC.velocity.Y < -3)
                    NPC.frame.Y = 0 * NPC.height;
                else
                {
                    NPC.frameCounter++;
                    if (NPC.frameCounter % 5 == 0)
                    {
                        if (NPC.frame.Y < 4 * NPC.height)
                            NPC.frame.Y += NPC.height;
                        else
                            NPC.frame.Y = 0 * NPC.height;
                    }
                }
            }
            else if (AIState == Plant)
            {
                if (NPC.frame.Y < 5 * NPC.height)
                    NPC.frame.Y = 5 * NPC.height;
                NPC.frameCounter++;
                if (NPC.frameCounter % 5 == 0)
                {
                    if (NPC.frame.Y < 14 * NPC.height)
                        NPC.frame.Y += NPC.height;
                    else
                    {
                        AITimer = 0;
                        AIState = Move;
                    }
                }
            }
        }
        public override bool? CanFallThroughPlatforms()
        {
            Player player = Main.player[NPC.target];
            return player.Center.Y > NPC.Center.Y;
        }
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            NPC.direction = player.Center.X > NPC.Center.X ? 1 : -1;
            NPC.spriteDirection = NPC.direction;
            if (AIState == Move)
            {
                NPC.knockBackResist = 0.8f;
                AITimer++;
                NPC.GetGlobalNPC<FighterGlobalAI>().FighterAI(NPC, 6, 1, true, 1);
                if (AITimer >= 300)
                {
                    AITimer = 0;
                    AIState = Plant;
                    NPC.velocity = Vector2.Zero;
                }
            }
            else if (AIState == Plant)
            {
                NPC.velocity.X *= 0;
                NPC.knockBackResist = 0f;
                if (NPC.frame.Y == 12 * NPC.height && AITimer == 0)
                {
                    AITimer = 1;
                    for (int i = 0; i < 5; i++)
                    {
                        Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center - new Vector2(1, 19), Main.rand.NextVector2Unit() * 2, ModContent.ProjectileType<ChillyCenturyFlowerSpore>(), 0, 0, NPC.target);
                    }
                }
            }
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 4; i++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Frost, 2 * hitDirection, -1.5f);
            }
            if (NPC.life <= 0)
            {
                if (Main.netMode == NetmodeID.Server)
                    return;

                Helper.SpawnGore(NPC, "MythosOfMoonlight/BorealFlower", 4, 1, Vector2.One * hitDirection);
                Helper.SpawnGore(NPC, "MythosOfMoonlight/BorealFlower", 2, 2, Vector2.One * hitDirection);
                Helper.SpawnGore(NPC, "MythosOfMoonlight/BorealFlower", 2, 3, Vector2.One * hitDirection);
                Helper.SpawnGore(NPC, "MythosOfMoonlight/BorealFlower", 2, 4, Vector2.One * hitDirection);
                Helper.SpawnDust(NPC.position, NPC.Size, DustID.Frost, new Vector2(2 * hitDirection, -1.5f), 10);
                /*for (int i = 0; i < 10; i++)
				{
					int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Grass, 2 * hitDirection, -1.5f);
				}*/
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneSnow)
                return SpawnCondition.OverworldDay.Chance * 0.2f;
            return 0;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CenturySprayer>(), 50));
        }
    }
}