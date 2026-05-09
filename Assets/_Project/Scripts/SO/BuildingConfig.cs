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
        
        [field:SerializeField] public float BlockHeight { get; private set; } = 1.5f;
        [field:SerializeField] public float SwingSpeed { get; private set; } = 5f;
        [field:SerializeField] public float SwingTilt { get; private set; } = -10f;
        [field:SerializeField] public float SwingRangeX { get; private set; } = 1.5f;
        [field:SerializeField] public float SwingHeight { get; private set; } = 3f;
        [field:SerializeField] public float OffsetSpawnY { get; private set; } = 2f;
        [field:SerializeField] public float PlacementTolerance { get; private set; } = 0.75f;
        [field:SerializeField] public float PerfectPlacementTolerance { get; private set; } = 0.05f;
        [field:SerializeField] public float NearFailPlacementTolerance { get; private set; } = 0.70f;
        [field:SerializeField] public Vector2 LimitMoveX { get; private set; } = new (-1.5f, 1.5f);
    }
}