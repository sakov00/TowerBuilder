using System.Linq;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects;
using _Project.Scripts.SO;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Factories
{
    public class BuildFactory
    {
        [Inject] private IObjectResolver _resolver;
        [Inject] private BuildingConfig _buildingConfig;
        
        public BuildController CreateBuild(BuildType buildType, Transform parent, Vector3 position = default, Quaternion rotation = default)
        {
            var prefab = _buildingConfig.allBuildPrefabs
                .FirstOrDefault(p => p.Model.BuildType == buildType);
            
            var obj = prefab != null ? Object.Instantiate(prefab, position, rotation, parent) : null;
            _resolver.Inject(obj);

            return obj;
        }
    }
}