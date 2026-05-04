using System.Collections.Generic;
using _Project.Scripts.GameObjects;
using _Project.Scripts.GameObjects.Abstract.Build;
using UnityEngine;

namespace _Project.Scripts.SO
{
    [CreateAssetMenu(fileName = "BuildingPrefabConfig", menuName = "SO/Building Prefab Config")]
    public class BuildingPrefabConfig : ScriptableObject
    {
        public BuildingZone buildZonePrefab;
        public List<BuildController> allBuildPrefabs;
    }
}