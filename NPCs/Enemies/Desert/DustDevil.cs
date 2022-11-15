using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MythosOfMoonlight.Dusts;
using Terraria.GameContent;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;
using System.IO;
using MythosOfMoonlight.Gores.Enemies;
using Terraria.GameContent.Bestiary;

namespace MythosOfMoonlight.NPCs.Enemies.Desert
{
    public class DustDevil : ModNPC
    {/*reminder it essentially is a barrier enemy
it creates a sandnado around itself where it floats up and down
and slowly moves towards the player
and slowly i mean very slowly
like snail pace 
then it stops and it drops to the floor

1. its only vulnerable to melee attacks when its grounded
2. it has a very tiny size as you can see from the sprite, making ranged harder to fuck with it
3. maybe only expert mode+ but IT PULLS!!!! YOU!!!! IN!!!!!!!!! slowly (like the velocity sandstorms pull you around)

fuck you minions cant target it while its sandnado'ing
and whips count as melee!  ! ! !
LITERALLY
THEIR DAMAGE IS LITERALLY CALLED
SUMMONERMELEE */
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Visuals.Sun,
                new FlavorTextBestiaryInfoElement("Horned lizard distantly related to spiny devils. Able to concoct a nasty sand twister to cloak themselves from predators, and to daze prey. While attacks are feared by many nomads, in reality, they very rarely cause any serious harm."),
            });
        }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 9;
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (AIState == Sleep || AIState == None)
            {
                if (NPC.frameCounter < 5)
                {
                    NPC.frame.Y = 32 * 4;
                }
                else if (NPC.frameCounter < 10)
                {
                    NPC.frame.Y = 32 * 5;
                }
                else if (NPC.frameCounter < 15)
                {
                    NPC.frame.Y = 32 * 6;
                }
                else if (NPC.frameCounter < 20)
                {
                    NPC.frame.Y = 32 * 7;
                }
                else if (NPC.frameCounter < 20)
                {
                    NPC.frame.Y = 32 * 8;
                }
            }
            if (AIState == Wake)
            {
                if (NPC.frameCounter < 5)
                {
                    NPC.frame.Y = 32 * 8;
                }
                else if (NPC.frameCounter < 10)
                {
                    NPC.frame.Y = 32 * 7;
                }
                else if (NPC.frameCounter < 15)
                {
                    NPC.frame.Y = 32 * 6;
                }
                else if (NPC.frameCounter < 20)
                {
                    NPC.frame.Y = 32 * 5;
                }
                else if (NPC.frameCounter < 20)
                {
                    NPC.frame.Y = 32 * 4;
                }
                else
                {
                    AIState = Sandnado;
                    NPC.frameCounter = 0;
                }
            }
            if (AIState == Sandnado)
            {
                if (NPC.frameCounter < 5)
                {
                    NPC.frame.Y = 0;
                }
                else if (NPC.frameCounter < 10)
                {
                    NPC.frame.Y = 32;
                }
                else if (NPC.frameCounter < 15)
                {
                    NPC.frame.Y = 32 * 2;
                }
                else if (NPC.frameCounter < 20)
                {
                    NPC.frame.Y = 32 * 3;
                }
                else
                {
                    NPC.frameCounter = 0;
                }
            }
        }
        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 32;
            NPC.lifeMax = 50;
            NPC.defense = 2;
            NPC.damage = 35;
            NPC.knockBackResist = 1.1f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.aiStyle = 0;
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
        const int Sleep = -1;
        const int None = 0;
        const int Wake = 1;
        const int Sandnado = 2;
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.Player.ZoneDesert ? 0.3f : 0f;
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    Dust.NewDust(NPC.Center, 32, 32, DustID.Blood, Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1));
                }
                Helper.SpawnGore(NPC, "MythosOfMoonlight/DDevil", 1, 1, Vector2.One * hitDirection);
                Helper.SpawnGore(NPC, "MythosOfMoonlight/DDevil", 1, 2, Vector2.One * hitDirection);
                Helper.SpawnGore(NPC, "MythosOfMoonlight/DDevil", 1, 3, Vector2.One * hitDirection);
            }
        }
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            if (AIState == None && NPC.Center.Distance(player.Center) < 500f)
                AIState = Sleep;
            if (AIState == Sleep)
            {
                if (NPC.Center.Distance(player.Center) < 500f)
                    AITimer++;
                NPC.dontTakeDamage = false;
                if (AITimer >= 300)
                {
                    AIState = Wake;
                    AITimer = 0;
                    NPC.frameCounter = 0;
                }
            }
            else if (AIState == Sandnado)
            {
                AITimer++;
                NPC.dontTakeDamage = true;
                if (AITimer == 20)
                {
                    Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center - Vector2.UnitY * 75f, Vector2.Zero, ModContent.ProjectileType<DustDevilP>(), 2, 0f, player.whoAmI, ai1: NPC.whoAmI);
                }
                AITimer2 += 0.0110f;
                if (AITimer >= 900)
                    NPC.velocity *= 0.98f;
                if (AITimer >= 950)
                {
                    AIState = Sleep;
                    AITimer = 0;
                    NPC.frameCounter = 0;
                }
            }
        }
    }
    public class DustDevilP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dust Devil");
        }
        public override string Texture => "Terraria/Images/Projectile_656";
        public override void SetDefaults()
        {
            Projectile.Size = Vector2.One * 10f;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 900;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            NPC npc = Main.npc[(int)Projectile.ai[1]];
            if (!npc.active)
                Projectile.Kill();
            if (Projectile.ai[0] < 900f - 120f && Projectile.timeLeft < 120f)
            {
                float num1048 = Projectile.ai[0] % 60f;
                Projectile.ai[0] = 900f - 120f + num1048;
            }
            Projectile.ai[0]++;
            if (Projectile.ai[0] >= 900)
                Projectile.ai[0] = 0;
            float num1045 = 10f;
            float num1046 = 10f;
            Point point7 = Projectile.Center.ToTileCoordinates();
            Collision.ExpandVertically(point7.X, point7.Y, out var topY, out var bottomY, (int)num1045, (int)num1046);
            topY++;
            bottomY--;
            Vector2 value20 = new Vector2(point7.X, topY) * 16f + new Vector2(8f);
            Vector2 value21 = new Vector2(point7.X, bottomY) * 16f + new Vector2(8f);
            Vector2 vector166 = Vector2.Lerp(value20, value21, 0.5f);
            Vector2 vector167 = new Vector2(0f, value21.Y - value20.Y);
            vector167.X = vector167.Y * 0.2f;
            Projectile.width = (int)(vector167.X * 0.65f);
            Projectile.height = (int)vector167.Y;
            Projectile.Center = vector166;
            Vector2 a = new Vector2(Projectile.Center.X, Projectile.Center.Y + 60 * (float)Math.Sin(npc.ai[2]));
            if (npc.ai[1] < 900)
                npc.velocity = (a - npc.Center) * 0.088f;
            Vector2 b = (Projectile.Center - Main.player[npc.target].Center);
            b.Normalize();
            Main.player[npc.target].velocity = new Vector2((b.X * 0.15f) + Main.player[npc.target].velocity.X, Main.player[npc.target].velocity.Y);
            if (Main.player[npc.target].Center.Distance(Projectile.Center) > 750f)
                Projectile.Kill();

        }

        public override bool PreDraw(ref Color lightColor)

        {
            float num290 = 900f;
            float num291 = 10f;
            float num292 = 10f;
            float num293 = Projectile.ai[0];
            float num294 = MathHelper.Clamp(num293 / 30f, 0f, 1f);
            if (num293 > num290 - 60f)
            {
                num294 = MathHelper.Lerp(1f, 0f, (num293 - (num290 - 60f)) / 60f);
            }
            Microsoft.Xna.Framework.Point point5 = Projectile.Center.ToTileCoordinates();
            Collision.ExpandVertically(point5.X, point5.Y, out var topY, out var bottomY, (int)num291, (int)num292);
            topY++;
            bottomY--;
            float num295 = 0.2f;
            Vector2 value77 = new Vector2(point5.X, topY) * 16f + new Vector2(8f);
            Vector2 value78 = new Vector2(point5.X, bottomY) * 16f + new Vector2(8f);
            Vector2.Lerp(value77, value78, 0.5f);
            Vector2 vector69 = new Vector2(0f, value78.Y - value77.Y);
            vector69.X = vector69.Y * num295;
            new Vector2(value77.X - vector69.X / 2f, value77.Y);
            Texture2D value79 = TextureAssets.Projectile[Type].Value;
            Microsoft.Xna.Framework.Rectangle rectangle19 = value79.Frame();
            Vector2 origin19 = rectangle19.Size() / 2f;
            float num296 = -(float)Math.PI / 50f * num293;
            Vector2 spinningpoint4 = Vector2.UnitY.RotatedBy(num293 * 0.1f);
            float num297 = 0f;
            float num298 = 5.1f;
            Microsoft.Xna.Framework.Color value80 = new Microsoft.Xna.Framework.Color(212, 192, 100);
            for (float num299 = (int)value78.Y; num299 > (float)(int)value77.Y; num299 -= num298)
            {
                num297 += num298;
                float num300 = num297 / vector69.Y;
                float num301 = num297 * ((float)Math.PI * 2f) / -20f;
                float num302 = num300 - 0.15f;
                Vector2 position19 = spinningpoint4.RotatedBy(num301);
                Vector2 vector70 = new Vector2(0f, num300 + 1f);
                vector70.X = vector70.Y * num295;
                Microsoft.Xna.Framework.Color color77 = Microsoft.Xna.Framework.Color.Lerp(Microsoft.Xna.Framework.Color.Transparent, value80, num300 * 2f);
                if (num300 > 0.5f)
                {
                    color77 = Microsoft.Xna.Framework.Color.Lerp(Microsoft.Xna.Framework.Color.Transparent, value80, 2f - num300 * 2f);
                }
                color77.A = (byte)((float)(int)color77.A * 0.5f);
                color77 *= num294;
                position19 *= vector70 * 100f;
                position19.Y = 0f;
                position19.X = 0f;
                position19 += new Vector2(value78.X, num299) - Main.screenPosition;
                Main.EntitySpriteDraw(value79, position19, rectangle19, color77 * 0.5f, num296 + num301, origin19, 1f + num302, SpriteEffects.None, 0);
            }
            return false;
        }
    }
}
