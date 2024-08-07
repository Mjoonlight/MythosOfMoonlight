using Microsoft.Xna.Framework;
using MythosOfMoonlight.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Common.Globals
{
    public class MoMGlobalNPC : GlobalNPC
    {
        public bool coldwind;
        public float coldwindCD;
        public override void ResetEffects(NPC npc)
        {
            if (coldwindCD > 0)
                coldwindCD--;
            coldwind = false;
        }
        public override bool PreAI(NPC npc)
        {
            if (coldwind)
            {
                npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y, 0, 100);
                npc.oldVelocity.X = 0;
                npc.velocity.X = 0;
                npc.direction = npc.oldDirection;
            }
            return base.PreAI(npc);
        }
        public override void AI(NPC npc)
        {
            if (coldwindCD >= 350)
            {
                npc.AddBuff(ModContent.BuffType<ColdwindDebuff>(), (int)(60 * 2 * (npc.knockBackResist + .1f)));
                coldwindCD = -100;
            }
            if (coldwind)
            {
                npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y, 0, 100);
                npc.oldVelocity.X = 0;
                npc.velocity.X = 0;
                npc.direction = npc.oldDirection;
            }
        }
        public override void PostAI(NPC npc)
        {
            if (coldwind)
            {
                npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y, 0, 100);
                npc.oldVelocity.X = 0;
                npc.velocity.X = 0;
                npc.direction = npc.oldDirection;
            }
        }
        public override bool InstancePerEntity => true;
    }
}
