using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ONIInsulatedSelfSealingAirLock
{
    [HarmonyPatch(typeof(Db), "Initialize")]
    public class Db_Initialize
    {
        public static void Prefix()
        {
            Strings.Add("STRINGS.BUILDINGS.PREFABS.INSULATEDSELFSEALINGAIRLOCK.NAME", STRINGS.UI.FormatAsLink("Insulated Self Sealing AirLock", InsulatedSelfSealingAirLockConfig.ID));
            Strings.Add("STRINGS.BUILDINGS.PREFABS.INSULATEDSELFSEALINGAIRLOCK.DESC", "The lowered thermal conductivity of insulated door block any liquid, gas and heat passing through them.");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.INSULATEDSELFSEALINGAIRLOCK.EFFECT", "Mantain liquid, gas and temperature between two rooms");

            Debug.Log("[InsulatedSelfSealingAirLock] Add new string.");
        }

        public static void Postfix()
        {
            // Add to Tech tree
            var tech = Db.Get().Techs.TryGet(InsulatedSelfSealingAirLockConfig.tech);
            if (tech != null)
            {
                tech.unlockedItemIDs.Add(InsulatedSelfSealingAirLockConfig.ID);
                Debug.Log("[InsulatedSelfSealingAirLock] Add door to tech tree.");
            }

            // Add to build menu
            var category = TUNING.BUILDINGS.PLANORDER.Find(x => x.category == InsulatedSelfSealingAirLockConfig.category).buildingAndSubcategoryData;
            var index = category.FindIndex(x => x.Key == InsulatedSelfSealingAirLockConfig.afterBuildingId);
            if (index != -1)
            {
                category.Insert(index + 1, new KeyValuePair<string, string>(InsulatedSelfSealingAirLockConfig.ID, "uncategorized"));
                Debug.Log("[InsulatedSelfSealingAirLock] Add door to build menu.");
            }

            // Add to build menu, official ways, but item will be placed at last in the menu
            //ModUtil.AddBuildingToPlanScreen(new HashedString(InsulatedSelfSealingAirLockConfig.category), InsulatedSelfSealingAirLockConfig.ID);
        }
    }
}