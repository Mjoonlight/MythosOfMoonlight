using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MythosOfMoonlight.Items.Materials;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs.Enemies.StrandedMartian
{
    public class StrandedMartian : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stranded Martian");
            Main.npcFrameCount[npc.type] = 9;
        }
        public override void SetDefaults()
        {
            npc.width = 38;
            npc.height = 40;
            npc.damage = 15;
            npc.lifeMax = 90;
            npc.defense = 2;
            npc.HitSound = SoundID.NPCHit39;
            npc.DeathSound = SoundID.NPCDeath57;
            npc.aiStyle = -1;
            npc.netAlways = true;
        }
        private enum NState
        {
            Wander,
            Shoot
        }
        private NState State
        {
            get { return (NState)(int)npc.ai[0]; }
            set { npc.ai[0] = (int)value; }
        }
        private void SwitchTo(NState state)
        {
            State = state;
        }
        public float JumpCD
        {
            get => npc.ai[1];
            set => npc.ai[1] = value;
        }
        public float Timer
        {
            get => npc.ai[2];
            set => npc.ai[2] = value;
        }
        public override void AI()
        {
            if (JumpCD > 60)
            {
                JumpCD = 0;
            }
            foreach(Player player in Main.player)
            {
                if (State == NState.Wander && Vector2.Distance(player.Center,npc.Center)<= 300f)
                {
                    Timer = 0;
                    npc.target = player.whoAmI;
                    SwitchTo(NState.Shoot);
                }
            }
            switch (State)
            {
                case NState.Wander:
                    {
                        Timer++;
                        npc.spriteDirection = npc.direction;
                        if (npc.velocity.Y <=0)
                        {
                            npc.frame = new Rectangle(0, ((2 + (int)Timer / 6) % 7) * 46, 38, 46);
                        }
                        else
                        {
                            npc.frame = new Rectangle(0, 46, 38, 46);
                        }
                        npc.GetGlobalNPC<FighterGlobalAI>().FighterAI(npc, 10, 1.8f, true);
                        npc.TargetClosest(true);
                        break;
                    }
                case NState.Shoot:
                    {
                        Timer++;
                        npc.direction = (Main.player[npc.target].Center.X >= npc.Center.X) ? 1 : -1;
                        npc.spriteDirection = npc.direction;
                        npc.frame = new Rectangle(0, 0, 38, 46);
                        if (Timer % 60 == 0)
                        {
                            Main.PlaySound(SoundID.Item68, npc.Center);
                            Projectile.NewProjectile(npc.Center, Vector2.Normalize(Main.player[npc.target].Center + new Vector2(0, -150) - npc.Center) * 10f, ModContent.ProjectileType<CometEmberMini>(), Main.expertMode ? 6 : 8, .075f, npc.target, Main.player[npc.target].Center.X >= npc.Center.X ? 1 : -1);
                        }
                        if (!Main.player[npc.target].active || Main.player[npc.target].dead || Vector2.Distance(Main.player[npc.target].Center, npc.Center) > 400f || Timer > 120)
                        {
                            npc.TargetClosest(true);
                            SwitchTo(NState.Wander);
                        }
                        npc.velocity *= .9f;
                        break;
                    }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Color color = Color.White;
            Vector2 drawPos = npc.Center - Main.screenPosition + new Vector2(0,npc.gfxOffY);
            Texture2D texture = mod.GetTexture("NPCs/Enemies/StrandedMartian/StrandedMartian_Glow");
            Texture2D origTexture = Main.npcTexture[npc.type];
            Rectangle frame = new Rectangle(0, npc.frame.Y, npc.width, npc.height);
            Vector2 orig = frame.Size() / 2f;
            Main.spriteBatch.Draw(origTexture, drawPos, frame, drawColor, npc.rotation, orig, npc.scale, npc.direction < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            Main.spriteBatch.Draw(texture, drawPos, frame, color, npc.rotation, orig, npc.scale, npc.direction < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            return false;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return base.SpawnChance(spawnInfo);
        }
        public override void NPCLoot()
        {
            Item.NewItem(npc.Hitbox, ModContent.ItemType<PurpurineQuartz>(), Main.rand.Next(3, 7));
        }
    }
}
