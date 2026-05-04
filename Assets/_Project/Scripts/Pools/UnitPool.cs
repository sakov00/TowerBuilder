using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Enums;
using _Project.Scripts.Factories;
using _Project.Scripts.GameObjects.Abstract.Unit;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.Pools
{
    public class UnitPool
    {
        [Inject] private UnitFactory _unitFactory;

        private Transform _containerTransform;
        private readonly List<UnitController> _availableUnits = new();

        public void SetContainer(Transform transform)
        {
            _containerTransform = transform;
        }
        
        public List<UnitController> GetAvailableUnits() => _availableUnits;
        
        public UnitController Get(UnitType unitType, Vector3 position = default, Quaternion rotation = default) 
        {
            var unit = _availableUnits.FirstOrDefault(c => c.UnitType == unitType);
            if (unit != null)
            {
                _availableUnits.Remove(unit);
                unit.transform.position = position;
                unit.transform.rotation = rotation;
                unit.gameObject.SetActive(true);
            }
            else
            {
                unit = _unitFactory.CreateUnit(unitType, position, rotation);
            }
            unit.transform.SetParent(null);
            return unit;
        }

        public void Return(UnitController unit)
        {
            if (!_availableUnits.Contains(unit))
            {
                _availableUnits.Add(unit);
            }

            unit.gameObject.SetActive(false);
            unit.transform.SetParent(_containerTransform, false);
        }
        
        public void Remove(UnitController unit)
        {
            if (!_availableUnits.Contains(unit))
            {
                _availableUnits.Remove(unit);
            }
        }
    }
}
