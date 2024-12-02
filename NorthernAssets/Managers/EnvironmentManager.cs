using System.Collections.Generic;
using System.Linq;
using BepInEx;
using HarmonyLib;
using Managers;
using UnityEngine;

namespace NorthernAssets.Managers;

public class EnvironmentManager
{
    private static readonly List<EnvSetup> m_setups = new();
    private static readonly Dictionary<string, GameObject> m_particles = new();
    private static readonly Dictionary<string, GameObject[]> m_registeredParticles = new();

    static EnvironmentManager()
    {
        Harmony harmony = new Harmony("org.bepinex.helpers.EnvironmentManager");
        harmony.Patch(AccessTools.DeclaredMethod(typeof(EnvMan), nameof(EnvMan.Awake)), postfix: new HarmonyMethod(AccessTools.DeclaredMethod(typeof(EnvironmentManager), nameof(Patch_EnvMan_Awake))));
        harmony.Patch(AccessTools.DeclaredMethod(typeof(EnvMan), nameof(EnvMan.Awake)), prefix: new HarmonyMethod(AccessTools.DeclaredMethod(typeof(EnvironmentManager), nameof(Patch_EnvMan_Awake_Prefix))));
    }

    internal static void Patch_EnvMan_Awake_Prefix(EnvMan __instance)
    {
        var followPlayer = __instance.transform.Find("FollowPlayer");
        foreach (GameObject prefab in m_particles.Values)
        {
            GameObject particle = Object.Instantiate(prefab, followPlayer, false);
            particle.name = prefab.name;
            particle.transform.localPosition = new Vector3(0f, 70f, 0f);
            particle.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            m_registeredParticles[particle.name] = (from Transform child in particle.transform select child.gameObject).ToArray();
        }
    }

    internal static void Patch_EnvMan_Awake(EnvMan __instance)
    {
        MusicManager.RegisterEnvironments();
        List<Heightmap.Biome> biomesToRemove = m_setups.Where(setup => setup.OverrideOriginal).Select(setup => setup.biome).ToList();
        List<BiomeEnvSetup> setupToRemove = new();
        foreach (BiomeEnvSetup? setup in __instance.m_biomes)
        {
            if (biomesToRemove.Contains(setup.m_biome)) setupToRemove.Add(setup);
        }

        __instance.m_biomes.RemoveAll(setup => setupToRemove.Contains(setup));
        foreach (EnvSetup? setup in m_setups)
        {
            var biomeEnvSetup = new BiomeEnvSetup()
            {
                m_name = setup.name,
                m_biome = setup.biome,
                m_musicMorning = setup.musicMorning,
                m_musicDay = setup.musicDay,
                m_musicEvening = setup.musicEvening,
                m_musicNight = setup.musicNight,
            };
            List<EnvEntry> entries = new();
            foreach (var weather in setup.weathers)
            {
                EnvEntry entry = new EnvEntry();
                entry.m_environment = weather.m_environment;
                entry.m_weight = weather.m_weight;
                entry.m_ashlandsOverride = weather.m_ashlandOverride;
                entry.m_deepnorthOverride = weather.m_deepNorthOverride;
                entry.m_env = __instance.GetEnv(weather.m_environment);
                if (m_registeredParticles.TryGetValue(weather.CustomParticles, out GameObject[] particleSystems))
                {
                    entry.m_env.m_psystems = particleSystems;
                    __instance.SetParticleArrayEnabled(entry.m_env.m_psystems, false);
                    entry.m_env.m_fogDensityDay = weather.FogDensity;
                    entry.m_env.m_fogDensityNight = weather.FogDensity;
                    entry.m_env.m_fogDensityEvening = weather.FogDensity;
                    entry.m_env.m_fogDensityMorning = weather.FogDensity;
                    entry.m_env.m_rainCloudAlpha = weather.CloudDensity;
                }
                entries.Add(entry);
            }

            biomeEnvSetup.m_environments = entries;
            __instance.m_biomes.Add(biomeEnvSetup);
        }
    }

    public class EnvSetup
    {
        public readonly string name;
        public readonly Heightmap.Biome biome;
        public readonly List<Weather> weathers = new();
        public string musicDay = "";
        public string musicMorning = "";
        public string musicNight = "";
        public string musicEvening = "";
        public bool OverrideOriginal = true;

        public EnvSetup(string name, Heightmap.Biome biome)
        {
            this.name = name;
            this.biome = biome;
            m_setups.Add(this);
        }
    }

    public static void RegisterParticleSystems(AssetBundle assetBundle, string assetName)
    {
        if (assetBundle.LoadAsset<GameObject>(assetName) is { } prefab)
        {
            m_particles[assetName] = prefab;
        }
        else Debug.LogWarning(assetName + " is null");
    }

    public class Weather
    {
        public readonly string m_environment;
        public readonly float m_weight;
        public bool m_ashlandOverride;
        public bool m_deepNorthOverride;
        public string CustomParticles = "";
        public float FogDensity;
        public float CloudDensity;

        public Weather(string name, float weight)
        {
            m_environment = name;
            m_weight = weight;
        }
    }
}