using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MythosOfMoonlight.Projectiles.RupturedPilgrim;

namespace MythosOfMoonlight.NPCs.Enemies.RupturedPilgrim {
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
            npc.lifeMax = 625;
            npc.defense = 5;
            npc.damage = 0;
            npc.aiStyle = 0;
            npc.noGravity = true;
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
                else {
                    hasDoneDeathDrama = true;
                    Projectile.NewProjectile(npc.Center - new Vector2(0, 45), Vector2.Zero, ModContent.ProjectileType<PilgrimExplosion>(), 45, 0);
                    npc.life = 0;
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
		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
        	if (damage >= npc.life && !hasDoneDeathDrama)
        	{
                damage = 0;
                AIState = DeathDrama;
                npc.frameCounter = 0;
                npc.dontTakeDamage = true;
        	}
		}
		public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
        	if (damage >= npc.life && !hasDoneDeathDrama)
        	{
                damage = 0;
                AIState = DeathDrama;
                npc.frameCounter = 0;
                npc.dontTakeDamage = true;
        	}
		}
        public override void AI()
        {
            Player player = Main.player[npc.target];
            if (AIState == Idle) {
                AITimer++;
                Vector2 pos = new Vector2(player.position.X + 350, player.position.Y);
                Vector2 moveTo = pos - npc.Center;
                npc.velocity = (moveTo) * 0.18f;
                if (AITimer >= 155) {
                    AITimer = 0;
                    AIState = Attack;
                    npc.frameCounter = 0;
                    npc.velocity = Vector2.Zero;
                }
            }
            else if (AIState == Attack) {
                if (npc.frameCounter == 26) {
                    Projectile.NewProjectile(npc.Center - new Vector2(0, 45), Vector2.Zero, ModContent.ProjectileType<PilgrimProj>(), 0, 0);
                }
            }
            else if (AIState == DeathDrama) {
                npc.life = npc.lifeMax;
                npc.dontTakeDamage = true;
            }
        }
    }
}