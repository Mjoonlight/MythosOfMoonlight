using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MythosOfMoonlight.Common.BiomeManager;
using MythosOfMoonlight.Common.Crossmod;
using MythosOfMoonlight.Common.Systems;
using MythosOfMoonlight.Common.Utilities;
using MythosOfMoonlight.Dusts;
using MythosOfMoonlight.NPCs.Enemies.CometFlyby.CometEmber;
using MythosOfMoonlight.NPCs.Minibosses.StarveiledProj;
using System;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs.Minibosses
{
    public class StarveiledScholar : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.TrailCacheLength[Type] = 5;
            NPCID.Sets.TrailingMode[Type] = 0;
            NPC.AddElement(CrossModHelper.Celestial);
        }
        public override void SetDefaults()
        {
            NPC.width = 68;
            NPC.height = 74;
            NPC.lifeMax = 2300;
            NPC.boss = true;
            NPC.defense = 22;
            NPC.damage = 0;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;
            NPC.npcSlots = 7f;
            NPC.HitSound = SoundID.NPCHit49;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            if (!Main.dedServ) Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/scholar");
        }
        float flickerGlow;
        float riftAlpha;
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new BestiaryPortraitBackgroundProviderPreferenceInfoElement(ModContent.GetInstance<PurpleCometBiome>().ModBiomeBestiaryInfoElement),
                new FlavorTextBestiaryInfoElement("When the dazzling purple witch vanished, her most loyal followers continued to follow the Purple Comet, soon falling into madness and into infestation. Restlessly following the comet to adore it they lived off the lands and rare towns encountered on their endless journey, be it simple dedication or the will of the unearthly creatures within them.")
            });
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 1)
            {
                for (int num901 = 0; num901 < 10; num901++)
                {
                    int num902 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<Dusts.PurpurineDust>(), 0f, 0f, 200, default(Color), 1f);
                    Main.dust[num902].position = NPC.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() * NPC.width / 2f;
                    Main.dust[num902].noGravity = true;
                    Dust dust2 = Main.dust[num902];
                    dust2.velocity = hit.HitDirection * Vector2.UnitX;
                    num902 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.PurpleTorch, 0f, 0f, 100, default(Color), 0.75f);
                    Main.dust[num902].position = NPC.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() * NPC.width / 2f;
                    dust2 = Main.dust[num902];
                    dust2.velocity = hit.HitDirection * Vector2.UnitX;
                    Main.dust[num902].noGravity = true;
                    Main.dust[num902].fadeIn = 2.5f;
                }
                for (int num903 = 0; num903 < 10; num903++)
                {
                    int num904 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.WitherLightning, 0f, 0f, 0, default(Color), 1);
                    Main.dust[num904].position = NPC.Center + Vector2.UnitX.RotatedByRandom(MathHelper.Pi).RotatedBy(NPC.velocity.ToRotation()) * NPC.width / 2f;
                    Dust dust2 = Main.dust[num904];
                    dust2.velocity = hit.HitDirection * Vector2.UnitX;
                }
            }
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
        public float AITimer3
        {
            get => NPC.ai[3];
            set => NPC.ai[3] = value;
        }
        const int P2Transition = -2, Death = -1, Intro = 0, Idle = 1, UnnamedAttackNumberOne = 2;
        bool ded;
        public override bool CheckDead()
        {
            if (NPC.life <= 0 && !ded)
            {
                NPC.life = 1;
                riftAlpha = 0;
                AIState = Death;
                NPC.frameCounter = 0;
                NPC.immortal = true;
                NPC.noGravity = false;
                NPC.noTileCollide = false;
                NPC.dontTakeDamage = true;
                ded = true;
                NPC.frame.X = 5 * 68;
                AITimer = AITimer2 = 0;
                NPC.velocity = Vector2.Zero;
                NPC.life = 1;
                return false;
            }
            return true;
        }
        int NextAttack = UnnamedAttackNumberOne;
        bool p2;
        void Teleport(Vector2 pos)
        {
            NPC.Center = pos;
            for (int num901 = 0; num901 < 10; num901++)
            {
                int num902 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<Dusts.PurpurineDust>(), 0f, 0f, 200, default(Color), 1f);
                Main.dust[num902].position = NPC.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() * NPC.width / 2f;
                Main.dust[num902].noGravity = true;
                Dust dust2 = Main.dust[num902];
                dust2.velocity *= 3f;
                num902 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.PurpleTorch, 0f, 0f, 100, default(Color), 0.75f);
                Main.dust[num902].position = NPC.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() * NPC.width / 2f;
                dust2 = Main.dust[num902];
                dust2.velocity *= 2f;
                Main.dust[num902].noGravity = true;
                Main.dust[num902].fadeIn = 2.5f;
            }
            for (int num903 = 0; num903 < 10; num903++)
            {
                int num904 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.WitherLightning, 0f, 0f, 0, default(Color), 1);
                Main.dust[num904].position = NPC.Center + Vector2.UnitX.RotatedByRandom(MathHelper.Pi).RotatedBy(NPC.velocity.ToRotation()) * NPC.width / 2f;
                Dust dust2 = Main.dust[num904];
                dust2.velocity *= 3f;
            }
        }
        public override void AI()
        {
            Player player = Main.player[NPC.target];
            if (NPC.life < NPC.lifeMax / 2 && !p2)
            {
                AIState = P2Transition;
                AITimer = AITimer2 = AITimer3 = 0;
                p2 = true;
            }
            NPC.TargetClosest(true);
            NPC.direction = player.Center.X > NPC.Center.X ? 1 : -1;
            NPC.spriteDirection = NPC.direction;
            if (player.dead)
            {
                NPC.active = false;

                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<CometEmberProj>(), 20, .1f, Main.myPlayer);
            }
            if (AIState == P2Transition)
            {
                p2 = true;
                AIState = Idle;
            }
            if (AIState == Intro)
            {
                AIState = Idle;
            }
            else if (AIState == Idle)
            {
                AITimer++;
                NPC.Center = Vector2.Lerp(NPC.Center, player.Center, 0.015f);
                if (AITimer >= 120)
                {
                    AIState = NextAttack;
                    NPC.frame.Y = 0;
                    AITimer = 0;
                }
            }

        }
    }
}
