using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.NPCs.Enemies.RupturedPilgrim.Projectiles;
using Terraria.ID;
using MythosOfMoonlight.Dusts;

namespace MythosOfMoonlight.NPCs.Enemies.RupturedPilgrim
{
    public class RupturedPilgrim : ModNPC {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ruptured Pilgrim");
            Main.npcFrameCount[npc.type] = 15;
        }
        public override void SetDefaults()
        {
            npc.width = 54;
            npc.height = 68;
            npc.lifeMax = 890;
            npc.defense = 8;
            npc.damage = 0;
            npc.aiStyle = 0;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit44;
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
                else if (npc.frameCounter < 25) {
                    npc.frame.Y = 4 * frameHeight;
                }
                else {
                    npc.frameCounter = 0;
                }
            }
            if (AIState == Attack) {
                if (npc.frameCounter < 5) {
                    npc.frame.Y = 5 * frameHeight;
                }
                else if (npc.frameCounter < 10) {
                    npc.frame.Y = 6 * frameHeight;
                }
                else if (npc.frameCounter < 15) {
                    npc.frame.Y = 7 * frameHeight;
                }
                else if (npc.frameCounter < 20) {
                    npc.frame.Y = 8 * frameHeight;
                }
                else if (npc.frameCounter < 25) {
                    npc.frame.Y = 9 * frameHeight;
                }
                else if (npc.frameCounter < 30) {
                    npc.frame.Y = 10 * frameHeight;
                }
                else {
                    AIState = Idle;
                    AITimer = 0;
                    npc.frameCounter = 0;
                }
            }
            if (AIState == DeathDrama) {
                if (npc.frameCounter < 5) {
                    npc.frame.Y = 11 * frameHeight;
                }
                else if (npc.frameCounter < 10) {
                    npc.frame.Y = 12 * frameHeight;
                }
                else if (npc.frameCounter < 15) {
                    npc.frame.Y = 13 * frameHeight;
                }
                else if (npc.frameCounter < 20) {
                    npc.frame.Y = 14 * frameHeight;
                }
                else if (!hasDoneDeathDrama) {
                    hasDoneDeathDrama = true;
                    Projectile.NewProjectileDirect(npc.Center, new Vector2(), ModContent.ProjectileType<PilgrimExplosion>(), 100, 100);
                    npc.life = 0;
                    Main.PlaySound(SoundID.NPCDeath43, npc.Center);
                    for (int i = 0; i < 30; i++)
                    {
                        int dust = Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<StarineDust>());
                        Main.dust[dust].velocity = Main.rand.NextVector2Unit() * 2f;
                        Main.dust[dust].scale = 1f * Main.rand.Next(1, 3);
                        Main.dust[dust].noGravity = true;
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
        public override void AI()
        {
            Player player = Main.player[npc.target];
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
                if (AITimer >= 155) {
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
                        for (int i = 0; i < 5; i++)
                        {
                            Vector2 speed = Main.rand.NextVector2Unit((float)MathHelper.Pi / 4, (float)MathHelper.Pi / 2) * Main.rand.NextFloat();
                            Projectile.NewProjectile(npc.Center - new Vector2(0, npc.height + 45), -speed * 6, ModContent.ProjectileType<StarineShaft>(), 0, 0);
                        }
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