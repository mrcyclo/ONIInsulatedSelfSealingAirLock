using HarmonyLib;

namespace ONIInsulatedSelfSealingAirLock
{
    [HarmonyPatch(typeof(Door), "OnCleanUp")]
    public class Door_OnCleanUp
    {
        public static void Prefix(Door __instance)
        {
            if (__instance.PrefabID() != InsulatedSelfSealingAirLockConfig.ID) return;

            // Vanilla clears property mask 12 (8+4); our sealed state also sets gas/liquid impermeable (1+2).
            foreach (int cell in __instance.building.PlacementCells)
            {
                SimMessages.ClearCellProperties(cell, 7);
            }
        }
    }
}
