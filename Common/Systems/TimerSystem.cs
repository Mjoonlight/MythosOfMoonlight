using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Common.Systems
{
    public class TimerSystem : ModSystem
    {
        public static float TimerNoPause;
        public static float Timer;
        public override void PreUpdateEntities()
        {
            if (Timer <= 999999)
            {
                if (!Main.gamePaused) Timer++;
            }
            else Timer = 0;
            if (TimerNoPause <= 999999) TimerNoPause++;
            else TimerNoPause = 0;
        }
    }
}
