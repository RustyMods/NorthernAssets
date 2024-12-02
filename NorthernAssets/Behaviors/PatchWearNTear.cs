using HarmonyLib;
using NorthernAssets.prefabs;

namespace NorthernAssets.Behaviors;

public static class PatchWearNTear
{
    private const float m_infiniteSupport = 1E19F;
    [HarmonyPatch(typeof(WearNTear), nameof(WearNTear.UpdateSupport))]
    private static class WearNTear_UpdateSupport_Patch
    {
        private static bool Prefix(WearNTear __instance)
        {
            if (__instance.transform.position.y < 4999f && !BuildPieces.IsDungeonPiece(__instance.name)) return true;
            if (__instance.transform.position.y > 4999f)
            {
                __instance.m_damages.m_blunt = HitData.DamageModifier.Ignore;
                __instance.m_damages.m_slash = HitData.DamageModifier.Ignore;
                __instance.m_damages.m_pierce = HitData.DamageModifier.Ignore;
                __instance.m_damages.m_fire = HitData.DamageModifier.Ignore;
                __instance.m_damages.m_frost = HitData.DamageModifier.Ignore;
                __instance.m_damages.m_lightning = HitData.DamageModifier.Ignore;
                __instance.m_damages.m_poison = HitData.DamageModifier.Ignore;
                __instance.m_damages.m_spirit = HitData.DamageModifier.Ignore;
            }
            if (__instance.m_support >= m_infiniteSupport) return false;
            __instance.m_nview.GetZDO().Set(ZDOVars.s_support, m_infiniteSupport);
            return false;
        }
    }
}