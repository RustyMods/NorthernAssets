using System.Collections.Generic;
using System.Linq;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace NorthernAssets.Managers;

public class ProjectileMan
{
    static ProjectileMan()
    {
        Harmony harmony = new("org.bepinex.helpers.ProjectileManager");
        harmony.Patch(AccessTools.DeclaredMethod(typeof(ZNetScene), nameof(ZNetScene.Awake)), postfix: new HarmonyMethod(AccessTools.DeclaredMethod(typeof(ProjectileMan), nameof(RegisterProjectiles))));
    }
    
    private static void RegisterProjectiles(ZNetScene __instance)
    {
        foreach (ProjectileData? projectile in m_projectiles)
        {
            if (projectile.Prefab.TryGetComponent(out Projectile component))
            {
                if (!projectile.m_spawnOnHit.IsNullOrWhiteSpace())
                {
                    if (__instance.GetPrefab(projectile.m_spawnOnHit) is { } spawnOnHit)
                    {
                        component.m_spawnOnHit = spawnOnHit;
                    }
                    component.m_spawnOnHitChance = projectile.m_spawnOnHitChance;
                }
                
                UpdateEffectList(projectile.m_hitEffects, ref component.m_hitEffects);
                UpdateEffectList(projectile.m_hitWaterEffects, ref component.m_hitWaterEffects);
                UpdateRandomSpawnOnHit(projectile.m_randomSpawnOnHit, ref component.m_randomSpawnOnHit);
            }

            Register(projectile.Prefab);
        }
    }
    
    private static void UpdateRandomSpawnOnHit(List<string> prefabNames, ref List<GameObject> data)
    {
        if (prefabNames.Count <= 0) return;
        foreach (var prefabName in prefabNames)
        {
            if (ZNetScene.instance.GetPrefab(prefabName) is { } prefab)
            {
                data.Add(prefab);
            }
        }
    }
    
    private static void Register(GameObject prefab)
    {
        if (!ZNetScene.instance.m_prefabs.Contains(prefab)) ZNetScene.instance.m_prefabs.Add(prefab);
        if (!ZNetScene.instance.m_namedPrefabs.ContainsKey(prefab.name.GetStableHashCode()))
        {
            ZNetScene.instance.m_namedPrefabs[prefab.name.GetStableHashCode()] = prefab;
        }
    }
    
    private static void UpdateEffectList(List<string> effects, ref EffectList effectList)
    {
        if (effects.Count <= 0) return;
        List<EffectList.EffectData> originals = effectList.m_effectPrefabs.ToList();
        foreach (var effect in effects)
        {
            if (ZNetScene.instance.GetPrefab(effect) is { } prefab)
            {
                originals.Add(new EffectList.EffectData()
                {
                    m_prefab = prefab,
                    m_enabled = true
                });
            }
        }

        effectList.m_effectPrefabs = originals.ToArray();
    }
    
    private static readonly List<ProjectileData> m_projectiles = new();

    public class ProjectileData
    {
        public readonly GameObject Prefab = null!;
        public readonly List<string> m_hitEffects = new();
        public readonly List<string> m_hitWaterEffects = new();
        public string m_spawnOnHit = "";
        public float m_spawnOnHitChance = 1f;
        public readonly List<string> m_randomSpawnOnHit = new();

        public ProjectileData(string name, AssetBundle bundle)
        {
            if (bundle.LoadAsset<GameObject>(name) is { } prefab)
            {
                Prefab = prefab;
                m_projectiles.Add(this);
            }
            else
            {
                Debug.LogWarning(name + " is null");
            }
        }

        public void AddHitEffect(string effectName) => m_hitEffects.Add(effectName);
        public void AddHitWaterEffect(string effectName) => m_hitWaterEffects.Add(effectName);
        public void AddSpawnOnHit(string name) => m_spawnOnHit = name;
        public void SetSpawnOnHitChance(float value) => m_spawnOnHitChance = value;
        public void AddRandomSpawnOnHit(string name) => m_randomSpawnOnHit.Add(name);
    }
}