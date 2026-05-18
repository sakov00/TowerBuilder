using System.Collections.Generic;
using _Project.Scripts.Enums;
using _Project.Scripts.Services;
using UnityEngine;

namespace _Project.Scripts.SO
{
    [CreateAssetMenu(fileName = "SoundConfig", menuName = "SO/Sound Config")]
    public class SoundConfig : ScriptableObject
    {
        [Header("Sounds")]
        [field:SerializeField] public List<Sound> MusicClips { get; private set; }
        [field:SerializeField] public List<Sound> SfxClips { get; private set; }
    }
    
    [System.Serializable]
    public class Sound
    {
        public SoundKey key;
        public AudioClip clip;
    }
}