using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using _Project.Scripts.Services;
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
        [Inject] private GameManager _gameManager;

        [SerializeField] private Button _pauseMenuButton;
        [SerializeField] private RectTransform _spawnPoint;
        [SerializeField] private RectTransform _spawnParent;
        [SerializeField] private RectTransform _gameZone;
        
        public RectTransform SpawnPoint => _spawnPoint;
        public RectTransform SpawnParent => _spawnParent;
        public RectTransform GameZone => _gameZone;
        
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
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}