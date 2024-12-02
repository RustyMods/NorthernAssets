using System.Collections.Generic;
using System.Linq;
using BepInEx;
using HarmonyLib;
using NorthernAssets.Behaviors;
using SoftReferenceableAssets;
using UnityEngine;

namespace NorthernAssets.Managers;

public class LocationManager
{
    private static readonly Dictionary<string, DungeonDB.RoomData> RegisteredRooms = new();
    private static readonly Dictionary<string, LocationData> m_locations = new();
    private static readonly Dictionary<string, GameObject> m_extra = new();
    private static readonly Dictionary<string, DungeonData> m_roomLists = new();
    private static readonly Dictionary<string, DungeonData> m_dungeons = new();
    private static readonly Dictionary<string, RoomReference> RegisteredRoomReferences = new();
    private static readonly Dictionary<string, DoorData> m_doors = new();
    private static readonly Dictionary<string, DoubleDoorData> m_doubleDoors = new();

    public static void RegisterToScene(AssetBundle assetBundle, string assetName)
    {
        if (assetBundle.LoadAsset<GameObject>(assetName) is { } prefab)
        {
            m_extra[assetName] = prefab;
        }
        else
        {
            Debug.LogWarning(assetName + " is null");
        }
    }

    public static void RegisterToScene(GameObject prefab) => m_extra[prefab.name] = prefab;
    
    static LocationManager()
    {
        Harmony harmony = new Harmony("org.bepinex.helpers.RustyLocationManager");
        harmony.Patch(AccessTools.DeclaredMethod(typeof(ZoneSystem), nameof(ZoneSystem.SetupLocations)), prefix: new HarmonyMethod(AccessTools.DeclaredMethod(typeof(LocationManager), nameof(SetupLocations_Prefix))));
        harmony.Patch(AccessTools.DeclaredMethod(typeof(ObjectDB), nameof(ObjectDB.Awake)), postfix: new HarmonyMethod(AccessTools.DeclaredMethod(typeof(LocationManager), nameof(Patch_ObjectDB_Awake))));
        harmony.Patch(AccessTools.DeclaredMethod(typeof(DungeonDB), nameof(DungeonDB.Awake)), postfix: new HarmonyMethod(AccessTools.DeclaredMethod(typeof(LocationManager), nameof(Patch_DungeonDB_Awake))));
        harmony.Patch(AccessTools.DeclaredMethod(typeof(DungeonGenerator), nameof(DungeonGenerator.SetupAvailableRooms)), prefix: new HarmonyMethod(AccessTools.DeclaredMethod(typeof(LocationManager), nameof(Patch_SetupAvailableRooms))));
        harmony.Patch(AccessTools.DeclaredMethod(typeof(Location), nameof(Location.Awake)), prefix: new HarmonyMethod(AccessTools.DeclaredMethod(typeof(LocationManager), nameof(Location_Awake_Patch))));
        harmony.Patch(AccessTools.DeclaredMethod(typeof(DungeonDB), nameof(DungeonDB.GenerateHashList)), prefix: new HarmonyMethod(AccessTools.DeclaredMethod(typeof(LocationManager), nameof(DungeonDB_GenerateHashList_Patch))));
    }
    

    internal static bool DungeonDB_GenerateHashList_Patch(DungeonDB __instance)
    {
        __instance.m_roomByHash.Clear();
        foreach (var room in __instance.m_rooms)
        {
            int hash = room.Hash;
            if (__instance.m_roomByHash.ContainsKey(hash)) continue;
            __instance.m_roomByHash[hash] = room;
        }

        return false;
    }

    internal static bool Location_Awake_Patch(Location __instance)
    {
        if (!m_locations.TryGetValue(Helpers.GetNormalizedName(__instance.name), out LocationData data)) return true;
        Location.s_allLocations.Add(__instance);
        if (!__instance.m_hasInterior || !data.SpawnInteriorEnv) return false;
        Vector3 zoneCenter = __instance.GetZoneCenter();
        GameObject environment = Object.Instantiate(__instance.m_interiorPrefab, new Vector3(zoneCenter.x, __instance.transform.position.y + data.Altitude, zoneCenter.z), Quaternion.identity, __instance.transform);
        environment.transform.localScale = data.EnvironmentScale;
        environment.GetComponent<EnvZone>().m_environment = data.Environment;
        return false;
    }

