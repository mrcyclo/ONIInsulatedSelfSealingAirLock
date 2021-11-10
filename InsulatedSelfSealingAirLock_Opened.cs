using HarmonyLib;

namespace ONIInsulatedSelfSealingAirLock
{
    [HarmonyPatch(typeof(Door), "OnSimDoorOpened")]
    public class InsulatedSelfSealingAirLock_Opened
    {
        public static void Postfix(ref Door __instance)
        {
            InsulatedSelfSealingAirLock currentDoor = __instance.gameObject.GetComponent<InsulatedSelfSealingAirLock>();
            if (currentDoor != null) return;

            currentDoor.SetInsulation(__instance.gameObject, 0f);
        }
    }
}
