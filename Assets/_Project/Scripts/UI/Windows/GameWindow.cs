using System.Linq;
using _Project.Scripts._GlobalLogic;
using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Additional.EnemyRoads;
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
        [SerializeField] private Button _fightButton;
        [SerializeField] private Button _modeButton;
        [SerializeField] private TMP_Text _modeText;
        [SerializeField] private Image _modeKingIcon;
        [SerializeField] private Image _modeMapIcon;

        [Header("UI Text")]
        [SerializeField] private TMP_Text _moneyText;
        [SerializeField] private TMP_Text _currentRoundText;

        [Header("Controls")]
        [SerializeField] private Joystick _joystick;
        [SerializeField] private TouchAndMouseDragInput _touchAndMouseDragInput;

        protected override void Awake()
        {
            base.Awake();
            
            _appData.LevelData.IsStrategyModeReactive
                .Subscribe(async isStrategy =>
                {
                    if (isStrategy)
                    {
                        _touchAndMouseDragInput.gameObject.SetActive(true);
                        _joystick.gameObject.SetActive(false);
                        // GlobalObjects.Camera.DisableFollowAnimation();
                    }
                    else
                    {
                        _touchAndMouseDragInput.gameObject.SetActive(false);
                        // await GlobalObjects.Camera.EnableFollowAnimation();
                        _joystick.gameObject.SetActive(true);
                    }
                })
                .AddTo(Disposables);
            
            _pauseMenuButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _settingsService.PlaySfx(SoundKey.ButtonClickSound);
                    WindowsManager.ShowWindow<PauseWindow>();
                })
                .AddTo(Disposables);

            _fightButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _settingsService.PlaySfx(SoundKey.ButtonClickSound);
                    _settingsService.PlayMusicAsync(SoundKey.BattleMusic).Forget();
                })
                .AddTo(Disposables);

            _modeButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _settingsService.PlaySfx(SoundKey.ButtonClickSound);
                })
                .AddTo(Disposables);
            
            // _appData.LevelData.IsFightingReactive
            //     .Subscribe(roundActive => _nextRoundButton.gameObject.SetActive(!roundActive))
            //     .AddTo(Disposables);
            
            _appData.LevelData.LevelMoneyReactive
                .Subscribe(money => _moneyText.text = money.ToString())
                .AddTo(Disposables);
            
            _appData.LevelData.CurrentRoundReactive
                .Subscribe(roundIndex => _currentRoundText.text = $"Round {roundIndex + 1}")
                .AddTo(Disposables);
        }

        public override void Initialize()
        {
            base.Initialize();
            _appData.LevelData.IsStrategyMode = false;
        }
    }
}