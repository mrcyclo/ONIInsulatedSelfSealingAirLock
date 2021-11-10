// ONIInsulatedSelfSealingAirLock

using HarmonyLib;
using static STRINGS.UI;
using Database;
using System.Collections.Generic;

namespace ONIInsulatedSelfSealingAirLock
{
    [HarmonyPatch(typeof(Db))]
    [HarmonyPatch("Initialize")]
    public static class Db_Initialize_Patch
    {
        public static void Prefix()
        {
            // Valilla prefers prefix() for adding buildings
            //DoorHelpers.doorBuildMenu(InsulatedSelfSealingAirLockConfig.ID, InsulatedSelfSealingAirLockConfig.menu, InsulatedSelfSealingAirLockConfig.pred);

            Strings.Add("STRINGS.BUILDINGS.PREFABS.INSULATEDSELFSEALINGAIRLOCK.NAME", FormatAsLink("Insulated Self Sealing AirLock", InsulatedSelfSealingAirLockConfig.ID));
            Strings.Add("STRINGS.BUILDINGS.PREFABS.INSULATEDSELFSEALINGAIRLOCK.DESC", "The lowered thermal conductivity of insulated door block any liquid, gas and heat passing through them.");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.INSULATEDSELFSEALINGAIRLOCK.EFFECT", "Mantain liquid, gas and temperature between two rooms");
        }

        public static void Postfix()
        {
            // DLC prefers postfix() for adding buildings
            DoorHelpers.doorBuildMenu(InsulatedSelfSealingAirLockConfig.ID, InsulatedSelfSealingAirLockConfig.menu, InsulatedSelfSealingAirLockConfig.pred);
            // Both prefer postfix() for adding tech tree entries
            DoorHelpers.doorTechTree(InsulatedSelfSealingAirLockConfig.ID, InsulatedSelfSealingAirLockConfig.tech);
        }
    }

    [HarmonyPatch(typeof(BuildingComplete), "OnSpawn")]
    public class InsulatedDoor_BuildingComplete_OnSpawn
    {
        public static void Postfix(ref BuildingComplete __instance)
        {

            if (string.Compare(__instance.name, "InsulatedSelfSealingAirLockComplete") == 0)
            {
                __instance.gameObject.AddOrGet<InsulatedSelfSealingAirLock>();
            }

            InsulatedSelfSealingAirLock insulatingDoor = __instance.gameObject.GetComponent<InsulatedSelfSealingAirLock>();

            if (insulatingDoor != null)
            {
                insulatingDoor.SetInsulation(__instance.gameObject, insulatingDoor.door.building.Def.ThermalConductivity);
            }
        }
    }

    public class DoorHelpers
    {
        public static void doorBuildMenu(string door, string menu, string pred)
        {
            int index = TUNING.BUILDINGS.PLANORDER.FindIndex(x => x.category == menu);
            if (index < 0)
                return;
            else
            {
                IList<string> data = TUNING.BUILDINGS.PLANORDER[index].data as IList<string>;
                int num = -1;
                foreach (string str in (IEnumerable<string>)data)
                {
                    if (str.Equals(pred))
                        num = data.IndexOf(str);
                }
                if (num == -1)
                    return;
                else
                    data.Insert(num + 1, door);
            }
        }

        public static void doorTechTree(string door, string group)
        {
            if (group == "none") return;

            // Vanilla
            //Techs.TECH_GROUPING[group] = new List<string>((IEnumerable<string>)Techs.TECH_GROUPING[group])
            //{
            //    door
            //}.ToArray();


            // Spaced Out
            var tech = Db.Get().Techs.TryGet(group);
            if (tech != null)
            {
                tech.unlockedItemIDs.Add(door);
            }
        }
    }
}