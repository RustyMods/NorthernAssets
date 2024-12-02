using BepInEx;
using BepInEx.Configuration;

namespace NorthernAssets.StatusEffects;

public class SE_Potions : SE_Stats
{
    public ConfigEntry<float>? m_carryWeightConfig;
    public ConfigEntry<float>? m_speedConfig;
    public ConfigEntry<float>? m_duration;
    public ConfigEntry<float>? m_fallSpeedConfig;

    public ConfigEntry<float>? m_healAmount;
    public ConfigEntry<float>? m_staminaAmount;

    public string m_cloneEffectsFrom = "";

    public override void Setup(Character character)
    {
        GetStartEffects();
        base.Setup(character);
        m_speedModifier = m_speedConfig?.Value ?? 0f;
        m_addMaxCarryWeight = m_carryWeightConfig?.Value ?? 0f;
        m_ttl = m_duration?.Value ?? m_ttl;
        m_maxMaxFallSpeed = m_fallSpeedConfig?.Value ?? 0f;

        if (m_healAmount != null)
        {
            character.Heal(m_healAmount.Value);
        }

        if (m_staminaAmount != null)
        {
            if (character is not Player player) return;
            player.m_stamina += m_staminaAmount.Value;
        }
    }

    private void GetStartEffects()
    {
        if (m_cloneEffectsFrom.IsNullOrWhiteSpace()) return;
        if (ObjectDB.instance.GetStatusEffect(m_cloneEffectsFrom.GetStableHashCode()) is { } original)
        {
            m_startEffects = original.m_startEffects;
        }
    }
}