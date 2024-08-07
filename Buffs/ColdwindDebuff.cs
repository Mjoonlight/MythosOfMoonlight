using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using MythosOfMoonlight.Items;
using MythosOfMoonlight.Common.Globals;
using MythosOfMoonlight.Dusts;

namespace MythosOfMoonlight.Buffs
{
    public class ColdwindDebuff : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_68";
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            BuffID.Sets.GrantImmunityWith[Type] = new List<int> { BuffID.Confused, BuffID.Frostburn };
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<ColdwindDust>(), npc.velocity.X * Main.rand.NextFloat(), Main.rand.NextFloat(-5, -1), 0, default, Main.rand.NextFloat(0.3f, 0.6f));
            if (Main.rand.NextBool())
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Frost, npc.velocity.X * Main.rand.NextFloat(), Main.rand.NextFloat(-5, -1), 0, default, Main.rand.NextFloat(.25f, .75f));
        }
    }
}
