using Terraria;
using Terraria.ID;

namespace MythosOfMoonlight.Events
{
    public abstract class ModInvasion
    {
        public virtual int[] Invaders => new int[1] { NPCID.Zombie };
        public static bool enabled;

        public virtual void StartInvasion()
        {
            if (Main.invasionType != 0)
                return;

            Main.invasionType = -1;
            Main.invasionSize = 100;
            Main.invasionSizeStart = Main.invasionSize;
            Main.invasionProgress = 0;
            Main.invasionProgressIcon = 3;
            Main.invasionProgressWave = 0;
            Main.invasionProgressMax = Main.invasionSizeStart;
            Main.invasionWarn = 3600;
            Main.invasionX = Main.maxTilesX;
        }
    }
}