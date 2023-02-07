using Battle;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Zone _zone;
    [SerializeField] private UnitManager _unitManager;

    public Zone Zone => _zone;
    public UnitManager UnitManager => _unitManager;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}