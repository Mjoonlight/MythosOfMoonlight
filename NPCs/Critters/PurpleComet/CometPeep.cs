using Microsoft.Xna.Framework;
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
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new(0) { Velocity = 1 };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetDefaults()
        {
            NPC.width = NPC.height = 22;
            NPC.friendly = true;
            NPC.aiStyle = -1;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
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
                if (NPC.ai[0] < 1)
                {
                    NPC.ai[0]++;
                    return true;
                }
                return false;
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
            Main.spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, frame, clr, 0f, frame.Size() / 2, 1f, SpriteEffects.None, 0f);
            return false;
        }
        */
        public override void AI()
        {
            if (IsFirstFrame)
            {
                NPC.direction = NPC.Center.X < Helper.CoordToTile(Main.LocalPlayer.Center).X ? 1 : -1;
                horizontalSpeed = Main.rand.NextFloat(9, 12);
            }

            NPC.noGravity = false; // initialize gravity
            var tilePos = Helper.CoordToTile(NPC.Center); // first convert position to be used for tile coordinates
            for (int y = (int)tilePos.Y; y < tilePos.Y + VerticalTileRange; y++) // go from the y position of the tile coordinates to 3 tiles below the y position of the tile coordinates
            {
                var tileY = Framing.GetTileSafely((int)tilePos.X, y);
                var tileX = Helper.GetTileInHorizontalRange(tilePos.X, y, HorizontalTileRange);
                if (!tileX.HasTile)
                {
                    if (tileX.TileFrameX - (int)tilePos.X == 1)
                    {
                        NPC.direction = -NPC.direction;
                        break;
                    }
                }
                else if (!tileX.HasTile) // if the tile at the y coordinate in the loop and the tilePos x coordinate  position is not active
                {
                    var getTile = Framing.GetTileSafely((int)tilePos.X, (int)tilePos.Y - 1);
                    if (!tileX.HasTile)
                    {
                        if (Main.tileSolid[getTile.TileType])
                            NPC.direction = -NPC.direction;
                    }
                    else
                    {
                        NPC.noGravity = true; // turn off gravity to overcome acceleration
                        NPC.velocity.Y = MathHelper.Lerp(NPC.velocity.Y, RecoverySpeed, 0.1f); // increase (decrease) vertical velocity by recovery speed
                    }
                    break;
                }
            }
            NPC.velocity.X = horizontalSpeed * NPC.direction; // move forward/backward according to direction
        }

        /*
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0.2f;
        }
        */
    }
}
