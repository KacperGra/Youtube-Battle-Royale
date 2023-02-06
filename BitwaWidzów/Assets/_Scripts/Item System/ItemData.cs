using Battle;
using System.Collections;
using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    [SerializeField] private string _name;

    public abstract void UseItem(Unit unit);
}