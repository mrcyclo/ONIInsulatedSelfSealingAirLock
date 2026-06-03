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
                int cell = cells[i];

                SimMessages.SetInsulation(cell, 0f);

                Door.ControlState controlState = Traverse.Create(__instance).Field("controlState").GetValue<Door.ControlState>();
                if (controlState != Door.ControlState.Opened)
                {
                    // GasImpermeable (1), LiquidImpermeable (2), SolidImpermeable (4)
                    SimMessages.SetCellProperties(cell, 7);
                }
                else
                {
                    // Allow transmission when set to Open
                    SimMessages.ClearCellProperties(cell, 7);
                }
            }
        }
    }
}
