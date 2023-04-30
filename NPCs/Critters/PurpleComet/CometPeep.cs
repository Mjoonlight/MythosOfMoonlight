using Microsoft.Xna.Framework;
using MythosOfMoonlight.BiomeManager;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace MythosOfMoonlight.NPCs.Critters.PurpleComet
{
    public class CometPeep : ModNPC
    {
        public Vector2[] lookTowards = new Vector2[2];
        public List<CometPeep> friends = new List<CometPeep>();
        public float rotate;
        public CometPeep leader;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Comet Peep");
            Main.npcFrameCount[NPC.type] = 7;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new(0) { Velocity = 1 };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
            NPCID.Sets.CountsAsCritter[Type] = true;
        }
        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 20;
            NPC.catchItem = ModContent.ItemType<Items.Critters.CometPeepItem>();
            //NPC.friendly = true;
            NPC.aiStyle = -1;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            SpawnModBiomes = new int[]
            {
                ModContent.GetInstance<PurpleCometBiome>().Type
            };
        }
        public override void FindFrame(int frameHeight)
        {
            if (NPC.ai[2]++ > 6)
            {
                NPC.ai[2] = 0;
                if (NPC.ai[3]++ > 1)
                    NPC.ai[3] = 0;
                if ((leader != null && leader.NPC.active && leader.NPC.velocity.Length() > 5) || (leader == null && NPC.velocity.Length() > 5))
                    NPC.frame.Y = 80 + (int)(20 * NPC.ai[3]);
                else
                    NPC.frame.Y = (int)(20 * NPC.ai[3]);
            }
        }
        public override void OnKill()
        {
            PurpleCometEvent.CritterDeath(NPC.Center);
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new BestiaryPortraitBackgroundProviderPreferenceInfoElement(ModContent.GetInstance<PurpleCometBiome>().ModBiomeBestiaryInfoElement),
                new FlavorTextBestiaryInfoElement("Often seen in flocks, known as showers (see: Meteor Shower), these undeniably adorable critters are able to communicate amongst each other, and can even be seen playing, although they do scurry when approached.")
            });
        }
        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }

        public override void AI()
        {
            if (NPC.ai[0]++ > 60 || lookTowards[1] == new Vector2())
            {
                NPC.ai[0] = 0;
                if (lookTowards[1] != new Vector2())
                    lookTowards[0] = lookTowards[1];
                else
                {
                    Set();
                    lookTowards[0] = new Vector2(NPC.width * 2 * (Main.rand.NextBool(2) ? -1 : 1));
                }
                Set();
                void Set()
                {
                    if (friends.Count > 0)
                        lookTowards[1] = new Vector2(Main.rand.Next(0, NPC.width) * NPC.direction, Main.rand.Next(0, NPC.height) * NPC.direction) - NPC.velocity;
                    else
                        lookTowards[1] = (new Vector2(Main.rand.Next(-NPC.width, NPC.width), Main.rand.Next(-NPC.height, NPC.height)) * 2) - (NPC.velocity * 2);
                }
            }
            if (leader == null || !leader.NPC.active)
            {
                if (friends.Count < 1)
                    NPC.noTileCollide = false;
                else
                    NPC.noTileCollide = true;
                if (friends.Count > 0)
                    rotate += (float)Math.PI / 90; NPC.spriteDirection = NPC.direction = NPC.velocity.X < 0 ? -1 : 1;
                if (lookTowards[0] != new Vector2() && lookTowards[1] != new Vector2())
                    FindObstruction();
                void FindObstruction()
                {
                    Vector2 usage = DynamicTowards();
                    NPC.velocity += new Vector2(usage.X * 0.001f, usage.Y * 0.0001f);
                    usage += NPC.Center;
                    Vector2 tileDetect = (NPC.velocity * 16) + NPC.Center;
                    Tile tile = Main.tile[(int)tileDetect.X / 16, (int)tileDetect.Y / 16];
                    if (friends.Count < 1)
                        if (Main.tileSolid[tile.TileType] && !WorldGen.TileEmpty((int)tileDetect.X / 16, (int)tileDetect.Y / 16))
                            NPC.velocity *= -0.5f;
                    if (!Main.rand.NextBool((int)(NPC.velocity.Length() / 2) + 1))
                        Dust.NewDustPerfect(NPC.Center + new Vector2(Main.rand.Next(0, NPC.width / 2) * -NPC.direction, Main.rand.Next(NPC.height / -4, NPC.height / 4)), ModContent.DustType<Dusts.PurpurineDust>(), new Vector2(NPC.velocity.X / -2, 0)).noGravity = true;
                    //Dust.NewDustPerfect(usage, DustID.CrystalSerpent, Vector2.Zero).noGravity = true;
                    //Dust.NewDustPerfect(tileDetect, DustID.UltraBrightTorch, Vector2.Zero).noGravity = true;
                }
            }
            else
            {
                Vector2 usage = new Vector2();
                float rot = (float)((2 * Math.PI) / (leader.friends.Count)) * NPC.ai[1];
                if (lookTowards[0] != Vector2.Zero)
                    usage = DynamicTowards();
                if (Main.rand.NextBool(3))
                    Dust.NewDustPerfect(NPC.Center + new Vector2(Main.rand.Next(0, NPC.width / 2) * -NPC.direction, Main.rand.Next(NPC.height / -4, NPC.height / 4)), ModContent.DustType<Dusts.PurpurineDust>(), new Vector2(-NPC.direction * 4, 0)).noGravity = true;
                NPC.noTileCollide = true;
                NPC.position = leader.NPC.Center + new Vector2(36 * (float)Math.Cos((leader.rotate + rot) * leader.NPC.direction), 36 * (float)Math.Sin((leader.rotate + rot) * leader.NPC.direction)) + new Vector2(leader.NPC.width / -2, leader.NPC.height / -2) + usage;
                NPC.direction = NPC.spriteDirection = leader.NPC.direction;
            }
            Vector2 DynamicTowards()
            {
                float rot = (float)Math.Atan(lookTowards[1].Y / lookTowards[1].X) + (lookTowards[1].X < 0 ? (float)Math.PI : 0);
                float dist = lookTowards[1].Length();
                float rot2 = (float)Math.Atan(lookTowards[0].Y / lookTowards[0].X) + (lookTowards[0].X < 0 ? (float)Math.PI : 0);
                float dist2 = lookTowards[0].Length();
                Vector2[] lerpTowards = new Vector2[2] { new Vector2(dist * (float)Math.Cos(rot), dist * (float)Math.Sin(rot)), new Vector2(dist2 * (float)Math.Cos(rot2), dist2 * (float)Math.Sin(rot2)) };
                return new Vector2(MathHelper.Lerp(lerpTowards[1].X, lerpTowards[0].X, (float)Math.Sin((float)Math.PI * (NPC.ai[0] / 120f))), MathHelper.Lerp(lerpTowards[1].Y, lerpTowards[0].Y, (float)Math.Sin((float)Math.PI * (NPC.ai[0] / 120f))));
            }
            /*
            public override float SpawnChance(NPCSpawnInfo spawnInfo)
            {
                return 0.2f;
            }
            */
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return !(PurpleCometEvent.PurpleComet && spawnInfo.Player.ZoneOverworldHeight) ? 0 : 0.37f;
        }
    }
}
