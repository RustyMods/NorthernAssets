using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using NorthernAssets.Managers;
using UnityEngine;
namespace NorthernAssets.Behaviors;

public class TriggerOfferingBowl : MonoBehaviour, Hoverable
{
    private static readonly Dictionary<string, TriggerOffering> m_triggerOfferings = new();

    private ZNetView m_nview = null!;
    public Vector3 m_spawnPoint;
    public Vector3 m_effectPoint;
    public float m_spawnBossDelay = 12f;
    public string m_name = "Altar";
    public string m_usedAltarText = "";
    public string m_hoverText = "";
    public ItemDrop m_offerItem = null!;
    public GameObject m_bossPrefab = null!;
    public EffectList m_spawnBossStartEffects = new EffectList();
    public EffectList m_spawnBossDoneEffects = new EffectList();
    public EffectList m_destroyItemEffects = new();
    private bool m_bossSpawned;
    private void Awake()
    {
        m_nview = GetComponent<ZNetView>();
        m_spawnPoint = transform.Find("spawn_point").position;
        m_effectPoint = transform.Find("spawn_effect").position;
    }

    private void Start()
    {
        if (!m_nview || !m_nview.IsValid()) return;
        m_nview.Register(nameof(RPC_SpawnBoss), RPC_SpawnBoss);
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<ItemDrop>() is { } itemDrop)
        {
            if (itemDrop.m_itemData.m_shared.m_name == m_offerItem.m_itemData.m_shared.m_name) SpawnBoss();
            ZNetScene.instance.Destroy(itemDrop.gameObject);
            m_destroyItemEffects.Create(m_effectPoint, Quaternion.identity);
        }
        else
        {
            if (other.GetComponentInParent<Player>() is { } player)
            {
                player.TeleportTo(m_spawnPoint, Quaternion.identity, false);
            }
            else if (other.GetComponentInParent<ZNetView>() is { } znv)
            {
                ZNetScene.instance.Destroy(znv.gameObject);
                m_destroyItemEffects.Create(m_effectPoint, Quaternion.identity);
            }
        }
    }
    
    private void SpawnBoss() => m_nview.InvokeRPC(nameof(RPC_SpawnBoss));

    private void RPC_SpawnBoss(long sender)
    {
        if (!m_nview || !m_nview.IsValid() || !m_nview.IsOwner()) return;
        if (IsBossSpawned()) return;
        m_bossSpawned = true;
        Invoke(nameof(DelayedSpawnBoss), m_spawnBossDelay);
        m_spawnBossStartEffects.Create(m_spawnPoint, Quaternion.identity);
        BossSpawnInitiated();
    }

    private void BossSpawnInitiated()
    {
        Player.m_localPlayer.Message(MessageHud.MessageType.Center, m_usedAltarText);
        foreach (Player player in Player.GetAllPlayers())
        {
            player.Message(MessageHud.MessageType.Center, m_usedAltarText);
        }
    }

    private void DelayedSpawnBoss()
    {
        GameObject boss = Instantiate(m_bossPrefab, m_spawnPoint, Quaternion.identity);
        if (boss.TryGetComponent(out BaseAI component))
        {
            component.SetPatrolPoint();
            component.Alert();
            component.m_character.m_onDeath += () => m_bossSpawned = false;
        }

        foreach (var effect in m_spawnBossDoneEffects.Create(m_spawnPoint, Quaternion.identity))
        {
            IProjectile[] components = effect.GetComponentsInChildren<IProjectile>();
            if (components.Length != 0)
            {
                foreach (var projectile in components)
                {
                    projectile.Setup(boss.GetComponent<Character>(), Vector3.zero, -1f, null, null, null);
                }
            }
        }
    }

    private bool IsBossSpawned() => m_bossSpawned;

    public string GetHoverText() => Localization.instance.Localize(m_hoverText);

    public string GetHoverName() => m_name;

    [HarmonyPatch(typeof(ZNetView), nameof(ZNetView.Awake))]
    private static class ZNetView_Awake_Patch
    {
        private static void Postfix(ZNetView __instance)
        {
            if (!m_triggerOfferings.TryGetValue(Helpers.GetNormalizedName(__instance.name), out TriggerOffering data)) return;
            if (__instance.gameObject.GetComponent<TriggerOfferingBowl>()) return;
            if (ZNetScene.instance.GetPrefab(data.BossPrefab) is not { } boss) return;
            if (ObjectDB.instance.GetItemPrefab(data.OfferItem) is not { } item || !item.TryGetComponent(out ItemDrop offerItem)) return;
            TriggerOfferingBowl offering = __instance.gameObject.AddComponent<TriggerOfferingBowl>();
            offering.m_bossPrefab = boss;
            offering.m_offerItem = offerItem;
            offering.m_spawnBossDelay = data.Delay;
            offering.m_hoverText = data.HoverText + offerItem.m_itemData.m_shared.m_name;
            offering.m_name = data.Name;
            offering.m_usedAltarText = data.Message;
            data.UpdateEffects(data.StartEffects, ref offering.m_spawnBossStartEffects);
            data.UpdateEffects(data.DoneEffects, ref offering.m_spawnBossDoneEffects);
            data.UpdateEffects(data.DestroyItemEffects, ref offering.m_destroyItemEffects);
        }
    }

    public class TriggerOffering
    {
        public string BossPrefab = "";
        public string OfferItem = "";
        public float Delay = 12f;
        public string HoverText = "Offer sacrifice";
        public string Name = "Altar";
        public string Message = "";
        public readonly List<string> StartEffects = new();
        public readonly List<string> DoneEffects = new();
        public readonly List<string> DestroyItemEffects = new();

        public TriggerOffering(AssetBundle assetBundle, string prefabName)
        {
            if (assetBundle.LoadAsset<GameObject>(prefabName) is { } prefab)
            {
                m_triggerOfferings[prefabName] = this;
                LocationManager.RegisterToScene(prefab);
            }
            else
            {
                Debug.LogWarning(prefabName + " is null");
            }
        }
        
        internal void UpdateEffects(List<string> effects, ref EffectList effectList)
        {
            if (effects.Count <= 0) return;
            List<EffectList.EffectData> data = effectList.m_effectPrefabs.ToList();
            foreach (string effect in effects)
            {
                GameObject? prefab = ZNetScene.instance.GetPrefab(effect);
                if (!prefab) continue;
                data.Add(new EffectList.EffectData() { m_prefab = prefab });
            }

            effectList.m_effectPrefabs = data.ToArray();
        }
    }
}