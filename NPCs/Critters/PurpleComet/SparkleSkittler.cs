using Microsoft.Xna.Framework;
using MythosOfMoonlight.Dusts;
using MythosOfMoonlight.Items.PurpleComet.Critters;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs.Critters.PurpleComet
{
    public class SparkleSkittler : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sparkle Skittler");
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void SetDefaults()
        {
            npc.friendly = true;
            npc.aiStyle = -1;
            npc.lifeMax = 5;
            npc.width = 30;
            npc.height = 22;
            npc.defense = 0;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            Main.npcCatchable[npc.type] = true;
            npc.catchItem = (short)ModContent.ItemType<SparkleSkittlerItem>();
            npc.dontCountMe = true;
            npc.npcSlots = 0;
            npc.dontTakeDamageFromHostiles = false;
        }
        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return true;
        }
        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return true;
        }
        const float SPEED = 3.5f;
        const int TRANSITION_CHANCE = 99;
        int State
        {
            get => (int)npc.ai[0];
            set => npc.ai[0] = value;
        }
        public override void AI()
        {
            Collision.StepUp(ref npc.position, ref npc.velocity, npc.width, npc.height, ref npc.stepSpeed, ref npc.gfxOffY, 1, false, 0);
            switch (State)
            {
                case 0:
                    if (npc.direction == 0)
                    {
                        npc.direction = Main.rand.NextBool(2) ? 1 : -1;
                    }
                    if (Main.rand.NextBool(TRANSITION_CHANCE))
                    {
                        npc.velocity.X = 0;
                        State = 1;
                    }
                    else if (npc.velocity.X == 0)
                    {
                        npc.direction = -npc.direction;
                    }
                    npc.velocity.X = npc.direction * SPEED;
                    break;
                case 1:
                    npc.velocity.X = 0;
                    if (Main.rand.NextBool(TRANSITION_CHANCE))
                    {
                        npc.direction = 0;
                        State = 0;
                    }
                    break;
            }
            npc.spriteDirection = npc.direction;
        }
        const int FRAME_RATE = 3;
        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 2; i++)
            {
                int dust = Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<PurpurineDust>(), 2 * hitDirection, -1.5f);
                Main.dust[dust].scale = 1f;
            }
            if (npc.life <= 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<PurpurineDust>(), 2 * hitDirection, -1.5f);
                    Main.dust[dust].scale = 1f;
                }
                for (int i = 0; i < 2; i++)
                {
                    Gore.NewGore(npc.Center + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), Vector2.Zero, mod.GetGoreSlot("Gores/Enemies/Purpurine"));
                }
            }
        }
        public override void FindFrame(int frameHeight)
        {
            switch (State)
            {
                case 0:
                    if (npc.frameCounter + 1 < FRAME_RATE * 4)
                    {
                        npc.frameCounter++;
                        if (npc.frameCounter < FRAME_RATE)
                        {
                            npc.frameCounter = FRAME_RATE;
                        }
                    }
                    else
                    {
                        npc.frameCounter = FRAME_RATE;
                    }
                    break;
                case 1:
                    npc.frameCounter = 0;
                    break;
            }
            npc.frame.Y = (int)npc.frameCounter / FRAME_RATE * frameHeight;
        }
        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return !PurpleCometEvent.PurpleComet ? 0 : 0.17f;
        }
    }
}