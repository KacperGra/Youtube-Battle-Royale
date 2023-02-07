using Battle;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerPrefab;
    [SerializeField] private Transform _content;
    [SerializeField] private int _playerAmount = 5;

    private List<TextMeshProUGUI> _playerSlots = new();
    private UnitManager _unitManager;

    private void Start()
    {
        _unitManager = GameManager.Instance.UnitManager;
        for (int i = 0; i < _playerAmount; ++i)
        {
            var newSlot = Instantiate(_playerPrefab, _content);
            _playerSlots.Add(newSlot);
        }
    }

    private void Update()
    {
        var bestUnits = GetBestUnits();

        for (int i = 0; i < _playerSlots.Count; ++i)
        {
            var playerSlot = _playerSlots[i];
            var unit = bestUnits[i];

            playerSlot.text = $"{i + 1}. {unit.Nickname} (Zabici {unit.LeaderboardStatistics.Kills} LVL {unit.LevelSystem.Level})";
        }
    }

    private List<Unit> GetBestUnits()
    {
        List<Unit> units = new();

        for (int i = 0; i < _playerSlots.Count; ++i)
        {
            int bestKills = 0;
            Unit bestUnit = null;
            foreach (Unit unit in _unitManager.UnitList)
            {
                if (units.Contains(unit))
                {
                    continue;
                }

                if (unit.LeaderboardStatistics.Kills >= bestKills)
                {
                    bestKills = unit.LeaderboardStatistics.Kills;
                    bestUnit = unit;
                }
            }

            units.Add(bestUnit);
        }

        return units;
    }
}