using UnityEngine;

namespace NorthernAssets.Behaviors;

public class DoubleDoor : MonoBehaviour, Hoverable, Interactable
{
    public string m_name = "";
    public ItemDrop? m_keyItem;
    public bool m_canNotBeClosed;
    public bool m_invertedOpenClosedText;
    public bool m_checkGuardStone = true;
    public GameObject? m_openEnable;
    public EffectList m_openEffects = new();
    public EffectList m_closeEffects = new();
    public EffectList m_lockedEffects = new();
    public ZNetView m_nview = null!;
    public Animator m_leftAnimator = null!;
    public Animator m_rightAnimator = null!;
    
    public uint m_lastDataRevision = uint.MaxValue;

    public void Awake()
    {
        m_nview = GetComponent<ZNetView>();
        if (m_nview.GetZDO() == null) return;
        m_leftAnimator = transform.Find("left_door").GetComponent<Animator>();
        m_rightAnimator = transform.Find("right_door").GetComponent<Animator>();
        if (m_nview) m_nview.Register<bool>(nameof(RPC_UseDoor), RPC_UseDoor);
        InvokeRepeating(nameof(UpdateState), 0.0f, 0.2f);
    }

    public void UpdateState()
    {
        if (!m_nview.IsValid()) return;
        var dataRevision = m_nview.GetZDO().DataRevision;
        if ((int)m_lastDataRevision == (int)dataRevision) return;
        m_lastDataRevision = dataRevision;
        SetState(m_nview.GetZDO().GetInt(ZDOVars.s_state));
    }

    public void SetState(int state)
    {
        
        if (m_leftAnimator.GetInteger(nameof(state)) != state)
        {
            if (state != 0) m_openEffects.Create(transform.position, transform.rotation);
            else m_closeEffects.Create(transform.position, transform.rotation);
            m_leftAnimator.SetInteger(nameof(state), state);
        }
        if (m_rightAnimator.GetInteger(nameof(state)) != state)
        {
            if (state != 0) m_openEffects.Create(transform.position, transform.rotation);
            else m_closeEffects.Create(transform.position, transform.rotation);
            m_rightAnimator.SetInteger(nameof(state), state);
        }
        

        if (m_openEnable is null) return;
        m_openEnable.SetActive(state != 0);

    }

    public bool CanInteract()
    {
        if (m_canNotBeClosed && m_nview.GetZDO().GetInt(ZDOVars.s_state) != 0) return false;
        return (m_leftAnimator.GetCurrentAnimatorStateInfo(0).IsTag("open") ||
                m_leftAnimator.GetCurrentAnimatorStateInfo(0).IsTag("closed")) &&
               (m_rightAnimator.GetCurrentAnimatorStateInfo(0).IsTag("open") ||
                m_rightAnimator.GetCurrentAnimatorStateInfo(0).IsTag("closed"));

    }

    public string GetHoverText()
    {
        if (!m_nview.IsValid() || (m_canNotBeClosed && !CanInteract()))
            return "";
        if (m_checkGuardStone && !PrivateArea.CheckAccess(transform.position, flash: false))
            return Localization.instance.Localize(m_name + "\n$piece_noaccess");
        if (!CanInteract())
            return Localization.instance.Localize(m_name);
        return m_nview.GetZDO().GetInt(ZDOVars.s_state) != 0
            ? Localization.instance.Localize(m_name + "\n[<color=yellow><b>$KEY_Use</b></color>] " +
                                             (m_invertedOpenClosedText ? "$piece_door_open" : "$piece_door_close"))
            : Localization.instance.Localize(m_name + "\n[<color=yellow><b>$KEY_Use</b></color>] " +
                                             (m_invertedOpenClosedText
                                                 ? "$piece_door_close"
                                                 : "$piece_door_open"));
    }

    public string GetHoverName()
    {
        return m_name;
    }

    public bool Interact(Humanoid character, bool hold, bool alt)
    {
        if (hold || !CanInteract())
            return false;
        if (m_checkGuardStone && !PrivateArea.CheckAccess(transform.position))
            return true;
        if (m_keyItem != null)
        {
            if (!HaveKey(character))
            {
                m_lockedEffects.Create(transform.position, transform.rotation);
                if (Game.m_worldLevel > 0 && HaveKey(character, false))
                    character.Message(MessageHud.MessageType.Center,
                        Localization.instance.Localize("$msg_ng_the_x") + m_keyItem.m_itemData.m_shared.m_name +
                        Localization.instance.Localize("$msg_ng_x_is_too_low"));
                else
                    character.Message(MessageHud.MessageType.Center,
                        Localization.instance.Localize("$msg_door_needkey", m_keyItem.m_itemData.m_shared.m_name));
                return true;
            }

            character.Message(MessageHud.MessageType.Center, Localization.instance.Localize("$msg_door_usingkey", m_keyItem.m_itemData.m_shared.m_name));
        }

        var normalized = (character.transform.position - transform.position).normalized;
        Game.instance.IncrementPlayerStat(m_nview.GetZDO().GetInt(ZDOVars.s_state) == 0
            ? PlayerStatType.DoorsOpened
            : PlayerStatType.DoorsClosed);
        Open(normalized);
        return true;
    }

    public void Open(Vector3 userDir)
    {
        m_nview.InvokeRPC(nameof(RPC_UseDoor), Vector3.Dot(transform.forward, userDir) < 0.0);
    }

    public bool UseItem(Humanoid user, ItemDrop.ItemData item)
    {
        if (!(m_keyItem != null) || m_keyItem.m_itemData.m_shared.m_name != item.m_shared.m_name || !CanInteract())
            return false;
        if (m_checkGuardStone && !PrivateArea.CheckAccess(transform.position))
            return true;
        user.Message(MessageHud.MessageType.Center,
            Localization.instance.Localize("$msg_door_usingkey", m_keyItem.m_itemData.m_shared.m_name));
        Open((user.transform.position - transform.position).normalized);
        return true;
    }

    public bool HaveKey(Humanoid player, bool matchWorldLevel = true)
    {
        return m_keyItem == null || player.GetInventory().HaveItem(m_keyItem.m_itemData.m_shared.m_name, matchWorldLevel);
    }

    public void RPC_UseDoor(long uid, bool forward)
    {
        if (!CanInteract())
            return;
        if (m_nview.GetZDO().GetInt(ZDOVars.s_state) == 0)
        {
            if (forward)
                m_nview.GetZDO().Set(ZDOVars.s_state, 1, false);
            else
                m_nview.GetZDO().Set(ZDOVars.s_state, -1, false);
        }
        else
        {
            m_nview.GetZDO().Set(ZDOVars.s_state, 0, false);
        }

        UpdateState();
    }
}