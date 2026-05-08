using System.Collections.Generic; 
using System.Linq;
using _Project.Scripts.Enums;
using _Project.Scripts.Factories;
using _Project.Scripts.GameObjects;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.Pools
{
    public class EffectPool
    {
        [Inject] private EffectFactory _effectFactory;
        
        private Transform _containerTransform;
        private readonly List<EffectController> _availableEffects = new();

        public void SetContainer(Transform transform)
        {
            _containerTransform = transform;
        }
        
        public List<EffectController> GetAvailableEffects() => _availableEffects;
        
        public EffectController Get(EffectType effectType, Transform parent, Vector3 position = default, Quaternion rotation = default) 
        {
            var effect = _availableEffects.FirstOrDefault(c => c.EffectType == effectType);
            if (effect != null)
            {
                _availableEffects.Remove(effect);
                effect.transform.position = position;
                effect.transform.rotation = rotation;
                effect.gameObject.SetActive(true);
            }
            else
            {
                effect = _effectFactory.CreateEffect(effectType, parent, position, rotation);
            }

            effect.transform.SetParent(parent);
            return effect;
        }

        public void Return(EffectController effect)
        {
            if (!_availableEffects.Contains(effect))
            {
                _availableEffects.Add(effect);
            }
            
            effect.gameObject.SetActive(false);
            effect.transform.SetParent(_containerTransform, false); 
        }
        
        public void Remove(EffectController effect)
        {
            if (!_availableEffects.Contains(effect))
            {
                _availableEffects.Remove(effect);
            }
        }
    }
}