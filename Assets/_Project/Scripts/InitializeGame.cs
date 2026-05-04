using System;
using System.Threading;
using _Project.Scripts._VContainer;
using _Project.Scripts.UI.Windows;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts
{
    public class InitializeGame : IInitializable, IAsyncStartable
    {
        [Inject] private WindowsManager _windowsManager;
        
        public void Initialize()
        {
            Application.targetFrameRate = 240;
        }
        
        public async UniTask StartAsync(CancellationToken cancellation = default)
        {
            // _windowsManager.ShowFastWindow<LoadingWindow>();
            // await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellation);
            // _windowsManager.ShowFastWindow<MainMenuWindow>();
            // _windowsManager.HideWindow<LoadingWindow>();
        }
    }
}