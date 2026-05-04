using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.UI.Windows;
using UnityEngine;
using VContainer.Unity;

namespace _Project.Scripts.SO
{
    [CreateAssetMenu(fileName = "WindowsConfig", menuName = "SO/Windows Config")]
    public class WindowsConfig : ScriptableObject, IInitializable
    {
        [SerializeField] private List<BaseWindow> _windowsList = new();
        
        public Dictionary<Type, BaseWindow> Windows;
        
        public void Initialize()
        {
            Windows = _windowsList.ToDictionary(w => w.GetType(), w => w);
        }
    }
}