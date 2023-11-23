
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim.Projectiles;
using Terraria.ID;
using MythosOfMoonlight.Dusts;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using System;
using System.IO;
using Microsoft.CodeAnalysis.Differencing;
using MythosOfMoonlight.Items.Jungle;
using Terraria.GameContent.ItemDropRules;
using MythosOfMoonlight.Items.Pets;
using MythosOfMoonlight.Common.Systems;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using Terraria.DataStructures;

namespace MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim
{
    [AutoloadBossHead]
    public class RupturedPilgrim : ModNPC
    {
        public static NPC Sym => Starine_Symbol.symbol;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ruptured Pilgrim");
            Main.npcFrameCount[NPC.type] = 44;
            NPCID.Sets.TrailCacheLength[NPC.type] = 9;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new() { Velocity = 1 };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
            NPC.AddElement(CrossModHelper.Celestial);
            NPC.AddElement(CrossModHelper.Arcane);
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<LilPilgI>(), 4));
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {

            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement("When the will is weak, either naturally or from their weakening minds, the astral power can rupture from flesh. Unfortunate, but determined to continue its pilgrimage, it guards it's shrine fiercely.")
            });
        }
        public override void SetDefaults()
        {
            NPC.width = 58;
            NPC.height = 92;
            NPC.lifeMax = 2000;
            NPC.boss = true;
            NPC.defense = 19;
            NPC.damage = 0;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;
            NPC.npcSlots = 7f;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit49;
            NPC.DeathSound = SoundID.NPCDeath52;
            NPC.noTileCollide = true;
            NPC.ai[0] = 1;
            NPC.alpha = 255;
            if (!Main.dedServ) Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Ruptured");
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * bossAdjustment * balance);
        }
        bool hasDoneDeathDrama;
        public int Direction;
        void IdleAnim(int frameHeight)
        {
            if (NPC.frameCounter % 5 == 0)
            {
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y > 3 * frameHeight)
                NPC.frame.Y = 0;
        }
        public override void FindFrame(int g)
        {
            NPC.frame.Height = 92;
            int frameHeight = 92;
            NPC.frameCounter++;
            if (NPC.frameCounter <= 1)
                NPC.ai[3] = 0;
            switch (State)
            {
                case AIState.Death:
                    if (NPC.frameCounter % 5 == 0)
                    {
                        if (AITimer <= 25 || AITimer > 250)
                            if (NPC.frame.Y < frameHeight * 43)
                                NPC.frame.Y += frameHeight;
                    }
                    if (AITimer <= 25 || AITimer > 250)
                        if (NPC.frame.Y < 30 * frameHeight)
                            NPC.frame.Y = 30 * frameHeight;
                    break;
                case AIState.StarineSigil:
                    if (NPC.frameCounter % 5 == 0 && NPC.ai[3] == 0)
                    {
                        if (NPC.frame.Y < frameHeight * 20)
                            NPC.frame.Y += frameHeight;
                        if (AITimer > 11)
                        {
                            if (NPC.frame.Y < 11 * frameHeight)
                                NPC.frame.Y = 11 * frameHeight;
                        }
                    }
                    if (NPC.frame.Y == frameHeight * 20)
                        NPC.ai[3] = 1;
                    if (NPC.ai[3] == 1)
                        IdleAnim(frameHeight);
                    break;
                case AIState.StarineStars:
                    if (AITimer > 0)
                    {
                        if (AITimer == 11)
                            NPC.frame.Y = 21 * frameHeight;
                        if (NPC.frameCounter % 5 == 0 && NPC.ai[3] == 0)
                        {
                            if (AITimer > 11)
                            {
                                if (AITimer < 80)
                                {
                                    if (NPC.frame.Y < frameHeight * 24)
                                        NPC.frame.Y += frameHeight;
                                }
                                else
                                {
                                    if (NPC.frame.Y < frameHeight * 29)
                                        NPC.frame.Y += frameHeight;
                                }
                            }
                            else
                            {
                                if (NPC.frame.Y < frameHeight * 7)
                                {
                                    NPC.frame.Y += frameHeight;
                                }
                            }
                        }
                        if (NPC.frame.Y == frameHeight * 29)
                            NPC.ai[3] = 1;
                        if (NPC.ai[3] == 1)
                            IdleAnim(frameHeight);
                    }
                    else
                    {

                    }
                    break;
                case AIState.StarineSlush:
                    if (NPC.frameCounter % 5 == 0 && NPC.ai[3] == 0)
                    {
                        if (AITimer > 11)
                        {
                            if (AITimer < 185)
                            {
                                if (NPC.frameCounter % 10 == 0)
                                    NPC.frame.Y = frameHeight * 14;
                                else
                                {
                                    if (NPC.frame.Y < frameHeight * 15)
                                        NPC.frame.Y += frameHeight;
                                }
                            }
                            else
                            {
                                if (NPC.frame.Y < frameHeight * 20)
                                    NPC.frame.Y += frameHeight;
                            }
                        }
                        else
                        {
                            if (NPC.frame.Y < frameHeight * 7)
                            {
                                NPC.frame.Y += frameHeight;
                            }
                        }
                    }
                    if (NPC.frame.Y == frameHeight * 20)
                        NPC.ai[3] = 1;
                    if (NPC.ai[3] == 1)
                        IdleAnim(frameHeight);
                    break;
                case AIState.StarineSpree:
                    if (AITimer > 140)
                    {
                        if (NPC.frameCounter % 5 == 0 && NPC.ai[3] <= 1f)
                        {
                            if (NPC.frame.Y == 7 * frameHeight)
                                return;
                            if (NPC.frame.Y < frameHeight * 20)
                                NPC.frame.Y += frameHeight;
                            if (NPC.frame.Y > 7 * frameHeight && NPC.frame.Y < 16 * frameHeight)
                            {
                                if (NPC.frame.Y < 8 * frameHeight || NPC.frame.Y > 10 * frameHeight)
                                    NPC.frame.Y = 8 * frameHeight;
                            }
                        }

                    }
                    else
                    {
                        if (AITimer == 140)
                            NPC.frame.Y = 4 * 92;
                        else
                        {
                            if (AITimer % 20 == 0)
                                NPC.frame.Y = (int)Main.rand.Next(0, 29) * frameHeight;
                        }
                    }
                    if (NPC.ai[3] > 1)
                        IdleAnim(frameHeight);
                    break;
                case AIState.StarineSwipe:
                    if (NPC.frameCounter % 5 == 0 && NPC.ai[3] == 0 && AITimer > 60 && AITimer < 125)
                    {
                        if (NPC.frame.Y < frameHeight * 20)
                            NPC.frame.Y += frameHeight;
                        if (AITimer > 75 && AITimer < 125)
                        {
                            if (NPC.frame.Y < 8 * frameHeight || NPC.frame.Y > 10 * frameHeight && (NPC.frame.Y > 7 * frameHeight && NPC.frame.Y < 16 * frameHeight))
                                NPC.frame.Y = 8 * frameHeight;
                        }
                    }
                    if (AITimer == 125)
                        NPC.frame.Y = 19 * frameHeight;
                    if (AITimer == 130)
                        NPC.frame.Y = 20 * frameHeight;
                    if (AITimer == 60)
                        NPC.frame.Y = 92 * 4;
                    if (AITimer < 60 || AITimer >= 135)
                        IdleAnim(frameHeight);
                    break;
                case AIState.SpringySphere:
                    if (NPC.frameCounter % 5 == 0 && NPC.ai[3] == 0 && AITimer > 60 && AITimer < 125)
                    {
                        if (NPC.frame.Y < frameHeight * 20)
                            NPC.frame.Y += frameHeight;
                        if (AITimer > 75 && AITimer < 125)
                        {
                            if (NPC.frame.Y < 8 * frameHeight || NPC.frame.Y > 10 * frameHeight && (NPC.frame.Y > 7 * frameHeight && NPC.frame.Y < 16 * frameHeight))
                                NPC.frame.Y = 8 * frameHeight;
                        }
                    }
                    if (AITimer == 125)
                        NPC.frame.Y = 19 * frameHeight;
                    if (AITimer == 130)
                        NPC.frame.Y = 20 * frameHeight;
                    if (AITimer == 60)
                        NPC.frame.Y = 92 * 4;
                    if (AITimer < 60 || AITimer >= 135)
                        IdleAnim(frameHeight);
                    break;
                default:
                    IdleAnim(frameHeight);
                    break;
            }

        }
        public enum AIState
        {
            Death,
            Spawn,
            Idle,
            StarineSigil,
            StarineSlush,
            StarineShafts,
            StarineSwipe,
            StarineSigilSequel,
            SeekingStarineStars,
            SpringySphere,
            StarineStars,
            SymbolShafts,
            StarineSpree,
            Despawn
        }
        /*
         * 1-4 idle
         * 5-8 plays before her attack anims as a transition
         * 9-11 is when she uses the tentacle attack
         * 12-21 she summons the sigil
         * 15-21 is used for the falling goop (specifically when she throws the big one)
         * 20-21 finishes all attack anims except the star one
         * 22-30 is for star attack 
         * 31-44 ded
        */
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((int)Next);
            writer.Write((int)StateBeforeP2);
            writer.Write(aitimer2);
            writer.WriteVector2(lastPPos);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Next = (AIState)reader.ReadSingle();
            StateBeforeP2 = (AIState)reader.ReadSingle();
            aitimer2 = reader.ReadSingle();
            lastPPos = reader.ReadVector2();
        }
        AIState StateBeforeP2;
        private AIState State
        {
            get { return (AIState)(int)NPC.ai[0]; }
            set { NPC.ai[0] = (int)value; }
        }
        AIState Next = AIState.StarineSigil;
        AIState OldState = AIState.StarineSigil;
        public float AITimer
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }
        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }
        Vector2 savedPos;
        public NPC owner = null;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            var off = new Vector2(NPC.width / 2, NPC.height / 2);
            var clr = new Color(255, 255, 255, 255) * (float)((float)(255f - NPC.alpha) / 255f);
            var drawPos = NPC.Center - screenPos;
            var origTexture = TextureAssets.Npc[NPC.type].Value;
            var texture = ModContent.Request<Texture2D>(NPC.ModNPC.Texture + "_Trail").Value;
            var glowTexture = ModContent.Request<Texture2D>(NPC.ModNPC.Texture + "_Glow").Value;
            var magicTexture = ModContent.Request<Texture2D>(NPC.ModNPC.Texture + "_Magic").Value;
            var backTexture = ModContent.Request<Texture2D>(NPC.ModNPC.Texture + "_Back").Value;
            var frame = NPC.frame;
            var orig = NPC.Size / 2;
            var trailLength = NPCID.Sets.TrailCacheLength[NPC.type];
            SpriteEffects flipType = NPC.spriteDirection != -1 /* or 1, idf  */ ? SpriteEffects.None : SpriteEffects.FlipHorizontally;


            {
                for (int i = 1; i < trailLength; i++)
                {
                    float _scale = MathHelper.Lerp(1f, 0.95f, (float)(trailLength - i) / trailLength);
                    var fadeMult = 1f / trailLength;
                    spriteBatch.Draw(texture, NPC.oldPos[i] - screenPos + off + new Vector2(0, 1), frame, clr * (1f - fadeMult * i), NPC.rotation, orig, _scale, flipType, 0f);
                }
            }
            const float TwoPi = (float)Math.PI * 2f;
            float scale = (float)Math.Sin(Main.GlobalTimeWrappedHourly * TwoPi / 2f) * 0.3f + 0.7f;
            if (NPC.life <= NPC.lifeMax / 2)
                for (int i = 0; i < 4; i++)
                    spriteBatch.Draw(texture, drawPos + (Vector2.UnitX * 10 * scale).RotatedBy(MathHelper.ToRadians((90) * i)), frame, Color.Cyan * 0.5f * scale, NPC.rotation, orig, NPC.scale, flipType, 0);
            spriteBatch.Draw(backTexture, drawPos, frame, drawColor, NPC.rotation, orig, NPC.scale, flipType, 0f);

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.active && projectile.type == ModContent.ProjectileType<TestTentacle2>())
                {
                    Color a = Color.Transparent;
                    projectile.ModProjectile.PreDraw(ref a);
                }
            }

            spriteBatch.Draw(origTexture, drawPos, frame, drawColor, NPC.rotation, orig, NPC.scale, flipType, 0f);
            spriteBatch.Draw(glowTexture, drawPos, frame, clr, NPC.rotation, orig, NPC.scale, flipType, 0f);

            if (State != AIState.Death)
            {
                if (State != AIState.StarineSlush && State != AIState.StarineSpree && State != AIState.StarineSwipe)
                    spriteBatch.Draw(magicTexture, drawPos + new Vector2(0, (float)Math.Sin(Main.GlobalTimeWrappedHourly * 1.5f) * 4), frame, clr, NPC.rotation, orig, NPC.scale, flipType, 0f);
            }
            else
            {
                if (!(AITimer > 25 && AITimer < 250))
                    spriteBatch.Draw(magicTexture, drawPos + new Vector2(0, (float)Math.Sin(Main.GlobalTimeWrappedHourly * 1.5f) * 4), frame, clr, NPC.rotation, orig, NPC.scale, flipType, 0f);
            }
            return false;
        }
        public override bool CheckDead()
        {
            if (NPC.life <= 0 && !hasDoneDeathDrama)
            {
                List<int> types = new List<int>
                {
                    ModContent.ProjectileType<StarineFlare>(),
                    ModContent.ProjectileType<PilgStar>(),
                    ModContent.ProjectileType<StarineSigil>(),
                    ModContent.ProjectileType<StarineShaft>()
                };
                foreach (Projectile proj in Main.projectile)
                {
                    if (proj.active && types.Contains(proj.type))
                        proj.Kill();
                }
                Player player = Main.player[NPC.target];
                SoundEngine.PlaySound(SoundID.NPCDeath51);
                SoundEngine.PlaySound(SoundID.NPCDeath52, NPC.Center);
                NPC.life = 1;
                NPC.frameCounter = 0;
                NPC.immortal = true;
                AITimer = 0;
                NPC.velocity = Helper.FromAToB(player.Center, NPC.Center) * 7;
                CameraSystem.ChangeCameraPos(owner.Center, 280, 1.3f);
                SwitchTo(AIState.Death);
                hasDoneDeathDrama = true;
                return false;
            }
            return true;
        }
        bool didp2;

        private void SwitchTo(AIState state)
        {
            State = state;
        }
        /*
         * pattern0: Sigils, Star, Slush, Explode, Tentacle, Shrinking Lasers, Spam
         */
        const int attackNum = 10;
        public AIState[] pattern = new AIState[attackNum];
        public AIState[] oldPattern = new AIState[attackNum];
        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < attackNum - 3; i++)
            {
                pattern[i] = (AIState)Main.rand.Next(3, attackNum);
            }
        }
        public void GenerateNewPattern()
        {
            int off = 3;
            if (didp2)
                off = 0;
            for (int i = 0; i < attackNum - off; i++)
            {
                pattern[i] = (AIState)Main.rand.Next(3, 13 - off);
            }
            for (int i = 0; i < attackNum - off; i++)
            {
                int attempts = 0;
                while (++attempts < 100 && (pattern.Count(p => p == pattern[i]) != 1 || pattern[i] == (AIState)0) || oldPattern.Last() == pattern.First())
                {
                    pattern[i] = (AIState)Main.rand.Next(3, 13 - off);
                }
            }
        }
        public void SwitchToRandom()
        {
            int off = 3;
            if (didp2)
                off = 0;
            if (pattern.Any())
            {
                if (State == (AIState)pattern[attackNum - 1 - off])
                {
                    GenerateNewPattern();
                    Next = (AIState)pattern.First();
                }
                else if (State == AIState.Spawn)
                {
                    GenerateNewPattern();
                    Next = pattern.First();
                }
                else
                {
                    oldPattern = pattern;
                    Next = (AIState)pattern[pattern.ToList().IndexOf(OldState) + 1];
                }
            }
        }
        public override void AI()
        {
            if (pattern.Contains(AIState.Death) && didp2 && State == AIState.Idle)
                GenerateNewPattern();
            if (!didp2)
            {
                AIState[] _pattern = new AIState[attackNum - 3];
                for (int i = 0; i < attackNum - 3; i++)
                {
                    _pattern[i] = pattern[i];
                }
                if (_pattern.Contains(AIState.Death) && State == AIState.Idle)
                    GenerateNewPattern();
            }
            //Main.NewText((int)State);
            if (State != AIState.Despawn)
            {
                if (State != AIState.StarineSwipe)
                    NPC.rotation = NPC.velocity.X * 0.025f;
                if (Main.netMode == NetmodeID.Server)
                    NPC.netUpdate = true;
                if (NPC.life <= NPC.lifeMax / 2)
                {
                    if (Main.rand.NextBool(5))
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            Dust a = Dust.NewDustPerfect(NPC.Center - Vector2.UnitY * 20, ModContent.DustType<Dusts.StarineDust>(), Main.rand.NextVector2Unit());
                            a.noGravity = false;
                        }
                    }
                    if (!didp2)
                    {
                        if (State != AIState.Idle && State != AIState.Death && State != AIState.Spawn)
                            StateBeforeP2 = State;
                        else
                            StateBeforeP2 = AIState.StarineSigil;
                        SoundEngine.PlaySound(SoundID.NPCDeath51);
                        didp2 = true;
                        GenerateNewPattern();
                        SwitchTo(AIState.StarineStars);
                        NPC.velocity = Vector2.Zero;
                        aitimer2 = 0;
                        Next = AIState.StarineStars;
                        AITimer = -30;
                    }
                    if (AITimer == -1)
                    {
                        NPC.velocity = Vector2.Zero;

                        for (int i = 4; i <= 360; i += 4)
                        {
                            Vector2 dVel = MathHelper.ToRadians(i).ToRotationVector2() * 6f;
                            Dust dust = Dust.NewDustDirect(NPC.Center, 1, 1, ModContent.DustType<StarineDust>(), dVel.X, dVel.Y);
                            dust.noGravity = true;
                        }
                        NPC.Center = owner.Center - Vector2.UnitY * 100;
                    }
                }
                NPC.TargetClosest(false);
                if (AITimer % 10 == 0)
                    if (State != AIState.StarineSwipe && State != AIState.Death)
                    {
                        NPC.FaceTarget();
                        NPC.spriteDirection = NPC.direction;
                    }
                    else
                    {
                        if (AITimer < (State == AIState.StarineSwipe ? (NPC.life >= NPC.lifeMax * .5f ? 90 : 120) : 150))
                        {
                            NPC.FaceTarget();
                            Direction = NPC.direction;
                            NPC.spriteDirection = NPC.direction;
                        }
                        else
                        {
                            NPC.direction = Direction;
                            NPC.spriteDirection = NPC.direction;
                        }
                    }
                Player player = Main.player[NPC.target];
                if (State == AIState.Death)
                {
                    NPC.velocity *= 0.95f;
                    int interval = 40;
                    if (AITimer > 75)
                        interval = 30;
                    if (AITimer > 125)
                        interval = 20;
                    if (AITimer > 175)
                        interval = 15;
                    if (AITimer > 225)
                        interval = 10;
                    if (AITimer % interval == 0 && AITimer > 25 && AITimer < 250 && owner != null)
                    {
                        NPC.frame.Y = (int)Main.rand.Next(0, 29) * 92;
                        SoundEngine.PlaySound(SoundID.NPCHit5, NPC.Center);
                        if (savedPos == Vector2.Zero)
                            savedPos = NPC.Center;
                        Vector2 pos = owner.Center + Main.rand.NextVector2Circular(100 + (AITimer * 1.5f), 100 + (AITimer * 1.5f));
                        //Vector2 origin = savedPos;
                        for (int i = 0; i < 30; i++)
                        {
                            Helper.SpawnDust(Vector2.Lerp(NPC.oldPosition, pos, (float)i / 30), NPC.Size / 2, ModContent.DustType<StarineDust>(), Helper.FromAToB(NPC.oldPosition, pos) * Main.rand.NextFloat(5f));
                        }
                        savedPos = pos;
                        NPC.Center = pos;
                    }
                    if (AITimer % interval != 0 && AITimer > 25 && AITimer < 250 && savedPos.Distance(NPC.Center) < 100)
                        NPC.Center = savedPos + Main.rand.NextVector2Circular(4 + (AITimer * 0.03f), 4 + (AITimer * 0.03f));
                    if (AITimer == 250)
                    {
                        NPC.frame.Y = 35 * 92;
                        SoundEngine.PlaySound(SoundID.NPCHit5, NPC.Center);
                        Vector2 pos = owner.Center + new Vector2(0, -100);
                        for (int i = 0; i < 30; i++)
                        {
                            Helper.SpawnDust(Vector2.Lerp(NPC.oldPosition, pos, (float)i / 30), NPC.Size / 2, ModContent.DustType<StarineDust>(), Helper.FromAToB(NPC.oldPosition, pos) * Main.rand.NextFloat(5f));
                        }
                        NPC.Center = pos;
                        CameraSystem.ChangeCameraPos(owner.Center, 50, 2f);
                    }
                    if (AITimer > 250)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<StarineDust>());
                            Main.dust[dust].velocity = Main.rand.NextVector2Unit() * 2f;
                            Main.dust[dust].scale = 1f * Main.rand.Next(2, 3);
                            Main.dust[dust].noGravity = true;
                        }
                    }
                    if (NPC.frame.Y == 92 * 43)
                    {
                        owner.ai[0] = 3;
                        NPC.immortal = false;
                        for (int i = 0; i < 8; i++)
                        {
                            Projectile.NewProjectile(NPC.GetSource_Death(), NPC.Center, Main.rand.NextVector2Unit() * Main.rand.NextFloat(0.1f, 1.1f), ModContent.ProjectileType<TinySightseer>(), 0, 0);
                        }
                        Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, new Vector2(), ModContent.ProjectileType<PilgrimExplosion>(), 0, 100);
                        if (Main.netMode != NetmodeID.Server)
                        {
                            Helper.SpawnGore(NPC, "MythosOfMoonlight/Starine", Main.rand.Next(30, 40), scale: Main.rand.NextFloat(1f, 1.5f));
                            Helper.SpawnGore(NPC, "MythosOfMoonlight/PurpFabric", 1, 1);
                            Helper.SpawnGore(NPC, "MythosOfMoonlight/PurpFabric", 2, 2);
                            Helper.SpawnGore(NPC, "MythosOfMoonlight/PurpMagFabric", 1, 1);
                            Helper.SpawnGore(NPC, "MythosOfMoonlight/PurpMagFabric", 2, 2);
                        }
                        SoundEngine.PlaySound(SoundID.Item62, NPC.Center);
                        NPC.life = 0;
                        NPC.checkDead();
                    }
                }
                foreach (NPC NPC in Main.npc)
                {
                    if (NPC.type == ModContent.NPCType<Starine_Symbol>())
                        owner = NPC;
                }
                if (!player.active || player.dead || player.Center.Distance(owner.Center) > 700)
                {
                    AITimer = 0;
                    lastPPos = NPC.Center;
                    State = AIState.Despawn;
                }
                if (Sym != owner && Sym.active)
                    owner = Sym;
                if (owner == null || !owner.active)
                    NPC.active = false;
                AITimer++;
                if (State != AIState.Idle)
                    OldState = State;
                switch (State)
                {
                    case AIState.Idle:
                        {
                            float lerpValue = (float)(Math.Sin(Main.GameUpdateCount * 0.025f) + 1) / 2;
                            float rot = MathHelper.Lerp(0, MathHelper.Pi, lerpValue);
                            NPC.velocity = (player.Center + new Vector2(-100, 0).RotatedBy(rot) - NPC.Center) / 30f;
                            if (AITimer >= MathHelper.Clamp(NPC.life / 10, 40, 1000))
                            {
                                NPC.frameCounter = 0;
                                AITimer = 0;
                                NPC.frame.Y = 4 * 92;
                                NPC.velocity = Vector2.Zero;
                                SwitchTo(Next);
                            }
                            break;
                        }
                    case AIState.Spawn:
                        {
                            NPC.alpha -= 5;
                            NPC.velocity *= 0;
                            if (NPC.alpha > 0)
                            {
                                for (int i = 1; i <= 6; i++)
                                {
                                    Vector2 pos = NPC.Center + new Vector2(90, 0).RotatedBy(Main.rand.NextFloat(0, 6.28f));
                                    Vector2 vel = Vector2.Normalize(pos - NPC.Center) * -4f;
                                    Dust dust = Dust.NewDustDirect(pos, 1, 1, ModContent.DustType<StarineDust>());
                                    dust.color = Color.Cyan;
                                    dust.velocity = vel;
                                    dust.scale = 1.2f;
                                    dust.fadeIn = .4f;
                                    dust.noGravity = true;
                                }
                            }
                            if (NPC.alpha < 0)
                                NPC.alpha = 0;

                            if (AITimer >= 155)
                            {
                                AITimer = 0;
                                NPC.frameCounter = 0;
                                SwitchToRandom();
                                SwitchTo(AIState.Idle);
                            }
                            break;
                        }
                    case AIState.StarineSigil:
                        {
                            if (AITimer < 40)
                            {
                                NPC.velocity = (player.Center + new Vector2(NPC.Center.X > player.Center.X ? 100 : -100, 0) - NPC.Center) / 20f;
                            }
                            if (AITimer == 40)
                            {
                                NPC.velocity = Vector2.Zero;
                                SoundEngine.PlaySound(SoundID.NPCHit5, NPC.Center);
                                for (int i = 4; i <= 360; i += 4)
                                {
                                    Vector2 dVel = MathHelper.ToRadians(i).ToRotationVector2() * 6f;
                                    Dust dust = Dust.NewDustDirect(NPC.Center, 1, 1, ModContent.DustType<StarineDust>(), dVel.X, dVel.Y);
                                    dust.noGravity = true;
                                }
                            }
                            if (AITimer == 60)
                            {
                                {
                                    Vector2 pos = Sym.Center - new Vector2(-300, 100);
                                    Projectile a = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), pos, Vector2.Zero, ModContent.ProjectileType<StarineSigil>(), 12, .1f, ai0: -1);
                                    a.ai[0] = -1;
                                    a.ai[1] = -50;
                                }
                                {
                                    Vector2 pos = Sym.Center - new Vector2(300, 100);
                                    Projectile a = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), pos, Vector2.Zero, ModContent.ProjectileType<StarineSigil>(), 12, .1f, ai0: 1);
                                    a.ai[0] = 1;
                                    a.ai[1] = -50;
                                }

                            }
                            if (AITimer == 120)
                            {
                                AITimer = 0;
                                NPC.frameCounter = 0;
                                //Next = AIState.StarineStars;
                                SwitchToRandom();
                                SwitchTo(AIState.Idle);
                            }
                            break;
                        }
                    case AIState.StarineStars:
                        {
                            if ((AITimer == 10 || AITimer < 0) && aitimer2 == 0)
                            {
                                NPC.velocity = new Vector2(-NPC.spriteDirection * 7, -4);
                                SoundEngine.PlaySound(SoundID.AbigailSummon, NPC.Center);
                                aitimer2 = 1;
                            }
                            if (AITimer == 20)
                            {
                                for (int i = 4; i <= 360; i += 4)
                                {
                                    Vector2 dVel = MathHelper.ToRadians(i).ToRotationVector2() * 6f;
                                    Dust dust = Dust.NewDustDirect(NPC.Center, 1, 1, ModContent.DustType<StarineDust>(), dVel.X, dVel.Y);
                                    dust.noGravity = true;
                                }
                                NPC.velocity = Vector2.Zero;
                                NPC.Center = owner.Center - Vector2.UnitY * 100;
                                lastPPos = NPC.Center - new Vector2(0, 100).RotatedBy(Main.rand.Next(new float[] { 0, MathHelper.PiOver2 }) * player.direction);
                            }
                            NPC.velocity *= 0.9f;
                            if (AITimer == 25)
                            {
                                Vector2 pos = Sym.Center;
                                bool hasReflected = false;
                                bool outside = false;
                                int times = 0;
                                for (int i = 0; i < 5; i++)
                                {
                                    Vector2 vel = -Vector2.UnitY.RotatedBy(Helper.CircleDividedEqually(i, 5)) * 30;
                                    while (times < 6 + (Main.getGoodWorld ? 5 : 0))
                                    {
                                        if (Vector2.Distance(Sym.Center, pos) > 420)
                                        {
                                            if (!outside)
                                            {
                                                times++;
                                                outside = true;
                                                if (!hasReflected)
                                                {
                                                    vel = -vel.RotatedBy((MathHelper.ToRadians(18)));
                                                    hasReflected = true;
                                                }
                                                else
                                                {
                                                    vel = -vel.RotatedBy((MathHelper.ToRadians(36)));
                                                }
                                            }
                                        }
                                        else
                                            outside = false;
                                        Dust.NewDustPerfect(pos, ModContent.DustType<StarineDustAlt>(), Vector2.Zero).noGravity = true;
                                        pos += vel;
                                    }
                                }
                            }
                            if (AITimer == 50)
                            {
                                /*if (Main.getGoodWorld)
                                {
                                    for (int i = 0; i < 10; i++)
                                    {
                                        Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), Sym.Center, Helper.FromAToB(NPC.Center, lastPPos).RotatedBy(Helper.CircleDividedEqually(i, 10)) * 0.1f, ModContent.ProjectileType<PilgStar>(), 12, .1f);
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < 5; i++)
                                    {
                                        Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), Sym.Center, Helper.FromAToB(NPC.Center, lastPPos).RotatedBy(Helper.CircleDividedEqually(i, 5)) * 0.1f, ModContent.ProjectileType<PilgStar>(), 12, .1f);
                                    }
                                }*/
                                Vector2 pos = Sym.Center;
                                bool hasReflected = false;
                                bool outside = false;
                                int times = 0;
                                for (int i = 0; i < 5; i++)
                                {
                                    Vector2 vel = -Vector2.UnitY.RotatedBy(Helper.CircleDividedEqually(i, 5)) * 30;
                                    while (times < 5)
                                    {
                                        if (Vector2.Distance(Sym.Center, pos) > 420)
                                        {
                                            if (!outside)
                                            {
                                                times++;
                                                outside = true;
                                                if (!hasReflected)
                                                {
                                                    vel = -vel.RotatedBy((MathHelper.ToRadians(18)));
                                                    hasReflected = true;
                                                }
                                                else
                                                {
                                                    vel = -vel.RotatedBy((MathHelper.ToRadians(36)));
                                                }
                                                Vector2 _vel = vel;
                                                _vel.Normalize();
                                                SoundEngine.PlaySound(SoundID.NPCHit5, NPC.Center);
                                                Projectile.NewProjectile(null, pos, _vel, ModContent.ProjectileType<PilgStarBeam>(), 10, 0);
                                                for (int j = -1; j < 2; j++)
                                                {
                                                    if (j == 0) continue;
                                                    Projectile.NewProjectileDirect(null, pos, _vel.RotatedBy(j * 0.5f) * 7, ModContent.ProjectileType<PilgStar2>(), 10, 0).tileCollide = false;
                                                }
                                            }
                                        }
                                        else
                                            outside = false;
                                        pos += vel;
                                    }
                                }
                            }
                            if (AITimer == 160)
                            {
                                AITimer = 0;
                                NPC.frameCounter = 0;
                                //Next = AIState.StarineSlush;
                                SwitchToRandom();
                                SwitchTo(AIState.Idle);
                            }
                            break;
                        }
                    case AIState.StarineShafts:
                        {
                            float lerpValue = (float)(Math.Sin(Main.GameUpdateCount * 0.025f) + 1) / 2;
                            float rot = MathHelper.Lerp(0, MathHelper.Pi, lerpValue);
                            NPC.velocity = (player.Center + new Vector2(-100, 0).RotatedBy(rot) - NPC.Center) / 30f;

                            if (AITimer == 60)
                            {
                                for (int i = 4; i <= 360; i += 4)
                                {
                                    Vector2 dVel = MathHelper.ToRadians(i).ToRotationVector2() * 6f;
                                    Dust dust = Dust.NewDustDirect(NPC.Center, 1, 1, ModContent.DustType<StarineDust>(), dVel.X, dVel.Y);
                                    dust.noGravity = true;
                                }
                            }
                            if (AITimer == 90)
                            {
                                SoundEngine.PlaySound(SoundID.Item62, NPC.Center);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), owner.Center, Vector2.Zero, ModContent.ProjectileType<PilgrimExplosion>(), 0, .1f, Main.myPlayer);
                            }
                            if (AITimer == 160)
                            {
                                AITimer = 0;
                                NPC.frameCounter = 0;
                                //Next = AIState.StarineSlush;
                                SwitchToRandom();
                                SwitchTo(AIState.Idle);
                            }
                            break;
                        }
                    case AIState.StarineSlush:
                        {
                            if (AITimer < 185)
                            {
                                NPC.velocity = Vector2.UnitY * -0.2f * (AITimer * 0.025f);
                            }
                            else if (AITimer > 185 && AITimer < 210)
                            {
                                if (NPC.velocity.Length() < 4)
                                    NPC.velocity.Y++;
                            }
                            else if (AITimer >= 210)
                                NPC.velocity *= 0.9f;

                            if (AITimer == 20)
                            {
                                NPC.Center = owner.Center - Vector2.UnitY * 100;

                                SoundEngine.PlaySound(SoundID.NPCHit5, NPC.Center);
                                for (int i = 4; i <= 360; i += 4)
                                {
                                    Vector2 dVel = MathHelper.ToRadians(i).ToRotationVector2() * 6f;
                                    Dust dust = Dust.NewDustDirect(NPC.Center, 1, 1, ModContent.DustType<StarineDust>(), dVel.X, dVel.Y);
                                    dust.noGravity = true;
                                }
                            }
                            if (!didp2 || !Main.expertMode)
                            {
                                if (AITimer == 30)
                                {
                                    SoundStyle style = SoundID.Item8;
                                    style.Volume = 0.5f;
                                    SoundEngine.PlaySound(style, NPC.Center);
                                    for (int i = -1; i < 2; i++)
                                    {
                                        if (i == 0)
                                            continue;
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(400 * i, -100), Vector2.Zero, ModContent.ProjectileType<StarineSlushSmall>(), 12, .1f, Main.myPlayer);
                                    }
                                }
                                if (AITimer == 50)
                                {
                                    SoundStyle style = SoundID.Item8;
                                    style.Volume = 0.5f;
                                    SoundEngine.PlaySound(style, NPC.Center);
                                    for (int i = -1; i < 2; i++)
                                    {
                                        if (i == 0)
                                            continue;
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(300 * i, -150), Vector2.Zero, ModContent.ProjectileType<StarineSlushSmall>(), 12, .1f, Main.myPlayer);
                                    }
                                }
                                if (AITimer == 70)
                                {
                                    SoundStyle style = SoundID.DD2_DarkMageCastHeal;
                                    style.Volume = 0.5f;
                                    SoundEngine.PlaySound(style, NPC.Center);
                                    for (int i = -1; i < 2; i++)
                                    {
                                        if (i == 0)
                                            continue;
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(200 * i, -200), Vector2.Zero, ModContent.ProjectileType<StarineSlushSmall>(), 12, .1f, Main.myPlayer);
                                    }
                                }
                                if (AITimer == 90)
                                {
                                    SoundStyle style = SoundID.DD2_DarkMageCastHeal;
                                    style.Volume = 0.5f;
                                    SoundEngine.PlaySound(style, NPC.Center);
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - new Vector2(0, 100), Vector2.Zero, ModContent.ProjectileType<StarineSlush>(), 12, .1f, Main.myPlayer);
                                    for (int i = -1; i < 2; i++)
                                    {
                                        if (i == 0)
                                            continue;
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(100 * i, -250), Vector2.Zero, ModContent.ProjectileType<StarineSlushSmall>(), 12, .1f, Main.myPlayer);
                                    }
                                }
                            }
                            else
                            {
                                if (AITimer == 30)
                                {
                                    SoundStyle style = SoundID.DD2_DarkMageCastHeal;
                                    style.Volume = 0.5f;
                                    SoundEngine.PlaySound(style, NPC.Center);
                                    for (int i = 1; i <= 3; i++)
                                    {
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(100 * i, 110 * -i), Vector2.Zero, ModContent.ProjectileType<StarineSlushSmall>(), 12, .1f, Main.myPlayer);
                                    }
                                    for (int i = -3; i <= -1; i++)
                                    {
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(100 * i, 110 * i), Vector2.Zero, ModContent.ProjectileType<StarineSlushSmall>(), 12, .1f, Main.myPlayer);
                                    }
                                }
                                NPC.velocity *= 0.9f;
                                if (AITimer == 130)
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - new Vector2(0, 420), Vector2.UnitY, ModContent.ProjectileType<StarineSlushTelegraph>(), 0, 0);
                                if (AITimer == 150)
                                    NPC.velocity = new Vector2(15, 0);
                                if (AITimer == 170)
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - new Vector2(0, 420), Vector2.UnitY, ModContent.ProjectileType<StarineSlushBeam>(), 30, 0);
                            }
                            if (AITimer == 300)
                            {
                                AITimer = 0;
                                NPC.frameCounter = 0;
                                //Next = AIState.StarineShafts;
                                SwitchToRandom();
                                SwitchTo(AIState.Idle);
                            }
                            break;
                        }
                    case AIState.StarineSwipe:
                        {
                            NPC.velocity *= .9f;
                            if (AITimer < 60 && AITimer > 30)
                                NPC.velocity = (player.Center - NPC.Center) / 40f;
                            if (AITimer == 1) savedPos = NPC.Center;
                            if (AITimer < 60 && AITimer > 1)
                            {
                                NPC.Center = savedPos + Main.rand.NextVector2Circular(AITimer * 0.05f, AITimer * 0.05f);
                            }
                            if (AITimer == 30)
                            {
                                Projectile.NewProjectile(Projectile.InheritSource(NPC), NPC.Center, Vector2.Zero, ModContent.ProjectileType<RingTelegraph>(), 0, 0);
                                for (int i = 4; i <= 360; i += 4)
                                {
                                    Vector2 dVel = MathHelper.ToRadians(i).ToRotationVector2() * 6f;
                                    Dust dust = Dust.NewDustDirect(NPC.Center, 1, 1, ModContent.DustType<StarineDust>(), dVel.X, dVel.Y);
                                    dust.noGravity = true;
                                }
                            }
                            if (AITimer == 60)
                            {
                                Projectile.NewProjectile(Projectile.InheritSource(NPC), NPC.Center, Vector2.Zero, ModContent.ProjectileType<RingTelegraph>(), 0, 0);
                                SoundEngine.PlaySound(SoundID.NPCHit5, NPC.Center);
                                for (int i = 4; i <= 360; i += 4)
                                {
                                    Vector2 dVel = MathHelper.ToRadians(i).ToRotationVector2() * 6f;
                                    Dust dust = Dust.NewDustDirect(NPC.Center, 1, 1, ModContent.DustType<StarineDust>(), dVel.X, dVel.Y);
                                    dust.noGravity = true;
                                }
                                NPC.Center = player.Center + new Vector2(150 * Main.rand.Next(new int[] { 1, -1 }), -Main.rand.NextFloat(105));
                                //NPC.rotation = Helper.FromAToB(NPC.Center, player.Center).X - MathHelper.PiOver2 + (NPC.direction == 1 ?MathHelper.Pi : 0);
                                lastPPos = player.Center;
                                NPC.rotation = (NPC.Center - player.Center).ToRotation();
                                if (NPC.Center.X < player.Center.X)
                                    NPC.rotation += MathHelper.Pi;
                            }
                            if (AITimer == 90)
                            {
                                SoundEngine.PlaySound(SoundID.DD2_SonicBoomBladeSlash, NPC.Center);
                                for (int i = 0; i < 20; i++)
                                {
                                    int dust = Dust.NewDust(NPC.Center + new Vector2(11 * -NPC.spriteDirection, 11), 1, 1, ModContent.DustType<StarineDust>(), 2f);
                                    Main.dust[dust].scale = Main.rand.NextFloat(1, 2);
                                    Main.dust[dust].velocity = Helper.FromAToB(NPC.Center, lastPPos).RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(1, 5);
                                    Main.dust[dust].noGravity = true;
                                }
                                for (int i = 0; i < 10; i++)
                                {
                                    int dust = Dust.NewDust(NPC.Center + new Vector2(11 * -NPC.spriteDirection, 11), 1, 1, ModContent.DustType<StarineDust>(), 2f);
                                    Main.dust[dust].scale = Main.rand.NextFloat(1, 2);
                                    Main.dust[dust].velocity = Helper.FromAToB(NPC.Center, lastPPos).RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(5, 10);
                                    Main.dust[dust].noGravity = true;
                                }
                                Projectile a = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + new Vector2(11 * -NPC.spriteDirection, 11).RotatedBy(NPC.rotation), Helper.FromAToB(NPC.Center, lastPPos), ModContent.ProjectileType<TestTentacle2>(), 12, .1f);
                                a.ai[0] = 100;
                                a.ai[1] = 0.5f;
                            }
                            if (AITimer == 125)
                                NPC.velocity = Helper.FromAToB(player.Center, NPC.Center) * 2;
                            if (AITimer == 140)
                            {
                                AITimer = 0;
                                NPC.frameCounter = 0;
                                /*if (NPC.life < NPC.lifeMax / 2)
                                    Next = AIState.SymbolShafts;
                                else
                                    Next = AIState.StarineSigil;*/
                                SwitchToRandom();
                                SwitchTo(AIState.Idle);
                            }
                            break;
                        }
                    case AIState.StarineSigilSequel:
                        {
                            if (AITimer == 40)
                            {
                                NPC.velocity = Vector2.Zero;
                                SoundEngine.PlaySound(SoundID.NPCHit5, NPC.Center);
                                for (int i = 4; i <= 360; i += 4)
                                {
                                    Vector2 dVel = MathHelper.ToRadians(i).ToRotationVector2() * 6f;
                                    Dust dust = Dust.NewDustDirect(NPC.Center, 1, 1, ModContent.DustType<StarineDust>(), dVel.X, dVel.Y);
                                    dust.noGravity = true;
                                }
                            }
                            if (AITimer == 50)
                            {
                                SoundEngine.PlaySound(SoundID.NPCHit5, NPC.Center);
                                Projectile.NewProjectile(null, owner.Center + new Vector2(200, -100), Vector2.Zero, ModContent.ProjectileType<StarineSigil2>(), 0, 0);
                            }
                            if (AITimer == 70 && didp2)
                            {
                                SoundEngine.PlaySound(SoundID.NPCHit5, NPC.Center);
                                Projectile.NewProjectile(null, owner.Center + new Vector2(100, -150), Vector2.Zero, ModContent.ProjectileType<StarineSigil2>(), 0, 0);
                            }
                            if (AITimer == 90)
                            {
                                SoundEngine.PlaySound(SoundID.NPCHit5, NPC.Center);
                                Projectile.NewProjectile(null, owner.Center + new Vector2(-200, -100), Vector2.Zero, ModContent.ProjectileType<StarineSigil2>(), 0, 0);
                            }
                            if (AITimer == 110 && didp2)
                            {
                                SoundEngine.PlaySound(SoundID.NPCHit5, NPC.Center);
                                Projectile.NewProjectile(null, owner.Center + new Vector2(-100, -150), Vector2.Zero, ModContent.ProjectileType<StarineSigil2>(), 0, 0);
                            }
                            if (AITimer == 130)
                            {
                                SoundEngine.PlaySound(SoundID.NPCHit5, NPC.Center);
                                Projectile.NewProjectile(null, owner.Center + new Vector2(0, -200), Vector2.Zero, ModContent.ProjectileType<StarineSigil2>(), 0, 0);
                            }
                            if (AITimer == 170)
                            {
                                AITimer = 0;
                                NPC.frameCounter = 0;
                                SwitchToRandom();
                                SwitchTo(AIState.Idle);
                            }
                        }
                        break;
                    case AIState.SpringySphere:
                        {
                            NPC.velocity *= .95f;
                            NPC.rotation = 0;
                            if (AITimer < 45 && AITimer > 30)
                                NPC.velocity = Helper.FromAToB(NPC.Center, Sym.Center - new Vector2(0, 150), false) * 0.04f;
                            if (AITimer == 1) savedPos = NPC.Center;
                            if (AITimer < 60 && AITimer > 1)
                            {
                                NPC.Center = savedPos + Main.rand.NextVector2Circular(AITimer * 0.05f, AITimer * 0.05f);
                            }
                            if (AITimer > 30 && AITimer < 90 && AITimer % 15 == 0)
                                SoundEngine.PlaySound(SoundID.Item154, NPC.Center);
                            if (AITimer == 90)
                            {
                                SoundEngine.PlaySound(SoundID.DD2_SonicBoomBladeSlash, NPC.Center);
                                for (int i = 0; i < 20; i++)
                                {
                                    int dust = Dust.NewDust(NPC.Center + new Vector2(11 * -NPC.spriteDirection, 11), 1, 1, ModContent.DustType<StarineDust>(), 2f);
                                    Main.dust[dust].scale = Main.rand.NextFloat(1, 2);
                                    Main.dust[dust].velocity = Helper.FromAToB(NPC.Center, player.Center).RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(1, 5);
                                    Main.dust[dust].noGravity = true;
                                }
                                for (int i = 0; i < 10; i++)
                                {
                                    int dust = Dust.NewDust(NPC.Center + new Vector2(11 * -NPC.spriteDirection, 11), 1, 1, ModContent.DustType<StarineDust>(), 2f);
                                    Main.dust[dust].scale = Main.rand.NextFloat(1, 2);
                                    Main.dust[dust].velocity = Helper.FromAToB(NPC.Center, player.Center).RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(5, 10);
                                    Main.dust[dust].noGravity = true;
                                }
                                Projectile a = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + new Vector2(11 * -NPC.spriteDirection, 11).RotatedBy(NPC.rotation), Helper.FromAToB(NPC.Center, player.Center), ModContent.ProjectileType<BouncyBallPilgrim>(), 12, .1f);
                                a.ai[0] = didp2 ? 1 : 0;
                                NPC.velocity = Helper.FromAToB(player.Center, NPC.Center) * 5;
                            }
                            if (AITimer == 140)
                            {
                                AITimer = 0;
                                NPC.frameCounter = 0;
                                /*if (NPC.life < NPC.lifeMax / 2)
                                    Next = AIState.SymbolShafts;
                                else
                                    Next = AIState.StarineSigil;*/
                                SwitchToRandom();
                                SwitchTo(AIState.Idle);
                            }
                        }
                        break;
                    case AIState.SeekingStarineStars:
                        {
                            if (AITimer == 1)
                            {
                                NPC.velocity = Helper.FromAToB(player.Center, NPC.Center) * 20;
                            }
                            if (AITimer == 35)
                                lastPPos = player.Center;
                            NPC.velocity *= 0.9f;
                            /*if (AITimer % 5 == 0 && AITimer <= 40 && AITimer >= 20)
                            {
                                NPC.velocity = Vector2.Zero;
                                SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
                                float rot = MathHelper.ToRadians((AITimer - 30) * 5) * NPC.direction;
                                float off = AITimer <= 25 || AITimer >= 35 ? AITimer : 0;
                                Projectile.NewProjectile(null, NPC.Center + rot.ToRotationVector2() * 35, new Vector2(10 * NPC.direction, 0).RotatedBy(rot), ModContent.ProjectileType<PilgStar2>(), 0, 0, ai1: off);
                            }*/
                            if (AITimer == 40)
                            {
                                SoundEngine.PlaySound(SoundID.AbigailSummon, NPC.Center);
                                NPC.velocity = Vector2.Zero;
                                SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
                                float rot1 = MathHelper.ToRadians((-12) * 5) + Helper.FromAToB(NPC.Center, lastPPos).ToRotation();
                                float rot2 = MathHelper.ToRadians((12) * 5) + Helper.FromAToB(NPC.Center, lastPPos).ToRotation();
                                Projectile.NewProjectile(null, NPC.Center + rot1.ToRotationVector2() * 35, rot1.ToRotationVector2() * 9, ModContent.ProjectileType<PilgStar2>(), 10, 0, ai1: 10);
                                Projectile.NewProjectile(null, NPC.Center + rot2.ToRotationVector2() * 35, rot2.ToRotationVector2() * 9, ModContent.ProjectileType<PilgStar2>(), 10, 0, ai1: 10);
                            }
                            if (AITimer == 50)
                            {
                                float rot1 = MathHelper.ToRadians((-6) * 5) + Helper.FromAToB(NPC.Center, lastPPos).ToRotation();
                                float rot2 = MathHelper.ToRadians((6) * 5) + Helper.FromAToB(NPC.Center, lastPPos).ToRotation();
                                Projectile.NewProjectile(null, NPC.Center + rot1.ToRotationVector2() * 35, rot1.ToRotationVector2() * 9, ModContent.ProjectileType<PilgStar2>(), 10, 0, ai1: 10);
                                Projectile.NewProjectile(null, NPC.Center + rot2.ToRotationVector2() * 35, rot2.ToRotationVector2() * 9, ModContent.ProjectileType<PilgStar2>(), 10, 0, ai1: 10);
                            }
                            if (AITimer == 60)
                            {
                                float rot1 = Helper.FromAToB(NPC.Center, lastPPos).ToRotation();
                                Projectile.NewProjectile(null, NPC.Center + rot1.ToRotationVector2() * 35, rot1.ToRotationVector2() * 9, ModContent.ProjectileType<PilgStar2>(), 10, 0, ai1: 10);
                            }
                            if (AITimer == 90)
                            {
                                AITimer = 0;
                                NPC.frameCounter = 0;
                                SwitchToRandom();
                                SwitchTo(AIState.Idle);
                            }
                        }
                        break;
                    case AIState.SymbolShafts:
                        {
                            if (AITimer == -20)
                            {
                                SoundEngine.PlaySound(SoundID.AbigailUpgrade);

                            }
                            if (AITimer < 40)
                            {
                                NPC.velocity = (owner.Center - new Vector2(0, 100) - NPC.Center) / 10f;
                            }
                            else
                                NPC.velocity *= .9f;

                            if (AITimer == 40)
                            {
                                SoundEngine.PlaySound(SoundID.NPCHit5, NPC.Center);
                                for (int i = 4; i <= 360; i += 4)
                                {
                                    Vector2 dVel = MathHelper.ToRadians(i).ToRotationVector2() * 6f;
                                    Dust dust = Dust.NewDustDirect(NPC.Center, 1, 1, ModContent.DustType<StarineDust>(), dVel.X, dVel.Y);
                                    dust.noGravity = true;
                                }
                            }
                            if (AITimer == 70)
                                owner.ai[0] = 2;

                            if (AITimer == 300)
                            {
                                AITimer = 0;
                                NPC.frameCounter = 0;
                                //Next = AIState.StarineSpree;
                                SwitchToRandom();
                                SwitchTo(AIState.Idle);
                            }
                            break;
                        }
                    case AIState.StarineSpree:
                        {
                            if (AITimer < 160 && AITimer > 100)
                            {
                                if (AITimer % 10 == 0)
                                    Projectile.NewProjectile(null, NPC.Center, Vector2.Zero, ModContent.ProjectileType<RingTelegraph>(), 0, 0);
                                if (AITimer % 5 == 0)
                                    for (int i = 4; i <= 360; i += 4)
                                    {
                                        Vector2 dVel = MathHelper.ToRadians(i).ToRotationVector2() * 6f;
                                        Dust dust = Dust.NewDustDirect(NPC.Center, 1, 1, ModContent.DustType<StarineDust>(), dVel.X, dVel.Y);
                                        dust.noGravity = true;
                                    }
                            }
                            if ((AITimer % 20 == 0 || AITimer == 1) && AITimer <= 100)
                            {
                                SoundEngine.PlaySound(SoundID.NPCHit5, NPC.Center);
                                for (int i = 4; i <= 360; i += 4)
                                {
                                    Vector2 dVel = MathHelper.ToRadians(i).ToRotationVector2() * 6f;
                                    Dust dust = Dust.NewDustDirect(NPC.Center, 1, 1, ModContent.DustType<StarineDust>(), dVel.X, dVel.Y);
                                    dust.noGravity = true;
                                }
                                NPC.Center = new Vector2(Sym.Center.X + (AITimer - 50) * 8, Sym.Center.Y + Main.rand.NextFloat(-15, 30));
                                Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center, Main.rand.NextVector2Circular(10, 10), ModContent.ProjectileType<StarineFlare>(), 10, 0).ai[0] = 210 + (70 * 4) - AITimer;
                            }
                            if (AITimer == 101)
                            {
                                for (int i = 4; i <= 360; i += 4)
                                {
                                    Vector2 dVel = MathHelper.ToRadians(i).ToRotationVector2() * 6f;
                                    Dust dust = Dust.NewDustDirect(NPC.Center, 1, 1, ModContent.DustType<StarineDust>(), dVel.X, dVel.Y);
                                    dust.noGravity = true;
                                }
                                NPC.Center = new Vector2(Sym.Center.X, Sym.Center.Y - 100);
                            }
                            if (AITimer > 160)
                            {
                                aitimer2++;
                            }
                            NPC.velocity *= 0.9f;
                            if (aitimer2 > 70 && NPC.ai[3] <= 1f)
                            {
                                NPC.velocity = Helper.FromAToB(player.Center, NPC.Center) * 15;
                                NPC.frame.Y = 19 * 92;
                                NPC.ai[3] += 0.25f;
                                aitimer2 = 0;
                            }
                            if (AITimer >= 160 && aitimer2 == 1 && AITimer < 180 + (70 * 4) && NPC.ai[3] < 1f)
                            {
                                NPC.frame.Y = 4 * 92;
                                savedPos = NPC.Center;
                            }
                            if (aitimer2 < 60 && NPC.ai[3] < 1f && savedPos.Distance(NPC.Center) < 100)
                            {
                                NPC.Center = savedPos + Main.rand.NextVector2Circular(aitimer2 * 0.1f, aitimer2 * 0.1f);
                            }
                            if (AITimer >= 160 && aitimer2 == 39 && AITimer < 180 + (70 * 4))
                            {
                                SoundEngine.PlaySound(SoundID.NPCHit5, NPC.Center);
                                for (int i = 4; i <= 360; i += 4)
                                {
                                    Vector2 dVel = MathHelper.ToRadians(i).ToRotationVector2() * 6f;
                                    Dust dust = Dust.NewDustDirect(NPC.Center, 1, 1, ModContent.DustType<StarineDust>(), dVel.X, dVel.Y);
                                    dust.noGravity = true;
                                }
                                lastPPos = player.Center;
                                NPC.Center = player.Center + MathHelper.ToRadians(AITimer * 2 - 110).ToRotationVector2() * 150f;
                            }
                            if (AITimer >= 160 && aitimer2 == 64 && AITimer < 180 + (70 * 4))
                            {
                                NPC.frame.Y = 8 * 92;
                                SoundEngine.PlaySound(SoundID.DD2_SonicBoomBladeSlash.WithVolumeScale(1.25f), NPC.Center);
                                for (int i = 0; i < 10; i++)
                                {
                                    int dust = Dust.NewDust(NPC.Center + new Vector2(11 * -NPC.spriteDirection, 11), 1, 1, ModContent.DustType<StarineDust>(), 2f);
                                    Main.dust[dust].scale = Main.rand.NextFloat(1, 2);
                                    Main.dust[dust].velocity = Helper.FromAToB(NPC.Center, lastPPos).RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(1, 5);
                                    Main.dust[dust].noGravity = true;
                                }
                                for (int i = 0; i < 10; i++)
                                {
                                    int dust = Dust.NewDust(NPC.Center + new Vector2(11 * -NPC.spriteDirection, 11), 1, 1, ModContent.DustType<StarineDust>(), 2f);
                                    Main.dust[dust].scale = Main.rand.NextFloat(1, 2);
                                    Main.dust[dust].velocity = Helper.FromAToB(NPC.Center, lastPPos).RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(5, 10);
                                    Main.dust[dust].noGravity = true;
                                }
                                Projectile a = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + new Vector2(11 * -NPC.spriteDirection, 11), Helper.FromAToB(NPC.Center, lastPPos), ModContent.ProjectileType<TestTentacle2>(), 12, .1f);
                                a.ai[0] = 40;
                                a.ai[1] = 0.5f;
                                a.timeLeft = 100;
                            }
                            if (AITimer == 210 + (70 * 4))
                            {
                                aitimer2 = 0;
                                AITimer = 0;
                                NPC.velocity = Vector2.Zero;
                                NPC.frameCounter = 0;
                                /*if (NPC.ai[2] == 0)
                                {
                                    Next = StateBeforeP2;
                                    NPC.ai[2] = 1;
                                }
                                else
                                    Next = AIState.StarineSigil;*/
                                SwitchToRandom();
                                SwitchTo(AIState.Idle);
                            }
                            break;
                        }
                }
            }
            else
            {
                AITimer++;
                NPC.Center = lastPPos + Main.rand.NextVector2Circular(AITimer * 0.1f, AITimer * 0.1f);
                if (AITimer >= 60)
                {
                    for (int i = 0; i < 30; i++)
                    {
                        Helper.SpawnDust(NPC.Center + Main.rand.NextVector2Circular(NPC.width * 0.25f, NPC.height * 0.25f), NPC.Size / 2, ModContent.DustType<StarineDust>(), Main.rand.NextVector2Unit() * Main.rand.NextFloat(5f));
                    }
                    NPC.active = false;
                }
            }
        }
        float aitimer2;
        Vector2 lastPPos;
    }
}