    internal static bool Patch_SetupAvailableRooms(DungeonGenerator __instance)
    {
        if (!m_dungeons.TryGetValue(Helpers.GetNormalizedName(__instance.name), out DungeonData data)) return true;
        DungeonGenerator.m_availableRooms.Clear();
        List<DungeonDB.RoomData> rooms = data.m_dungeonRooms.Select(room => room.m_data).ToList();
        DungeonGenerator.m_availableRooms.AddRange(rooms);
        return false;
    }

    internal static void Patch_DungeonDB_Awake(DungeonDB __instance)
    {
        foreach (DungeonData list in m_roomLists.Values)
        {
            if (__instance.m_roomLists.Contains(list.Prefab)) continue;
            __instance.m_roomLists.Add(list.Prefab);
        }
    }

    [HarmonyPriority(Priority.Last)]
    internal static void Patch_ObjectDB_Awake(ObjectDB __instance)
    {
        if (!ZNetScene.instance) return;
        RegisterExtra();
        RegisterDungeons();
        RegisterDoors();
    }

    private static void RegisterDoors()
    {
        foreach (var door in m_doors.Values)
        {
           door.SetData();
           Register(door.Prefab);
        }

        foreach (var door in m_doubleDoors.Values)
        {
            door.SetData();
            Register(door.Prefab);
        }
    }

    private static void RegisterDungeons()
    {
        foreach(var dungeon in m_roomLists.Values) Register(dungeon.Generator);
    }

    private static void RegisterExtra()
    {
        foreach(var asset in m_extra.Values) Register(asset);
    }
    
    private static void Register(GameObject prefab)
    {
        if (!ZNetScene.instance.m_prefabs.Contains(prefab)) ZNetScene.instance.m_prefabs.Add(prefab);
        if (!ZNetScene.instance.m_namedPrefabs.ContainsKey(prefab.name.GetStableHashCode())) ZNetScene.instance.m_namedPrefabs[prefab.name.GetStableHashCode()] = prefab;
    }

    private static void SetupLocations_Prefix(ZoneSystem __instance)
    {
        List<ZoneSystem.ZoneLocation> locations = new();
        foreach (var location in m_locations.Values)
        {
            var data = location.GetLocation();
            if (data.m_prefab.IsValid)
            {
                locations.Add(data);
            }
            else
            {
                NorthernAssetsPlugin.NorthernAssetsLogger.LogDebug(data.m_prefabName + " is not valid");
            }
        }
        NorthernAssetsPlugin.NorthernAssetsLogger.LogDebug("Added " + locations.Count + " locations");
        __instance.m_locations.AddRange(locations);
    }
    
    public class CreatureSpawnerInfo
    {
        private readonly string PrefabName = "";
        public CreatureSpawnerInfo(AssetBundle assetBundle, string prefabName, string creatureName)
        {
            if (assetBundle.LoadAsset<GameObject>(prefabName) is { } prefab)
            {
                PrefabName = prefabName;
                CreatureSpawnerPatcher.m_creatureSpawners[prefabName] = new List<string>(){creatureName};
                RegisterToScene(prefab);
            }
            else
            {
                Debug.LogWarning(prefabName + " is null");
            }
        }

        public void Add(string creatureName) => CreatureSpawnerPatcher.m_creatureSpawners[PrefabName].Add(creatureName);
    }
    
    [HarmonyPatch(typeof(Door), nameof(Door.Awake))]
    private static class Door_Awake_Patch
    {
        private static void Postfix(Door __instance)
        {
            if (!m_doors.TryGetValue(Helpers.GetNormalizedName(__instance.name), out DoorData data)) return;
            LoadData(__instance, data);
        }

