using System;
using System.Collections.Generic;
using _Project.Scripts.GameObjects;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Pools;
using _Project.Scripts.UI.Windows;
using Cysharp.Threading.Tasks;
using VContainer;

namespace _Project.Scripts.Services
{
    public class SceneCreator 
    {
        [Inject] private BuildPool _buildPool;
        [Inject] private WindowsManager _windowsManager;
        
        private static readonly Dictionary<Type, int> TypePriority = new()
        {
            { typeof(BuildModel), 0 },
        };
        
        public async UniTask InstantiateObjects<T>(List<T> objects, bool isInitialize = true) where T : ISavableModel
        {
            // SortSavableModels(objects);
            foreach (var model in objects)
            {
                ISavableController savableController = model switch
                {
                    BuildModel buildModel => 
                        _buildPool.Get(buildModel.BuildType, _windowsManager.GetWindow<GameWindow>().transform, buildModel.SavePosition, buildModel.SaveRotation),
                    _ => null
                };

                if(savableController == null)
                    continue;
                
                savableController.SetSavableModel(model);
                if(isInitialize) savableController.Initialize();
                
                await UniTask.Yield();
            }
        }
        
        public void SortSavableModels<T>(List<T> objects) where T : ISavableModel
        {
            objects.Sort((a, b) =>
            {
                int aPriority = GetPriority(a.GetType());
                int bPriority = GetPriority(b.GetType());
                return aPriority.CompareTo(bPriority);
            });
        }

        private int GetPriority(Type type)
        {
            if (TypePriority.TryGetValue(type, out var priority))
                return priority;

            foreach (var kvp in TypePriority)
            {
                if (kvp.Key.IsAssignableFrom(type))
                    return kvp.Value;
            }

            return int.MaxValue;
        }
    }
}