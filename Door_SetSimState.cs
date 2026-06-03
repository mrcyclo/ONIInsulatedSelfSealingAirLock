using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using static STRINGS.BUILDINGS.PREFABS.DOOR.CONTROL_STATE;
using static STRINGS.ELEMENTS;
using static STRINGS.UI.NEWBUILDCATEGORIES;

namespace ONIInsulatedSelfSealingAirLock
{
    [HarmonyPatch(typeof(Door), "SetSimState")]
    public class Door_SetSimState
    {
        public static void Postfix(Door __instance, bool is_door_open, IList<int> cells)
        {
            if (__instance.PrefabID() != InsulatedSelfSealingAirLockConfig.ID) return;

            // is_door_open can be incorrect on load, overwrite with control status
            is_door_open = (__instance.CurrentState == Door.ControlState.Opened);

            foreach (int cell in cells)
            {
                if (is_door_open)
                {
                    // allow temperature transmission when set to Open
                    SimMessages.SetInsulation(cell, 1f);
                
                    // Allow transmission when set to Open
                    SimMessages.ClearCellProperties(cell, 7);
                }
                else
                {
                    // perfect insulator
                    SimMessages.SetInsulation(cell, 0f);
                
                    // GasImpermeable (1), LiquidImpermeable (2), SolidImpermeable (4)
                    SimMessages.SetCellProperties(cell, 7);
                }
            }
        }
    }

    [HarmonyPatch(typeof(Door), "OnSpawn")]
    public class Door_OnSpawn
    {
        public static void Postfix(Door __instance)
        {
            if (__instance.PrefabID() != InsulatedSelfSealingAirLockConfig.ID) return;

            bool is_door_open = (__instance.CurrentState == Door.ControlState.Opened);

            foreach (int cell in __instance.building.PlacementCells)
            {
                if (is_door_open)
                {
                    // allow temperature transmission when set to Open
                    SimMessages.SetInsulation(cell, 1f);

                    // Allow transmission when set to Open
                    SimMessages.ClearCellProperties(cell, 7);
                }
                else
                {
                    // perfect insulator
                    SimMessages.SetInsulation(cell, 0f);

                    // GasImpermeable (1), LiquidImpermeable (2), SolidImpermeable (4)
                    SimMessages.SetCellProperties(cell, 7);
                }
            }
        }
    }
}
