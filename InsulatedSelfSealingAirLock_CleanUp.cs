

using HarmonyLib;

namespace ONIInsulatedSelfSealingAirLock
{
    [HarmonyPatch(typeof(BuildingComplete), "OnCleanUp")]
    public class InsulatedSelfSealingAirLock_CleanUp
    {
        public static void Postfix(ref BuildingComplete __instance)
        {
            InsulatedSelfSealingAirLock currentDoor = __instance.gameObject.GetComponent<InsulatedSelfSealingAirLock>();
            if (currentDoor == null) return;

            currentDoor.SetInsulation(__instance.gameObject, currentDoor.door.building.Def.ThermalConductivity);
        }
    }
}
