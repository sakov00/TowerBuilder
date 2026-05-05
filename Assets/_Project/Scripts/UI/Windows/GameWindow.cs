using System.Linq;
using _Project.Scripts._GlobalLogic;
using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using _Project.Scripts.Registries;
using _Project.Scripts.Services;
using _Project.Scripts.UI.WindowElements;
using Cysharp.Threading.Tasks;
using TMPro;
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

        [Header("Buttons")]
        [SerializeField] private Button _pauseMenuButton;

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