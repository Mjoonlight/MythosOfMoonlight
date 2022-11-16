using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MythosOfMoonlight.BiomeManager;
using MythosOfMoonlight.Items.Materials;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using MythosOfMoonlight.Dusts;

namespace MythosOfMoonlight.NPCs.Enemies.CometFlyby.CometEmber
{
    public class CometEmber : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Comet Ember");
            //NPCID.Sets.NPCBestiaryDrawModifiers value = new(0) { Velocity = 1f };
            //NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
            NPCID.Sets.TrailCacheLength[Type] = 12;
            NPCID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            NPC.width = 38;
            NPC.height = 42;
            NPC.damage = 28;
            NPC.lifeMax = 70;
            NPC.defense = 2;
            NPC.HitSound = SoundID.NPCHit39;
            NPC.DeathSound = SoundID.Item74;
            NPC.aiStyle = -1;
            NPC.netAlways = true;
            NPC.noTileCollide = false;
            NPC.noGravity = true;
            NPC.knockBackResist = 0f;
            SpawnModBiomes = new int[]
            {
                ModContent.GetInstance<PurpleCometBiome>().Type
            };
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                //BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime, //making the bg too dark lol
                new BestiaryPortraitBackgroundProviderPreferenceInfoElement(ModContent.GetInstance<PurpleCometBiome>().ModBiomeBestiaryInfoElement),
                new FlavorTextBestiaryInfoElement("//PlaceHolder// The Comet Ember, which spins in place, slowly moving up and down. Once the player is close, it tries to float above them and crash back down, exploding.")
            });
        }
        private enum NState
        {
            Wander,
            Hover,
            Stomp
        }
        private NState State
        {
            get { return (NState)(int)NPC.ai[0]; }
            set { NPC.ai[0] = (int)value; }
        }
        private void SwitchTo(NState state)
        {
            State = state;
        }
        public float PhaseTimer
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }
        public float Timer
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }
        public float Opaque;
        public override void AI()
        {
            Timer++;
            foreach (Player player in Main.player)
            {
                if (State == NState.Wander && Vector2.Distance(player.Center, NPC.Center) <= 100f && PhaseTimer > 60)
                {
                    PhaseTimer = 0;
                    NPC.target = player.whoAmI;
                    for (int i = 6; i <= 360; i += 6)
                    {
                        Vector2 pos = MathHelper.ToRadians(i).ToRotationVector2() * 70f;
                        Dust dust = Dust.NewDustDirect(NPC.Center + pos, 1, 1, ModContent.DustType<PurpurineDust>());
                        dust.velocity *= 0;
                        dust.noGravity = true;
                        dust.scale = 1.5f;
                    }
                    SoundEngine.PlaySound(SoundID.NPCDeath7, NPC.Center);
                    SwitchTo(NState.Hover);
                }
                if (State == NState.Hover && Math.Abs(Main.player[NPC.target].Center.X - NPC.Center.X) <= 30f && PhaseTimer >= 120)
                {
                    PhaseTimer = 0;
                    NPC.velocity = new Vector2(0, .1f);
                    SwitchTo(NState.Stomp);
                }
            }
            Player target = Main.player[NPC.target];
            switch (State)
            {
                case NState.Wander:
                    PhaseTimer++;
                    NPC.velocity.Y = (float)Math.Sin(MathHelper.ToRadians(Timer * 1.33f));
                    Opaque = 2f;
                    break;
                case NState.Hover:
                    Opaque = 2f - (float)Math.Pow(.985f, (float)Math.Abs(target.Center.X - NPC.Center.X));
                    PhaseTimer++;
                    if (PhaseTimer >= 180) SwitchTo(NState.Stomp);
                    MoMNPC.EscapeCheck(1200f, 1200f, NPC);
                    NPC.velocity = Vector2.Lerp(NPC.velocity, (target.Center - new Vector2(0, 300) - NPC.Center) / 10f, .33f);
                    break;
                case NState.Stomp:
                    Opaque = 1f;
                    PhaseTimer++;
                    NPC.velocity.X *= 0;
                    if (NPC.Center.Y >= target.Center.Y - 50) NPC.noTileCollide = false;
                    else NPC.noTileCollide = true;
                    NPC.velocity.Y += .5f;
                    if (NPC.collideY || NPC.collideX)
                    {
                        NPC.life = 0;
                        NPC.checkDead();
                    }
                    break;
            }
        }
        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            if (State == NState.Wander)
            {
                PhaseTimer = 0;
                NPC.target = player.whoAmI;
                for (int i = 6; i <= 360; i += 6)
                {
                    Vector2 pos = MathHelper.ToRadians(i).ToRotationVector2() * 70f;
                    Dust dust = Dust.NewDustDirect(NPC.Center + pos, 1, 1, ModContent.DustType<PurpurineDust>());
                    dust.velocity *= 0;
                    dust.noGravity = true;
                    dust.scale = 1.5f;
                }
                SoundEngine.PlaySound(SoundID.NPCDeath7, NPC.Center);
                SwitchTo(NState.Hover);
            }
        }
        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            if (State == NState.Wander)
            {
                PhaseTimer = 0;
                NPC.target = projectile.owner;
                for (int i = 6; i <= 360; i += 6)
                {
                    Vector2 pos = MathHelper.ToRadians(i).ToRotationVector2() * 70f;
                    Dust dust = Dust.NewDustDirect(NPC.Center + pos, 1, 1, ModContent.DustType<PurpurineDust>());
                    dust.velocity *= 0;
                    dust.noGravity = true;
                    dust.scale = 1.5f;
                }
                SoundEngine.PlaySound(SoundID.NPCDeath7, NPC.Center);
                SwitchTo(NState.Hover);
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Color color = Color.White;
            Vector2 drawPos = NPC.Center - screenPos;
            Texture2D origTexture = TextureAssets.Npc[NPC.type].Value;
            Vector2 orig = origTexture.Size() / 2f;
            if (!NPC.IsABestiaryIconDummy)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                for (int j = 0; j <= 10; j += 2)
                {
                    for (int i = -60; i <= 60; i += 60)
                    {
                        float rad = MathHelper.ToRadians(i + (i / 60f) * (Timer - j));
                        float pulse = 1.5f + .5f * (float)Math.Sin(MathHelper.ToRadians(2 * i + (Timer - j)));
                        Main.spriteBatch.Draw(origTexture, NPC.oldPos[j] + new Vector2(NPC.width / 2, NPC.height / 2) - screenPos, null, color * (float)((float)(12 - j) / 24), rad, orig, pulse, SpriteEffects.None, 0f);
                    }
                }
                for (int i = -60; i <= 60; i += 60)
                {
                    float rad = MathHelper.ToRadians(i + (i / 60f) * Timer);
                    float pulse = 1.5f + .5f * (float)Math.Sin(MathHelper.ToRadians(2 * i + Timer));
                    Main.spriteBatch.Draw(origTexture, drawPos, null, color, rad, orig, pulse, SpriteEffects.None, 0f);
                }
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
            else
            {
                return true;
            }
            return false;
        }
        public override bool CheckDead()
        {
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<CometEmberProj>(), 20, .1f, Main.myPlayer);
            return true;
        }
    }
}
