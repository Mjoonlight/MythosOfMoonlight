using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MythosOfMoonlight.Common.BiomeManager;
using MythosOfMoonlight.Common.Crossmod;
using MythosOfMoonlight.Common.Systems;
using MythosOfMoonlight.Common.Utilities;
using MythosOfMoonlight.Dusts;
using MythosOfMoonlight.NPCs.Enemies.CometFlyby.CometEmber;
using MythosOfMoonlight.NPCs.Minibosses.StarveiledProj;
using MythosOfMoonlight.Projectiles.VFXProjectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs.Minibosses
{
    public class StarveiledScholar : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.TrailCacheLength[Type] = 5;
            NPCID.Sets.TrailingMode[Type] = 0;
            NPC.AddElement(CrossModHelper.Celestial);
        }
        public override void SetDefaults()
        {
            NPC.width = 68;
            NPC.height = 74;
            NPC.lifeMax = 2300;
            NPC.boss = true;
            NPC.defense = 22;
            NPC.damage = 0;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;
            NPC.npcSlots = 7f;
            NPC.HitSound = SoundID.NPCHit49;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            if (!Main.dedServ) Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/scholar");
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new BestiaryPortraitBackgroundProviderPreferenceInfoElement(ModContent.GetInstance<PurpleCometBiome>().ModBiomeBestiaryInfoElement),
                new FlavorTextBestiaryInfoElement("When the dazzling purple witch vanished, her most loyal followers continued to follow the Purple Comet, soon falling into madness and into infestation. Restlessly following the comet to adore it they lived off the lands and rare towns encountered on their endless journey, be it simple dedication or the will of the unearthly creatures within them.")
            });
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 1)
            {
                for (int num901 = 0; num901 < 10; num901++)
                {
                    int num902 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<Dusts.PurpurineDust>(), 0f, 0f, 200, default(Color), 1f);
                    Main.dust[num902].position = NPC.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() * NPC.width / 2f;
                    Main.dust[num902].noGravity = true;
                    Dust dust2 = Main.dust[num902];
                    dust2.velocity = hit.HitDirection * Vector2.UnitX;
                    num902 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.PurpleTorch, 0f, 0f, 100, default(Color), 0.75f);
                    Main.dust[num902].position = NPC.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() * NPC.width / 2f;
                    dust2 = Main.dust[num902];
                    dust2.velocity = hit.HitDirection * Vector2.UnitX;
                    Main.dust[num902].noGravity = true;
                    Main.dust[num902].fadeIn = 2.5f;
                }
                for (int num903 = 0; num903 < 10; num903++)
                {
                    int num904 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.WitherLightning, 0f, 0f, 0, default(Color), 1);
                    Main.dust[num904].position = NPC.Center + Vector2.UnitX.RotatedByRandom(MathHelper.Pi).RotatedBy(NPC.velocity.ToRotation()) * NPC.width / 2f;
                    Dust dust2 = Main.dust[num904];
                    dust2.velocity = hit.HitDirection * Vector2.UnitX;
                }
            }
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
        public override bool CheckDead()
        {
            if (NPC.life <= 0 && !ded)
            {
                NPC.life = 1;
                AIState = Death;
                NPC.frameCounter = 0;
                NPC.immortal = true;
                NPC.noGravity = false;
                NPC.noTileCollide = false;
                NPC.dontTakeDamage = true;
                ded = true;
                AITimer = AITimer2 = 0;
                NPC.velocity = Vector2.Zero;
                NPC.life = 1;
                return false;
            }
            return true;
        }
        const int P2Transition = -2, Death = -1, Intro = 0, Idle = 1, UnnamedAttackNumberOne = 2;
        bool ded;
        int NextAttack = Idle;
        bool p2;
        Vector2[] disposablePos = new Vector2[10];
        public override void AI()
        {
            Player player = Main.player[NPC.target];
            if (NPC.life < NPC.lifeMax / 2 && !p2)
            {
                AIState = P2Transition;
                AITimer = AITimer2 = AITimer3 = 0;
                p2 = true;
            }
            NPC.TargetClosest(true);
            NPC.direction = player.Center.X > NPC.Center.X ? 1 : -1;
            NPC.spriteDirection = NPC.direction;
            if (player.dead)
            {
                NPC.active = false;

                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<CometEmberProj>(), 20, .1f, Main.myPlayer);
            }
            if (AIState == P2Transition)
            {
                p2 = true;
                AIState = Idle;
            }
            if (AIState == Intro)
            {
                AIState = Idle;
            }
            else if (AIState == Idle)
            {
                AITimer++;
                if (AITimer == 1)
                {
                    AITimer3 = Main.rand.NextBool().ToInt();
                    AITimer2 = Main.rand.Next(3);
                    disposablePos[1].X = Main.rand.Next(2);
                    AITimer3 = 1;
                }

                float lerpValue = (float)(Math.Sin(Main.GameUpdateCount * 0.01f) + 1) / 2;
                float rot = MathHelper.Lerp(-MathHelper.Pi, MathHelper.Pi, lerpValue);
                int dir = disposablePos[1].X == 0 ? -1 : 1;
                if (AITimer < 120)
                {
                    if (AITimer3 == 0)
                        NPC.velocity = (player.Center + new Vector2(190 * dir, -30) + new Vector2(-50 + rot * 3, 0).RotatedBy(rot) - NPC.Center) / 13f;
                    else
                    {
                        if (AITimer % 3 == 0)
                        {
                            if (AITimer2 == 2)
                                NPC.velocity = Vector2.Lerp(NPC.velocity, (player.Center + new Vector2((190 + AITimer * 0.6f) * dir, -30) + new Vector2(-50 + rot * 3, 0).RotatedBy(rot) - NPC.Center) / 13f, 0.1f);
                            else
                                NPC.velocity = Vector2.Lerp(NPC.velocity, (player.Center + new Vector2(190 * dir, -30) + new Vector2(-50 + rot * 3, 0).RotatedBy(rot) - NPC.Center) / 13f, 0.1f);
                        }
                    }
                }
                else
                    NPC.velocity *= 0.9f;
                if (AITimer3 == 1)
                {
                    switch (AITimer2)
                    {
                        case 0:
                            if (AITimer > 40 && AITimer % (NPC.life < NPC.lifeMax * 0.75f ? 20 : 30) == 0)
                                disposablePos[0] = player.Center;
                            if (AITimer > 40 && AITimer % (NPC.life < NPC.lifeMax * 0.75f ? 20 : 30) == 10)
                            {
                                SoundEngine.PlaySound(SoundID.Item5, NPC.Center);
                                SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
                                NPC.velocity += new Vector2(Helper.FromAToB(player.Center, NPC.Center).X * 4, 6 * (AITimer % 40 == 0 ? -1 : 1));
                                Projectile.NewProjectile(null, NPC.Center, Helper.FromAToB(NPC.Center, disposablePos[0]) * (4.5f + AITimer * 0.08f), ModContent.ProjectileType<ScholarArrow>(), 20, 0);
                            }
                            break;
                        case 1:
                            if (AITimer == 100)
                                disposablePos[0] = player.Center;
                            if (AITimer == 110)
                            {
                                SoundEngine.PlaySound(SoundID.Item5, NPC.Center);
                                SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
                                int max = Main.rand.Next(1, 3);
                                NPC.velocity += Helper.FromAToB(player.Center, NPC.Center) * 5;
                                for (int i = -max; i < max + 1; i++)
                                    Projectile.NewProjectile(null, NPC.Center, Helper.FromAToB(NPC.Center, disposablePos[0]).RotatedBy((float)i / max * 0.5f) * (9f - MathF.Abs(i)), ModContent.ProjectileType<ScholarArrow>(), 20, 0);
                            }
                            break;
                        case 2:
                            if (AITimer == 20)
                                Projectile.NewProjectile(null, NPC.Center, Vector2.Zero, ModContent.ProjectileType<ScholarChargeUp>(), 0, 0, -1, NPC.whoAmI);
                            if (AITimer == 70)
                                disposablePos[0] = player.Center;
                            if (AITimer == 80)
                            {
                                SoundEngine.PlaySound(SoundID.Item5, NPC.Center);
                                SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
                                CameraSystem.ScreenShakeAmount = 5f;
                                for (int i = 0; i < 25; i++)
                                    Dust.NewDustPerfect(NPC.Center, ModContent.DustType<LineDustFollowPoint>(), Helper.FromAToB(NPC.Center, disposablePos[0]) * Main.rand.NextFloat(10, 30), 0, Color.Lerp(Color.Purple, Color.Indigo, Main.rand.NextFloat()), Main.rand.NextFloat(0.08f, 0.25f));
                                Projectile.NewProjectile(null, NPC.Center, Helper.FromAToB(NPC.Center, disposablePos[0]) * 10, ModContent.ProjectileType<FastScholarArrow>(), 50, 0);
                            }
                            break;
                    }
                }
                if (AITimer >= 140)
                {
                    NPC.frame.Y = 0;
                    Reset(NextAttack);
                }
            }

        }
        void Reset(int attack)
        {
            NPC.velocity = Vector2.Zero;
            AITimer = 0;
            AITimer2 = 0;
            AITimer3 = 0;
            AIState = attack;
            for (int i = 0; i < disposablePos.Length; i++)
                disposablePos[i] = Vector2.Zero;
        }
        void Teleport(Vector2 pos)
        {
            NPC.Center = pos;
            for (int num901 = 0; num901 < 10; num901++)
            {
                int num902 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<Dusts.PurpurineDust>(), 0f, 0f, 200, default(Color), 1f);
                Main.dust[num902].position = NPC.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() * NPC.width / 2f;
                Main.dust[num902].noGravity = true;
                Dust dust2 = Main.dust[num902];
                dust2.velocity *= 3f;
                num902 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.PurpleTorch, 0f, 0f, 100, default(Color), 0.75f);
                Main.dust[num902].position = NPC.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() * NPC.width / 2f;
                dust2 = Main.dust[num902];
                dust2.velocity *= 2f;
                Main.dust[num902].noGravity = true;
                Main.dust[num902].fadeIn = 2.5f;
            }
            for (int num903 = 0; num903 < 10; num903++)
            {
                int num904 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.WitherLightning, 0f, 0f, 0, default(Color), 1);
                Main.dust[num904].position = NPC.Center + Vector2.UnitX.RotatedByRandom(MathHelper.Pi).RotatedBy(NPC.velocity.ToRotation()) * NPC.width / 2f;
                Dust dust2 = Main.dust[num904];
                dust2.velocity *= 3f;
            }
        }
    }
}
