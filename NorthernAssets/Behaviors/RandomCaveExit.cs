using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using HarmonyLib;
using ServerSync;
using UnityEngine;
using YamlDotNet.Serialization;
using Random = UnityEngine.Random;

namespace NorthernAssets.Behaviors;

public static class RandomCaveExit
{
    private static List<Position> m_caves = new();
    private static readonly CustomSyncedValue<string> m_cavePositions = new(NorthernAssetsPlugin.ConfigSync, "YetiCaveLocations", "");

    [HarmonyPatch(typeof(Teleport), nameof(Teleport.Interact))]
    private static class Teleport_Interact_Patch
    {
        private static bool Prefix(Teleport __instance, Humanoid character, ref bool __result)
        {
            if (m_caves.Count <= 0 || __instance.m_targetPoint != null) return true;
            string name = __instance.name.Replace("(Clone)", string.Empty);
            if (name != "YetiCave_ExteriorGateway") return true;

            Position? randomExit = m_caves[Random.Range(0, m_caves.Count)];
            Vector3 target = Get(randomExit);
            target.y += 10f;
            if (!character.TeleportTo(target, Quaternion.identity, true)) __result = false;
            __result = true;
            return false;
        }
    }

    [HarmonyPatch(typeof(ZoneSystem), nameof(ZoneSystem.GenerateLocationsIfNeeded))]
    private static class RegisterYetiCaveLocations
    {
        private static void Postfix()
        {
            if (!ZNet.instance || !ZNet.instance.IsServer()) return;
            if (!UpdateCavePositions())
            {
                NorthernAssetsPlugin._Plugin.Invoke(nameof(NorthernAssetsPlugin.UpdateLocationPositions), 120f);
            }
        }
    }

    public static bool UpdateCavePositions()
    {
        if (!ZoneSystem.instance.LocationsGenerated) return false;
        List<ZoneSystem.LocationInstance> caves = ZoneSystem.instance.GetLocationList().Where(location => location.m_location.m_prefab.Name.Contains("YetiCave_DeepNorth")).ToList();
        int count = 0;
        foreach (var cave in caves)
        {
            Add(cave.m_position);
            ++count;
        }
            
        NorthernAssetsPlugin.NorthernAssetsLogger.LogDebug("Server: Registered " + count + " yeti caves for random exits");

        ISerializer serializer = new SerializerBuilder().Build();
        string data = serializer.Serialize(m_caves);
        m_cavePositions.Value = data;

        return count > 0;
    }

    public static void Setup()
    {
        m_cavePositions.ValueChanged += () =>
        {
            if (!ZNet.instance || ZNet.instance.IsServer()) return;
            if (m_cavePositions.Value.IsNullOrWhiteSpace()) return;
            NorthernAssetsPlugin.NorthernAssetsLogger.LogDebug("Client: Received yeti cave locations for random exits");
            var deserializer = new DeserializerBuilder().Build();
            var data = deserializer.Deserialize<List<Position>>(m_cavePositions.Value);
            m_caves = data;
        };
    }

    private static void Add(Vector3 position) => m_caves.Add(new Position()
    {
        x = position.x,
        y = position.y,
        z = position.z
    });

    private static Vector3 Get(Position position) => new Vector3(position.x, position.y, position.z);

    [Serializable]
    public class Position
    {
        public float x;
        public float y;
        public float z;
    }
}