        private static void LoadData(Door door, DoorData data)
        {
            if (door.m_keyItem is null && !data.KeyItem.IsNullOrWhiteSpace() && ObjectDB.instance.GetItemPrefab(data.KeyItem) is { } key && key.TryGetComponent(out ItemDrop keyComponent))
            {
                door.m_keyItem = keyComponent;
            }
            if (door.m_openEffects.m_effectPrefabs.Length <= 0 && !data.CloneEffectsFrom.IsNullOrWhiteSpace() && ZNetScene.instance.GetPrefab(data.CloneEffectsFrom) is { } original && original.TryGetComponent(out Door component))
            {
                door.m_openEffects = component.m_openEffects;
                door.m_closeEffects = component.m_closeEffects;
                door.m_lockedEffects = component.m_lockedEffects;
            }
        }
    }

    public class DoorData
    {
        public readonly GameObject Prefab = null!;
        public string CloneEffectsFrom = "";
        public string KeyItem = "";

        public DoorData(AssetBundle assetBundle, string assetName)
        {
            if (assetBundle.LoadAsset<GameObject>(assetName) is { } prefab)
            {
                Prefab = prefab;
                m_doors[assetName] = this;
            }
            else
            {
                Debug.LogWarning(assetName + " is null");
            }
        }

        public void SetData()
        {
            if (!Prefab.TryGetComponent(out Door door)) return;
            if (!KeyItem.IsNullOrWhiteSpace() && ObjectDB.instance.GetItemPrefab(KeyItem) is { } key && key.TryGetComponent(out ItemDrop keyComponent))
            {
                door.m_keyItem = keyComponent;
            }
            if (!CloneEffectsFrom.IsNullOrWhiteSpace() && ZNetScene.instance.GetPrefab(CloneEffectsFrom) is { } original && original.TryGetComponent(out Door component))
            {
                door.m_openEffects = component.m_openEffects;
                door.m_closeEffects = component.m_closeEffects;
                door.m_lockedEffects = component.m_lockedEffects;
            }
        }
    }
    
    [HarmonyPatch(typeof(ZNetView), nameof(ZNetView.Awake))]
    private static class ZNetView_Awake_Patch
    {
        private static void Postfix(ZNetView __instance)
        {
            if (!m_doubleDoors.TryGetValue(Helpers.GetNormalizedName(__instance.name), out DoubleDoorData data)) return;
            LoadData(__instance, data);
        }

        private static void LoadData(ZNetView __instance, DoubleDoorData data)
        {
            if (__instance.GetComponent<DoubleDoor>()) return;
            DoubleDoor door = __instance.gameObject.AddComponent<DoubleDoor>();
            if (door.m_keyItem is null && !data.KeyItem.IsNullOrWhiteSpace() && ObjectDB.instance.GetItemPrefab(data.KeyItem) is { } key && key.TryGetComponent(out ItemDrop keyComponent))
            {
                door.m_keyItem = keyComponent;
            }
            if (door.m_openEffects.m_effectPrefabs.Length <= 0 && !data.CloneEffectsFrom.IsNullOrWhiteSpace() && ZNetScene.instance.GetPrefab(data.CloneEffectsFrom) is { } original && original.TryGetComponent(out Door component))
            {
                door.m_openEffects = component.m_openEffects;
                door.m_closeEffects = component.m_closeEffects;
                door.m_lockedEffects = component.m_lockedEffects;
            }
        }
    }

    public class DoubleDoorData
    {
        public readonly GameObject Prefab = null!;
        public string CloneEffectsFrom = "";
        public string KeyItem = "";

        public DoubleDoorData(AssetBundle assetBundle, string assetName)
        {
            if (assetBundle.LoadAsset<GameObject>(assetName) is { } prefab)
            {
                Prefab = prefab;
                m_doubleDoors[assetName] = this;
            }
            else
            {
                Debug.LogWarning(assetName + " is null");
            }
        }

        public void SetData()
        {
            DoubleDoor door = Prefab.AddComponent<DoubleDoor>();
            if (!KeyItem.IsNullOrWhiteSpace() && ObjectDB.instance.GetItemPrefab(KeyItem) is { } key && key.TryGetComponent(out ItemDrop keyComponent))
            {
                door.m_keyItem = keyComponent;
            }
            if (!CloneEffectsFrom.IsNullOrWhiteSpace() && ZNetScene.instance.GetPrefab(CloneEffectsFrom) is { } original && original.TryGetComponent(out Door component))
            {
                door.m_openEffects = component.m_openEffects;
                door.m_closeEffects = component.m_closeEffects;
                door.m_lockedEffects = component.m_lockedEffects;
            }
        }
    }

