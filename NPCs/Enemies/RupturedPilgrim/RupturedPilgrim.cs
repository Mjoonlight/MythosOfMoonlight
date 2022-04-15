
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.NPCs.Enemies.RupturedPilgrim.Projectiles;
using Terraria.ID;
using MythosOfMoonlight.Dusts;
using Microsoft.Xna.Framework.Graphics;
using MythosOfMoonlight.Projectiles;

namespace MythosOfMoonlight.NPCs.Enemies.RupturedPilgrim
{
    public class RupturedPilgrim : ModNPC {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ruptured Pilgrim");
            Main.npcFrameCount[npc.type] = 24;
            NPCID.Sets.TrailCacheLength[npc.type] = 9;
            NPCID.Sets.TrailingMode[npc.type] = 1;
        }
        public override void SetDefaults()
        {
            npc.width = 54;
            npc.height = 70;
            npc.lifeMax = 1100;
            npc.defense = 12;
            npc.damage = 0;
            npc.aiStyle = 0;
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit49;
            npc.DeathSound = SoundID.NPCDeath52;
            npc.noTileCollide = true;
        }
        bool hasDoneDeathDrama;
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (State == (AIState)0) 
            {
                if (AITimer < 60)
                {
                    if (AITimer > 35)
                    {
                        npc.frame.Y = (int)((npc.frameCounter / 5) + 15) * frameHeight;
                    }
                    else
                    {
                        if (AITimer == 35)
                        {
                            npc.frameCounter = 0;
                        }
                        else
                        {
                            if (npc.frameCounter >= 19)
                            {
                                npc.frameCounter = 0;
                            }
                            npc.frame.Y = (int)(npc.frameCounter / 5) * frameHeight;
                        }
                    }
                }
                else
                {
                    if (AITimer == 60)
                    {
                        npc.frameCounter = 0;
                    }
                    else
                    {
                        if (npc.frameCounter >= 19)
                        {
                            npc.frameCounter = 0;
                        }
                        npc.frame.Y = (int)(npc.frameCounter / 5) * frameHeight;
                    }
                }
            }
            if (State == (AIState)1) 
            {
                if (AITimer < 30)
                {
                    if (AITimer > 20)
                    {
                        npc.frame.Y = (int)((npc.frameCounter / 5) + 8) * frameHeight;
                    }
                    else
                    {
                        if (AITimer == 30)
                        {
                            npc.frameCounter = 0;
                        }
                        else
                        {
                            if (npc.frameCounter >= 19)
                            {
                                npc.frameCounter = 0;
                            }
                            npc.frame.Y = (int)(npc.frameCounter / 5) * frameHeight;
                        }
                    }
                }
                else
                {
                    if (AITimer == 30)
                    {
                        npc.frameCounter = 0;
                    }
                    else
                    {
                        if (AITimer < 90)
                        {
                            if (AITimer > 80)
                            {
                                npc.frame.Y = (int)((npc.frameCounter / 5) + 10) * frameHeight;
                            }
                            else
                            {
                                if (AITimer == 80)
                                {
                                    npc.frameCounter = 0;
                                }
                                else
                                {
                                    if (npc.frameCounter >= 19)
                                    {
                                        npc.frameCounter = 0;
                                    }
                                    npc.frame.Y = (int)(npc.frameCounter / 5) * frameHeight;
                                }
                            }
                        }
                        else
                        {
                            if (AITimer <= (npc.life < npc.lifeMax * .5f ? 150 : 180))
                            {
                                npc.frameCounter = 0;
                                npc.frame.Y = 12 * frameHeight;
                            }
                            else
                            {
                                if (AITimer <= (npc.life < npc.lifeMax * .5f ? 164 : 194))
                                {
                                    npc.frame.Y = (int)((npc.frameCounter / 5) + 13) * frameHeight;
                                }
                                else
                                {
                                    if (AITimer == (npc.life < npc.lifeMax * .5f ? 165 : 195))
                                    {
                                        npc.frameCounter = 0;
                                    }
                                    else
                                    {
                                        if (npc.frameCounter >= 19)
                                        {
                                            npc.frameCounter = 0;
                                        }
                                        npc.frame.Y = (int)(npc.frameCounter / 5) * frameHeight;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (State == (AIState)2)
            {
                if (AITimer < 60)
                {
                    if (AITimer > 35)
                    {
                        npc.frame.Y = (int)((npc.frameCounter / 5) + 15) * frameHeight;
                    }
                    else
                    {
                        if (AITimer == 35)
                        {
                            npc.frameCounter = 0;
                        }
                        else
                        {
                            if (npc.frameCounter >= 19)
                            {
                                npc.frameCounter = 0;
                            }
                            npc.frame.Y = (int)(npc.frameCounter / 5) * frameHeight;
                        }
                    }
                }
                else
                {
                    if (AITimer == 60)
                    {
                        npc.frameCounter = 0;
                    }
                    else
                    {
                        if (npc.frameCounter >= 19)
                        {
                            npc.frameCounter = 0;
                        }
                        npc.frame.Y = (int)(npc.frameCounter / 5) * frameHeight;
                    }
                }
            }
            if (State == (AIState)6) {
                 npc.velocity.X = npc.velocity.Y *= 0.9f;
                if (npc.frameCounter < 5)
                {
                    npc.frame.Y = 20 * frameHeight;
                    for (int i = 0; i < 2; i++)
                    {
                        int dust = Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<StarineDust>());
                        Main.dust[dust].velocity = Main.rand.NextVector2Unit() * 2f;
                        Main.dust[dust].scale = 1f;
                        Main.dust[dust].noGravity = true;
                    }
                }
                else if (npc.frameCounter < 40)
                {
                    npc.frame.Y = 21 * frameHeight;
                    for (int i = 0; i < 5; i++)
                    {
                        int dust = Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<StarineDust>());
                        Main.dust[dust].velocity = Main.rand.NextVector2Unit() * 2f;
                        Main.dust[dust].scale = 1f * Main.rand.Next(1, 2);
                        Main.dust[dust].noGravity = true;
                    }
                }
                else if (npc.frameCounter < 75)
                {
                    npc.frame.Y = 22 * frameHeight;
                    for (int i = 0; i < 8; i++)
                    {
                        int dust = Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<StarineDust>());
                        Main.dust[dust].velocity = Main.rand.NextVector2Unit() * 2f;
                        Main.dust[dust].scale = 2f;
                        Main.dust[dust].noGravity = true;
                    }
                }
                else if (npc.frameCounter < 110)
                {
                    npc.frame.Y = 23 * frameHeight;
                    for (int i = 0; i < 15; i++)
                    {
                        int dust = Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<StarineDust>());
                        Main.dust[dust].velocity = Main.rand.NextVector2Unit() * 2f;
                        Main.dust[dust].scale = 1f * Main.rand.Next(2, 3);
                        Main.dust[dust].noGravity = true;
                    }
                }
                else if (!hasDoneDeathDrama)
                {
                    hasDoneDeathDrama = true;
                    Projectile.NewProjectileDirect(npc.Center, new Vector2(), ModContent.ProjectileType<PilgrimExplosion>(), 100, 100);
                    npc.life = 0;
                    Helper.SpawnGore(npc, "Gores/Enemies/Starine", Main.rand.Next(4, 5));
                    Helper.SpawnGore(npc, "Gores/Enemies/PurpFabric", 1, 1);
                    Helper.SpawnGore(npc, "Gores/Enemies/PurpFabric", 2, 2);
                    Helper.SpawnGore(npc, "Gores/Enemies/PurpMagFabric", 1, 1);
                    Helper.SpawnGore(npc, "Gores/Enemies/PurpMagFabric", 2, 2);
                    Main.PlaySound(SoundID.Item62, npc.Center);
                    for (int i = 0; i < 80; i++)
                    {
                        int dust = Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<StarineDust>());
                        Main.dust[dust].velocity = Main.rand.NextVector2Unit() * 4f;
                        Main.dust[dust].scale = 1f * Main.rand.Next(2, 3);
                        Main.dust[dust].noGravity = true;
                    }
                    for (int a = 0; a < 5; a++)
                    {
                        Vector2 speed2 = Main.rand.NextVector2Unit((float)MathHelper.Pi / 4, (float)MathHelper.Pi / 2);
                        Projectile.NewProjectile(npc.Center, -speed2 * 4.5f, ModContent.ProjectileType<StarineShaft>(), 10, 0);
                    }   
                }
            }
        }
        private enum AIState
        {
            StarineSigil,
            TentacleP1,
            SymbolLaser,
            PhaseSwitch,
            ArrowExplosion,
            TentacleP2,
            Death
        }
        private AIState State
        {
            get { return (AIState)(int)npc.ai[0]; }
            set { npc.ai[0] = (int)value; }
        }
        private void SwitchTo(AIState state)
        {
            State = state;
        }
        public float AITimer
        {
            get => npc.ai[1];
            set => npc.ai[1] = value;
        }
        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }
        public NPC owner = null;
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            var off = new Vector2(npc.width / 2, npc.height / 2);
            var clr = new Color(255, 255, 255, 255); // full white
            var drawPos = npc.Center - Main.screenPosition;
            var origTexture = Main.npcTexture[npc.type];
            var texture = mod.GetTexture("NPCs/Enemies/RupturedPilgrim/RupturedPilgrim_Trail");
            var glowTexture = mod.GetTexture("NPCs/Enemies/RupturedPilgrim/RupturedPilgrim_Glow");
            var frame = npc.frame;
            var orig = frame.Size() / 2f;
            var trailLength = NPCID.Sets.TrailCacheLength[npc.type];
            SpriteEffects flipType = npc.spriteDirection == 1 /* or 1, idf  */ ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            if (npc.life <= npc.lifeMax / 2)
            {
                for (int i = 1; i < trailLength; i++)
                {
                    float scale = MathHelper.Lerp(1f, 0.95f, (float)(trailLength - i) / trailLength);
                    var fadeMult = 1f / trailLength;
                    Main.spriteBatch.Draw(texture, npc.oldPos[i] - Main.screenPosition + off, frame, clr * (1f - fadeMult * i), npc.oldRot[i], orig, scale, flipType, 0f);
                }
            }
            Main.spriteBatch.Draw(origTexture, drawPos, frame, drawColor, npc.rotation, orig, npc.scale, flipType, 0f);
            Main.spriteBatch.Draw(glowTexture, drawPos, frame, clr, npc.rotation, orig, npc.scale, flipType, 0f);
            return false;
        }
        public override bool CheckDead()
        {
            if (npc.life <= 0 && !hasDoneDeathDrama)
            {
                Main.PlaySound(SoundID.NPCDeath52, npc.Center);
                npc.life = 1;
                npc.frameCounter = 0;
                npc.immortal = true;
                return false;
            }
            return true;
        }
        public override void AI()
        {
            npc.TargetClosest(true);
            npc.FaceTarget();
            npc.spriteDirection = -npc.direction;
            Player player = Main.player[npc.target];
            foreach(NPC npc in Main.npc)
            {
                if (npc.type == ModContent.NPCType<Starine_Symbol>())
                {
                    owner = npc;
                }
            }
            if (!player.active || player.dead)
            {
                npc.active = false;
                owner.active = false;
            }
            AITimer++;
            switch (State)
            {
                case AIState.StarineSigil:
                    {
                        if (AITimer < 60)
                        {
                            npc.velocity = (player.Center + new Vector2(npc.Center.X > player.Center.X ? 100 : -100, 0) - npc.Center) / 20f;
                        }
                        if (AITimer == 60)
                        {
                            Main.PlaySound(SoundID.NPCHit5, npc.Center);
                            for (int i = 4; i <= 360; i += 4)
                            {
                                Vector2 dVel = MathHelper.ToRadians(i).ToRotationVector2() * 6f;
                                Dust dust = Dust.NewDustDirect(npc.Center, 1, 1, ModContent.DustType<StarineDust>(), dVel.X, dVel.Y);
                                dust.noGravity = true;
                            }
                        }
                        if (AITimer == 90)
                        {
                            if (Main.rand.NextBool(2))
                            {
                                Projectile.NewProjectile(npc.Center - new Vector2(0, 200), Vector2.Zero, ModContent.ProjectileType<StarineSigil>(), 8, .1f);
                            }
                            else
                            {
                                Projectile.NewProjectile(player.Center - new Vector2(0, 200), Vector2.Zero, ModContent.ProjectileType<StarineSigil>(), 8, .1f);
                            }
                        }
                        if (AITimer == 120)
                        {
                            AITimer = 0;
                            npc.frameCounter = 0;
                            SwitchTo((AIState)Main.rand.Next(npc.life >= npc.lifeMax * .5f ? new int[] { 1, 2 } : new int[] { 1, 2, 3, 4, 5 }));
                        }
                        break;
                    }
                case AIState.TentacleP1:
                    {
                        npc.velocity *= .9f;
                        if (AITimer == 30)
                        {
                            Main.PlaySound(SoundID.NPCHit5, npc.Center);
                            for (int i = 4; i <= 360; i += 4)
                            {
                                Vector2 dVel = MathHelper.ToRadians(i).ToRotationVector2() * 6f;
                                Dust dust = Dust.NewDustDirect(npc.Center, 1, 1, ModContent.DustType<StarineDust>(), dVel.X, dVel.Y);
                                dust.noGravity = true;
                            }
                            npc.Center = player.Center + Main.rand.NextFloat(0, 6.28f).ToRotationVector2() * 150f;
                        }
                        if (AITimer == 90)
                        {
                            Projectile.NewProjectile(npc.Center + new Vector2(11 * npc.direction, 11), Utils.SafeNormalize(player.Center - npc.Center, Vector2.UnitX), ModContent.ProjectileType<TestTentacleProj>(), 8, .1f);
                        }
                        if (npc.life < npc.lifeMax * .5f)
                        {
                            if (AITimer == 120)
                            {
                                Projectile.NewProjectile(npc.Center + new Vector2(11 * npc.direction, 11), Utils.SafeNormalize(player.Center - npc.Center, Vector2.UnitX), ModContent.ProjectileType<TestTentacleProj>(), 8, .1f);
                            }
                        }
                        if (AITimer == (npc.life >= npc.lifeMax * .5f ? 180 : 210)) 
                        {
                            AITimer = 0;
                            npc.frameCounter = 0;
                            SwitchTo((AIState)Main.rand.Next(npc.life >= npc.lifeMax * .5f ? new int[] { 0, 2 } : new int[] { 0, 2, 3, 4, 5 }));
                        }
                        break;
                    }
                case AIState.SymbolLaser:
                    {
                        if (AITimer < 60)
                        {
                            npc.velocity = (owner.Center - new Vector2(0, 100) - npc.Center) / 10f;
                        }
                        else
                        {
                            npc.velocity *= .9f;
                        }
                        if (AITimer == 60) 
                        {
                            Main.PlaySound(SoundID.NPCHit5, npc.Center);
                            for (int i = 4; i <= 360; i += 4)
                            {
                                Vector2 dVel = MathHelper.ToRadians(i).ToRotationVector2() * 6f;
                                Dust dust = Dust.NewDustDirect(npc.Center, 1, 1, ModContent.DustType<StarineDust>(), dVel.X, dVel.Y);
                                dust.noGravity = true;
                            }
                        }
                        if (AITimer == 90)
                        {
                            owner.ai[0] = 2;
                        }
                        if (AITimer == 210)
                        {
                            AITimer = 0;
                            npc.frameCounter = 0;
                            SwitchTo((AIState)Main.rand.Next(npc.life >= npc.lifeMax * .5f ? new int[] { 0, 1 } : new int[] { 0, 1, 3, 4, 5 }));
                        }
                        break;
                    }
            }
        }
    }
}