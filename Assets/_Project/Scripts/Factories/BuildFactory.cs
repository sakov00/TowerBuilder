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
        [Inject] private BuildingPrefabConfig _buildingPrefabConfig;
        
        public BuildController CreateBuild(BuildType buildType, Transform parent, Vector3 position = default, Quaternion rotation = default)
        {
            var prefab = _buildingPrefabConfig.allBuildPrefabs
                .FirstOrDefault(p => p.BuildType == buildType);

            return prefab != null ? _resolver.Instantiate(prefab, position, rotation, parent) : null;
        }
    }
}