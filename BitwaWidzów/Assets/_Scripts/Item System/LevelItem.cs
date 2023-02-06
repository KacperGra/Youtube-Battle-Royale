using Battle;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Battle/New Level Item")]
public class LevelItem : ItemData
{
    [SerializeField] private int _xpAmount;

    public override void UseItem(Unit unit)
    {
        unit.LevelSystem.Exp += _xpAmount;
    }
}