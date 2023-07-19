using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.GameContent;

namespace MythosOfMoonlight.NPCs.Critters
{
    public class GulperEel : ModNPC
    {
        public override void SetStaticDefaults()
        {

            Main.npcFrameCount[NPC.type] = 2;
            NPCID.Sets.CountsAsCritter[Type] = true;
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            NPCID.Sets.TrailCacheLength[Type] = 30;
            NPCID.Sets.TrailingMode[Type] = -1;
            NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new(0) { Velocity = 1 };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
            // DisplayName.SetDefault("Gulpie");
        }
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 5;
            NPC.noGravity = false;
            NPC.width = 20;
            NPC.height = 8;
            NPC.defense = 0;
            NPC.catchItem = ModContent.ItemType<Items.Critters.TheFishItem>();
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.dontCountMe = true;
            NPC.npcSlots = 0;
            NPC.dontTakeDamageFromHostiles = false;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
                new FlavorTextBestiaryInfoElement("YES!.")
            });
        }
        int state; //0 = idle 1 = scared 2 = just fuckin runs away lmao 3 = ded
        int cooldown;
        float tailRot;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D a = Helper.GetTex("MythosOfMoonlight/NPCs/Critters/GulperEel_TailBase");
            Texture2D b = Helper.GetTex("MythosOfMoonlight/NPCs/Critters/GulperEel_Tail");
            Texture2D c = Helper.GetTex("MythosOfMoonlight/NPCs/Critters/GulperEel");
            Texture2D d = Helper.GetTex("MythosOfMoonlight/NPCs/Critters/GulperEel_Tail2");
            Texture2D e = Helper.GetTex("MythosOfMoonlight/NPCs/Critters/GulperEel_fat");
            Vector2 posA = NPC.Bottom.RotatedBy(tailRot);
            tailRot = MathHelper.Lerp(tailRot, NPC.rotation, 0.35f);
            SpriteEffects effects = NPC.direction == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None;
            SpriteEffects effects2 = NPC.direction == 1 ? SpriteEffects.FlipVertically : SpriteEffects.None;

            //default(TailTrail).Draw(NPC);
            NPC rCurrentNPC = NPC;
            Texture2D value59 = TextureAssets.Npc[rCurrentNPC.type].Value;
            Vector2 vector63 = rCurrentNPC.position - screenPos;
            vector63 -= new Vector2(value59.Width, value59.Height / Main.npcFrameCount[rCurrentNPC.type]) * rCurrentNPC.scale / 2f;
            vector63 += NPC.Size / 2 * rCurrentNPC.scale;
            int iteration = 0;
            float num215 = 2f / (float)rCurrentNPC.oldPos.Length * 0.7f;
            float just600f = 600f;
            float just600fbutless = just600f - 30f;
            float lmao = Utils.Remap(rCurrentNPC.ai[2], 0f, just600f, 0f, 1f);
            float nofuckingcluetbh = 1f - Utils.Remap(lmao, 0.5f, just600fbutless / just600f, 0f, 1f) * Utils.Remap(lmao, just600fbutless / just600f, 1f, 1f, 0f);
            int oldposShit = rCurrentNPC.oldPos.Length - 1;
            while ((float)oldposShit >= 1f)
            {
                for (int num221 = 0; num221 < 2; num221++)
                {
                    value59 = ((oldposShit == 7 && num221 == 1) ? a : d);
                    Vector2 position21 = vector63 + rCurrentNPC.oldPos[oldposShit] - rCurrentNPC.position;
                    float rotation10 = rCurrentNPC.oldRot[oldposShit];
                    if (oldposShit >= 1 && num221 == 1)
                    {
                        Vector2 vector64 = Vector2.Lerp(rCurrentNPC.oldPos[oldposShit], rCurrentNPC.oldPos[oldposShit - 1], 0.5f) - rCurrentNPC.oldPos[oldposShit];
                        rotation10 = MathHelper.WrapAngle(rCurrentNPC.oldRot[oldposShit - 1] * 0.5f + rCurrentNPC.oldRot[oldposShit] * 0.5f);
                        position21 += vector64;
                    }
                    float aaa = NPC.direction == 1 ? value59.Height : 0;
                    Vector2 origin = new(value59.Width / 2, aaa);
                    if (value59 == a)
                        origin = new(value59.Width - 4, aaa);
                    if (value59 == b)
                        spriteBatch.Draw(value59, position21, null, rCurrentNPC.GetAlpha(drawColor), rotation10, new(value59.Width / 2, value59.Height / 2), NPC.scale, SpriteEffects.None /*value59 == a ? effects2 : SpriteEffects.None*/, 0f);
                    else
                        spriteBatch.Draw(value59, position21, null, rCurrentNPC.GetAlpha(drawColor), rotation10, new(value59.Width / 2, aaa), NPC.scale, value59 == a ? effects2 : SpriteEffects.None, 0f);
                    //value59 = ((iteration != 0) ? TextureAssets.GlowMask[133].Value : TextureAssets.GlowMask[134].Value);
                    //spriteBatch.Draw(value59, position21, null, new Microsoft.Xna.Framework.Color(255, 255, 255, 0) * (1f - num215 * (float)oldposShit / 2f) * nofuckingcluetbh, rotation10, NPC.Size / 2, scale7, effects, 0f);
                    iteration++;
                }
                oldposShit -= 2;
            }
            spriteBatch.Draw(c, vector63, rCurrentNPC.frame, rCurrentNPC.GetAlpha(drawColor), rCurrentNPC.rotation, NPC.Size / 2, rCurrentNPC.scale, effects, 0f);
            //Main.EntitySpriteDraw(a, posA - screenPos, null, Color.White, NPC.rotation, a.Size() / 2, 1, effects, 0);
            /*for (int i = 0; i < 4; i++)
            {
                Vector2 posB = NPC.Center - (new Vector2(a.Width * 0.8f, 0).RotatedBy(tailRot[0])) - (new Vector2((b.Width * 0.8f) - i * 6, 0)).RotatedBy(tailRot[i]) - Main.screenPosition;
                Main.EntitySpriteDraw(b, posB, new Rectangle(i * 6, 0, 24, 6), Color.White, tailRot[i], b.Size() / 2, 1, effects, 0);
            }*/
            //Main.EntitySpriteDraw(c, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, c.Size() / 2, 1, effects, 0);
            return false;
        }
        int fml;
        int savedDir;
        public override void AI()
        {
            //if (state != 1)
            //{
            for (int num25 = NPC.oldPos.Length - 1; num25 > 0; num25--)
            {
                NPC.oldPos[num25] = NPC.oldPos[num25 - 1];
                NPC.oldRot[num25] = NPC.oldRot[num25 - 1];
            }
            NPC.oldPos[0] = NPC.position;
            NPC.oldRot[0] = NPC.rotation;
            float amount = 0.65f;
            int num26 = 1;
            for (int num27 = 0; num27 < num26; num27++)
            {
                for (int num28 = NPC.oldPos.Length - 1; num28 > 0; num28--)
                {
                    if (!(NPC.oldPos[num28] == Vector2.Zero))
                    {
                        if (NPC.oldPos[num28].Distance(NPC.oldPos[num28 - 1]) > 2f)
                        {
                            NPC.oldPos[num28] = Vector2.Lerp(NPC.oldPos[num28], NPC.oldPos[num28 - 1], amount);
                        }
                        NPC.oldRot[num28] = (NPC.oldPos[num28 - 1] - NPC.oldPos[num28]).SafeNormalize(Vector2.Zero).ToRotation();
                    }
                }
            }
            //}
            if (fml % 5 == 0)
                NPC.direction = NPC.spriteDirection = NPC.velocity.X > 0 ? 1 : -1;
            //NPC.velocity = Helper.FromAToB(NPC.Center, Main.MouseWorld) * 5;
            Player player = Main.player[NPC.target];
            NPC.TargetClosest(false);
            if (player.Center.Distance(NPC.Center) < 100 && cooldown <= 0)
            {
                if (player.velocity.X > 4f || player.velocity.X < -4f)
                {
                    state = 2;
                }
                else
                {
                    state = 1;
                }
                cooldown = 200;
            }
            if (cooldown > 0)
                cooldown--;
            else
                state = 0;
            //if (state != 3)
            if (!NPC.wet)
                state = 3;
            else if (NPC.wet && cooldown <= 0)
                state = 0;
            else
                state = 1;
            //NPC.rotation = MathHelper.Lerp(NPC.rotation, NPC.rotation, 0.25f);
            fml++;
            switch (state)
            {
                case 0:
                    NPC.rotation = NPC.velocity.ToRotation();
                    Vector2 vel = new();
                    //if (TRay.CastLength(NPC.Center, -Vector2.UnitY, 100) < 16 * 32)
                    //{

                    NPC.rotation += MathHelper.ToRadians(Main.rand.NextFloat(-1, 1f) * 5);
                    vel = NPC.rotation.ToRotationVector2() * 5f;
                    NPC.noGravity = true;
                    //}
                    //else
                    /*if (TRay.CastLength(NPC.Center, Vector2.UnitX, 300) < 16 * 1.5f && NPC.direction == 1)
                    {
                        NPC.rotation = NPC.rotation - MathHelper.ToRadians(100);
                    }
                    if (TRay.CastLength(NPC.Center, -Vector2.UnitX, 300) < 16 * 1.5f && NPC.direction == -1)
                    {
                        NPC.rotation = NPC.rotation + MathHelper.ToRadians(100);
                    }*/
                    NPC.velocity = vel;
                    NPC.frame.Y = 0;
                    break;
                case 1:
                    NPC.noGravity = true;
                    NPC.rotation += MathHelper.ToRadians(1);
                    NPC.velocity = NPC.rotation.ToRotationVector2();
                    NPC.frame.Y = NPC.height;
                    break;
                case 2:
                    NPC.rotation = NPC.velocity.ToRotation();
                    NPC.noGravity = true;
                    NPC.velocity = Helper.FromAToB(NPC.Center, player.Center, reverse: true) * 20f;
                    state = 0;
                    NPC.frame.Y = 0;
                    break;
                case 3:
                    NPC.rotation = NPC.velocity.ToRotation();
                    NPC.noGravity = false;
                    NPC.frame.Y = 0;
                    NPC.velocity.X = 0;
                    //NPC.rotation = 0;
                    break;
            }
        }
    }
}
