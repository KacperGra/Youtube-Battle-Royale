using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem
{
    private const int RequiredExpMultiplier = 100;

    public event Action OnLevelChanged;

    public event Action OnXPChanged;

    private int _level;
    private int _exp;

    public int Level => _level;

    public int Exp
    {
        get => _exp;
        set
        {
            _exp = value;

            int targetExp = GetTargetExp();
            if (_exp >= targetExp)
            {
                int expDiff = _exp - targetExp;

                _exp = 0;
                ++_level;
                if (expDiff > 0)
                {
                    Exp += expDiff;
                }

                OnLevelChanged?.Invoke();
            }

            OnXPChanged?.Invoke();
        }
    }

    public LevelSystem()
    {
        _level = 1;
        _exp = 0;
    }

    public int GetTargetExp()
    {
        return _level * RequiredExpMultiplier;
    }
}