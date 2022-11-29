using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MythosOfMoonlight.Dusts;
using Terraria.GameContent;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;
using System.IO;
using MythosOfMoonlight.Gores.Enemies;
using Terraria.Audio;

namespace MythosOfMoonlight.NPCs.Enemies.Jungle.Vivine
{ 
    public class Vivine : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 10;
        }
        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 58;
            NPC.lifeMax = 120;
            NPC.defense = 5;
            NPC.damage = 15;
            NPC.knockBackResist = 0.8f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.aiStyle = -1;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle,
                new FlavorTextBestiaryInfoElement("Stupid Dumb PLant Idiot I hate you.")
            });
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
        const int Idle = 0, Move = 1, Spit = 2;
        public override void FindFrame(int frameHeight)
        {
            if (!NPC.IsABestiaryIconDummy)
            {
                if (NPC.velocity.Y > .1f || NPC.velocity.Y < -.1f)
                {
                    NPC.frameCounter = 1;
                    NPC.frame.Y = 4 * NPC.height;
                }
                if (AIState == Idle)
                    NPC.frame.Y = 0;
                else if (AIState == Move)
                {
                    NPC.frameCounter++;
                    if (NPC.frameCounter % 5 == 0)
                    {
                        if (NPC.frame.Y < 4 * NPC.height)
                            NPC.frame.Y += NPC.height;
                        else
                            NPC.frame.Y = 2 * NPC.height;
                    }
                }
                else if (AIState == Spit)
                {
                    if (NPC.frame.Y < 5 * NPC.height)
                        NPC.frame.Y = 5 * NPC.height;
                    NPC.frameCounter++;
                    if (NPC.frameCounter % 5 == 0)
                    {
                        if (NPC.frame.Y < 9 * NPC.height)
                            NPC.frame.Y += NPC.height;
                        else
                        {
                            AITimer = 0;
                            AIState = Idle;
                        }
                    }
                }
            }
            else
            {
                NPC.frameCounter++;
                if (NPC.frameCounter % 5 == 0)
                {
                    if (NPC.frame.Y < 7 * NPC.height)
                        NPC.frame.Y += NPC.height;
                    else
                        NPC.frame.Y = 2 * NPC.height;
                }
            }
        }
        public override void HitEffect(int hitDirection, double damage)
        {   
            Helper.SpawnDust(NPC.position, NPC.Size, DustID.Grass, new Vector2(2 * hitDirection, -1.5f), 4);
            Helper.SpawnDust(NPC.position, NPC.Size, ModContent.DustType<JunglePinkDust>(), Vector2.One * hitDirection * 2, 4);
            if (NPC.life <= 0)
            {
                Helper.SpawnDust(NPC.position, NPC.Size, DustID.Grass, new Vector2(2 * hitDirection, -1.5f), 8);
                Helper.SpawnGore(NPC, "MythosOfMoonlight/Vivine", 2, 1, Vector2.One * hitDirection * 2);
                Helper.SpawnGore(NPC, "MythosOfMoonlight/Vivine", 1, 2, Vector2.One * hitDirection * 2);
                Helper.SpawnGore(NPC, "MythosOfMoonlight/Vivine", 3, 3, Vector2.One * hitDirection * 2);
                Helper.SpawnDust(NPC.position, NPC.Size, ModContent.DustType<JunglePinkDust>(), Vector2.One * hitDirection * 2, 8);
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
                return SpawnCondition.UndergroundJungle.Chance * 0.2f;
        }
        public override bool? CanFallThroughPlatforms()
        {
            Player player = Main.player[NPC.target];
            return player.Center.Y > NPC.Center.Y;
        }
        public override void AI()
        {
            Lighting.AddLight(NPC.Center, new Vector3(19, 8, 11) * 0.3f);
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            NPC.direction = player.Center.X > NPC.Center.X ? 1 : -1;
            NPC.spriteDirection = NPC.direction;
            if (AIState == Idle)
            {
                NPC.velocity.X *= .9f;
                NPC.knockBackResist = 0.8f;
                AITimer++;
                if (AITimer >= 100)
                {
                    AITimer = 0;
                    AIState = Move;
                    NPC.frame.Y = NPC.height * 2;
                }
            }
            else if (AIState == Move)
            {
                NPC.knockBackResist = 0.8f;
                AITimer++;
                NPC.GetGlobalNPC<FighterGlobalAI>().FighterAI(NPC, 6, 1, true, -1, 0/*, 1, 0*/);
                if (AITimer >= 400)
                {
                    AITimer = 0;
                    AIState = Spit;
                    NPC.frame.Y = NPC.height * 8;
                    NPC.velocity = Vector2.Zero;
                }
            }
            else if (AIState == Spit)
            {
                NPC.velocity.X *= 0;
                NPC.knockBackResist = 0f;
                if (NPC.frame.Y == 15 * NPC.height && AITimer == 0)
                {
                    AITimer = 1;
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - new Vector2(1, 19), Vector2.Normalize(Main.player[NPC.target].Center + new Vector2(0, -150) - NPC.Center) * 10f, ModContent.ProjectileType<VivineSpit>(), 0, 0);
                    }
                }
            }
        }
    }
    public class VivineSpit : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.Size = Vector2.One * 10;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = true;
            Projectile.aiStyle = 1;
            Projectile.timeLeft = 300;
        }
        public override void Kill(int timeleft)
        {
            Helper.SpawnDust(Projectile.Center, Projectile.Size, ModContent.DustType<JunglePinkDust>(), Projectile.velocity * .5f, 8);
        }
    }
}