    public class DungeonData
    {
        private readonly AssetBundle AssetBundle = null!;
        public readonly GameObject Prefab = null!;
        public GameObject Generator = null!;
        public readonly List<RoomReference> m_dungeonRooms = new();
        private readonly RoomList m_roomList = null!;
        public DungeonData(AssetBundle assetBundle, string roomListName, string generatorName)
        {
            if (assetBundle.LoadAsset<GameObject>(roomListName) is { } prefab)
            {
                Prefab = prefab;
                m_roomList = Prefab.GetComponent<RoomList>();
                AssetBundle = assetBundle;
                m_roomLists[roomListName] = this;
                RegisterDungeonGenerator(generatorName);
            }
            else
            {
                Debug.LogWarning(roomListName + " is null");
            }
        }

        public void AddRoom(string assetName)
        {
            if (!RegisteredRoomReferences.TryGetValue(assetName, out RoomReference reference))
            {
                reference = new RoomReference(AssetBundle, assetName);
                RegisteredRoomReferences[assetName] = reference;
            }
            if (!RegisteredRooms.TryGetValue(assetName, out DungeonDB.RoomData data))
            {
                data = new DungeonDB.RoomData()
                {
                    m_prefab = reference.GetReference(),
                    m_enabled = true,
                    m_theme = Room.Theme.None,
                };
                reference.m_data = data;
                RegisteredRooms[assetName] = data;
            }
            m_dungeonRooms.Add(reference);
            m_roomList.m_rooms.Add(data);
        }

        private void RegisterDungeonGenerator(string assetName)
        {
            if (AssetBundle.LoadAsset<GameObject>(assetName) is { } prefab)
            {
                Generator = prefab;
                m_dungeons[assetName] = this;
            }
            else
            {
                Debug.LogWarning(assetName + " is null");
            }
        }
    }

    public class RoomReference
    {
        private readonly AssetID AssetID;
        public DungeonDB.RoomData m_data = null!;

        public SoftReference<GameObject> GetReference() => NorthernAssetsPlugin.m_assetLoaderManager.GetSoftReference(AssetID);

        public RoomReference(AssetBundle assetBundle, string assetName)
        {
            if (assetBundle.LoadAsset<GameObject>(assetName) is { } prefab)
            {
                NorthernAssetsPlugin.m_assetLoaderManager.AddAsset(prefab, out AssetID assetID);
                AssetID = assetID;
            }
            else
            {
                Debug.LogWarning(assetName + " is null");
            }
        }
    }
    
    public class LocationData
    {
        private readonly AssetID AssetID;
        public readonly ZoneData m_data = new();
        public float Altitude = 5000f;
        public Vector3 EnvironmentScale = new Vector3(200f, 500f, 200f);
        public string Environment = "Caves";
        public bool SpawnInteriorEnv = true;
        public readonly GameObject Prefab = null!;
        
        public LocationData(string assetName, AssetBundle assetBundle)
        {
            if (assetBundle.LoadAsset<GameObject>(assetName) is { } prefab)
            {
                Prefab = prefab;
                GetZoneData();
                NorthernAssetsPlugin.m_assetLoaderManager.AddAsset(prefab, out AssetID assetID);
                AssetID = assetID;
                m_data.m_prefabName = assetName;
                m_locations[prefab.name] = this;
                NorthernAssetsPlugin.NorthernAssetsLogger.LogDebug("Registered location: " + assetName);
            }
            else
            {
                Debug.LogWarning(assetName + " is null");
            }
        }

        private void GetZoneData()
        {
            if (!Prefab.TryGetComponent(out Location component)) return;
            m_data.m_clearArea = component.m_clearArea;
            m_data.m_exteriorRadius = component.m_exteriorRadius;
            m_data.m_interiorRadius = component.m_interiorRadius;
        }

