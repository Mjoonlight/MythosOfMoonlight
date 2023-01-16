/*using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using MythosOfMoonlight.Common.Systems;
using MythosOfMoonlight.NPCs.Enemies.CometFlyby.CometEmber;
using MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim.Projectiles;
using MythosOfMoonlight.NPCs.Minibosses.StarveiledProj;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
namespace MythosOfMoonlight.NPCs.Minibosses.StarveiledProj
{
    public class ScholarRift : ModNPC
    {
        public override string Texture => "MythosOfMoonlight/Textures/Extra/corona_transparent";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rift");
        }
        public override void SetDefaults()
        {
            NPC.lifeMax = 200;
            NPC.defense = 10;
            NPC.damage = 0;
            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.knockBackResist = 0f;
            NPC.noTileCollide = true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
            DrawData a = new(tex, NPC.Center - screenPos, null, Color.White, Main.GameUpdateCount * 0.0035f, tex.Size() / 2, 1, SpriteEffects.None, 0);
            DrawData b = new(tex, NPC.Center - screenPos, null, Color.White * 0.85f, Main.GameUpdateCount * 0.0055f, tex.Size() / 2, 0.75f, SpriteEffects.None, 0);
            GameShaders.Armor.GetShaderFromItemId(ItemID.TwilightDye).Apply(NPC, a);
            GameShaders.Armor.GetShaderFromItemId(ItemID.TwilightDye).Apply(NPC, b);
            for (int i = 0; i < 2; i++)
            {
                a.Draw(spriteBatch);
                b.Draw(spriteBatch);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
            return false;
        }
        public override void AI()
        {
            NPC owner = Main.npc[(int)NPC.ai[0]];
            if (!owner.active || owner.type != ModContent.NPCType<StarveiledScholar>())
                NPC.life = 0;
            Player player = Main.player[owner.target];
        }
    }
}
*/