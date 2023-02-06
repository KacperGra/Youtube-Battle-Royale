using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Zone _zone;

    public Zone Zone => _zone;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}