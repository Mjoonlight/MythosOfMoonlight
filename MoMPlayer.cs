using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MythosOfMoonlight.Events;
using MythosOfMoonlight.NPCs.Enemies.RupturedPilgrim;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight
{
    public class MoMPlayer : ModPlayer
    {
        NPC sym => Starine_Symbol.symbol;
        public override void UpdateBiomeVisuals()
        {
            var purpleComet = PurpleCometEvent.PurpleComet && Main.LocalPlayer.ZoneOverworldHeight;
            player.ManageSpecialBiomeVisuals("PurpleComet", purpleComet);
        }
        public override void OnEnterWorld(Player player)
        {
            Starine_Symbol.symbol = null;
        }
        public Vector2 targetCameraPosition = new Vector2(-1, -1);
        public readonly Vector2 setToPlayer = new Vector2(-1, -1);
        public int source = -1;
        public float lerpSpeed;
        public float LerpTimer;
        public override void ResetEffects()
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc.type == ModContent.NPCType<Starine_Symbol>())
                {
                    if (npc.active)
                    {
                        if (npc.ai[0] == 1 || npc.ai[0] == 2)
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
            foreach (NPC npc in Main.npc)
            {
                if (npc.type == ModContent.NPCType<Starine_Symbol>())
                {
                    if (sym != null)
                    {
                        if (sym.active)
                        {
                            if (Vector2.Distance(player.Center, ((Starine_Symbol)sym.modNPC).CircleCenter) < 1000f)
                            {
                                Main.screenPosition = player.Center - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2) + (((Starine_Symbol)sym.modNPC).CircleCenter - player.Center) * (1 - (float)Math.Pow(0.95f, LerpTimer));
                            }
                        }
                    }
                }
            }
        }
    }
}
