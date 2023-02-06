using Battle;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nicknameText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private Slider _healthSlider;

    private Unit _unit;

    public void Initialize(Unit unit)
    {
        _unit = unit;
        UpdateLevel();
        UpdateHealth();

        unit.HealthSystem.OnHealthChanged += UpdateHealth;
        unit.LevelSystem.OnLevelChanged += UpdateLevel;
    }

    private void Update()
    {
        transform.LookAt(Camera.main.transform.position);
    }

    private void UpdateLevel()
    {
        _levelText.text = $"LVL: {_unit.LevelSystem.Level} (XP {_unit.LevelSystem.Exp}/{_unit.LevelSystem.GetTargetExp()})";
    }

    private void UpdateHealth()
    {
        _healthText.text = $"{_unit.HealthSystem.Health}/{_unit.HealthSystem.MaxHealth}";
        _healthSlider.value = _unit.HealthSystem.GetRatio();
    }
}