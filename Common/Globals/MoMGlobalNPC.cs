using Microsoft.Xna.Framework;
using MythosOfMoonlight.Buffs;
using MythosOfMoonlight.Dusts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Common.Globals
{
    public class MoMGlobalNPC : GlobalNPC
    {
        public float coldwindCD;
        public override void ResetEffects(NPC npc)
        {
            if (coldwindCD > 0)
                coldwindCD--;
        }
        public override bool PreAI(NPC npc)
        {
            if (npc.HasBuff<ColdwindDebuff>())
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
            if (coldwindCD >= 270)
            {
                for (int i = 0; i < 15; i++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<ColdwindDust>(), Main.rand.NextFloat(-10, 10), Main.rand.NextFloat(-15, -1), 0, default, Main.rand.NextFloat(0.3f, 0.6f));
                    if (Main.rand.NextBool())
                        Dust.NewDust(npc.position, npc.width, npc.height, DustID.Frost, Main.rand.NextFloat(-10, 10), Main.rand.NextFloat(-15, -1), 0, default, Main.rand.NextFloat(.25f, .75f));
                }
                npc.StrikeNPC(new NPC.HitInfo() { Damage = 40, DamageType = DamageClass.Default });
                npc.AddBuff(ModContent.BuffType<ColdwindDebuff>(), (int)(60 * 2 * (npc.knockBackResist + .1f)));
                coldwindCD = -100;
            }
            if (npc.HasBuff<ColdwindDebuff>())
            {
                npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y, 0, 100);
                npc.oldVelocity.X = 0;
                npc.velocity.X = 0;
                npc.direction = npc.oldDirection;
            }
        }
        public override void PostAI(NPC npc)
        {
            if (npc.HasBuff<ColdwindDebuff>())
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
