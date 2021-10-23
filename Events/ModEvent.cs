using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

public abstract class ModEvent
{
    public virtual int[] invaders => new int[1] { NPCID.Zombie };
    public static bool enabled;

    public virtual void StartInvasion()
    {
        if (Main.invasionType != 0)
        {
            return;
        }

        else
        {
            Main.invasionType = -1;
            Main.invasionSize = 100;
            Main.invasionSizeStart = Main.invasionSize;
            Main.invasionProgress = 0;
            Main.invasionProgressIcon = 3;
            Main.invasionProgressWave = 0;
            Main.invasionProgressMax = Main.invasionSizeStart;
            Main.invasionWarn = 3600;
            Main.invasionX = (double)Main.maxTilesX;
        }
    }
}