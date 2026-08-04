using _Project.Scripts._GlobalLogic;
using _Project.Scripts.AllAppData;
using _Project.Scripts.Factories;
using _Project.Scripts.GameObjects;
using _Project.Scripts.Pools;
using _Project.Scripts.Registries;
using _Project.Scripts.Services;
using _Project.Scripts.ServicesGameplay;
using _Project.Scripts.SO;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts._VContainer
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] protected WindowsManager _windowsManager;
        [SerializeField] protected SettingsService _settingsService;
        [SerializeField] protected PoolsManager _poolsManager;
        [SerializeField] protected ApplicationEventsHandler _applicationEventsHandler;
        [SerializeField] protected BlocksContainer _blocksContainer;
        
        [Header("Configs")]
        [SerializeField] protected PlayerConfig _playerConfig;
        [SerializeField] protected ImagesConfig _imagesConfig;
        [SerializeField] protected BuildingConfig _buildingConfig;
        [SerializeField] protected EffectsConfig _effectsConfig;
        [SerializeField] protected CameraConfig _cameraConfig;
        [SerializeField] protected WindowsConfig _windowsConfig;
        [SerializeField] protected SoundConfig _soundConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterBuildCallback(InjectManager.Initialize);
            
            builder.Register<GameTimer>(Lifetime.Singleton).AsSelf().As<ITickable>();
            
            RegisterGame(builder);
            RegisterAppData(builder);
            RegisterWindows(builder);
            RegisterRegistries(builder);
            RegisterPools(builder);
            RegisterFactories(builder);
            RegisterSO(builder);
            RegisterServices(builder);
        }

        protected virtual void RegisterGame(IContainerBuilder builder)
        {
            builder.Register<InitializeGame>(Lifetime.Singleton).AsSelf().As<IInitializable, IAsyncStartable>();
            builder.Register<GameManager>(Lifetime.Singleton).AsSelf();
        }
        
        private void RegisterAppData(IContainerBuilder builder)
        {
            builder.Register<AppData>(Lifetime.Singleton).AsSelf().As<IInitializable>();
        }
        
        private void RegisterWindows(IContainerBuilder builder)
        {
            builder.RegisterInstance(_windowsManager).AsSelf().As<IInitializable>();
        }
        
        private void RegisterRegistries(IContainerBuilder builder)
        {
            builder.Register<LiveRegistry>(Lifetime.Singleton).AsSelf();
            builder.Register<SaveRegistry>(Lifetime.Singleton).AsSelf();
        }
        
        private void RegisterPools(IContainerBuilder builder)
        {
            builder.RegisterComponent(_poolsManager).As<IStartable>().AsSelf();
            builder.Register<BuildPool>(Lifetime.Singleton).AsSelf();
            builder.Register<EffectPool>(Lifetime.Singleton).AsSelf();
        }
        
        private void RegisterFactories(IContainerBuilder builder)
        {
            builder.Register<BuildFactory>(Lifetime.Singleton).AsSelf();
            builder.Register<EffectFactory>(Lifetime.Singleton).AsSelf();
        }
        
        private void RegisterSO(IContainerBuilder builder)
        {
            builder.RegisterInstance(_playerConfig).AsSelf();
            builder.RegisterInstance(_imagesConfig).AsSelf();
            builder.RegisterInstance(_buildingConfig).AsSelf();
            builder.RegisterInstance(_effectsConfig).AsSelf();
            builder.RegisterInstance(_cameraConfig).AsSelf();
            builder.RegisterInstance(_windowsConfig).AsSelf().As<IInitializable>();
            builder.RegisterInstance(_soundConfig).AsSelf();
        }

        private void RegisterServices(IContainerBuilder builder)
        {
            builder.RegisterInstance(_settingsService).AsSelf();
            builder.RegisterInstance(_applicationEventsHandler).AsSelf();
            builder.RegisterInstance(_blocksContainer).AsSelf();
            
            builder.Register<SaveLoadLevelService>(Lifetime.Singleton).AsSelf();
            builder.Register<SceneCreator>(Lifetime.Singleton).AsSelf();
            builder.Register<AdsService>(Lifetime.Singleton).AsSelf();
            builder.Register<AnalyticService>(Lifetime.Singleton).AsSelf();
            builder.Register<LanguageService>(Lifetime.Singleton).AsSelf();
            
            builder.Register<BlockSwingService>(Lifetime.Singleton).AsSelf();
            builder.Register<BlockDropService>(Lifetime.Singleton).AsSelf();
            builder.Register<BlockPlacementService>(Lifetime.Singleton).AsSelf();
            builder.Register<BlockSpawnService>(Lifetime.Singleton).AsSelf();
            builder.Register<TowerSwayService>(Lifetime.Singleton).AsSelf();
            builder.Register<CameraFollowService>(Lifetime.Singleton).AsSelf();
            builder.Register<GameplayFeedbackService>(Lifetime.Singleton).AsSelf();
                
            builder.Register<TickScheduler>(Lifetime.Singleton).AsSelf().As<ITickable>();
        }
    }
}