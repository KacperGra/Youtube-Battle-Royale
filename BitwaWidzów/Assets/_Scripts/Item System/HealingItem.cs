using Battle;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Battle/New Healing Item")]
public class HealingItem : ItemData
{
    [SerializeField] private int _healingValue = 100;

    public override void UseItem(Unit unit)
    {
        unit.HealthSystem.Health += _healingValue;
    }
}