using System.Collections.Generic;
using _Project.Scripts.GameObjects;
using UnityEngine;

namespace _Project.Scripts.SO
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "SO/Player Config")]
    public class PlayerConfig : ScriptableObject
    {
        [field:SerializeField] public int MaxHealth { get; private set; } = 3;
    }
}