using System;
using System.Collections.Generic;
using HarmonyLib;

namespace NorthernAssets.Behaviors;

public static class Potion
{
    private static readonly Dictionary<string, Action<Player, Inventory, ItemDrop.ItemData>> m_actions = new();

    public static void RegisterAction(string sharedName, Action<Player, Inventory, ItemDrop.ItemData> action) => m_actions[sharedName] = action;

    [HarmonyPatch(typeof(Player), nameof(Player.ConsumeItem))]
    private static class Player_ConsumeItem_Patch
    {
        private static bool Prefix(Player __instance, Inventory inventory, ItemDrop.ItemData item)
        {
            if (!m_actions.TryGetValue(item.m_shared.m_name, out Action<Player, Inventory, ItemDrop.ItemData> action)) return true;
            action(__instance, inventory, item);
            return false;
        }
    }
}