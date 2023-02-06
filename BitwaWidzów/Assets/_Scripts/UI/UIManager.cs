using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UnitManagerUI _unitManagerUI;

    public UnitManagerUI UnityManager => _unitManagerUI;
}