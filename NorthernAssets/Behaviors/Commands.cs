using HarmonyLib;

namespace NorthernAssets.Behaviors;

public static class Commands
{
    [HarmonyPatch(typeof(Terminal), nameof(Terminal.Awake))]
    private static class Terminal_Awake_Patch
    {
        private static void Postfix()
        {
            Terminal.ConsoleCommand CoatOfArms = new Terminal.ConsoleCommand("CoatOfArms",
                "Gives all coat of arms items, admin only",
                _ =>
                {
                    if (!ObjectDB.instance || !Player.m_localPlayer) return;
                    for (int index = 1; index < 13; ++index)
                    {
                        var prefabName = $"CoatOfArmsItem{index}_RS";
                        if (ObjectDB.instance.GetItemPrefab(prefabName) is { } item)
                        {
                            Player.m_localPlayer.GetInventory().AddItem(item, 1);
                        }
                    }
                }, onlyAdmin: true);
        }
    }
}