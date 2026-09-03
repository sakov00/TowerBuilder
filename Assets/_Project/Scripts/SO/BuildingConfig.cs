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
        
        [field: Header("SwingData")]
        [field:SerializeField] public float SwingSpeed { get; private set; } = 5f;
        [field:SerializeField] public float SwingTilt { get; private set; } = -10f;
        [field:SerializeField] public Vector2 SwingRange { get; private set; } = new (2f, 1.5f);
        [field:SerializeField] public float SwingHeight { get; private set; } = 3f;
        
        [field: Header("PlaceData")]
        [field:SerializeField] public float PlacementTolerance { get; private set; } = 0.75f;
        [field:SerializeField] public float PerfectPlacementTolerance { get; private set; } = 0.05f;
        [field:SerializeField] public float NearFailPlacementTolerance { get; private set; } = 0.70f;
        
        [field: Header("SwayData")]
        [field:SerializeField] public float DestroyImbalance { get; private set; } = 10f;
        [field:SerializeField] public float DestroyImbalanceYellow { get; private set; } = 7f;
        [field:SerializeField] public float DestroyImbalanceRed { get; private set; } = 3f;
        [field:SerializeField] public float SwaySpeed { get; private set; } = 1f;
        [field:SerializeField] public float SwaySensitivityImbalance { get; private set; } = 1.1f;
        [field:SerializeField] public Vector2 SwayAmplitude { get; private set; } = new (-1, 1);
        [field:SerializeField] public int StartSwayFrom { get; private set; } = 10;
        [field:SerializeField] public int MaxSwayFrom { get; private set; } = 20;
        
        [field: Header("BlockData")]
        [field:SerializeField] public float BlockHeight { get; private set; } = 1.5f;
        
        [field: Header("Tutorial")]
        [field:SerializeField] public int TutorialBlocksCount { get; private set; } = 5;
    }
}