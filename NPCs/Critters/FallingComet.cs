using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using MythosOfMoonlight.Dusts;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Audio;

namespace MythosOfMoonlight.NPCs.Critters
{
    public class FallingComet : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Purple Chunk");
            Main.npcFrameCount[NPC.type] = 3;
            NPCID.Sets.TrailCacheLength[NPC.type] = 10;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers(0) { Hide = true, });
            NPCID.Sets.DebuffImmunitySets.Add(Type, new NPCDebuffImmunityData
            {
                ImmuneToAllBuffsThatAreNotWhips = true
            });
            //NPCID.Sets.NPCBestiaryDrawModifiers value = new(0) { Velocity = 1 };
            //NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 28;
            NPC.height = 26;
            NPC.defense = 10;
            NPC.lifeMax = 10;
            NPC.knockBackResist = 0f;
            NPC.npcSlots = 1f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.Item10;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Vector2 drawOrigin = NPC.frame.Size() / 2;
            Color color;

            Vector2 drawPos = NPC.position - screenPos + drawOrigin + new Vector2(5f);
            color = NPC.GetAlpha(drawColor);

            spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos, NPC.frame, color, NPC.rotation, drawOrigin, NPC.scale, NPC.spriteDirection == 0 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            spriteBatch.Draw(Request<Texture2D>("MythosOfMoonlight/NPCs/Critters/FallingCometGlow").Value, drawPos, NPC.frame, Color.White, NPC.rotation, drawOrigin, NPC.scale, NPC.spriteDirection == 0 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
        }
        Vector2 Drawoffset => new Vector2(0) + Vector2.UnitX * NPC.spriteDirection * 24;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            //3hi31mg
            var off = new Vector2(NPC.width / 2, NPC.height / 2);
            var clr = new Color(255, 255, 255, 255); // full white
            var texture = Request<Texture2D>("MythosOfMoonlight/NPCs/Critters/FallingCometTrail").Value;
            var frame = new Rectangle(0, NPC.frame.Y, NPC.width, NPC.height);
            var orig = frame.Size() / 2f;
            var trailLength = NPCID.Sets.TrailCacheLength[NPC.type];

            for (int i = 1; i < trailLength; i++)
            {
                float scale = MathHelper.Lerp(1f, 0.1f, (float)(trailLength - i) / trailLength);
                var fadeMult = 1f / trailLength;
                Main.spriteBatch.Draw(texture, NPC.oldPos[i] - screenPos + off, frame, clr * (scale - fadeMult * i), NPC.oldRot[i], orig, 1f, SpriteEffects.None, 0f);
            }
            return true;
        }
        //private static int FRAMESPEED = 7;
        bool firstFrame = false;
        public override void AI()
        {
            if (firstFrame)
                NPC.frame.Y = Main.rand.Next(2) * NPC.height;

            if (NPC.timeLeft > 60)
            {
                if (Main.rand.NextBool(12))
                {
                    int d = Dust.NewDust(NPC.Center + new Vector2(5f, 0f).RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4) + NPC.velocity.ToRotation()), 4, 4, ModContent.DustType<PurpurineStoneDust>());
                    Main.dust[d].velocity = NPC.velocity * 0.6f;
                }
            }

            NPC.velocity.Y = 12f; //Base velocity.
            NPC.rotation += 0.03f; //Base rotation.
            if (NPC.velocity.Y != 0)
            {
                NPC.rotation += 0.2f;
                if (NPC.soundDelay == 0)
                {
                    NPC.soundDelay = 20 + Main.rand.Next(40); //Sound.
                    SoundEngine.PlaySound(SoundID.Item9, NPC.Center);
                }
            }
            else
            {
                NPC.rotation += 0.2f;
            }
            firstFrame = true;
            if (NPC.velocity.Y > 0)
            {
                if (Main.rand.NextBool(12))
                {
                    int d = Dust.NewDust(NPC.Center + new Vector2(6f, 6f).RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4)), 5, 5, DustType<PurpurineStoneDust>());
                    Main.dust[d].velocity = NPC.velocity * 0.6f;
                }
            }
            if (!NPC.noGravity) //Custom thing to not use vanilla noGravity.
            {
                NPC.velocity.Y += 0.3f;
                NPC.rotation += 0.2f;
            }
            if (!NPC.noTileCollide && NPC.velocity.Y > 0) //Ensures tile collision(?) Also custom thing to not use vanilla noTileCollide.
            {
                NPC.position.Y += Collision.TileCollision(NPC.position, new Vector2(0, NPC.velocity.Y), NPC.width, NPC.height).Y;
                NPC.velocity.Y += 0.2f;
                NPC.rotation += 0f;
            }

            if (Collision.TileCollision(NPC.position, new Vector2(0, 16), NPC.width, NPC.height - 17) != new Vector2(0, 16) && NPC.velocity.Y > 1.06) //Stuff that happens when chunk collides with tile.
            {
                NPC.velocity.Y = 0;
                NPC.SimpleStrikeNPC(NPC.lifeMax, 0, true);
                for (int i = 0; i < 4; i++)
                {
                    SoundEngine.PlaySound(SoundID.Item10, NPC.Center); //Sound on collision with tile.

                    Vector2 scale = new Vector2(1, 1) + new Vector2(1, 1) * (float)Math.Sin(NPC.ai[0] / 2) * 0.1f;
                    Vector2 offset = new Vector2(16, 0).RotatedBy(NPC.rotation + i * MathHelper.PiOver2);
                    offset.X *= scale.X;
                    offset.Y *= scale.Y;
                    Vector2 position = NPC.Center + offset;
                    NPC.NewNPC(NPC.GetSource_FromAI(), (int)position.X, (int)position.Y, NPCType<PurpleComet.SparkleSkittler>()); //NPC spawn.
                }
                // This is part of "public override void AI()" which is a void, so using a return keyword doesn't do nothing but cause an error.
                if (NPC.velocity.Y < 0)
                {
                    NPC.SimpleStrikeNPC(NPC.lifeMax, 0, true);
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return !PurpleCometEvent.PurpleComet && !(spawnInfo.Player.ZoneOverworldHeight || spawnInfo.Player.ZoneSkyHeight) ? 0 : 0f;
        }

        public override bool CheckDead()
        {
            for (int i = 0; i < 5; i++)
            {
                SoundEngine.PlaySound(SoundID.Item10, NPC.Center); //Sound when chunk is destroyed by player.

                Vector2 scale = new Vector2(1, 1) + new Vector2(1, -1) * (float)Math.Sin(NPC.ai[0] / 2) * 0.1f;
                Vector2 offset = new Vector2(16, 0).RotatedBy(NPC.rotation + i * MathHelper.PiOver2);
                offset.X *= scale.X;
                offset.Y *= scale.Y;
                Vector2 position = NPC.Center + offset;
                NPC.NewNPC(NPC.GetSource_FromAI(), (int)position.X, (int)position.Y, NPCType<PurpleComet.SparkleSkittler>()); //NPC spawn.
            }
            return true;
        }
    }
}


