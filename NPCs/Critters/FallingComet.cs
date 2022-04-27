using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using Terraria.Utilities;
using MythosOfMoonlight.Dusts;
using MythosOfMoonlight.Events;

namespace MythosOfMoonlight.NPCs.Critters
{
    public class FallingComet : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Purple Chunk");
            Main.npcFrameCount[npc.type] = 3;
			NPCID.Sets.TrailCacheLength[npc.type] = 10;
            NPCID.Sets.TrailingMode[npc.type] = 1;
        }
        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.width = 28;
            npc.height = 26;
            npc.defense = 10;
            npc.lifeMax = 10;
            npc.knockBackResist = 0f;
            npc.npcSlots = 1f;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.Item10;

            for (int i = 0; i < Main.maxBuffTypes; i++)
            {
                npc.buffImmune[i] = true;
            }
        }
		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor) //Glowmask code
		{
			Vector2 drawOrigin = npc.frame.Size() / 2;
			Vector2 drawPos = npc.position - Main.screenPosition + drawOrigin + new Vector2(0f);
			Color color;
		  
			drawPos = npc.position - Main.screenPosition + drawOrigin + new Vector2(5f);
			color = npc.GetAlpha(lightColor);

			spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, npc.frame, color, npc.rotation, drawOrigin, npc.scale, npc.spriteDirection == 0 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
			spriteBatch.Draw(mod.GetTexture("NPCs/Critters/FallingCometGlow"), drawPos, npc.frame, Color.White, npc.rotation, drawOrigin, npc.scale, npc.spriteDirection == 0 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);	
		}
		Vector2 Drawoffset => new Vector2(0) + Vector2.UnitX * npc.spriteDirection * 24;
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            //3hi31mg
            var off = new Vector2(npc.width / 2, npc.height / 2);
            var clr = new Color(255, 255, 255, 255); // full white
            var drawPos = npc.Center - Main.screenPosition;
            var texture = mod.GetTexture("NPCs/Critters/FallingCometTrail");
            var frame = new Rectangle(0, npc.frame.Y, npc.width, npc.height);
            var orig = frame.Size() / 2f;
            var trailLength = NPCID.Sets.TrailCacheLength[npc.type];

            for (int i = 1; i < trailLength; i++)
            {
                float scale = MathHelper.Lerp(1f, 0.1f, (float)(trailLength - i) / trailLength);
                var fadeMult = 1f / trailLength;
                Main.spriteBatch.Draw(texture, npc.oldPos[i] - Main.screenPosition + off, frame, clr * (scale - fadeMult * i), npc.oldRot[i], orig, 1f, SpriteEffects.None, 0f);
            }
            return true;
        }
            //private static int FRAMESPEED = 7;
            bool firstFrame = false;
        public override void AI()
        {
			if (firstFrame)
			{
				npc.frame.Y = Main.rand.Next(2) * npc.height;
			}

			if (npc.timeLeft > 60)
            {
                  if (Main.rand.NextBool(12))
            {
                int d = Dust.NewDust(npc.Center + new Vector2(5f, 0f).RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4) + npc.velocity.ToRotation()), 4, 4, ModContent.DustType<PurpurineStoneDust>());
                Main.dust[d].velocity = npc.velocity * 0.6f;
            } 
			}

			npc.velocity.Y = 12f; //Base velocity.
			npc.rotation += 0.03f; //Base rotation.
            if (npc.velocity.Y != 0)
            {
                npc.rotation += 0.2f;
                if (npc.soundDelay == 0)
                {
                    npc.soundDelay = 20 + Main.rand.Next(40); //Sound.
                    Main.PlaySound(SoundID.Item9, npc.Center);
                }
            }
            else
            {
                npc.rotation += 0.2f;
            }
			firstFrame = true;
            if (npc.velocity.Y > 0)
            {
                if (Main.rand.NextBool(12))
              {
                int d = Dust.NewDust(npc.Center + new Vector2(6f, 6f).RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4)), 5, 5,ModContent.DustType<Dusts.PurpurineStoneDust>());
                Main.dust[d].velocity = npc.velocity * 0.6f;
		      }
			}
			if (!npc.noGravity) //Custom thing to not use vanilla noGravity.
			{
				npc.velocity.Y += 0.3f;
				npc.rotation += 0.2f;
			}
			if (!npc.noTileCollide && npc.velocity.Y > 0) //Ensures tile collision(?) Also custom thing to not use vanilla noTileCollide.
			{
				npc.position.Y += Collision.TileCollision(npc.position, new Vector2(0, npc.velocity.Y), npc.width, npc.height).Y;
				npc.velocity.Y += 0.2f;
				npc.rotation += 0f;
			}
			
			if (Collision.TileCollision(npc.position, new Vector2(0, 16), npc.width, npc.height - 17) != new Vector2(0, 16) && npc.velocity.Y > 1.06) //Stuff that happens when chunk collides with tile.
			{
				    npc.velocity.Y = 0;
					npc.life = 0;
			for (int i = 0; i < 4; i++)
		        {
			    Main.PlaySound(SoundID.Item10, npc.Center); //Sound on collision with tile.

			    Vector2 scale = new Vector2(1, 1) + new Vector2(1, 1) * (float)Math.Sin(npc.ai[0] / 2) * 0.1f;
			    Vector2 offset = new Vector2(16, 0).RotatedBy(npc.rotation + i * MathHelper.PiOver2);
			    offset.X *= scale.X;
			    offset.Y *= scale.Y;
			    Vector2 position = npc.Center + offset;
		    	NPC.NewNPC((int)position.X, (int)position.Y, NPCType<NPCs.Critters.PurpleComet.SparkleSkittler>()); //NPC spawn.
		        }
				// This is part of "public override void AI()" which is a void, so using a return keyword doesn't do nothing but cause an error.
			if (npc.velocity.Y < 0)
            {
                    npc.life = 0;
            }
        }
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {	
			return !PurpleCometEvent.PurpleComet ? 0 : 0.17f;
		}
	
		public override bool CheckDead()
		{
		for (int i = 0; i < 5; i++)
		{
			Main.PlaySound(SoundID.Item10, npc.Center); //Sound when chunk is destroyed by player.

			Vector2 scale = new Vector2(1, 1) + new Vector2(1, -1) * (float)Math.Sin(npc.ai[0] / 2) * 0.1f;
			Vector2 offset = new Vector2(16, 0).RotatedBy(npc.rotation + i * MathHelper.PiOver2);
			offset.X *= scale.X;
			offset.Y *= scale.Y;
			Vector2 position = npc.Center + offset;
			NPC.NewNPC((int)position.X, (int)position.Y, NPCType<NPCs.Critters.PurpleComet.SparkleSkittler>()); //NPC spawn.
		}
		return true;
    }
}
}
   

