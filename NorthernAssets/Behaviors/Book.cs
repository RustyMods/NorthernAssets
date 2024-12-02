using System.Text;
using BepInEx;
using UnityEngine;

namespace NorthernAssets.Behaviors;

public class Book : MonoBehaviour, TextReceiver, Interactable, Hoverable
{
    private ZNetView m_nview = null!;
    private readonly string m_key = "BookInputKey";
    public void Awake()
    {
        m_nview = GetComponent<ZNetView>();
        if (!m_nview.IsValid()) return;
        m_nview.Register<string>(nameof(RPC_SetText), RPC_SetText);
    }

    private void RPC_SetText(long sender, string text)
    {
        if (!m_nview.IsValid()) return;
        m_nview.GetZDO().Set(m_key, text);
    }

    public string GetText()
    {
        if (!m_nview.IsValid()) return "";
        return m_nview.GetZDO().GetString(m_key);
    }

    public void SetText(string text)
    {
        if (!m_nview.IsValid()) return;
        m_nview.InvokeRPC(nameof(RPC_SetText), text);
    }

    public bool Interact(Humanoid user, bool hold, bool alt)
    {
        if (hold) return false;
        if (alt && IsCreator())
        {
            TextInput.instance.RequestText(this, "Set text", 100);
            return true;
        }
        string text = GetText();
        if (text.IsNullOrWhiteSpace()) return false;
        TextViewer.instance.ShowText(TextViewer.Style.Rune, "", text, true);
        return true;
    }

    private bool IsCreator() => m_nview.GetZDO().GetLong(ZDOVars.s_creator) == Game.instance.GetPlayerProfile().GetPlayerID();

    public bool UseItem(Humanoid user, ItemDrop.ItemData item) => false;
    
    public string GetHoverText()
    {
        StringBuilder stringBuilder = new StringBuilder();
        if (!GetText().IsNullOrWhiteSpace())
        {
            stringBuilder.Append("[<color=yellow>$KEY_Use</color>] $hover_read");
        }
        if (IsCreator())
        {
            stringBuilder.Append("\n[<color=yellow>L.Shift + $KEY_Use</color>] $hover_write");
        }

        return Localization.instance.Localize(stringBuilder.ToString());
    }

    public string GetHoverName() => name;
}