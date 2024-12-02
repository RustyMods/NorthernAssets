using HarmonyLib;
using NorthernAssets.Managers;
using PieceManager;

namespace NorthernAssets.Behaviors;

public static class DoorPatches
{
    [HarmonyPatch(typeof(Door), nameof(Door.CanInteract))]
    private static class Door_CanInteract_Patch
    {
        private static bool Prefix(Door __instance, ref bool __result)
        {
            if (!BuildPiece.buildPieceMap.ContainsKey(Helpers.GetNormalizedName(__instance.name))) return true;
            if (__instance.m_keyItem == null || __instance.m_canNotBeClosed) return true;
            __result = __instance.m_animator.GetCurrentAnimatorStateInfo(0).IsTag("open") || __instance.m_animator.GetCurrentAnimatorStateInfo(0).IsTag("closed");
            return false;
        }
    }
}