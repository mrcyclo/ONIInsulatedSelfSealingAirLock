﻿using System.Collections.Generic;
using UnityEngine;

namespace ONIInsulatedSelfSealingAirLock
{
    class InsulatedSelfSealingAirLock : KMonoBehaviour
    {
        [MyCmpGet]
        public Door door;

        public void SetInsulation(GameObject go, float insulation)
        {
            IList<int> cells = go.GetComponent<Building>().PlacementCells;
            for (int i = 0; i < cells.Count; i++)
            {
                SimMessages.SetInsulation(cells[i], insulation);
            }
        }
    }
}
