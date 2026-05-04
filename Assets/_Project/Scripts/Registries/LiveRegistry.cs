using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using UniRx;

namespace _Project.Scripts.Registries
{
    public class LiveRegistry
    {
        private readonly ReactiveCollection<ObjectController> _liveObjects = new();

        public void Register(ObjectController obj)
        {
            if (_liveObjects.Contains(obj)) return;
            _liveObjects.Add(obj);
        }

        public void Unregister(ObjectController obj)
        {
            if (!_liveObjects.Contains(obj)) return; 
            _liveObjects.Remove(obj);
        }

        public ReactiveCollection<ObjectController> GetAllReactive() => _liveObjects;
        
        public void GetAllByType<T>(List<T> result)
        {
            result.Clear();

            for (int i = 0; i < _liveObjects.Count; i++)
            {
                if (_liveObjects[i] is T t)
                    result.Add(t);
            }
        }

        public void Clear()
        {
            _liveObjects.Clear();
        }

        public IObservable<CollectionAddEvent<ObjectController>> OnAddAsObservable() => _liveObjects.ObserveAdd();
        public IObservable<CollectionRemoveEvent<ObjectController>> OnRemoveAsObservable() => _liveObjects.ObserveRemove();
    }
}