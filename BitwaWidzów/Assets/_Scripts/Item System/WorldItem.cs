using Battle;
using System.Collections;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    [SerializeField] private ItemData _data;

    public void Pickup(Unit unit)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        gameObject.SetActive(false);

        _data.UseItem(unit);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Unit unit))
        {
            Pickup(unit);
        }
    }
}