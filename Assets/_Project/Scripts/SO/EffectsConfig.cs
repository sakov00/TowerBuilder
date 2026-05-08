using System.Collections.Generic;
using _Project.Scripts.GameObjects;
using UnityEngine;

namespace _Project.Scripts.SO
{
    [CreateAssetMenu(fileName = "EffectsConfig", menuName = "SO/Effects Config")]
    public class EffectsConfig : ScriptableObject
    {
        public List<EffectController> allEffectsPrefabs;
    }
}