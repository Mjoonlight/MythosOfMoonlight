using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs.Field
{
    public abstract class FieldNPC : ModNPC
    {
        public virtual int _Music => MusicID.TownDay;
        public override void SetDefaults()
        {
            NPC.friendly = true;
            NPC.damage = 0;
            NPC.townNPC = true;
            TownNPCStayingHomeless = true;
            NPC.aiStyle = 7;
            Defaults();
        }
        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            return false;
        }
        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                shop = true;
                NPC.ai[3] = 1;
                OnButtonClick(firstButton, ref shop);
            }
        }
        public override void AI()
        {
            NPC.homeless = true;
            if (NPC.ai[3] == 1)
            {
                if (Main.LocalPlayer.Center.Distance(NPC.Center) > 1000)
                {
                    NPC.ai[3] = 0;
                }
                Music = _Music;
            }
            else
                Music = -1;
            _AI();
        }
        public virtual void Defaults()
        {

        }
        public virtual void OnButtonClick(bool firstButton, ref bool shop)
        {

        }
        public virtual void _AI()
        {

        }
    }
}
