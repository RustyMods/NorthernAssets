using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NorthernAssets.Behaviors;

public class ToggleLight : MonoBehaviour, Interactable, Hoverable
{
    public ZNetView m_nview = null!;
    private readonly string m_enabledKey = "RustyEnabled";
    public List<GameObject> m_objectsToEnable = new();
    public void Awake()
    {
        m_nview = GetComponent<ZNetView>();
        if (!m_nview.IsValid()) return;
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeInHierarchy) continue;
            m_objectsToEnable.Add(child.gameObject);
        }
        m_nview.Register<bool>(nameof(RPC_Enable), RPC_Enable);
        var enable = m_nview.GetZDO().GetBool(m_enabledKey, true);
        m_nview.InvokeRPC(nameof(RPC_Enable), enable);
    }

    private void SetEnable(bool enable)
    {
        foreach (var obj in m_objectsToEnable) obj.SetActive(enable);
    }

    private void RPC_Enable(long sender, bool enable)
    {
        m_nview.GetZDO().Set(m_enabledKey, enable);
        SetEnable(enable);
    }

    public bool Interact(Humanoid user, bool hold, bool alt)
    {
        if (hold || alt) return false;
        if (!m_nview.IsValid()) return false;
        m_nview.InvokeRPC(nameof(RPC_Enable), !m_nview.GetZDO().GetBool(m_enabledKey));
        return true;
    }

    public bool UseItem(Humanoid user, ItemDrop.ItemData item) => false;
    
    public string GetHoverText()
    {
        if (!m_nview.IsValid()) return "";
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendFormat("[<color=yellow>$KEY_Use</color>] {0}", m_nview.GetZDO().GetBool(m_enabledKey) ? "$hover_off" : "$hover_on");
        return Localization.instance.Localize(stringBuilder.ToString());
    }

    public string GetHoverName() => "";
}