        public class ZoneData
        {
            public Heightmap.Biome m_biome = Heightmap.Biome.DeepNorth;
            public bool m_enabled = true;
            public string m_prefabName = null!;
            public Heightmap.BiomeArea m_biomeArea = Heightmap.BiomeArea.Everything;
            public int m_quantity = 100;
            public bool m_prioritized = true;
            public bool m_centerFirst = false;
            public bool m_unique = false;
            public string m_group = "";
            public float m_minDistanceFromSimilar = 0f;
            public string m_groupMax = "";
            public float m_maxDistanceFromSimilar = 0f;
            public bool m_iconAlways = false;
            public bool m_iconPlaced = false;
            public bool m_randomRotation = false;
            public bool m_slopeRotation = false;
            public bool m_snapToWater = false;
            public float m_interiorRadius = 0f;
            public float m_exteriorRadius = 50f;
            public bool m_clearArea;
            public float m_minTerrainDelta = 0f; 
            public float m_maxTerrainDelta = 100f;
            public float m_minimumVegetation;
            public float m_maximumVegetation = 1f;
            public bool m_surroundCheckVegetation = false;
            public float m_surroundCheckDistance = 20f;
            public int m_surroundCheckLayers = 2;
            public float m_surroundBetterThanAverage;
            public bool m_inForest = false;
            public float m_forestThresholdMin = 0f;
            public float m_forestThresholdMax = 1f;
            public float m_minDistance = 0f;
            public float m_maxDistance = 10000f;
            public float m_minAltitude = 0f;
            public float m_maxAltitude = 1000f;
            public bool m_foldout = false;
        }
        
        public ZoneSystem.ZoneLocation GetLocation()
        {
            return new ZoneSystem.ZoneLocation()
            {
                m_enable = m_data.m_enabled,
                m_prefabName = m_data.m_prefabName,
                m_prefab = NorthernAssetsPlugin.m_assetLoaderManager.GetSoftReference(AssetID),
                m_biome = m_data.m_biome,
                m_biomeArea = m_data.m_biomeArea,
                m_quantity = m_data.m_quantity,
                m_prioritized = m_data.m_prioritized,
                m_centerFirst = m_data.m_centerFirst,
                m_unique = m_data.m_unique,
                m_group = m_data.m_group,
                m_minDistanceFromSimilar = m_data.m_minDistanceFromSimilar,
                m_groupMax = m_data.m_groupMax,
                m_maxDistanceFromSimilar = m_data.m_maxDistanceFromSimilar,
                m_iconAlways = m_data.m_iconAlways,
                m_iconPlaced = m_data.m_iconPlaced,
                m_randomRotation = m_data.m_randomRotation,
                m_slopeRotation = m_data.m_slopeRotation,
                m_snapToWater = m_data.m_snapToWater,
                m_interiorRadius = m_data.m_interiorRadius,
                m_exteriorRadius = m_data.m_exteriorRadius,
                m_clearArea = m_data.m_clearArea,
                m_minTerrainDelta = m_data.m_minTerrainDelta,
                m_maxTerrainDelta = m_data.m_maxTerrainDelta,
                m_minimumVegetation = m_data.m_minimumVegetation,
                m_maximumVegetation = m_data.m_maximumVegetation,
                m_surroundCheckVegetation = m_data.m_surroundCheckVegetation,
                m_surroundCheckDistance = m_data.m_surroundCheckDistance,
                m_surroundCheckLayers = m_data.m_surroundCheckLayers,
                m_surroundBetterThanAverage = m_data.m_surroundBetterThanAverage,
                m_inForest = m_data.m_inForest,
                m_forestTresholdMin = m_data.m_forestThresholdMin,
                m_forestTresholdMax = m_data.m_forestThresholdMax,
                m_minDistance = m_data.m_minDistance,
                m_maxDistance = m_data.m_maxDistance,
                m_minAltitude = m_data.m_minAltitude,
                m_maxAltitude = m_data.m_maxAltitude,
                m_foldout = m_data.m_foldout
            };
        }
    }
}

