using HarmonyLib;
using System.Collections.Generic;

namespace ONIInsulatedSelfSealingAirLock
{
    // Postfix (not Prefix) so vanilla SetSimState runs first (Dig / ReplaceAndDisplaceElement),
    // then we re-apply insulation and impermeability on top. This fixes the load-time leak
    // from PR #1 where stale is_door_open left cells permeable after vanilla opened them.
    [HarmonyPatch(typeof(Door), "SetSimState")]
    public class Door_SetSimState
    {
        public static void Postfix(Door __instance, IList<int> cells)
        {
            if (__instance.PrefabID() != InsulatedSelfSealingAirLockConfig.ID) return;

            InsulatedSelfSealingAirLockSimState.Apply(__instance, cells);
        }
    }
}
