using HarmonyLib;

namespace ONIInsulatedSelfSealingAirLock
{
    // Re-apply sealed sim state after OnSpawn grid setup.
    [HarmonyPatch(typeof(Door), "OnSpawn")]
    public class Door_OnSpawn
    {
        public static void Postfix(Door __instance)
        {
            if (__instance.PrefabID() != InsulatedSelfSealingAirLockConfig.ID) return;

            InsulatedSelfSealingAirLockSimState.Apply(__instance, __instance.building.PlacementCells);
        }
    }
}
