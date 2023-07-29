
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

namespace MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim
{
    public class RupturedPilgrim : ModNPC
    {
        public static NPC Sym => Starine_Symbol.symbol;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ruptured Pilgrim");
            Main.npcFrameCount[NPC.type] = 24;
            NPCID.Sets.TrailCacheLength[NPC.type] = 9;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new(0) { Velocity = 1 };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
            NPC.AddElement(CrossModHelper.Celestial);
            NPC.AddElement(CrossModHelper.Arcane);
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
            NPC.width = 54;
            NPC.height = 70;
            NPC.lifeMax = 1600;
            NPC.boss = true;
            NPC.defense = 18;
            NPC.damage = 0;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;
            NPC.npcSlots = 7f;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit49;
            NPC.DeathSound = SoundID.NPCDeath52;
            NPC.noTileCollide = true;
            NPC.ai[0] = 6;
            NPC.alpha = 255;
            if (!Main.dedServ) Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Ruptured");
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * bossAdjustment * balance);
        }
        bool hasDoneDeathDrama;
        public int Direction;
        public override void FindFrame(int frameHeight)
        {

            if (State != AIState.TentacleP2 || AITimer >= 100)
            {
                NPC.frameCounter++;
            }
            if (State == 0)
            {
                if (AITimer < 60)
                {
                    if (AITimer > 35)
                        NPC.frame.Y = (int)((NPC.frameCounter / 5) + 15) * frameHeight;
                    else
                    {
                        if (AITimer == 35)
                            NPC.frameCounter = 0;
                        else
                        {
                            if (NPC.frameCounter >= 19)
                                NPC.frameCounter = 0;

                            NPC.frame.Y = (int)(NPC.frameCounter / 5) * frameHeight;
                        }
                    }
                }
                else
                {
                    if (AITimer == 60)
                        NPC.frameCounter = 0;
                    else
                    {
                        if (NPC.frameCounter >= 19)
                            NPC.frameCounter = 0;

                        NPC.frame.Y = (int)(NPC.frameCounter / 5) * frameHeight;
                    }
                }
            }
            if (State == AIState.Spawn || State == AIState.Idle)
            {
                if (NPC.frameCounter >= 19)
                    NPC.frameCounter = 0;

                NPC.frame.Y = (int)(NPC.frameCounter / 5) * frameHeight;
            }
            /*if (State == AIState.StarineSpree)
            {
                if (AITimer < 150)
                {
                    if (AITimer > 100)
                        NPC.frame.Y = (int)(((AITimer - 100) / 6) + 4) * frameHeight;
                    else
                    {
                        if (AITimer == 100)
                            NPC.frameCounter = 0;
                        else
                        {
                            if (AITimer % 20 == 0)
                                NPC.frameCounter = Main.rand.Next(0, 19);

                            NPC.frame.Y = (int)NPC.frameCounter * frameHeight;
                        }
                    }
                }
                else
                {
                    if (NPC.frameCounter >= 7)
                        NPC.frameCounter = 0;
                    NPC.frame.Y = frameHeight * (NPC.frameCounter >= 4 ? 11 : 12);
                }
            }*/
            if (State == (AIState)1)
            {
                if (AITimer < 60)
                {
                    if (AITimer > 20)
                        NPC.frame.Y = (int)(((AITimer - 20) / 5) + 4) * frameHeight;
                    else
                    {
                        if (AITimer == 20)
                            NPC.frameCounter = 0;
                        else
                        {
                            NPC.frame.Y = (int)(AITimer / 5) * frameHeight;
                        }
                    }
                }
                else
                {
                    if (NPC.frameCounter >= 7)
                        NPC.frameCounter = 0;
                    NPC.frame.Y = frameHeight * (NPC.frameCounter >= 4 ? 11 : 12);
                }
            }
            if (State == AIState.StarineStars)
            {
                if (AITimer < 60)
                {
                    if (AITimer > 20)
                        NPC.frame.Y = (int)(((AITimer - 20) / 5) + 4) * frameHeight;
                    else
                    {
                        if (AITimer == 20)
                            NPC.frameCounter = 0;
                        else
                        {
                            NPC.frame.Y = (int)(AITimer / 5) * frameHeight;
                        }
                    }
                }
                else
                {
                    if (NPC.frameCounter >= 19)
                        NPC.frameCounter = 0;

                    NPC.frame.Y = (int)(NPC.frameCounter / 5) * frameHeight;
                }
            }
            if (State == (AIState)2 || State == (AIState)3)
            {
                if (AITimer < 60)
                {
                    if (AITimer > 35)
                        NPC.frame.Y = (int)((NPC.frameCounter / 5) + 15) * frameHeight;
                    else
                    {
                        if (AITimer == 35)
                            NPC.frameCounter = 0;
                        else
                        {
                            if (NPC.frameCounter >= 19)
                                NPC.frameCounter = 0;

                            NPC.frame.Y = (int)(NPC.frameCounter / 5) * frameHeight;
                        }
                    }
                }
                else
                {
                    if (AITimer == 60)
                        NPC.frameCounter = 0;
                    else
                    {
                        if (NPC.frameCounter >= 19)
                            NPC.frameCounter = 0;

                        NPC.frame.Y = (int)(NPC.frameCounter / 5) * frameHeight;
                    }
                }
            }
            if (State == AIState.Death)
            {
                NPC.velocity.X = NPC.velocity.Y *= 0.9f;
                if (NPC.frameCounter < 5)
                {
                    NPC.frame.Y = 20 * frameHeight;
                    for (int i = 0; i < 2; i++)
                    {
                        int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<StarineDust>());
                        Main.dust[dust].velocity = Main.rand.NextVector2Unit() * 2f;
                        Main.dust[dust].scale = 1f;
                        Main.dust[dust].noGravity = true;
                    }
                }
                else if (NPC.frameCounter < 40)
                {
                    NPC.frame.Y = 21 * frameHeight;
                    for (int i = 0; i < 5; i++)
                    {
                        int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<StarineDust>());
                        Main.dust[dust].velocity = Main.rand.NextVector2Unit() * 2f;
                        Main.dust[dust].scale = 1f * Main.rand.Next(1, 2);
                        Main.dust[dust].noGravity = true;
                    }
                }
                else if (NPC.frameCounter < 75)
                {
                    NPC.frame.Y = 22 * frameHeight;
                    for (int i = 0; i < 8; i++)
                    {
                        int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<StarineDust>());
                        Main.dust[dust].velocity = Main.rand.NextVector2Unit() * 2f;
                        Main.dust[dust].scale = 2f;
                        Main.dust[dust].noGravity = true;
                    }
                }
                else if (NPC.frameCounter < 110)
                {
                    NPC.frame.Y = 23 * frameHeight;
                    for (int i = 0; i < 15; i++)
                    {
                        int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<StarineDust>());
                        Main.dust[dust].velocity = Main.rand.NextVector2Unit() * 2f;
                        Main.dust[dust].scale = 1f * Main.rand.Next(2, 3);
                        Main.dust[dust].noGravity = true;
                    }
                }
                else if (!hasDoneDeathDrama)
                {
                    hasDoneDeathDrama = true;
                    owner.ai[0] = 3;
                    Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, new Vector2(), ModContent.ProjectileType<PilgrimExplosion>(), 0, 100);
                    NPC.life = 0;
                    NPC.checkDead();
                    if (Main.netMode != NetmodeID.Server)
                    {
                        Helper.SpawnGore(NPC, "MythosOfMoonlight/Starine", Main.rand.Next(4, 5));
                        Helper.SpawnGore(NPC, "MythosOfMoonlight/PurpFabric", 1, 1);
                        Helper.SpawnGore(NPC, "MythosOfMoonlight/PurpFabric", 2, 2);
                        Helper.SpawnGore(NPC, "MythosOfMoonlight/PurpMagFabric", 1, 1);
                        Helper.SpawnGore(NPC, "MythosOfMoonlight/PurpMagFabric", 2, 2);
                    }
                    SoundEngine.PlaySound(SoundID.Item62, NPC.Center);
                    for (int i = 0; i < 80; i++)
                    {
                        int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<StarineDust>());
                        Main.dust[dust].velocity = Main.rand.NextVector2Unit() * 4f;
                        Main.dust[dust].scale = 1f * Main.rand.Next(2, 3);
                        Main.dust[dust].noGravity = true;
                    }
                    for (int a = 0; a < 5; a++)
                    {
                        Vector2 speed2 = Main.rand.NextVector2Unit(MathHelper.Pi / 4, MathHelper.Pi / 2);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, -speed2 * 4.5f, ModContent.ProjectileType<StarineShaft>(), 10, 0);
                    }
                }
            }
        }
        private enum AIState
        {
            StarineSigil,
            StarineSwipe,
            SymbolLaser,
            StarineShafts,
            TentacleP2,
            Death,
            Spawn,
            Idle,
            StarineStars,
            StarineSlush,
            StarineSpree
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((int)Next);
            writer.Write(aitimer2);
            writer.WriteVector2(lastPPos);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Next = (AIState)reader.ReadSingle();
            aitimer2 = reader.ReadSingle();
            lastPPos = reader.ReadVector2();
        }
        private AIState State
        {
            get { return (AIState)(int)NPC.ai[0]; }
            set { NPC.ai[0] = (int)value; }
        }
        AIState Next = AIState.StarineSigil;
        public float AITimer
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }
        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }
        public NPC owner = null;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            var off = new Vector2(NPC.width / 2, NPC.height / 2);
            var clr = new Color(255, 255, 255, 255) * (float)((float)(255f - NPC.alpha) / 255f);
            var drawPos = NPC.Center - screenPos;
            var origTexture = TextureAssets.Npc[NPC.type].Value;
            var texture = ModContent.Request<Texture2D>(NPC.ModNPC.Texture + "_Trail").Value;
            var glowTexture = ModContent.Request<Texture2D>(NPC.ModNPC.Texture + "_Glow").Value;
            var frame = NPC.frame;
            var orig = frame.Size() / 2f;
            var trailLength = NPCID.Sets.TrailCacheLength[NPC.type];
            SpriteEffects flipType = NPC.spriteDirection == -1 /* or 1, idf  */ ? SpriteEffects.None : SpriteEffects.FlipHorizontally;


            {
                for (int i = 1; i < trailLength; i++)
                {
                    float _scale = MathHelper.Lerp(1f, 0.95f, (float)(trailLength - i) / trailLength);
                    var fadeMult = 1f / trailLength;
                    spriteBatch.Draw(texture, NPC.oldPos[i] - screenPos + off, frame, clr * (1f - fadeMult * i), NPC.oldRot[i], orig, _scale, flipType, 0f);
                }
            }
            const float TwoPi = (float)Math.PI * 2f;
            float scale = (float)Math.Sin(Main.GlobalTimeWrappedHourly * TwoPi / 2f) * 0.3f + 0.7f;
            if (NPC.life <= NPC.lifeMax / 2)
                for (int i = 0; i < 4; i++)
                    spriteBatch.Draw(texture, drawPos + (Vector2.UnitX * 10 * scale).RotatedBy(MathHelper.ToRadians((90) * i)), frame, Color.Cyan * 0.5f * scale, NPC.rotation, orig, NPC.scale, flipType, 0);
            spriteBatch.Draw(origTexture, drawPos, frame, drawColor, NPC.rotation, orig, NPC.scale, flipType, 0f);
            spriteBatch.Draw(glowTexture, drawPos, frame, clr, NPC.rotation, orig, NPC.scale, flipType, 0f);
            return false;
        }
        public override bool CheckDead()
        {
            if (NPC.life <= 0 && !hasDoneDeathDrama)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath52, NPC.Center);
                NPC.life = 1;
                NPC.frameCounter = 0;
                NPC.immortal = true;
                SwitchTo(AIState.Death);
                return false;
            }
            return true;
        }
        bool didp2;

        private void SwitchTo(AIState state)
        {
            State = state;
        }
        public override void AI()
        {
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
                    SoundEngine.PlaySound(SoundID.NPCDeath51);
                    SwitchTo(AIState.SymbolLaser);
                    NPC.velocity = Vector2.Zero;
                    aitimer2 = 0;
                    Next = AIState.SymbolLaser;
                    AITimer = -60;
                    didp2 = true;
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
            if (State != AIState.StarineSwipe && State != AIState.TentacleP2)
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
            foreach (NPC NPC in Main.npc)
            {
                if (NPC.type == ModContent.NPCType<Starine_Symbol>())
                    owner = NPC;
            }
            if (!player.active || player.dead)
            {
                NPC.active = false;
                owner.active = false;
            }
            if (Sym != owner && Sym.active)
                owner = Sym;
            if (owner == null || !owner.active)
                NPC.active = false;
            AITimer++;
            switch (State)
            {
                case AIState.Idle:
                    {
                        NPC.velocity = (player.Center + new Vector2(AITimer > 150 ? 50 : -50, 0) - NPC.Center) / 30f;
                        if (AITimer >= NPC.life / 7)
                        {
                            NPC.frameCounter = 0;
                            AITimer = 0;
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
                            Next = (AIState.StarineSigil);
                            SwitchTo(AIState.Idle);
                        }
                        break;
                    }
                case AIState.StarineSigil:
                    {
                        if (AITimer < 60)
                        {
                            NPC.velocity = (player.Center + new Vector2(NPC.Center.X > player.Center.X ? 100 : -100, 0) - NPC.Center) / 20f;
                        }
                        if (AITimer == 60)
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
                        if (AITimer == 90)
                        {
                            if (Main.rand.NextBool(2))
                            {
                                Vector2 pos = Sym.Center - new Vector2(-300, 100);
                                Projectile a = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), pos, Vector2.Zero, ModContent.ProjectileType<StarineSigil>(), 12, .1f, ai0: -1);
                                a.ai[0] = -1;
                                a.ai[1] = -50;
                            }
                            else
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
                            Next = AIState.StarineStars;
                            SwitchTo(AIState.Idle);
                        }
                        break;
                    }
                case AIState.StarineStars:
                    {
                        if (AITimer == 30)
                            SoundEngine.PlaySound(SoundID.AbigailSummon, NPC.Center);
                        if (AITimer % 30 == 0 && AITimer <= 60)
                        {
                            for (int i = 4; i <= 360; i += 4)
                            {
                                Vector2 dVel = MathHelper.ToRadians(i).ToRotationVector2() * 6f;
                                Dust dust = Dust.NewDustDirect(NPC.Center, 1, 1, ModContent.DustType<StarineDust>(), dVel.X, dVel.Y);
                                dust.noGravity = true;
                            }
                            NPC.Center = owner.Center - Vector2.UnitY * 100;
                        }
                        if (AITimer == 80)
                        {
                            lastPPos = player.Center;
                            Vector2 pos = owner.Center;
                            bool hasReflected = false;
                            bool outside = false;
                            int times = 0;
                            Vector2 vel = Helper.FromAToB(NPC.Center, player.Center) * 30;
                            while (times < 6 + (Main.getGoodWorld ? 5 : 0))
                            {
                                if (Vector2.Distance(owner.Center, pos) > 420)
                                {
                                    if (!outside)
                                    {
                                        times++;
                                        outside = true;
                                        if (!hasReflected)
                                        {
                                            vel = -vel.RotatedBy((MathHelper.ToRadians(18 / (Main.getGoodWorld ? 2 : 1))));
                                            hasReflected = true;
                                        }
                                        else
                                        {
                                            vel = -vel.RotatedBy((MathHelper.ToRadians(36 / (Main.getGoodWorld ? 2 : 1))));
                                        }
                                    }
                                }
                                else
                                    outside = false;
                                Dust.NewDustPerfect(pos, ModContent.DustType<StarineDustAlt>(), Vector2.Zero).noGravity = true;
                                pos += vel;
                            }
                        }
                        if (AITimer == 100)
                        {
                            if (Main.getGoodWorld)
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
                            }
                        }
                        if (AITimer == 250)
                        {
                            AITimer = 0;
                            NPC.frameCounter = 0;
                            Next = AIState.StarineSlush;
                            SwitchTo(AIState.Idle);
                        }
                        break;
                    }
                case AIState.StarineShafts:
                    {
                        if (AITimer < 60)
                            NPC.velocity = (owner.Center - new Vector2(0, 50) - NPC.Center) / 10f;
                        else
                            NPC.velocity *= .9f;

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
                            SwitchTo(AIState.StarineSwipe);
                        }
                        break;
                    }
                case AIState.StarineSlush:
                    {
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
                        if (AITimer == 30)
                        {
                            SoundStyle style = SoundID.Item8;
                            style.Volume = 0.5f;
                            SoundEngine.PlaySound(style, NPC.Center);
                            for (int i = -1; i < 2; i++)
                            {
                                if (i == 0)
                                    continue;
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(400 * i, -200), Vector2.Zero, ModContent.ProjectileType<StarineSlushSmall>(), 12, .1f, Main.myPlayer);
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
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(300 * i, -250), Vector2.Zero, ModContent.ProjectileType<StarineSlushSmall>(), 12, .1f, Main.myPlayer);
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
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(200 * i, -300), Vector2.Zero, ModContent.ProjectileType<StarineSlushSmall>(), 12, .1f, Main.myPlayer);
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
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(100 * i, -350), Vector2.Zero, ModContent.ProjectileType<StarineSlushSmall>(), 12, .1f, Main.myPlayer);
                            }
                        }
                        if (AITimer == 340 + 100)
                        {
                            AITimer = 0;
                            NPC.frameCounter = 0;
                            Next = AIState.StarineShafts;
                            SwitchTo(AIState.Idle);
                        }
                        break;
                    }
                case AIState.StarineSwipe:
                    {
                        NPC.velocity *= .9f;
                        if (AITimer < 60 && AITimer > 30)
                            NPC.velocity = (player.Center - NPC.Center) / 40f;
                        if (AITimer == 30)
                        {
                            SoundEngine.PlaySound(SoundID.NPCHit5, NPC.Center);
                            for (int i = 4; i <= 360; i += 4)
                            {
                                Vector2 dVel = MathHelper.ToRadians(i).ToRotationVector2() * 6f;
                                Dust dust = Dust.NewDustDirect(NPC.Center, 1, 1, ModContent.DustType<StarineDust>(), dVel.X, dVel.Y);
                                dust.noGravity = true;
                            }
                            NPC.Center = player.Center + Main.rand.NextFloat(0, 3.14f).ToRotationVector2() * -150f;
                        }
                        if (AITimer == 60)
                            lastPPos = player.Center;
                        if (AITimer == 90)
                        {

                            SoundEngine.PlaySound(SoundID.DD2_SonicBoomBladeSlash, NPC.Center);
                            Projectile a = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + new Vector2(11 * NPC.spriteDirection, 11), Helper.FromAToB(NPC.Center, lastPPos), ModContent.ProjectileType<TestTentacle2>(), 12, .1f);
                            a.ai[0] = 100;
                            a.ai[1] = 0.5f;
                        }
                        if (AITimer == 120)
                        {
                            AITimer = 0;
                            NPC.frameCounter = 0;
                            if (NPC.life < NPC.lifeMax / 2)
                                Next = AIState.SymbolLaser;
                            else
                                Next = AIState.StarineSigil;
                            SwitchTo(AIState.Idle);
                        }
                        break;
                    }
                case AIState.SymbolLaser:
                    {
                        if (AITimer == -20)
                        {
                            SoundEngine.PlaySound(SoundID.AbigailUpgrade);

                        }
                        if (AITimer < 40)
                            NPC.velocity = (owner.Center - new Vector2(0, 100) - NPC.Center) / 10f;
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

                        if (AITimer == 150)
                        {
                            AITimer = 0;
                            NPC.frameCounter = 0;
                            Next = AIState.StarineSpree;
                            SwitchTo(AIState.Idle);
                        }
                        break;
                    }
                case AIState.StarineSpree:
                    {
                        if (AITimer < 160 && AITimer > 100 && AITimer % 5 == 0)
                            for (int i = 4; i <= 360; i += 4)
                            {
                                Vector2 dVel = MathHelper.ToRadians(i).ToRotationVector2() * 6f;
                                Dust dust = Dust.NewDustDirect(NPC.Center, 1, 1, ModContent.DustType<StarineDust>(), dVel.X, dVel.Y);
                                dust.noGravity = true;
                            }
                        if (AITimer % 20 == 0 && AITimer < 100)
                        {
                            NPC.frame.Y = 78 * Main.rand.Next(19);
                            SoundEngine.PlaySound(SoundID.NPCHit5, NPC.Center);
                            for (int i = 4; i <= 360; i += 4)
                            {
                                Vector2 dVel = MathHelper.ToRadians(i).ToRotationVector2() * 6f;
                                Dust dust = Dust.NewDustDirect(NPC.Center, 1, 1, ModContent.DustType<StarineDust>(), dVel.X, dVel.Y);
                                dust.noGravity = true;
                            }
                            NPC.Center = player.Center + MathHelper.ToRadians(Main.rand.Next(new int[] { 180, 270, 360 })).ToRotationVector2() * 150f;
                            Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center + Main.rand.NextVector2Circular(NPC.width, NPC.height), Vector2.Zero, ModContent.ProjectileType<StarineFlare>(), 20, 0).ai[0] = 210 + (65 * 4) - AITimer;
                        }
                        if (AITimer > 160)
                        {
                            NPC.frame.Y = 78 * 11;
                            aitimer2++;
                        }
                        if (aitimer2 > 65)
                            aitimer2 = 0;
                        if (AITimer >= 160 && aitimer2 == 40 && AITimer < 180 + (65 * 4))
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
                        if (AITimer >= 160 && aitimer2 == 65 && AITimer < 180 + (65 * 4))
                        {

                            SoundEngine.PlaySound(SoundID.DD2_SonicBoomBladeSlash, NPC.Center);
                            Projectile a = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + new Vector2(11 * NPC.spriteDirection, 11), Helper.FromAToB(NPC.Center, lastPPos), ModContent.ProjectileType<TestTentacle2>(), 12, .1f);
                            a.ai[0] = 40;
                            a.ai[1] = 0.5f;
                            a.timeLeft = 100;
                        }
                        if (AITimer == 210 + (65 * 4))
                        {
                            aitimer2 = 0;
                            AITimer = 0;
                            NPC.frameCounter = 0;
                            Next = AIState.StarineSigil;
                            SwitchTo(AIState.Idle);
                        }
                        break;
                    }
            }
        }
        float aitimer2;
        Vector2 lastPPos;
    }
}