using _Project.Scripts.AllAppData;
using _Project.Scripts.UI.Windows;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.ServicesGameplay
{
    public class GameplayFeedbackService
    {
        [Inject] private AppData _appData;
        [Inject] private WindowsManager _windowsManager;

        public void ShowPerfect()
        {
            var gameWindow = _windowsManager.GetWindow<GameWindow>();
            var additionInfo = _appData.LevelData.PerfectMultiplier == 1 ? "" : _appData.LevelData.PerfectMultiplier.ToString() + "x";
            gameWindow.ShowText($"PERFECT! {additionInfo}", Color.yellow);
        }

        public void ShowNearMiss()
        {
            var gameWindow = _windowsManager.GetWindow<GameWindow>();
            var additionInfo = _appData.LevelData.NearFailMultiplier == 1 ? "" : _appData.LevelData.NearFailMultiplier.ToString() + "x";
            gameWindow.ShowText($"LUCKY! {additionInfo}", Color.cyan);
        }
    }
}