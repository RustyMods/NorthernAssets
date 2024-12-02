using System.Collections.Generic;
using HarmonyLib;
using NorthernAssets.Managers;
using UnityEngine;

namespace NorthernAssets.Behaviors;

public static class ForceTimeOfDay
{
    private static readonly Dictionary<string, float> m_data = new();
    
    [HarmonyPatch(typeof(EnvZone), nameof(EnvZone.OnTriggerStay))]
    private static class EnvZone_OnTriggerStay_Patch
    {
        private static void Postfix(EnvZone __instance)
        {
            if (!m_data.TryGetValue(Helpers.GetNormalizedName(__instance.name), out float tod)) return;
            EnvMan.instance.m_debugTimeOfDay = true;
            EnvMan.instance.m_debugTime = Mathf.Clamp01(tod);
        }
    }

    [HarmonyPatch(typeof(EnvZone), nameof(EnvZone.OnTriggerExit))]
    private static class EnvZone_OnTriggerExit_Patch
    {
        private static void Postfix(EnvZone __instance)
        {
            if (!m_data.ContainsKey(__instance.name.Replace("(Clone)", string.Empty))) return;
            EnvMan.instance.m_debugTimeOfDay = false;
        }
    }

    public static void RegisterForcedTODEnvZone(string prefabName, float tod) => m_data[prefabName] = tod;
}