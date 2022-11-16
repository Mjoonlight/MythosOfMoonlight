using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MythosOfMoonlight.Items.CenturySet;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;

namespace MythosOfMoonlight.NPCs.Enemies.CenturyFlower
{
    public class CenturyFlower : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Century Flower");
            Main.npcFrameCount[NPC.type] = 10;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
                new FlavorTextBestiaryInfoElement("These unique plants are capable of spewing large clouds of spores, dangerous to much of the wildlife. Their age makes them extremely irrational and ill-tempered, spewing their suffocating spores at anything they deem to be less wise.")
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
            NPC.buffImmune[BuffID.Poisoned] = true;
            NPC.aiStyle = 3;
            AIType = NPCID.GoblinScout;
            NPC.netAlways = true;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * bossLifeScale);
        }
        private void SetState(int newState)
        {
            NPC.ai[0] = newState;
            NPC.frameCounter = 0;
        }
        public override void FindFrame(int frameHeight)
        {
            if (!NPC.IsABestiaryIconDummy)
                NPC.frame.Y = GetFrame() * frameHeight;
            else
            {
                NPC.frameCounter++;
                if (NPC.frameCounter % 5 == 0)
                {
                    if (NPC.frame.Y < NPC.height * 4)
                        NPC.frame.Y += NPC.height;
                    else
                        NPC.frame.Y = 0;
                }
            }
        }
        private void SetFrame(int frame)
        {
            NPC.frame.Y = NPC.height * frame;
        }
        const float strideSpeed = 1f, jumpHeight = 7f;
        public override bool PreAI()
        {
            NPC.frameCounter++;
            if (Main.rand.NextFloat() <= .05f && NPC.frameCounter > 250 && NPC.ai[0] == 0)
            {
                RealFrame = ScaleFrame(5);
                SetState(1);
            }

            switch (NPC.ai[0])
            {
                case 1:
                    OpenPetals();
                    OpenPetalsAnimation();
                    break;
                default:
                    ManageMovement();
                    ManageMovementAnimation();
                    break;
            }
            return false;
        }
        const float animationSpdOffset = 4f;
        int GetFrame() => (int)(RealFrame / strideSpeed / animationSpdOffset);
        static int ScaleFrame(int frame) => (int)(animationSpdOffset * frame * strideSpeed); // returns value necessary for real frame to set animation frame to target frame frame
        float RealFrame
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }
        float Jump
        {
            get => NPC.ai[3];
            set => NPC.ai[3] = value;
        }
        void ManageMovement()
        {
            NPC.TargetClosest(false);
            Collision.StepUp(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed, ref NPC.gfxOffY, 1, false, 0);
            var player = Main.player[NPC.target];
            // var sqrDistance = player.DistanceSQ(NPC.position);
            if (NPC.collideX && NPC.frameCounter > 2 && Jump <= 0)
            {
                NPC.velocity.Y = -jumpHeight;
                Jump = 1;
            }
            if (Main.tile[NPC.Hitbox.Center.X / 16, NPC.Hitbox.Bottom / 16].HasTile)
            {
                if (Main.tile[NPC.Hitbox.Center.X / 16, NPC.Hitbox.Bottom / 16].LeftSlope || Main.tile[NPC.Hitbox.Center.X / 16, NPC.Hitbox.Bottom / 16].BottomSlope || Main.tile[NPC.Hitbox.Center.X / 16, NPC.Hitbox.Bottom / 16].RightSlope)
                {
                    NPC.velocity.Y = -jumpHeight;
                    Jump = 1;
                }
                else Jump = 0;
            }

            FitVelocityXToTarget(strideSpeed * NPC.direction);
            var horizontalDistance = Math.Abs(NPC.Center.X - player.Center.X);
            if (horizontalDistance >= 35.78f)
            {
                NPC.FaceTarget();
            }
            NPC.spriteDirection = NPC.direction;
        }

        void ManageMovementAnimation()
        {
            RealFrame++;
            if (NPC.velocity.Y != 0 || NPC.oldVelocity == Vector2.Zero || GetFrame() > 4 || GetFrame() < 0)
            {
                RealFrame = 3;
            }
        }

        public void OpenPetals()
        {
            NPC.knockBackResist = 0.5f;
            if (NPC.frameCounter == 1)
                NPC.velocity.X = 0;
            else if (NPC.frameCounter > 75 && NPC.frameCounter % 10 == 0)
            {
                if (NPC.frameCounter >= 150)
                {
                    RealFrame = ScaleFrame(4);
                    SetState(0);
                }
                else
                {
                    Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center - new Vector2(1, 19), Main.rand.NextVector2Unit() * 2, ModContent.ProjectileType<CenturyFlowerSpore.CenturyFlowerSpore>(), 0, 0);
                }
            }
            if (NPC.velocity.Y == 0)
                NPC.velocity.X *= 0.9f;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 4; i++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GrassBlades, 2 * hitDirection, -1.5f);
            }
            if (NPC.life <= 0)
            {
                if (Main.netMode == NetmodeID.Server)
                    return;

                Helper.SpawnGore(NPC, "MythosOfMoonlight/Century", 4, 1, Vector2.One * hitDirection);
                Helper.SpawnGore(NPC, "MythosOfMoonlight/Century", 2, 2, Vector2.One * hitDirection);
                Helper.SpawnGore(NPC, "MythosOfMoonlight/Century", 2, 3, Vector2.One * hitDirection);
                Helper.SpawnGore(NPC, "MythosOfMoonlight/Century", 2, 4, Vector2.One * hitDirection);
                Helper.SpawnGore(NPC, "MythosOfMoonlight/CenturyLeaf", 5, -1, Vector2.One * hitDirection);
                Helper.SpawnDust(NPC.position, NPC.Size, DustID.Grass, new Vector2(2 * hitDirection, -1.5f), 10);
                /*for (int i = 0; i < 10; i++)
				{
					int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Grass, 2 * hitDirection, -1.5f);
				}*/
            }
        }
        public void OpenPetalsAnimation()
        {
            if (NPC.frameCounter <= 75 && GetFrame() < 9)
                RealFrame++;
            else if (Helper.InRange(NPC.frameCounter, 140, 150) && GetFrame() > 5)
                RealFrame--;
        }

        void FitVelocityToTarget(Vector2 newVelocity) => NPC.velocity = Vector2.Lerp(NPC.velocity, newVelocity, .1f);
        void FitVelocityXToTarget(float newX) => NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, newX, 0.1f);
        void FitVelocityYToTarget(float newY) => NPC.velocity.Y = MathHelper.Lerp(NPC.velocity.Y, newY, 0.1f);
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZonePurity)
                return SpawnCondition.OverworldDay.Chance * 0.2f;
            return 0;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CenturySprayer>(), 20));
        }
    }
}