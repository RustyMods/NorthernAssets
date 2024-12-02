using System.Collections.Generic;
using UnityEngine;

namespace NorthernAssets.Behaviors;

public class BrightnessControl : MonoBehaviour
{
    private ZNetView m_nview = null!;
    private readonly List<Light> m_lights = new();
    public static readonly List<BrightnessControl> m_instances = new();
    public void Awake()
    {
        m_nview = GetComponent<ZNetView>();
        if (!m_nview.IsValid()) return;
        foreach (var light in GetComponentsInChildren<Light>())
        {
            m_lights.Add(light);
        }
        m_instances.Add(this);
    }

    public void OnDestroy()
    {
        m_instances.Remove(this);
    }

    private void UpdateIntensity()
    {
        foreach (var light in m_lights)
        {
            light.intensity = NorthernAssetsPlugin.GetCandleIntensity();
        }
    }

    public static void UpdateCandleLights()
    {
        foreach (var instance in m_instances)
        {
            instance.UpdateIntensity();
        }
    }
}