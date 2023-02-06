using Battle;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitManagerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _amountText;

    private UnitManager _unitManager;

    private void Awake()
    {
        _unitManager = FindObjectOfType<UnitManager>();
    }

    private void Update()
    {
        _amountText.text = $"Liczba Widzów: {_unitManager.GetUnitCount()}";
    }
}