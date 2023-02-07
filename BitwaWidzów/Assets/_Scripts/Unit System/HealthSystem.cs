using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem
{
    public event Action OnHealthChanged;

    private int _health;
    private int _maxHealth;

    public int Health
    {
        get => _health;
        set
        {
            _health = value;
            if (_health > _maxHealth)
            {
                _health = _maxHealth;
            }

            OnHealthChanged?.Invoke();
        }
    }

    public int MaxHealth
    {
        get => _maxHealth;
        set
        {
            _maxHealth = value;

            OnHealthChanged?.Invoke();
        }
    }

    public HealthSystem(int health)
    {
        _health = health;
        _maxHealth = health;
    }

    public float GetRatio()
    {
        return (float)_health / _maxHealth;
    }
}