using System.Linq;
using _Project.Scripts._GlobalLogic;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects;
using _Project.Scripts.GameObjects.Abstract.Unit;
using _Project.Scripts.SO;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Factories
{
    public class UnitFactory
    {
        [Inject] private IObjectResolver _resolver;
        [Inject] private UnitPrefabConfig _unitPrefabConfig;
        
        public UnitController CreateUnit(UnitType type, Vector3 position = default, Quaternion rotation = default)
        {
            var prefab = _unitPrefabConfig.allUnitPrefabs
                .FirstOrDefault(p => p.UnitType == type);

            var newUnit = prefab != null ? _resolver.Instantiate(prefab, position, rotation) : null;
            if (newUnit?.UnitType == UnitType.Player)
            {
                // GlobalObjects.CameraController.Initialize(newUnit.transform);
            }

            return newUnit;
        } 
        
        public FriendsGroupController CreateFriendsGroup(UnitType type, Vector3 position = default, Quaternion rotation = default)
        {
            var prefab = _unitPrefabConfig.allGroupController
                .FirstOrDefault(p => p.Model.UnitType == type);

            return prefab != null ? _resolver.Instantiate(prefab, position, rotation) : null;
        }
    }
}