using HarmonyLib;
using System.Collections.Generic;

namespace ONIInsulatedSelfSealingAirLock
{
    // Shared sim-layer logic for insulated self-sealing doors.
    // Extracted so SetSimState and OnSpawn apply the same rules and stay in sync after load.
    internal static class InsulatedSelfSealingAirLockSimState
    {
        public static void Apply(Door door, IList<int> cells)
        {
            // Use the player control state, not IsOpen(). In Auto/Locked mode the door animates
            // open for dupes but must remain a perfect seal in the sim (self-sealing behavior).
            // Only the explicit "Opened" control state allows gas/liquid/heat through.
            bool allowTransmission = door.CurrentState == Door.ControlState.Opened;

            foreach (int cell in cells)
            {
                if (allowTransmission)
                {
                    // GasImpermeable (1), LiquidImpermeable (2), SolidImpermeable (4)
                    SimMessages.SetInsulation(cell, 1f);
                    SimMessages.ClearCellProperties(cell, 7);
                }
                else
                {
                    SimMessages.SetInsulation(cell, 0f);
                    SimMessages.SetCellProperties(cell, 7);
                }
            }
        }

        // True when the deserialized state machine is in a mid-transition pose that is only valid
        // while a dupe is passing. On a fresh spawn the default is "closed"; any other state here
        // means the save captured the door mid-animation.
        public static bool NeedsPostLoadSync(Door door, Door.Controller.Instance controller)
        {
            if (door.CurrentState == Door.ControlState.Opened) return false;
            if (controller.IsInsideState(controller.sm.closed)) return false;
            if (IsLockedDoorInExpectedState(door, controller)) return false;
            return true;
        }

        private static bool IsLockedDoorInExpectedState(Door door, Door.Controller.Instance controller)
        {
            if (door.CurrentState != Door.ControlState.Locked) return false;
            if (controller.IsInsideState(controller.sm.locked)) return true;
            if (controller.IsInsideState(controller.sm.locking)) return true;
            return false;
        }

        // Fixes issue #2: save/load while a dupe is passing leaves the animation stuck in an
        // open or mid-transition state even though the sim is correctly sealed (CurrentState is Auto/Locked).
        // Vanilla skips Open()/Close() when ApplyControlState(force: true) runs.
        public static void SyncStateMachineAfterLoad(Door door)
        {
            if (door == null || door.CurrentState == Door.ControlState.Opened) return;

            var controller = Traverse.Create(door).Field("controller").GetValue<Door.Controller.Instance>();
            if (controller == null || !NeedsPostLoadSync(door, controller)) return;

            // openCount is private; a non-zero value from a mid-passage save would block Close().
            Traverse.Create(door).Field("openCount").SetValue(0);
            controller.sm.isOpen.Set(false, controller);

            // Covers "open", "opening", "closedelay", "closeblocked", and "closing" in one step.
            if (!controller.IsInsideState(controller.sm.closed))
            {
                controller.GoTo(controller.sm.closed);
            }

            Apply(door, door.building.PlacementCells);
        }
    }
}
