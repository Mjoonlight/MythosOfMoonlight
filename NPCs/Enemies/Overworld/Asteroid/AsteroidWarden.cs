using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using System.IO;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using MythosOfMoonlight.Items.Accessories;
using MythosOfMoonlight.Items.Weapons;
using MythosOfMoonlight.Common.Crossmod;

namespace MythosOfMoonlight.NPCs.Enemies.Overworld.Asteroid
{
    public class AsteroidWarden : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 4;
            NPC.AddNPCElementList("Inorganic");
        }
        public override void SetDefaults()
        {
            NPC.Size = new Vector2(32);
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.damage = 0;
            NPC.defense = 5;
            NPC.lifeMax = 400;
            NPC.value = Item.buyPrice(0, 0, 5, 15);
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0.6f;
            NPC.HitSound = SoundID.NPCHit13;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("These cosmic watchers come down from space wherever they sense a high amount of arcane star energy, such as the flowing river of starlight above the skies during meteor showers.")
            });
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
        public float handRot
        {
            get => NPC.ai[3];
            set => NPC.ai[3] = value;
        }
        public Vector2 handOffset;
        public float headRot;
        public int handFrameY;
        int next = 1;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(handOffset);
            writer.Write(headRot);
            writer.Write(handFrameY);
            writer.Write(next);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            handOffset = reader.ReadVector2();
            headRot = reader.ReadSingle();
            handFrameY = reader.Read();
            next = reader.Read();
        }
        public override void FindFrame(int frameHeight)
        {
            if (++NPC.frameCounter % 5 == 0)
            {
                if (NPC.frame.Y < frameHeight * 3)
                    NPC.frame.Y += frameHeight;
                else
                    NPC.frame.Y = 0;
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StarBit>(), 15));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<WardingStar>(), 15));
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item14, NPC.Center);
                Color newColor7 = Color.CornflowerBlue;
                for (int num613 = 0; num613 < 7; num613++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Enchanted_Pink, NPC.velocity.X * 0.1f, NPC.velocity.Y * 0.1f, 150, default, 0.8f);
                }
                for (float num614 = 0f; num614 < 1f; num614 += 0.125f)
                {
                    Dust.NewDustPerfect(NPC.Center, 278, Vector2.UnitY.RotatedBy(num614 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (4f + Main.rand.NextFloat() * 4f), 150, newColor7).noGravity = true;
                }
                for (float num615 = 0f; num615 < 1f; num615 += 0.25f)
                {
                    Dust.NewDustPerfect(NPC.Center, 278, Vector2.UnitY.RotatedBy(num615 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (2f + Main.rand.NextFloat() * 3f), 150, Color.Gold).noGravity = true;
                }
                Vector2 vector52 = new Vector2(Main.screenWidth, Main.screenHeight);
                if (NPC.Hitbox.Intersects(Utils.CenteredRectangle(Main.screenPosition + vector52 / 2f, vector52 + new Vector2(400f))))
                {
                    for (int num616 = 0; num616 < 7; num616++)
                    {
                        Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, Main.rand.NextVector2CircularEdge(0.5f, 0.5f) * NPC.velocity.Length(), Utils.SelectRandom(Main.rand, 16, 17, 17, 17, 17, 17, 17, 17));
                    }
                }
                Helper.SpawnGore(NPC, "MythosOfMoonlight/Warden", vel: -Vector2.UnitY * 3);
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Player player = Main.player[NPC.target];
            Texture2D tex = TextureAssets.Npc[Type].Value; // https://cdn.discordapp.com/attachments/795335225034670100/1114973113281675367/u23t27yzz0y21.png
            Texture2D head = ModContent.Request<Texture2D>(NPC.ModNPC.Texture + "Head").Value;
            Texture2D hand = ModContent.Request<Texture2D>(NPC.ModNPC.Texture + "Hand").Value;
            Texture2D handGlow = ModContent.Request<Texture2D>(NPC.ModNPC.Texture + "Hand_Glow").Value;
            Texture2D headGlow = ModContent.Request<Texture2D>(NPC.ModNPC.Texture + "Head_Glow").Value;
            Texture2D glow = ModContent.Request<Texture2D>(NPC.ModNPC.Texture + "_Glow").Value;
            spriteBatch.Draw(hand, NPC.Center - screenPos + new Vector2(AIState == 2 ? handOffset.X : -handOffset.X, -handOffset.Y), new Rectangle(0, handFrameY * 24, 18, 24), drawColor, handRot, new Vector2(18, 24) / 2, NPC.scale, handRot < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(handGlow, NPC.Center - screenPos + new Vector2(AIState == 2 ? handOffset.X : -handOffset.X, -handOffset.Y), new Rectangle(0, handFrameY * 24, 18, 24), Color.White, handRot, new Vector2(18, 24) / 2, NPC.scale, handRot < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            spriteBatch.Draw(tex, NPC.Center - screenPos, NPC.frame, drawColor, NPC.rotation, NPC.Size / 2, NPC.scale, NPC.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            spriteBatch.Draw(glow, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, NPC.Size / 2, NPC.scale, NPC.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            spriteBatch.Draw(head, NPC.Center - new Vector2(0, 25) - screenPos, null, drawColor, headRot, head.Size() / 2, NPC.scale, SpriteEffects.None, 0);
            spriteBatch.Draw(headGlow, NPC.Center - new Vector2(0, 25) - screenPos, null, Color.White, headRot, head.Size() / 2, NPC.scale, SpriteEffects.None, 0);

            spriteBatch.Draw(hand, NPC.Center - screenPos + new Vector2(handOffset.X, handOffset.Y), new Rectangle(0, handFrameY * 24, 18, 24), drawColor, handRot, new Vector2(18, 24) / 2, NPC.scale, handRot < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            spriteBatch.Draw(handGlow, NPC.Center - screenPos + new Vector2(handOffset.X, handOffset.Y), new Rectangle(0, handFrameY * 24, 18, 24), Color.White, handRot, new Vector2(18, 24) / 2, NPC.scale, handRot < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D vortex = ModContent.Request<Texture2D>("MythosOfMoonlight/Assets/Textures/Extra/vortex").Value;
            Texture2D star = ModContent.Request<Texture2D>("MythosOfMoonlight/Assets/Textures/star0").Value;
            if (AIState == 2)
            {
                spriteBatch.Reload(BlendState.Additive);
                spriteBatch.Draw(vortex, NPC.Center + new Vector2(handOffset.X, 0) - screenPos, null, Color.CornflowerBlue * 0.75f * AITimer2, Main.GameUpdateCount * 0.005f, vortex.Size() / 2, 0.05f, SpriteEffects.None, 0);
                spriteBatch.Draw(star, NPC.Center + new Vector2(handOffset.X, 0) - screenPos, null, Color.White * AITimer2, Main.GameUpdateCount * 0.005f, star.Size() / 2, 1, SpriteEffects.None, 0);
                spriteBatch.Reload(BlendState.AlphaBlend);
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return (Star.starfallBoost > 2 && !Main.dayTime && spawnInfo.Player.ZoneNormalSpace) ? 0.1f : 0;
        }
        public override void AI()
        {
            Player player = Main.player[NPC.target];
            AITimer2 = MathHelper.Clamp(AITimer2, 0, 1f);
            if (Main.dayTime)
            {
                NPC.velocity.Y--;
                return;
            }
            if (AIState == 0)
            {
                NPC.TargetClosest(false);
                handOffset = Vector2.Lerp(handOffset, new Vector2(20, 0), 0.2f);
                handRot = MathHelper.Lerp(handRot, MathHelper.Pi, 0.2f);
                headRot = MathHelper.Lerp(headRot, Helper.FromAToB(NPC.Center - new Vector2(0, 25), player.Center).ToRotation() - MathHelper.Pi, 0.1f);
                handFrameY = 0;
                NPC.direction = player.Center.X > NPC.Center.X ? 1 : -1;
                NPC.spriteDirection = NPC.direction;

                if (!Main.dayTime && player.Center.Distance(NPC.Center) < 1050)
                    AITimer++;

                if (player.Center.Distance(NPC.Center) < 200)
                    NPC.velocity = Vector2.Lerp(NPC.velocity, Helper.FromAToB(NPC.Center, player.Center + new Vector2(100, 0).RotatedBy(Helper.FromAToB(NPC.Center, player.Center, true, true).ToRotation()), false) / 55, 0.05f);
                else
                    NPC.velocity = Vector2.Lerp(NPC.velocity, Helper.FromAToB(NPC.Center, player.Center + new Vector2(100, 0).RotatedBy(Helper.FromAToB(NPC.Center, player.Center, true, true).ToRotation()), true) * 10, 0.05f);

                if (AITimer > 200)
                {

                    NPC.velocity = Vector2.Zero;
                    AITimer = 0;
                    AIState = next;
                }
            }
            else if (AIState == 1)
            {
                NPC.velocity *= 0.98f;
                headRot = MathHelper.Lerp(headRot, Main.GameUpdateCount * 0.005f, 0.1f);
                AITimer++;
                if (AITimer < 30)
                {
                    handOffset = Vector2.Lerp(handOffset, new Vector2(10, -40), 0.05f);
                    handRot = MathHelper.Lerp(handRot, MathHelper.PiOver4 * NPC.direction, 0.05f);
                    handFrameY = 1;
                }
                else if (AITimer > 40)
                {
                    handOffset = Vector2.Lerp(handOffset, new Vector2(-20, 10), 0.2f);
                    handRot = MathHelper.Lerp(handRot, MathHelper.Pi - MathHelper.PiOver4 * NPC.direction, 0.2f);
                    handFrameY = 0;
                }
                if (AITimer == 50)
                {
                    SoundStyle style = SoundID.AbigailSummon;
                    style.Volume = 0.5f;
                    SoundEngine.PlaySound(style, NPC.Center);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(100 * NPC.direction, 0), Vector2.Zero, ModContent.ProjectileType<WardenSigil>(), 0, 0);
                }
                if (AITimer > 100)
                {
                    NPC.velocity = Vector2.Zero;
                    AITimer = 0;
                    AIState = 0;//Main.rand.Next(1, 3);
                    next = 2;
                }
            }
            else
            {
                NPC.velocity *= 0.98f;
                headRot = MathHelper.Lerp(headRot, Main.GameUpdateCount * 0.005f, 0.1f);
                AITimer++;
                if (AITimer < 11)
                    AITimer2 += 0.1f;

                if (AITimer > 145)
                    AITimer2 -= 0.2f;

                handOffset = Vector2.Lerp(handOffset, new Vector2(20 * NPC.direction, 15), 0.2f);
                handRot = MathHelper.Lerp(handRot, MathHelper.PiOver2 * NPC.direction, 0.2f);
                handFrameY = 1;
                if (AITimer % 40 == 0 && AITimer >= 40)
                {
                    SoundStyle style = SoundID.Item82;
                    style.Volume = 0.5f;
                    SoundEngine.PlaySound(style, NPC.Center);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(20 * NPC.direction, 0), Helper.FromAToB(NPC.Center, player.Center) * 0.1f, ModContent.ProjectileType<WardenStar>(), 10, 0);
                }
                if (AITimer >= 150)
                {
                    AITimer = 0;
                    AITimer2 = 0;
                    AIState = 0;
                    next = 1;
                }
            }
        }
    }
    public class WardenStar : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            Projectile.AddElement(CrossModHelper.Celestial);
        }
        public override void SetDefaults()
        {
            Projectile.damage = 10;
            Projectile.width = 26;
            Projectile.height = 60;
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 500;
        }

        public override void AI()
        {
            if (Projectile.velocity.Length() < 25f)
                Projectile.velocity *= 1.1f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            float progress = Utils.GetLerpValue(0, 500, Projectile.timeLeft);
            Projectile.scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 5, 0, 1);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D drawTexture = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle sourceRectangle = new(0, 0, drawTexture.Width, drawTexture.Height);
            Main.EntitySpriteDraw(drawTexture, Projectile.Center - Main.screenPosition, sourceRectangle, Color.White * Projectile.scale, Projectile.rotation, drawTexture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);

            //3hi31mg
            var off = new Vector2(Projectile.width / 2, Projectile.height / 2);
            var clr = new Color(255, 255, 255, 255); // full white
            var texture = TextureAssets.Projectile[Projectile.type].Value;
            var frame = new Rectangle(0, Projectile.frame, Projectile.width, Projectile.height);
            var orig = frame.Size() / 2f;
            var trailLength = ProjectileID.Sets.TrailCacheLength[Projectile.type];

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 1; i < trailLength; i++)
            {
                float scale = MathHelper.Lerp(0.70f, 1f, (float)(trailLength - i) / trailLength);
                var fadeMult = 1f / trailLength;
                SpriteEffects flipType = Projectile.spriteDirection == -1 /* or 1, idfk */ ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i] - Main.screenPosition + off, frame, clr * Projectile.scale * (1f - fadeMult * i), Projectile.oldRot[i], orig, scale * Projectile.scale, flipType, 0f);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
    public class WardenSigil : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.damage = 0;
            Projectile.width = 26;
            Projectile.timeLeft = 121;
            Projectile.height = 60;
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D vortex = ModContent.Request<Texture2D>("MythosOfMoonlight/Assets/Textures/Extra/vortex").Value;
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Main.spriteBatch.Reload(BlendState.Additive);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Main.GameUpdateCount * 0.005f, tex.Size() / 2, 0.25f * Projectile.scale, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(vortex, Projectile.Center - Main.screenPosition, null, Color.Gold * 0.5f, -Main.GameUpdateCount * 0.005f, vortex.Size() / 2, 0.2f * Projectile.scale, SpriteEffects.None, 0);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
        }
        public override void AI()
        {
            float progress = Utils.GetLerpValue(0, 121, Projectile.timeLeft);
            Projectile.scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 3, 0, 1);
            if (Projectile.ai[0] == 25)
            {
                Color newColor7 = Color.CornflowerBlue;
                for (int num613 = 0; num613 < 7; num613++)
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Enchanted_Pink, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 150, default, 0.8f);
                }
                for (float num614 = 0f; num614 < 1f; num614 += 0.125f)
                {
                    Dust.NewDustPerfect(Projectile.Center, 278, Vector2.UnitY.RotatedBy(num614 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (4f + Main.rand.NextFloat() * 4f), 150, newColor7).noGravity = true;
                }
                for (float num615 = 0f; num615 < 1f; num615 += 0.25f)
                {
                    Dust.NewDustPerfect(Projectile.Center, 278, Vector2.UnitY.RotatedBy(num615 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (2f + Main.rand.NextFloat() * 3f), 150, Color.Gold).noGravity = true;
                }
                Vector2 vector52 = new Vector2(Main.screenWidth, Main.screenHeight);
                if (Projectile.Hitbox.Intersects(Utils.CenteredRectangle(Main.screenPosition + vector52 / 2f, vector52 + new Vector2(400f))))
                {
                    for (int num616 = 0; num616 < 7; num616++)
                    {
                        Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Main.rand.NextVector2CircularEdge(0.5f, 0.5f) * Projectile.velocity.Length(), Utils.SelectRandom(Main.rand, 16, 17, 17, 17, 17, 17, 17, 17));
                    }
                }
            }
            if (++Projectile.ai[0] == 50)
            {
                for (int i = 0; i < 5; i++)
                {
                    Projectile.ai[1] += MathHelper.Pi / 2.5f;
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + new Vector2(60, 0).RotatedBy(Projectile.ai[1]), new Vector2(0.15f, 0).RotatedBy(Projectile.ai[1]), ModContent.ProjectileType<WardenStar>(), 10, 0);
                }
            }
        }
    }
}