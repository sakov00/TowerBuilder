using System.Collections.Generic;
using _Project.Scripts.GameObjects;
using UnityEngine;

namespace _Project.Scripts.SO
{
    [CreateAssetMenu(fileName = "ImagesConfig", menuName = "SO/Images Config")]
    public class ImagesConfig : ScriptableObject
    {
        [field:SerializeField] public Sprite HeartFull { get; private set; }
        [field:SerializeField] public Sprite HeartEmpty { get; private set; }
    }
}