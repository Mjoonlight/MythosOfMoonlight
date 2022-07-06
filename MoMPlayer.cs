using Microsoft.Xna.Framework;
using MythosOfMoonlight.NPCs.Enemies.RupturedPilgrim;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight
{
    public class MoMPlayer : ModPlayer
    {
        static NPC Sym => Starine_Symbol.symbol;
        //public override void UpdateBiomeVisuals()
        //{
        //    var purpleComet = PurpleCometEvent.PurpleComet && Main.LocalPlayer.ZoneOverworldHeight;
        //    player.ManageSpecialBiomeVisuals("PurpleComet", purpleComet);
        //}
        public override void OnEnterWorld(Player player)
        {
            Starine_Symbol.symbol = null;
            if (SymbolRespawnSystem.SymbolHome != Vector2.Zero)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC symbol = NPC.NewNPCDirect(null, SymbolRespawnSystem.SymbolHome, ModContent.NPCType<Starine_Symbol>());
                }
            }
        }
        public Vector2 targetCameraPosition = new(-1, -1);
        public readonly Vector2 setToPlayer = new(-1, -1);
        public int source = -1;
        public float lerpSpeed;
        public float LerpTimer;
        public override void ResetEffects()
        {
            foreach (NPC NPC in Main.npc)
            {
                if (NPC.type == ModContent.NPCType<Starine_Symbol>())
                {
                    if (NPC.active)
                    {
                        if (NPC.ai[0] == 1 || NPC.ai[0] == 2)
                        {
                            LerpTimer++;
                        }
                        else
                        {
                            LerpTimer = LerpTimer > 1 ? LerpTimer *= .9f : 0;
                        }
                    }
                }
            }
        }
        public void NewCameraPosition(Vector2 point, float lerpSpeed, int source)
        {
            targetCameraPosition = point - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
            this.lerpSpeed = lerpSpeed;
            this.source = source;
        }
        public override void ModifyScreenPosition()
        {
            foreach (NPC NPC in Main.npc)
            {
                if (NPC.type == ModContent.NPCType<Starine_Symbol>())
                {
                    if (Sym != null)
                    {
                        if (Sym.active)
                        {
                            if (Vector2.Distance(Player.Center, ((Starine_Symbol)Sym.ModNPC).CircleCenter) < 1000f)
                            {
                                Main.screenPosition = Player.Center - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2) + (((Starine_Symbol)Sym.ModNPC).CircleCenter - Player.Center) * (1 - (float)Math.Pow(0.95f, LerpTimer));
                            }
                        }
                    }
                }
            }
        }
    }
}
