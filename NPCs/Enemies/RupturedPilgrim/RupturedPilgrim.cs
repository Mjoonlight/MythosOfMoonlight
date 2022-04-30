
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.NPCs.Enemies.RupturedPilgrim.Projectiles;
using Terraria.ID;
using MythosOfMoonlight.Dusts;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;

namespace MythosOfMoonlight.NPCs.Enemies.RupturedPilgrim
{
    public class RupturedPilgrim : ModNPC
    {
        static NPC Sym => Starine_Symbol.symbol;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ruptured Pilgrim");
            Main.npcFrameCount[NPC.type] = 24;
            NPCID.Sets.TrailCacheLength[NPC.type] = 9;
            NPCID.Sets.TrailingMode[NPC.type] = 1;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new(0) { Velocity = 1 };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetDefaults()
        {
            NPC.width = 54;
            NPC.height = 70;
            NPC.lifeMax = 1100;
            NPC.defense = 12;
            NPC.damage = 0;
            NPC.aiStyle = 0;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit49;
            NPC.DeathSound = SoundID.NPCDeath52;
            NPC.noTileCollide = true;
            NPC.ai[0] = 6;
            NPC.alpha = 255;
        }
        bool hasDoneDeathDrama;
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
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
            if (State == AIState.Spawn)
            {
                if (NPC.frameCounter >= 19)
                    NPC.frameCounter = 0;

                NPC.frame.Y = (int)(NPC.frameCounter / 5) * frameHeight;
            }
            if (State == (AIState)1 || State == AIState.TentacleP2)
            {
                if (AITimer < 30)
                {
                    if (AITimer > 20)
                        NPC.frame.Y = (int)((NPC.frameCounter / 5) + 8) * frameHeight;
                    else
                    {
                        if (AITimer == 30)
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
                    if (AITimer == 30)
                        NPC.frameCounter = 0;
                    else
                    {
                        if (AITimer < 90)
                        {
                            if (AITimer > 80)
                                NPC.frame.Y = (int)((NPC.frameCounter / 5) + 10) * frameHeight;
                            else
                            {
                                if (AITimer == 80)
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
                            if (State == AIState.TentacleP1 ? (AITimer <= (NPC.life < NPC.lifeMax * .5f ? 190 : 220)) : AITimer <= 290)
                            {
                                NPC.frameCounter = 0;
                                NPC.frame.Y = 12 * frameHeight;
                            }
                        }
                    }
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
                    Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, new Vector2(), ModContent.ProjectileType<PilgrimExplosion>(), 100, 100);
                    NPC.life = 0;
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
            TentacleP1,
            SymbolLaser,
            ArrowExplosion,
            TentacleP2,
            Death,
            Spawn
        }
        private AIState State
        {
            get { return (AIState)(int)NPC.ai[0]; }
            set { NPC.ai[0] = (int)value; }
        }
        private void SwitchTo(AIState state)
        {
            State = state;
        }
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

            if (NPC.life <= NPC.lifeMax / 2)
            {
                for (int i = 1; i < trailLength; i++)
                {
                    float scale = MathHelper.Lerp(1f, 0.95f, (float)(trailLength - i) / trailLength);
                    var fadeMult = 1f / trailLength;
                    spriteBatch.Draw(texture, NPC.oldPos[i] - screenPos + off, frame, clr * (1f - fadeMult * i), NPC.oldRot[i], orig, scale, flipType, 0f);
                }
            }
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
        public override void AI()
        {
            NPC.TargetClosest(true);
            if (State != AIState.TentacleP1 && State != AIState.TentacleP2)
            {
                NPC.spriteDirection = NPC.direction;
                NPC.FaceTarget();
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
            if (owner == null || !owner.active)
                NPC.active = false;

            AITimer++;
            switch (State)
            {
                case AIState.Spawn:
                    {
                        NPC.alpha -= 5;
                        NPC.velocity *= 0;
                        if (NPC.alpha > 0)
                        {
                            for (int i = 1; i <= 6; i++)
                            {
                                Vector2 pos = NPC.Center + new Vector2(90, 0).RotatedBy(Main.rand.NextFloat(0, 6.28f));
                                Vector2 vel = Vector2.Normalize(pos - NPC.Center) * -9f;
                                Dust dust = Dust.NewDustDirect(pos, 1, 1, DustID.FireworkFountain_Blue);
                                dust.color = Color.Cyan;
                                dust.velocity = vel;
                                dust.scale = .8f;
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
                            SwitchTo(AIState.StarineSigil);
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
                                Vector2 pos = Vector2.Distance(NPC.Center - new Vector2(0, 200), ((Starine_Symbol)Sym.ModNPC).CircleCenter) <= 400 ? NPC.Center - new Vector2(0, 200) : Utils.SafeNormalize(NPC.Center - new Vector2(0, 200) - ((Starine_Symbol)Sym.ModNPC).CircleCenter, Vector2.Zero) * 390f + ((Starine_Symbol)Sym.ModNPC).CircleCenter;
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, Vector2.Zero, ModContent.ProjectileType<StarineSigil>(), 8, .1f);
                            }
                            else
                            {
                                Vector2 pos = Vector2.Distance(player.Center - new Vector2(0, 200), ((Starine_Symbol)Sym.ModNPC).CircleCenter) <= 400 ? player.Center - new Vector2(0, 200) : Utils.SafeNormalize(player.Center - new Vector2(0, 200) - ((Starine_Symbol)Sym.ModNPC).CircleCenter, Vector2.Zero) * 390f + ((Starine_Symbol)Sym.ModNPC).CircleCenter;
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, Vector2.Zero, ModContent.ProjectileType<StarineSigil>(), 8, .1f);
                            }
                        }
                        if (AITimer == 120)
                        {
                            AITimer = 0;
                            NPC.frameCounter = 0;
                            if (NPC.life >= NPC.lifeMax * .5f)
                                SwitchTo((AIState)Main.rand.Next(new int[] { 1, 2 }));
                            else
                            {
                                if (Main.rand.Next(100) < 20)
                                    SwitchTo(AIState.ArrowExplosion);
                                else
                                    SwitchTo((AIState)Main.rand.Next(new int[] { 1, 2, 4 }));
                            }
                        }
                        break;
                    }
                case AIState.TentacleP1:
                    {
                        NPC.velocity *= .9f;
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
                        if (AITimer == 90)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(11 * NPC.spriteDirection, 11), Utils.SafeNormalize(player.Center - NPC.Center, Vector2.UnitX), ModContent.ProjectileType<TestTentacleProj>(), 8, .1f);
                        }
                        if (NPC.life < NPC.lifeMax * .5f)
                        {
                            if (AITimer == 120)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(11 * NPC.spriteDirection, 11), Utils.SafeNormalize(player.Center - NPC.Center, Vector2.UnitX), ModContent.ProjectileType<TestTentacleProj>(), 8, .1f);
                            }
                        }
                        if (AITimer == (NPC.life >= NPC.lifeMax * .5f ? 190 : 220))
                        {
                            AITimer = 0;
                            NPC.frameCounter = 0;
                            if (NPC.life >= NPC.lifeMax * .5f)
                                SwitchTo((AIState)Main.rand.Next(new int[] { 0, 2 }));
                            else
                            {
                                if (Main.rand.Next(100) < 20)
                                    SwitchTo(AIState.ArrowExplosion);
                                else
                                    SwitchTo((AIState)Main.rand.Next(new int[] { 0, 2, 4 }));
                            }
                        }
                        break;
                    }
                case AIState.SymbolLaser:
                    {
                        if (AITimer < 60)
                            NPC.velocity = (owner.Center - new Vector2(0, 100) - NPC.Center) / 10f;
                        else
                            NPC.velocity *= .9f;

                        if (AITimer == 60)
                        {
                            SoundEngine.PlaySound(SoundID.NPCHit5, NPC.Center);
                            for (int i = 4; i <= 360; i += 4)
                            {
                                Vector2 dVel = MathHelper.ToRadians(i).ToRotationVector2() * 6f;
                                Dust dust = Dust.NewDustDirect(NPC.Center, 1, 1, ModContent.DustType<StarineDust>(), dVel.X, dVel.Y);
                                dust.noGravity = true;
                            }
                        }
                        if (AITimer == 90)
                            owner.ai[0] = 2;

                        if (AITimer == 210)
                        {
                            AITimer = 0;
                            NPC.frameCounter = 0;
                            if (NPC.life >= NPC.lifeMax * .5f)
                                SwitchTo((AIState)Main.rand.Next(new int[] { 0, 1 }));
                            else
                            {
                                if (Main.rand.Next(100) < 20)
                                    SwitchTo(AIState.ArrowExplosion);
                                else
                                    SwitchTo((AIState)Main.rand.Next(new int[] { 0, 1, 4 }));
                            }
                        }
                        break;
                    }
                case AIState.ArrowExplosion:
                    {
                        if (AITimer < 60)
                            NPC.velocity = (owner.Center - new Vector2(0, 50) - NPC.Center) / 10f;
                        else
                            NPC.velocity *= .9f;

                        if (AITimer == 60)
                        {
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
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), owner.Center, Vector2.Zero, ModContent.ProjectileType<PilgrimExplosion>(), 10, .1f, Main.myPlayer);
                        }
                        if (AITimer == 130)
                        {
                            AITimer = 0;
                            NPC.frameCounter = 0;
                            SwitchTo((AIState)Main.rand.Next(new int[] { 0, 1, 2, 4 }));
                        }
                        break;
                    }
                case AIState.TentacleP2:
                    {
                        NPC.velocity *= .9f;
                        if (AITimer <= 30 && AITimer % 10 == 0)
                        {
                            SoundEngine.PlaySound(SoundID.NPCHit5, NPC.Center);
                            for (int i = 4; i <= 360; i += 4)
                            {
                                Vector2 dVel = MathHelper.ToRadians(i).ToRotationVector2() * 6f;
                                Dust dust = Dust.NewDustDirect(NPC.Center, 1, 1, ModContent.DustType<StarineDust>(), dVel.X, dVel.Y);
                                dust.noGravity = true;
                            }
                            NPC.Center = player.Center + MathHelper.ToRadians(Main.rand.Next(new int[] { 180, 270, 360 })).ToRotationVector2() * 150f;
                        }
                        if (AITimer == 90)
                        {
                            float ai1 = Main.rand.Next(new int[] { -1, 1 });
                            for (int i = 90; i <= 360; i += 90)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(11 * NPC.spriteDirection, 11), Vector2.UnitX.RotatedBy(MathHelper.ToRadians(i)), ModContent.ProjectileType<TestTentacleProj1>(), 8, .1f, Main.myPlayer, 0, ai1);
                            }
                        }
                        if (AITimer == 290)
                        {
                            AITimer = 0;
                            NPC.frameCounter = 0;
                            if (Main.rand.Next(100) < 20)
                                SwitchTo(AIState.ArrowExplosion);
                            else
                                SwitchTo((AIState)Main.rand.Next(new int[] { 0, 1, 2 }));
                        }
                        break;
                    }
            }
        }
    }
}