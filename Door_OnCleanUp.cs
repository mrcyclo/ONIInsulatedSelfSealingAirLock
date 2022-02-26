using HarmonyLib;

namespace ONIInsulatedSelfSealingAirLock
{
    [HarmonyPatch(typeof(Door), "OnCleanUp")]
    public class Door_OnCleanUp
    {
        public static void Prefix(Door __instance)
        {
            foreach (int num in __instance.building.PlacementCells)
            {
                SimMessages.ClearCellProperties(num, 7);
            }
        }
    }
}
