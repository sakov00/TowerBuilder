using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using _Project.Scripts.Services;
using _Project.Scripts.ServicesGameplay;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Project.Scripts.UI.Windows
{
    public class GameWindow : BaseWindow
    {
        [Inject] private AppData _appData;
        [Inject] private SettingsService _settingsService;
        [Inject] private BlockDropService _blockDropService;

        [SerializeField] private Button _pauseMenuButton;
        [SerializeField] private Button _clickArea;
        
        protected override void Awake()
        {
            base.Awake();
            
            _pauseMenuButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _settingsService.PlaySfx(SoundKey.ButtonClickSound);
                    WindowsManager.ShowWindow<PauseWindow>();
                })
                .AddTo(Disposables);
            _clickArea.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _settingsService.PlaySfx(SoundKey.ButtonClickSound);
                    _blockDropService.DropBlock();
                })
                .AddTo(Disposables);
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}