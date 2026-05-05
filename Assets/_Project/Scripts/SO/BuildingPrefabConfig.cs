using System.Collections.Generic;
using _Project.Scripts.GameObjects;
using UnityEngine;

namespace _Project.Scripts.SO
{
    [CreateAssetMenu(fileName = "BuildingPrefabConfig", menuName = "SO/Building Prefab Config")]
    public class BuildingPrefabConfig : ScriptableObject
    {
        public List<BuildController> allBuildPrefabs;
    }
}