using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MythosOfMoonlight.Dusts;
using MythosOfMoonlight.Items.PurpleComet.Critters;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs.Critters.PurpleComet
{
    public class CometPeep : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Comet Peep");
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void SetDefaults()
        {
            npc.width = npc.height = 22;
            npc.friendly = true;
            npc.aiStyle = -1;
            npc.defense = 0;
            npc.lifeMax = 5;
            npc.noGravity = false;
            npc.noTileCollide = false;
        }
        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }

        const float RecoverySpeed = -3f;
        const int VerticalTileRange = 7, HorizontalTileRange = 3;
        float horizontalSpeed = 1f;
        bool IsFirstFrame
        {
            get
            {
                if (npc.ai[0] < 1)
                {
                    npc.ai[0]++;
                    return true;
                } return false;
            }
        }
        /*
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            var texture = mod.GetTexture("NPCs/Critters/PurpleComet/CometPeep_Trail");
            var frame = texture.Bounds;
            var clr = Color.White;

            // Main.spriteBatch.End();
            // Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, frame, clr, 0f, frame.Size() / 2, 1f, SpriteEffects.None, 0f);
            return false;
        }
        */
        public override void AI()
        {
            if (IsFirstFrame)
            {
                npc.direction = npc.Center.X < Helper.CoordToTile(Main.LocalPlayer.Center).X ? 1 : -1;
                horizontalSpeed = Main.rand.NextFloat(9, 12);
            }

            npc.noGravity = false; // initialize gravity
            var tilePos = Helper.CoordToTile(npc.Center); // first convert position to be used for tile coordinates
            for (int y = (int)tilePos.Y; y < tilePos.Y + VerticalTileRange; y++) // go from the y position of the tile coordinates to 3 tiles below the y position of the tile coordinates
            {
                var tileY = Framing.GetTileSafely((int)tilePos.X, y);
                var tileX = Helper.GetTileInHorizontalRange(tilePos.X, y, HorizontalTileRange);
                if (tileX?.active() ?? false)
                {
                    if (tileX.frameX - (int)tilePos.X == 1)
                    {
                        npc.direction = -npc.direction;
                        break;
                    }
                }
                else if (tileY?.active() ?? false) // if the tile at the y coordinate in the loop and the tilePos x coordinate  position is not active
                {
                    var getTile = Framing.GetTileSafely((int)tilePos.X, (int)tilePos.Y - 1);
                    if (getTile?.active() ?? false)
                    {
                        if (Main.tileSolid[getTile.type])
                        {
                            npc.direction = -npc.direction;
                        }
                    }
                    else
                    {
                        npc.noGravity = true; // turn off gravity to overcome acceleration
                        npc.velocity.Y = MathHelper.Lerp(npc.velocity.Y, RecoverySpeed, 0.1f); // increase (decrease) vertical velocity by recovery speed
                    }
                    break;
                }
            }
            npc.velocity.X = horizontalSpeed * npc.direction; // move forward/backward according to direction
        }

        /*
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0.2f;
        }
        */
    }
}
