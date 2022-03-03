using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight
{
    public class MoMPlayer : ModPlayer
    {
        public override void UpdateBiomeVisuals()
        {
            var purpleComet = PurpleCometEvent.PurpleComet;
            player.ManageSpecialBiomeVisuals("PurpleComet", purpleComet);
        }
        public Vector2 targetCameraPosition = new Vector2(-1, -1);
        public readonly Vector2 setToPlayer = new Vector2(-1, -1);
        public int source = -1;
        public float lerpSpeed;
        public void NewCameraPosition(Vector2 point, float lerpSpeed, int source)
        {
            targetCameraPosition = point - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
            this.lerpSpeed = lerpSpeed;
            this.source = source;
        }
        public override void ModifyScreenPosition()
        {

            if (targetCameraPosition != setToPlayer)
            {
                Main.screenPosition = targetCameraPosition;
            }
            if (source != -1)
            {
                if (!Main.npc[source].active)
                {
                    targetCameraPosition = setToPlayer;
                }
            }
        }
    }
}
