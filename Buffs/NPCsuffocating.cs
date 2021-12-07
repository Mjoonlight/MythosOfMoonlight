using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

public class NPCsuffocating : ModBuff
{
    public override bool Autoload(ref string name, ref string texture)
    {
		texture = "Terraria/Buff_68";
        return true;
    }
    public override void SetDefaults()
	{
		DisplayName.SetDefault("Choking");
		Description.SetDefault("haha dumb enemies");
		Main.debuff[Type] = true;
		Main.pvpBuff[Type] = true;
		Main.buffNoSave[Type] = true;
		longerExpertDebuff = false;
	}
    public override void Update(NPC npc, ref int buffIndex)
	{
		if (npc.lifeRegen > 0)
		{
			npc.lifeRegen = 0;
		}
		npc.lifeRegen -= 40;
	}
}