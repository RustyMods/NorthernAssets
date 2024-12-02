using System;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using JetBrains.Annotations;
using NorthernAssets.Behaviors;
using NorthernAssets.Managers;
using NorthernAssets.prefabs;
using ServerSync;
using UnityEngine;

namespace NorthernAssets
{
    [BepInDependency("RustyMods.Bestiary", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class NorthernAssetsPlugin : BaseUnityPlugin
    {
        internal const string ModName = "NorthernAssets";
        internal const string ModVersion = "0.0.1";
        internal const string Author = "RustyMods";
        private const string ModGUID = Author + "." + ModName;
        private static readonly string ConfigFileName = ModGUID + ".cfg";
        private static readonly string ConfigFileFullPath = Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;
        internal static string ConnectionError = "";
        private readonly Harmony _harmony = new(ModGUID);
        public static readonly ManualLogSource NorthernAssetsLogger = BepInEx.Logging.Logger.CreateLogSource(ModName);
        public static readonly ConfigSync ConfigSync = new(ModGUID) { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion };
        public enum Toggle { On = 1, Off = 0 }

        public static NorthernAssetsPlugin _Plugin = null!;
        public static AssetLoaderManager m_assetLoaderManager = null!;
        public static readonly AssetBundle _Assets = GetAssetBundle("northernbundle");
        public static GameObject _Root = null!;
        private static ConfigEntry<Toggle> _serverConfigLocked = null!;
        private static ConfigEntry<float> _candleIntensity = null!;
        public static float GetCandleIntensity() => _candleIntensity.Value;
        public static ConfigEntry<float> _chestExplodeChance = null!;

        private void InitConfigs()
        {
            _serverConfigLocked = config("1 - General", "Lock Configuration", Toggle.On, "If on, the configuration is locked and can be changed by server admins only.");
            _ = ConfigSync.AddLockingConfigEntry(_serverConfigLocked);
            _candleIntensity = config("1 - General", "Candle Light Intensity", 1f, new ConfigDescription("Set candle light intensity", new AcceptableValueRange<float>(1f, 5f)));
            _candleIntensity.SettingChanged += (_, _) => BrightnessControl.UpdateCandleLights();
            _chestExplodeChance = config("1 - General", "Dungeon Chest Explode Chance", 0f, new ConfigDescription("Set chance for chest to explode", new AcceptableValueRange<float>(0f, 1f)));
        }
        public void Awake()
        {
            InitConfigs();
            _Plugin = this;
            _Root = new GameObject("root");
            DontDestroyOnLoad(_Root);
            _Root.SetActive(false);
            Localizer.Load();

            m_assetLoaderManager = new(_Plugin.Info.Metadata);

            AssetManager.Load();
            RandomCaveExit.Setup();
            
            Assembly assembly = Assembly.GetExecutingAssembly();
            _harmony.PatchAll(assembly);
            // SetupWatcher();
        }
        
        public void UpdateLocationPositions()
        {
            if (RandomCaveExit.UpdateCavePositions()) return;
            Invoke(nameof(UpdateLocationPositions), 120f);
        }
        
        private void OnDestroy() => Config.Save();

        private static AssetBundle GetAssetBundle(string fileName)
        {
            Assembly execAssembly = Assembly.GetExecutingAssembly();
            string resourceName = execAssembly.GetManifestResourceNames().Single(str => str.EndsWith(fileName));
            using Stream? stream = execAssembly.GetManifestResourceStream(resourceName);
            return AssetBundle.LoadFromStream(stream);
        }

        public void SetupWatcher()
        {
            FileSystemWatcher watcher = new(Paths.ConfigPath, ConfigFileName);
            watcher.Changed += ReadConfigValues;
            watcher.Created += ReadConfigValues;
            watcher.Renamed += ReadConfigValues;
            watcher.IncludeSubdirectories = true;
            watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
            watcher.EnableRaisingEvents = true;
        }

        private void ReadConfigValues(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(ConfigFileFullPath)) return;
            try
            {
                NorthernAssetsLogger.LogDebug("ReadConfigValues called");
                Config.Reload();
            }
            catch
            {
                NorthernAssetsLogger.LogError($"There was an issue loading your {ConfigFileName}");
                NorthernAssetsLogger.LogError("Please check your config entries for spelling and format!");
            }
        }

        public ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description,
            bool synchronizedSetting = true)
        {
            ConfigDescription extendedDescription =
                new(
                    description.Description +
                    (synchronizedSetting ? " [Synced with Server]" : " [Not Synced with Server]"),
                    description.AcceptableValues, description.Tags);
            ConfigEntry<T> configEntry = Config.Bind(group, name, value, extendedDescription);
            //var configEntry = Config.Bind(group, name, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = ConfigSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }

        public ConfigEntry<T> config<T>(string group, string name, T value, string description,
            bool synchronizedSetting = true)
        {
            return config(group, name, value, new ConfigDescription(description), synchronizedSetting);
        }

        // private class ConfigurationManagerAttributes
        // {
        //     [UsedImplicitly] public int? Order;
        //     [UsedImplicitly] public bool? Browsable;
        //     [UsedImplicitly] public string? Category;
        //     [UsedImplicitly] public Action<ConfigEntryBase>? CustomDrawer;
        // }
    }

    // public static class KeyboardExtensions
    // {
    //     public static bool IsKeyDown(this KeyboardShortcut shortcut)
    //     {
    //         return shortcut.MainKey != KeyCode.None && Input.GetKeyDown(shortcut.MainKey) &&
    //                shortcut.Modifiers.All(Input.GetKey);
    //     }
    //
    //     public static bool IsKeyHeld(this KeyboardShortcut shortcut)
    //     {
    //         return shortcut.MainKey != KeyCode.None && Input.GetKey(shortcut.MainKey) &&
    //                shortcut.Modifiers.All(Input.GetKey);
    //     }
    // }
}