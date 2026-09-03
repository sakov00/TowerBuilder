using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.GameObjects;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using UniRx;

namespace _Project.Scripts.Registries
{
    public class MultiplyZonesRegistry
    {
        private readonly List<MultiplyAddScoreZone> _zones = new();

        public void Register(MultiplyAddScoreZone obj)
        {
            if (_zones.Contains(obj)) return;
            _zones.Add(obj);
        }

        public void Unregister(MultiplyAddScoreZone obj)
        {
            if (!_zones.Contains(obj)) return; 
            _zones.Remove(obj);
        }

        public List<MultiplyAddScoreZone> GetAll() => _zones;
        
        public void Clear()
        {
            _zones.Clear();
        }
    }
}