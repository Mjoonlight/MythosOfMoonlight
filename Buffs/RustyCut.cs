using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Buffs
{
    public class RustyCut : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_68";
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }
        public override void Update(NPC NPC, ref int buffIndex)
        {
            if (NPC.lifeRegen > 0)
                NPC.lifeRegen = 0;
            if (Main.rand.NextBool(3))
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1), newColor: Color.Brown);
            NPC.lifeRegen -= 15;
        }
    }
}