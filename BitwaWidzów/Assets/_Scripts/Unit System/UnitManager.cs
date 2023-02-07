using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class UnitManager : MonoBehaviour
    {
        private List<Unit> _unitList = new List<Unit>();
        private Zone _zone;

        public List<Unit> UnitList => _unitList;

        private void Start()
        {
            _zone = GameManager.Instance.Zone;
        }

        public void AddUnit(Unit unit)
        {
            _unitList.Add(unit);
        }

        private void Update()
        {
            List<Unit> disabledUnits = new List<Unit>();

            foreach (Unit unit in _unitList)
            {
                if (_zone.IsPositionInZone(unit.transform.position))
                {
                    _zone.AddUnitToZone(unit);
                }
                else
                {
                    _zone.RemoveUnitFromZone(unit);
                }

                if (!unit.gameObject.activeSelf)
                {
                    disabledUnits.Add(unit);
                }
            }

            while (disabledUnits.Count > 0)
            {
                _unitList.Remove(disabledUnits[0]);
                disabledUnits.RemoveAt(0);
            }
        }

        public int GetUnitCount()
        {
            return _unitList.Count;
        }
    }
}