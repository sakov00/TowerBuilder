using System;
using _Project.Scripts._VContainer;
using _Project.Scripts.Enums;
using _Project.Scripts.Pools;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.GameObjects
{
    public class EffectController : MonoBehaviour
    {
        [Inject] private EffectPool _effectPool;
        
        [SerializeField] private EffectType _effectType;
        [SerializeField] private ParticleSystem _particleSystem;
        
        public EffectType EffectType => _effectType;

        private void OnValidate()
        {
            _particleSystem ??= GetComponent<ParticleSystem>();
        }
        
        private void OnParticleSystemStopped()
        {
            Debug.Log("Particle system stopped");
            _effectPool.Return(this);
        }
    }
}