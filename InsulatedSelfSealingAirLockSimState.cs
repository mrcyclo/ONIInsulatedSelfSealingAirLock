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
    }
}
