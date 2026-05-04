using System.Linq;
using System.Threading;
using _Project.Scripts.Enums;
using _Project.Scripts.Interfaces;
using _Project.Scripts.UI.Windows;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts
{
    public class RedactorManager : GameManager
    {
        // public override void Initialize()
        // {
        //     Application.targetFrameRate = 120;
        // }
        //
        // public override async UniTask StartAsync(CancellationToken cancellation = default)
        // {
        //     // WindowsManager.ShowFastWindow<LevelRedactorWindowPresenter>();
        //     // SetSavableData();
        // }
        //
        // public override async UniTask StartLevel(int levelIndex)
        // {
        //     WindowsManager.HideFastWindow<LevelRedactorWindow>();
        //     await base.StartLevel(levelIndex);
        // }
        //
        // public override async UniTask LoadLevel(int levelIndex, bool isInitialize = true)
        // {
        //     SetSavableData();
        //     await ResetService.ResetLevel();
        //     await SaveLoadLevelService.LoadLevelDefault(levelIndex);
        //     await SceneCreator.InstantiateObjects(AppData.LevelData.SavableModels, true);
        // }
        //
        // public void SaveLevel(int levelIndex)
        // {
        //     SetSavableData();
        //     SaveLoadLevelService.SaveLevelDefault(levelIndex).Forget();
        // }
        //
        // private void SetSavableData()
        // {
        //     var savableObjects = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
        //         .OfType<ISavableController>().ToList();
        //     SaveRegistry.Clear();
        //     SaveRegistry.RegisterRange(savableObjects);
        // }
    }
}