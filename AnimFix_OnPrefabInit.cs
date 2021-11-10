using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONIInsulatedSelfSealingAirLock
{
    [HarmonyPatch(typeof(Door), "OnPrefabInit")]
    internal class AnimFix_OnPrefabInit
    {
        private static void Postfix(ref Door __instance)
        {
            __instance.overrideAnims = new KAnimFile[]
            {
                Assets.GetAnim("anim_use_remote_kanim")
            };
        }
    }
}
