using Battle;
using System.Collections;
using UnityEngine;

public enum ArmorType
{
    NoobVR,
    ProVR,
    Shield
}

[CreateAssetMenu(menuName = "Battle/New Armor Item")]
public class ArmorItem : ItemData
{
    [SerializeField] private ArmorType _armorType;
    [SerializeField] private int _armor = 5;
    [SerializeField] private int _health = 100;

    public ArmorType ArmorType => _armorType;
    public int Armor => _armor;
    public int Health => _health;

    public override void UseItem(Unit unit)
    {
        unit.Equipment.EquipArmor(this);
    }
}