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
    }
}
