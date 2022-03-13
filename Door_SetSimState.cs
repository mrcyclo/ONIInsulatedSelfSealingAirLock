using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ONIInsulatedSelfSealingAirLock
{
    [HarmonyPatch(typeof(Door), "SetSimState")]
    public class Door_SetSimState
    {
        public static void Prefix(Door __instance, bool is_door_open, IList<int> cells)
        {
            if (__instance.PrefabID() != InsulatedSelfSealingAirLockConfig.ID) return;

            for (int i = 0; i < cells.Count; i++)
            {
                int num = cells[i];

                SimMessages.SetInsulation(num, 0f);

                if (__instance.CurrentState == Door.ControlState.Opened)
                {
                    SimMessages.ClearCellProperties(num, 7);
                }
                else
                {
                    SimMessages.SetCellProperties(num, 7);
                }
            }
        }
    }
}
