using HarmonyLib;

namespace ONIInsulatedSelfSealingAirLock
{
    // ApplyControlState(force: true) is only called from Door.OnSpawn, immediately after
    // controller.StartSM() restores the serialized animation state. That is the correct lifecycle
    // hook: the state machine is ready, but vanilla deliberately skips Open()/Close() reconciliation.
    [HarmonyPatch(typeof(Door), "ApplyControlState")]
    public class Door_ApplyControlState
    {
        public static void Postfix(Door __instance, bool force)
        {
            if (!force || __instance.PrefabID() != InsulatedSelfSealingAirLockConfig.ID) return;

            InsulatedSelfSealingAirLockSimState.SyncStateMachineAfterLoad(__instance);
        }
    }
}
