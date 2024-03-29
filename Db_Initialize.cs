﻿using HarmonyLib;
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
            tech?.unlockedItemIDs.Add(InsulatedSelfSealingAirLockConfig.ID);

            // Add to build menu
            ModUtil.AddBuildingToPlanScreen(
                new HashedString(InsulatedSelfSealingAirLockConfig.category),
                InsulatedSelfSealingAirLockConfig.ID,
                "doors",
                InsulatedSelfSealingAirLockConfig.afterBuildingId
            );
        }
    }
}