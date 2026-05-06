using System.Collections.Generic;
using _Project.Scripts.GameObjects;
using UnityEngine;

namespace _Project.Scripts.SO
{
    [CreateAssetMenu(fileName = "BuildingConfig", menuName = "SO/Building Config")]
    public class BuildingConfig : ScriptableObject
    {
        public List<BuildController> allBuildPrefabs;
        public List<Sprite> allBlockImages;
    }
}