using _Project.Scripts._GlobalLogic;
using _Project.Scripts.AllAppData;
using _Project.Scripts.Factories;
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
        
        [Header("Configs")]
        [SerializeField] protected BuildingConfig _buildingConfig;
        [SerializeField] protected WindowsConfig _windowsConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterBuildCallback(InjectManager.Initialize);
            
            builder.Register<GameTimer>(Lifetime.Singleton).As<GameTimer, ITickable>();
            
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
            builder.Register<BuildPool>(Lifetime.Singleton).AsSelf();
        }
        
        private void RegisterFactories(IContainerBuilder builder)
        {
            builder.Register<BuildFactory>(Lifetime.Singleton).AsSelf();
        }
        
        private void RegisterSO(IContainerBuilder builder)
        {
            builder.RegisterInstance(_buildingConfig).AsSelf();
            builder.RegisterInstance(_windowsConfig).AsSelf().As<IInitializable>();
        }

        private void RegisterServices(IContainerBuilder builder)
        {
            builder.RegisterInstance(_settingsService).As<IInitializable>().AsSelf();
            builder.RegisterInstance(_poolsManager).AsSelf();
            builder.RegisterInstance(_applicationEventsHandler).AsSelf();
            builder.Register<SaveLoadLevelService>(Lifetime.Singleton).AsSelf();
            builder.Register<SceneCreator>(Lifetime.Singleton).AsSelf();
            
            builder.Register<BlockSwingService>(Lifetime.Singleton).AsSelf();
            builder.Register<BlockDropService>(Lifetime.Singleton).AsSelf();
            builder.Register<BlockPlacementService>(Lifetime.Singleton).AsSelf();
            builder.Register<BlockSpawnService>(Lifetime.Singleton).AsSelf();
            builder.Register<CameraFollowService>(Lifetime.Singleton).AsSelf();
                
            builder.Register<TickScheduler>(Lifetime.Singleton).AsSelf().As<ITickable>();
        }
    }
}