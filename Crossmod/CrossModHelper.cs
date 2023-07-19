using Terraria.ModLoader;
using Terraria;

namespace MythosOfMoonlight
{
    public static class CrossModHelper
    {
        #region MoR
        public const short Arcane = 1;
        public const short Fire = 2;
        public const short Water = 3;
        public const short Ice = 4;
        public const short Earth = 5;
        public const short Wind = 6;
        public const short Thunder = 7;
        public const short Holy = 8;
        public const short Shadow = 9;
        public const short Nature = 10;
        public const short Poison = 11;
        public const short Blood = 12;
        public const short Psychic = 13;
        public const short Celestial = 14;
        public const short Explosive = 15;

        public static void AddItemToBluntSwing(this Item item)
        {
            if (!ModLoader.TryGetMod("Redemption", out var redemption))
                return;
            redemption.Call("addItemToBluntSwing", item.type);
        }
        public static void AddElement(this Item item, int ElementID)
        {
            if (!ModLoader.TryGetMod("Redemption", out var redemption))
                return;
            redemption.Call("addElementItem", ElementID, item.type);
        }
        public static void AddElement(this NPC npc, int ElementID)
        {
            if (!ModLoader.TryGetMod("Redemption", out var redemption))
                return;
            redemption.Call("addElementNPC", ElementID, npc.type);
        }
        public static void AddElement(this Projectile proj, int ElementID)
        {
            if (!ModLoader.TryGetMod("Redemption", out var redemption))
                return;
            redemption.Call("addElementProj", ElementID, proj.type);
        }
        public static void AddNPCElementList(this NPC npc, string TypeString)
        {
            if (!ModLoader.TryGetMod("Redemption", out var redemption))
                return;
            redemption.Call("addNPCToElementTypeList", TypeString, npc.type);
        }
        #endregion
    }
}