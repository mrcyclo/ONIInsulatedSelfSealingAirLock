using HarmonyLib;

namespace ONIInsulatedSelfSealingAirLock
{
    // Must run after Door.OnPrefabInit, which can assign a null static overrideAnims array.
    [HarmonyPatch(typeof(Door), "OnPrefabInit")]
    internal class Door_OnPrefabInit
    {
        public static void Postfix(Door __instance)
        {
            if (__instance.PrefabID() != InsulatedSelfSealingAirLockConfig.ID) return;

            InsulatedSelfSealingAirLockWorkerAnims.Apply(__instance);
        }
    }
}
