using HarmonyLib;

namespace ONIInsulatedSelfSealingAirLock
{
    [HarmonyPatch(typeof(Door), "OnSimDoorClosed")]
    public class InsulatedSelfSealingAirLock_Closed
    {
        public static void Postfix(ref BuildingComplete __instance)
        {
            InsulatedSelfSealingAirLock currentDoor = __instance.gameObject.GetComponent<InsulatedSelfSealingAirLock>();
            if (currentDoor == null) return;

            currentDoor.SetInsulation(__instance.gameObject, currentDoor.door.building.Def.ThermalConductivity);
        }
    }
}
