using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Tiles
{
    public class SymbolPointTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileMergeDirt[Type] = true;
            AddMapEntry(Color.Transparent);
            Main.tileBlockLight[Type] = false;
            Main.tileLavaDeath[Type] = false;
            Main.tileWaterDeath[Type] = false;
            Main.tileNoAttach[Type] = true;
            Main.tileMergeDirt[Type] = false;
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            return false;
        }
        public override bool KillSound(int i, int j, bool fail)
        {
            return false;
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            blockDamaged = false;
            return false;
        }
    }
}
