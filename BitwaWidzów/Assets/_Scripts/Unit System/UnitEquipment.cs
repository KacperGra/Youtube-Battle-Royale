using System.Collections;
using UnityEngine;

public class UnitEquipment : MonoBehaviour
{
    [SerializeField] private GameObject _baseball;
    [SerializeField] private GameObject _sword;
    [SerializeField] private GameObject _woodenPlank;
    [SerializeField] private GameObject _metalPipe;
    [SerializeField] private GameObject _boxingGlove;

    private WeaponItem _weaponData;

    public int Damage
    {
        get
        {
            if (_weaponData == null)
            {
                return 0;
            }

            return _weaponData.Damage;
        }
    }

    private void Start()
    {
        DisableAllWeapons();
    }

    public void EquipWeapon(WeaponItem item)
    {
        if (item.Damage <= Damage)
        {
            return;
        }

        _weaponData = item;

        DisableAllWeapons();
        switch (_weaponData.WeaponType)
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
        }
    }

    private void DisableAllWeapons()
    {
        _baseball.SetActive(false);
        _sword.SetActive(false);
        _woodenPlank.SetActive(false);
        _metalPipe.SetActive(false);
        _boxingGlove.SetActive(false);
    }
}