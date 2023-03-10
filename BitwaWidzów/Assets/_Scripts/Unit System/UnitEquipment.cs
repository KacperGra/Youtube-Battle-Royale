using System;
using System.Collections;
using UnityEngine;

public class UnitEquipment : MonoBehaviour
{
    public event Action OnItemEquiped;

    public event Action OnArmorEquiped;

    [Header("Weapon")]
    [SerializeField] private GameObject _baseball;
    [SerializeField] private GameObject _sword;
    [SerializeField] private GameObject _woodenPlank;
    [SerializeField] private GameObject _metalPipe;
    [SerializeField] private GameObject _boxingGlove;
    [SerializeField] private GameObject _demonSword;

    [Header("Armor")]
    [SerializeField] private GameObject _noobVR;
    [SerializeField] private GameObject _proVR;
    [SerializeField] private GameObject _shield;

    private WeaponItem _weaponItem;
    private ArmorItem _armorItem;

    public WeaponItem WeaponItem => _weaponItem;
    public ArmorItem ArmorItem => _armorItem;

    public int Damage
    {
        get
        {
            if (_weaponItem == null)
            {
                return 0;
            }

            return _weaponItem.Damage;
        }
    }

    public int Armor
    {
        get
        {
            if (_armorItem == null)
            {
                return 0;
            }

            return _armorItem.Armor;
        }
    }

    public int Health
    {
        get
        {
            if (_armorItem == null)
            {
                return 0;
            }

            return _armorItem.Health;
        }
    }

    private void Start()
    {
        DisableAllWeapons();
    }

    public void EquipWeapon(WeaponItem item)
    {
        if (item == null)
        {
            return;
        }

        if (item.Damage <= Damage)
        {
            return;
        }

        _weaponItem = item;

        DisableAllWeapons();
        switch (_weaponItem.WeaponType)
        {
            case WeaponType.Baseball:
                _baseball.SetActive(true);
                break;

            case WeaponType.Sword:
                _sword.SetActive(true);
                break;

            case WeaponType.WoodenPlank:
                _woodenPlank.SetActive(true);
                break;

            case WeaponType.MetalPipe:
                _metalPipe.SetActive(true);
                break;

            case WeaponType.BoxingGlove:
                _boxingGlove.SetActive(true);
                break;

            case WeaponType.DemonSword:
                _demonSword.SetActive(true);
                break;
        }

        OnItemEquiped?.Invoke();
    }

    public void EquipArmor(ArmorItem item)
    {
        if (item == null)
        {
            return;
        }

        if (item.Health < Health)
        {
            return;
        }

        _armorItem = item;
        DisableAllArmor();
        switch (_armorItem.ArmorType)
        {
            case ArmorType.NoobVR:
                _noobVR.SetActive(true);
                break;

            case ArmorType.ProVR:
                _proVR.SetActive(true);
                break;

            case ArmorType.Shield:
                _shield.SetActive(true);
                break;
        }

        OnArmorEquiped?.Invoke();
        OnItemEquiped?.Invoke();
    }

    private void DisableAllWeapons()
    {
        _baseball.SetActive(false);
        _sword.SetActive(false);
        _woodenPlank.SetActive(false);
        _metalPipe.SetActive(false);
        _boxingGlove.SetActive(false);
    }

    private void DisableAllArmor()
    {
        _noobVR.SetActive(false);
        _proVR.SetActive(false);
        _shield.SetActive(false);
    }
}