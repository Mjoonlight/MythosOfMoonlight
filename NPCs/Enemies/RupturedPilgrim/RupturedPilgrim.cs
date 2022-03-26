using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.NPCs.Enemies.RupturedPilgrim.Projectiles;
using Terraria.ID;
using MythosOfMoonlight.Dusts;
using Microsoft.Xna.Framework.Graphics;

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
            if (AIState == Idle) {
                if (npc.frameCounter < 5) {
                    npc.frame.Y = 0 * frameHeight;
                }
                else if (npc.frameCounter < 10) {
                    npc.frame.Y = 1 * frameHeight;
                }
                else if (npc.frameCounter < 15) {
                    npc.frame.Y = 2 * frameHeight;
                }
                else if (npc.frameCounter < 20) {
                    npc.frame.Y = 3 * frameHeight;
                }
                else {
                    npc.frameCounter = 0;
                }
            }
            if (AIState == Attack) {
                npc.frame.Y = (int)(npc.frameCounter / 5 + 16) * frameHeight;
                Main.NewText((int)(npc.frameCounter / 5) + 16);
                if (npc.frameCounter > 30) { 
                    AIState = Idle;
                    AITimer = 0;
                    npc.frameCounter = 0;
                }
            }
            if (AIState == DeathDrama) {
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
                        Projectile.NewProjectile(npc.Center, -speed2 * 4.5f, ModContent.ProjectileType<StarineShaft>(), projDamage, 0);
                    }   
                }
            }
        }
        private const int AISlot = 0;
        private const int TimerSlot = 1;
        public float AIState
        {
            get => npc.ai[AISlot];
            set => npc.ai[AISlot] = value;
        }

        public float AITimer
        {
            get => npc.ai[TimerSlot];
            set => npc.ai[TimerSlot] = value;
        }
        private const int Idle = 0;
        private const int DeathDrama = -1;
        private const int Attack = 1;
        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }
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
            SpriteEffects flipType = npc.spriteDirection == -1 /* or 1, idf  */ ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

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

            var symbolTexture = mod.GetTexture("NPCs/Enemies/RupturedPilgrim/Starine_Barrier");
            var symbolFrame = symbolTexture.Bounds;
            var symbolOrig = symbolFrame.Size() / 2f;
            Main.spriteBatch.Draw(symbolTexture, origin - Main.screenPosition, symbolFrame, clr, 0f, symbolOrig, 1f, SpriteEffects.None, 0f);
            return false;
        }
        public override bool CheckDead()
        {
            if (npc.life <= 0 && !hasDoneDeathDrama)
            {
                Main.PlaySound(SoundID.NPCDeath52, npc.Center);
                npc.life = 1;
                AIState = DeathDrama;
                npc.frameCounter = 0;
                npc.immortal = true;
                return false;
            }
            return true;
        }
        float modX = 150;
        int movetimer = 0;
        int currentAttack, attackRepeat = -1;
        int projDamage = 10;
        Vector2 origin = new Vector2(-1, -1);
        const float MinBarrierDistance = 422;

        public override void AI()
        {
            float speedMod = (npc.lifeMax - npc.life * .3f) / (npc.lifeMax * .7f);
            Player player = Main.player[npc.target];
            if (origin == Vector2.One * -1)
            {
                origin = npc.position;
            }
            player.GetModPlayer<MoMPlayer>().NewCameraPosition(origin, 0.05f, npc.whoAmI);
            if (AIState == Idle)
            {
                AITimer++;
                Vector2 pos = new Vector2(player.Center.X + modX, player.position.Y + 10);
                Vector2 moveTo = pos - npc.Center;
                npc.velocity = (moveTo) * 0.08f;

                if (movetimer++ >= Main.rand.Next(400, 500))
                {
                    modX *= -1f;
                    movetimer = 0;
                }
                if (AITimer >= 155 / speedMod) {
                    AITimer = 0;
                    currentAttack = -1;
                    do currentAttack = Main.rand.Next(1, npc.life <= (npc.lifeMax / 2) ? 4 : 3); while (currentAttack == attackRepeat);
                    AIState = Attack;
                    npc.frameCounter = 0;
                    npc.velocity = Vector2.Zero;
                }
            npc.rotation = MathHelper.Clamp(npc.velocity.X * .15f, MathHelper.ToRadians(-10), MathHelper.ToRadians(10));
            }
            else if (AIState == Attack) 
            {
                if (npc.frameCounter != 26)
                {
                    if (currentAttack == 3)
                    {
                        Vector2 atk3PositionVector = new Vector2(player.Center.X, player.Center.Y - 180) - npc.Center;
                        npc.velocity = atk3PositionVector * 0.08f;
                    }
                }
                else 
                {
                    if (currentAttack == 1 && attackRepeat != 1)
                    {
                        Projectile.NewProjectile(new Vector2(player.Center.X, player.Center.Y - 230), Vector2.Zero, ModContent.ProjectileType<StarineSigil>(), 0, 0);
                        attackRepeat = 1;
                    }
                    else if (currentAttack == 2 && attackRepeat != 2)
                    {
                        Projectile.NewProjectile(npc.Center - new Vector2(0, npc.height + 45), Vector2.Zero, ModContent.ProjectileType<StarineSigil>(), 0, 0);
                        attackRepeat = 2;
                    }
                    else if (currentAttack == 3 && npc.life <= (npc.lifeMax / 2) && attackRepeat != 3)
                    {
                        Projectile.NewProjectile(npc.Center - new Vector2(0, npc.height + 45), Vector2.Zero, ModContent.ProjectileType<PilgrimExplosion>(), 0, 0);
                        float quantity = !Main.expertMode ? 8: 10;
                        float multSpeed = !Main.expertMode ? 4 : 6;
                        for (int i = 0; i < quantity; i++)
                        {    
                            Vector2 speed = Main.rand.NextVector2Unit((float)MathHelper.Pi / 4, (float)MathHelper.Pi / 2);
                            Projectile.NewProjectile(npc.Center - new Vector2(0, npc.height + 45), -speed * multSpeed, ModContent.ProjectileType<StarineShaft>(), projDamage, 0);
                        }
                        Main.PlaySound(SoundID.Item62, npc.Center);
                        attackRepeat = 3;
                    }
                }
            }
            else if (AIState == DeathDrama) {
                npc.life = npc.lifeMax;
                npc.dontTakeDamage = true;
            }
        }
    }
}