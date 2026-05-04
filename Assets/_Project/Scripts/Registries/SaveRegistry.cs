using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Interfaces;
using Unity.VisualScripting;

namespace _Project.Scripts.Registries
{
    public class SaveRegistry
    {
        private readonly HashSet<ISavableController> _savableControllers = new();

        public void Register(ISavableController obj)
        {
            _savableControllers.Add(obj);
        }
        
        public void RegisterRange(List<ISavableController> obj)
        {
            _savableControllers.AddRange(obj);
        }

        public void Unregister(ISavableController obj)
        {
            _savableControllers.Remove(obj);
        }

        public List<T> GetAllByType<T>()
        {
            return _savableControllers.OfType<T>().ToList();
        }
        
        public HashSet<ISavableController> GetAll() => _savableControllers;

        public void Clear()
        {
            _savableControllers.Clear();
        }
    }
}