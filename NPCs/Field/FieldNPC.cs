using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs.Field
{
    public class FieldNPCTypes
    {
        public static int Friendly = 0, Sinister = 1, Mysterious = 2;
    }
    public abstract class FieldNPC : ModNPC
    {
        public bool shouldMusic;
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
                shouldMusic = true;
            }
            OnButtonClick(firstButton, ref shop);
        }
        public override void AI()
        {
            NPC.homeless = true;

            if (Main.npcChatText != "")
            {
                NPC.direction = NPC.Center.X < Main.LocalPlayer.Center.X ? -1 : 1;
                NPC.velocity.X = 0;
            }
            if (shouldMusic)
            {
                FieldSpawnRateNPC.rateDecrease = true;
                if (Main.LocalPlayer.Center.Distance(NPC.Center) > 1000)
                {
                    shouldMusic = false;
                }
            }
            else
            {
                FieldSpawnRateNPC.rateDecrease = false;
                FieldSpawnRateNPC.activeNPC = -1;
            }
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
    public class FriendlyFieldSceneEffect : ModSceneEffect
    {
        public override int Music => MusicID.TownDay;
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override bool IsSceneEffectActive(Player player)
        {
            return FieldSpawnRateNPC.rateDecrease && FieldSpawnRateNPC.activeNPC == 0;
        }
    }
    public class FieldSpawnRateNPC : GlobalNPC
    {
        public static int activeNPC;
        public static bool rateDecrease;
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (rateDecrease)
                spawnRate = 2000;
        }
    }
}
