using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using NorthernAssets.Managers;
using UnityEngine;

namespace NorthernAssets.Behaviors;

public static class OfferingBowlPatches
{
    private static readonly Dictionary<string, OfferingData> m_offerings = new();

    [HarmonyPatch(typeof(OfferingBowl), nameof(OfferingBowl.Awake))]
    private static class OfferingBowl_Awake_Patch
    {
        private static void Postfix(OfferingBowl __instance)
        {
            if (!m_offerings.TryGetValue(Helpers.GetNormalizedName(__instance.name), out OfferingData data)) return;
            if (ZNetScene.instance.GetPrefab(data.BossPrefab) is not { } boss) return;
            __instance.m_bossPrefab = boss;
            __instance.m_spawnBossDelay = data.Delay;
            __instance.m_name = data.Name;
            __instance.m_usedAltarText = data.Message;
            data.UpdateEffects(data.StartEffects, ref __instance.m_spawnBossStartEffects);
            data.UpdateEffects(data.DoneEffects, ref __instance.m_spawnBossDoneffects);
        }
    }

    [HarmonyPatch(typeof(OfferingBowl), nameof(OfferingBowl.RPC_SpawnBoss))]
    private static class OfferingBowl_RPC_SpawnBoss_Patch
    {
        private static bool Prefix(OfferingBowl __instance, long senderId, Vector3 point)
        {
            if (!m_offerings.ContainsKey(Helpers.GetNormalizedName(__instance.name))) return true;
            if (!__instance.m_nview || !__instance.m_nview.IsValid() || !__instance.m_nview.IsOwner()) return false;
            if (__instance.IsBossSpawnQueued()) return false;
            __instance.SpawnBoss(point);
            __instance.m_nview.InvokeRPC(senderId, nameof(OfferingBowl.RPC_BossSpawnInitiated));
            __instance.RemoveAltarItems();
            return false;
        }
    }
    
    public class OfferingData
    {
        public string BossPrefab = "";
        public float Delay = 12f;
        public string Name = "Altar";
        public string Message = "";
        public readonly List<string> StartEffects = new();
        public readonly List<string> DoneEffects = new();

        public OfferingData(string prefabName)
        {
            m_offerings[prefabName] = this;
        }
        
        internal void UpdateEffects(List<string> effects, ref EffectList effectList)
        {
            if (effects.Count <= 0) return;
            List<EffectList.EffectData> data = effectList.m_effectPrefabs.ToList();
            foreach (string effect in effects)
            {
                GameObject? prefab = ZNetScene.instance.GetPrefab(effect);
                if (!prefab) continue;
                data.Add(new EffectList.EffectData() { m_prefab = prefab });
            }

            effectList.m_effectPrefabs = data.ToArray();
        }
    }
}