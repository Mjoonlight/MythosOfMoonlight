using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.GameContent;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MythosOfMoonlight.BossBars
{
    public class PilgrimBar : ModBossBar
    {
        public override Asset<Texture2D> GetIconTexture(ref Rectangle? iconFrame) => ModContent.Request<Texture2D>("MythosOfMoonlight/Assets/Textures/Extra/blank");

        public override bool? ModifyInfo(ref BigProgressBarInfo info, ref float life, ref float lifeMax, ref float shield, ref float shieldMax)
        {
            NPC npc = Main.npc[info.npcIndexToAimAt];
            if (!npc.active)
                return false;

            life = npc.life;
            lifeMax = npc.lifeMax;

            return true;
        }
    }
}
