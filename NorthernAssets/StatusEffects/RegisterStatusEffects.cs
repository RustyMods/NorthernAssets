using System.Collections.Generic;
using HarmonyLib;
using ItemManager;
using UnityEngine;

namespace NorthernAssets.StatusEffects;

public static class RegisterStatusEffects
{
    [HarmonyPriority(500)]
    [HarmonyPatch(typeof(ObjectDB), nameof(ObjectDB.Awake))]
    private static class Register_StatusEffects
    {
        private static void Postfix(ObjectDB __instance)
        {
            if (!ZNetScene.instance) return;
            SE_Stats SE_HotSprings = ScriptableObject.CreateInstance<SE_Stats>();
            SE_HotSprings.name = "SE_HotSprings";
            LocalizeKey hotSpringKey = new LocalizeKey("$se_hotsprings");
            hotSpringKey.English("Soothed");
            SE_HotSprings.m_name = $"${hotSpringKey.Key}";
            SE_HotSprings.m_icon = NorthernAssetsPlugin._Assets.LoadAsset<Sprite>("hotspring_icon");
            SE_HotSprings.m_ttl = 30f;
            SE_HotSprings.m_healthOverTimeTicks = 100f;
            SE_HotSprings.m_healthOverTimeInterval = 1f;
            SE_HotSprings.m_healthOverTimeTickHP = 1f;
            SE_HotSprings.m_healthPerTickMinHealthPercentage = 1f;
            SE_HotSprings.m_swimStaminaUseModifier = 0f;
            SE_HotSprings.m_tooltip = "$se_hotsprings_tooltip";
            SE_HotSprings.m_startEffects = GetEffectList(new() { "vfx_Frost" });
            __instance.m_StatusEffects.Add(SE_HotSprings);
            
            SE_Stats SE_Tundra = ScriptableObject.CreateInstance<SE_Stats>();
            SE_Tundra.name = "SE_Tundra";
            LocalizeKey tundraKey = new LocalizeKey("$se_tundra");
            tundraKey.English("Frostbite");
            SE_Tundra.m_name = $"${tundraKey.Key}";
            SE_Tundra.m_tooltip = "$se_tundra_tooltip";
            SE_Tundra.m_icon = NorthernAssetsPlugin._Assets.LoadAsset<Sprite>("TundraIcon");
            SE_Tundra.m_startEffects = GetEffectList(new() { "vfx_Frost" });
            SE_Tundra.m_tickInterval = 1f;
            SE_Tundra.m_healthPerTickMinHealthPercentage = 0.1f;
            SE_Tundra.m_healthPerTick = -1f;
            __instance.m_StatusEffects.Add(SE_Tundra);
        }

        private static EffectList GetEffectList(List<string> effectNames)
        {
            List<EffectList.EffectData> data = new();
            foreach (string? name in effectNames)
            {
                GameObject effect = ZNetScene.instance.GetPrefab(name);
                if (!effect) continue;
                data.Add(new()
                {
                    m_prefab = effect, m_attach = true
                });
            }

            return new EffectList() { m_effectPrefabs = data.ToArray() };
        }
    }
    
    private static bool IsFrigid(Player __instance)
    {
        if (__instance.GetCurrentBiome() != Heightmap.Biome.DeepNorth) return false;
        var SEMan = __instance.GetSEMan();
        if (__instance.InShelter() || EffectArea.IsPointInsideArea(__instance.transform.position, EffectArea.Type.WarmCozyArea, 1f)) return false;
        if (SEMan.HaveStatusEffect(SEMan.s_statusEffectCampFire) ||
            SEMan.HaveStatusEffect(SEMan.s_statusEffectBurning)) return false;
        if (SEMan.HaveStatusEffect("SE_TundraPotion".GetStableHashCode()) || SEMan.HaveStatusEffect("SE_HotSprings".GetStableHashCode())) return false;
        if (__instance.transform.position.y > 1000f) return false;
        return true;
    }

    [HarmonyPatch(typeof(Player), nameof(Player.UpdateEnvStatusEffects))]
    private static class Player_UpdateEnvStatusEffects_Patch
    {
        private static void Postfix(Player __instance)
        {
            var SEMan = __instance.GetSEMan();
            if (!IsFrigid(__instance)) SEMan.RemoveStatusEffect("SE_Tundra".GetStableHashCode());
            else SEMan.AddStatusEffect("SE_Tundra".GetStableHashCode());
        }
    }
}