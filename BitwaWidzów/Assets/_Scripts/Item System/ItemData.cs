using Battle;
using System.Collections;
using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    public abstract void UseItem(Unit unit);
}