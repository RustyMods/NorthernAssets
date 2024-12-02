using System.Collections.Generic;
using BepInEx;
using HarmonyLib;
using NorthernAssets.Managers;
using UnityEngine;

namespace NorthernAssets.Behaviors;

public static class ItemStandPatches
{
    private static readonly Dictionary<string, ItemStandData> m_data = new();
    
    [HarmonyPatch(typeof(ItemStand), nameof(ItemStand.Awake))]
    private static class ItemStand_Awake_Patch
    {
        private static void Postfix(ItemStand __instance)
        {
            if (__instance.m_effects.m_effectPrefabs.Length > 0) return;
            if (!m_data.TryGetValue(Helpers.GetNormalizedName(__instance.name), out ItemStandData data)) return;
            Helpers.UpdateEffects(data.Effects, ref __instance.m_effects);
            Helpers.UpdateEffects(data.DestroyEffects, ref __instance.m_destroyEffects);
            if (__instance.m_supportedItems[0] is { } item && data.HoverText.IsNullOrWhiteSpace())
            {
                __instance.m_name = item.m_itemData.m_shared.m_name;
            }
            else
            {
                __instance.m_name = data.HoverText;
            }
        }
    }

    public class ItemStandData
    {
        public string HoverText = "";
        public List<string> Effects = new();
        public List<string> DestroyEffects = new();

        public ItemStandData(AssetBundle assetBundle, string prefabName)
        {
            if (assetBundle.LoadAsset<GameObject>(prefabName) is { } prefab)
            {
                LocationManager.RegisterToScene(prefab);
                m_data[prefabName] = this;
            }
            else
            {
                Debug.LogWarning(prefabName + " is null");
            }
        }
    }
}