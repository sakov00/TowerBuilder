using System.Collections.Generic;
using _Project.Scripts.GameObjects;
using UnityEngine;

namespace _Project.Scripts.SO
{
    [CreateAssetMenu(fileName = "CameraConfig", menuName = "SO/Camera Config")]
    public class CameraConfig : ScriptableObject
    {
        [field:SerializeField] public float SmoothSpeed { get; private set; } = 5f;
        [field:SerializeField] public float OffsetMoveY { get; private set; } = 1.5f;
        [field:SerializeField] public float MinY { get; private set; } = 5f;
        [field:SerializeField] public float MaxY { get; private set; } = 300f;
    }
}