using System.Collections.Generic;
using _Project.Scripts.GameObjects.Additional.EnemyRoads;
using _Project.Scripts.GameObjects.Additional.LevelEnvironment.Environment;
using _Project.Scripts.GameObjects.Additional.LevelEnvironment.Terrain;
using UnityEngine;

namespace _Project.Scripts.SO
{
    [CreateAssetMenu(fileName = "LevelsConfig", menuName = "SO/Levels Config")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] public TerrainController terrainPrefab;
        [SerializeField] public List<EnvironmentController> environmentPrefabs;
        [SerializeField] public EnemyRoadController roadPrefab;
    }
}