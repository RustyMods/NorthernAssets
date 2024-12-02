using System.Collections.Generic;
using System.Text.RegularExpressions;
using HarmonyLib;
using NorthernAssets.Managers;
using UnityEngine;

namespace NorthernAssets.Behaviors;

public static class CreatureSpawnerPatcher
{
    public static readonly Dictionary<string, List<string>> m_creatureSpawners = new();
    [HarmonyPatch(typeof(CreatureSpawner), nameof(CreatureSpawner.Awake))]
    private static class CreatureSpawner_Awake_Patch
    {
        private static void Postfix(CreatureSpawner __instance)
        {
            if (__instance.m_creaturePrefab != null) return;
            if (!m_creatureSpawners.TryGetValue(Helpers.GetNormalizedName(__instance.name), out List<string> creatures))
            {
                __instance.m_creaturePrefab = ZNetScene.instance.GetPrefab("Skeleton");
                return;
            }
            if (ZNetScene.instance.GetPrefab(creatures[Random.Range(0, creatures.Count)]) is { } creature)
            {
                __instance.m_creaturePrefab = creature;
            }
            else
            {
                __instance.m_creaturePrefab = ZNetScene.instance.GetPrefab("Skeleton");
            }
        }
    }
}