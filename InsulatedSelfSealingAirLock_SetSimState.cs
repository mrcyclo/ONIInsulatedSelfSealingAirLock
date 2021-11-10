using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ONIInsulatedSelfSealingAirLock
{
    [HarmonyPatch(typeof(Door), "SetSimState")]
    class InsulatedSelfSealingAirLock_SetSimState
    {
        private static void Prefix(Door __instance, bool is_door_open, IList<int> cells)
        {
            if (__instance.PrefabID() != InsulatedSelfSealingAirLockConfig.ID) return;

            PrimaryElement component = __instance.GetComponent<PrimaryElement>();
            float mass = component.Mass / (float)cells.Count;
            for (int i = 0; i < cells.Count; i++)
            {
                int offsetCell = cells[i];

                SimMessages.SetInsulation(offsetCell, 0f);

                Door.DoorType doorType = __instance.doorType;
                if (doorType <= Door.DoorType.ManualPressure || doorType == Door.DoorType.Sealed)
                {
                    World.Instance.groundRenderer.MarkDirty(offsetCell);
                    if (is_door_open)
                    {
                        MethodInfo method_opened = AccessTools.Method(typeof(Door), "OnSimDoorOpened");
                        System.Action cb_opened = (System.Action)Delegate.CreateDelegate(typeof(System.Action), __instance, method_opened);
                        HandleVector<Game.CallbackInfo>.Handle handle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(cb_opened));

                        SimMessages.Dig(offsetCell, handle.index, true);

                        SimMessages.ClearCellProperties(offsetCell, 1);
                        SimMessages.ClearCellProperties(offsetCell, 2);
                        SimMessages.ClearCellProperties(offsetCell, 4);

                        SimMessages.SetCellProperties(offsetCell, (byte)(__instance.CurrentState == Door.ControlState.Auto ? 7 : 4));

                        if (__instance.ShouldBlockFallingSand)
                        {
                            SimMessages.ClearCellProperties(offsetCell, 4);
                        }
                    }
                    else
                    {
                        MethodInfo method_closed = AccessTools.Method(typeof(Door), "OnSimDoorClosed");
                        System.Action cb_closed = (System.Action)Delegate.CreateDelegate(typeof(System.Action), __instance, method_closed);
                        HandleVector<Game.CallbackInfo>.Handle handle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(cb_closed));

                        float temperature = component.Temperature;
                        if (temperature <= 0f)
                        {
                            temperature = component.Temperature;
                        }

                        SimMessages.ReplaceAndDisplaceElement(offsetCell, component.ElementID, CellEventLogger.Instance.DoorClose, mass, temperature, byte.MaxValue, 0, handle.index);

                        SimMessages.ClearCellProperties(offsetCell, 1);
                        SimMessages.ClearCellProperties(offsetCell, 2);
                        SimMessages.SetCellProperties(offsetCell, 4);
                    }
                }
            }
        }
    }
}
