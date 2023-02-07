using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Zone : MonoBehaviour
{
    [SerializeField] private float _zoneDecreaseValue = 10f;
    [SerializeField] private int _zoneDamage = 20;

    private readonly List<Unit> _unitsInZone = new();

    private void Start()
    {
        StartCoroutine(DamageUnits());
    }

    private void Update()
    {
        float decreaseValue = _zoneDecreaseValue * Time.deltaTime;
        transform.localScale -= new Vector3(decreaseValue, 0, decreaseValue);
        if (transform.localScale.x < 0f || transform.localScale.z < 0f)
        {
            transform.localScale = new Vector3(0, transform.localScale.y, 0);
        }
    }

    private IEnumerator DamageUnits()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);

            foreach (var unit in _unitsInZone)
            {
                unit.TakeDamage((int)(_zoneDamage * 0.25f), true);
            }
        }
    }

    public void AddUnitToZone(Unit unit)
    {
        if (_unitsInZone.Contains(unit))
        {
            return;
        }

        _unitsInZone.Add(unit);
    }

    public void RemoveUnitFromZone(Unit unit)
    {
        if (!_unitsInZone.Contains(unit))
        {
            return;
        }

        _unitsInZone.Remove(unit);
    }

    public bool IsPositionInZone(Vector3 position)
    {
        float zoneDistance = (transform.localScale.x) / 2;
        float distance = Vector3.Distance(Vector3.zero, position);

        return distance > zoneDistance;
    }

    public bool IsUnitInZone(Unit unit)
    {
        return _unitsInZone.Contains(unit);
    }
}