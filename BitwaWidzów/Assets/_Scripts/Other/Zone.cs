using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Zone : MonoBehaviour
{
    [SerializeField] private float _zoneDecreaseValue = 10f;
    [SerializeField] private int _zoneDamage = 20;
    [SerializeField] private int _minZoneSize = 250;

    private readonly List<Unit> _unitsInZone = new();

    public int MinZoneSize => _minZoneSize;

    private void Start()
    {
        StartCoroutine(DamageUnits());
    }

    private void Update()
    {
        float decreaseValue = _zoneDecreaseValue * Time.deltaTime;
        transform.localScale -= new Vector3(decreaseValue, 0, decreaseValue);
        if (transform.localScale.x < _minZoneSize || transform.localScale.z < _minZoneSize)
        {
            transform.localScale = new Vector3(_minZoneSize, transform.localScale.y, _minZoneSize);
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

    public bool IsPositionInSafeZone(Vector3 position)
    {
        float zoneDistance = _minZoneSize / 2;
        float distance = Vector3.Distance(Vector3.zero, position);

        return distance < zoneDistance;
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(Vector3.zero, _minZoneSize / 2);
    }
}