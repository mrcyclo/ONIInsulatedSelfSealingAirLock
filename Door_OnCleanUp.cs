using HarmonyLib;

namespace ONIInsulatedSelfSealingAirLock
{
    [HarmonyPatch(typeof(Door), "OnCleanUp")]
    public class Door_OnCleanUp
    {
        public static void Prefix(Door __instance)
        {
            Debug.Log("[InsulatedSelfSealingAirLock] Resetting element transmission for destroyed doors");
            foreach (int cell in __instance.building.PlacementCells)
            {
                SimMessages.ClearCellProperties(cell, 7);
            }
        }
    }
}
