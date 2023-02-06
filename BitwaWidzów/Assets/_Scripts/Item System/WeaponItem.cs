using Battle;
using System.Collections;
using UnityEngine;

public enum WeaponType
{
    Baseball,
    Sword,
    WoodenPlank,
    MetalPipe,
    BoxingGlove
}

[CreateAssetMenu(menuName = "Battle/New Weapon Item")]
public class WeaponItem : ItemData
{
    [SerializeField] private WeaponType _weaponType;
    [SerializeField] private int _damage = 10;

    public WeaponType WeaponType => _weaponType;
    public int Damage => _damage;

    public override void UseItem(Unit unit)
    {
        unit.Equipment.EquipWeapon(this);
    }
}