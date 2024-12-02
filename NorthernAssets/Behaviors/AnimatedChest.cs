using HarmonyLib;
using NorthernAssets.Managers;
using UnityEngine;

namespace NorthernAssets.Behaviors;

public class AnimatedChest : MonoBehaviour
{
    public ZSyncAnimation m_animator = null!;

    public bool open;
    public void Awake()
    {
        m_animator = GetComponent<ZSyncAnimation>();
    }

    [HarmonyPatch(typeof(Container), nameof(Container.SetInUse))]
    private static class Container_SetInUse_Patch
    {
        private static void Postfix(Container __instance, bool inUse)
        {
            if (!__instance.TryGetComponent(out AnimatedChest component) || !__instance.m_nview.IsOwner()) return;
            if (component.open == inUse) return;
            switch (Helpers.GetNormalizedName(__instance.name))
            {
                case "RS_ChestAnimated":
                    component.m_animator.SetTrigger(inUse ? "open" : "idle");
                    break;
                case "RS_ChestAnimated1":
                    component.m_animator.SetTrigger(inUse ? "open" : "close");
                    break;
            }
            component.open = inUse;
        }
    }

    [HarmonyPatch(typeof(Container), nameof(Container.Interact))]
    private static class Container_Interact_Patch
    {
        private static bool Prefix(Container __instance, Character character)
        {
            if (!__instance.GetComponent<AnimatedChest>()) return true;
            if (Random.value > NorthernAssetsPlugin._chestExplodeChance.Value) return true;
            var pos = __instance.transform.position + new Vector3(0f, 0.5f, 0f);
            ChestExplosion.Create(pos, Quaternion.identity);
            ZNetScene.instance.Destroy(__instance.gameObject);
            if (character is Player player)
            {
                player.AddFireDamage(5f);
                player.AddLightningDamage(5f);
            }
            return false;
        }
    }

    private static EffectList ChestExplosion = null!;

    [HarmonyPriority(Priority.Last)]
    [HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
    private static class ZNetScene_Awake_Patch
    {
        private static void Postfix()
        {
            var explosion = ZNetScene.instance.GetPrefab("vfx_chest_explosion");
            var sfx = Instantiate(ZNetScene.instance.GetPrefab("sfx_unstablerock_explosion"), NorthernAssetsPlugin._Root.transform, false);
            sfx.name = "sfx_chest_explosion";
            if (sfx.TryGetComponent(out ZSFX component))
            {
                component.m_delay = 2f;
                component.m_maxDelay = 2f;
                component.m_minDelay = 2f;
            }
            ZNetScene.instance.m_prefabs.Add(sfx);
            ZNetScene.instance.m_namedPrefabs[sfx.name.GetStableHashCode()] = sfx;
            ChestExplosion = new EffectList()
            {
                m_effectPrefabs = new EffectList.EffectData[]
                {
                    new EffectList.EffectData()
                    { m_prefab = explosion, },
                    new EffectList.EffectData()
                    { m_prefab = sfx }
                }
            };
        }
    }
}