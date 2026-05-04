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
        [SerializeField] protected LevelConfig _levelConfig;
        [SerializeField] protected UnitPrefabConfig _unitPrefabConfig;
        [SerializeField] protected BuildingPrefabConfig _buildingPrefabConfig;
        [SerializeField] protected ProjectilePrefabConfig _projectilePrefabConfig;
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
            builder.Register<ProjectileRegistry>(Lifetime.Singleton).AsSelf();
        }
        
        private void RegisterPools(IContainerBuilder builder)
        {
            builder.Register<BuildPool>(Lifetime.Singleton).AsSelf();
            builder.Register<UnitPool>(Lifetime.Singleton).AsSelf();
            builder.Register<ProjectilePool>(Lifetime.Singleton).AsSelf();
        }
        
        private void RegisterFactories(IContainerBuilder builder)
        {
            builder.Register<BuildFactory>(Lifetime.Singleton).AsSelf();
            builder.Register<ProjectileFactory>(Lifetime.Singleton).AsSelf();
            builder.Register<UnitFactory>(Lifetime.Singleton).AsSelf();
            builder.Register<EnvironmentFactory>(Lifetime.Singleton).AsSelf();
        }
        
        private void RegisterSO(IContainerBuilder builder)
        {
            builder.RegisterInstance(_levelConfig).AsSelf();
            builder.RegisterInstance(_unitPrefabConfig).AsSelf();
            builder.RegisterInstance(_buildingPrefabConfig).AsSelf();
            builder.RegisterInstance(_projectilePrefabConfig).AsSelf();
            builder.RegisterInstance(_windowsConfig).AsSelf().As<IInitializable>();
        }

        private void RegisterServices(IContainerBuilder builder)
        {
            builder.RegisterInstance(_settingsService).As<IInitializable>().AsSelf();
            builder.RegisterInstance(_poolsManager).AsSelf();
            builder.RegisterInstance(_applicationEventsHandler).AsSelf();
            builder.Register<SaveLoadLevelService>(Lifetime.Singleton).AsSelf();
            builder.Register<SceneCreator>(Lifetime.Singleton).AsSelf();
            
            builder.Register<BlobShadowRotateService>(Lifetime.Singleton).AsSelf();
            builder.Register<DetectionService>(Lifetime.Singleton).AsSelf();
            builder.Register<MoveAllMovablesService>(Lifetime.Singleton).AsSelf();
            builder.Register<PlayerMovementService>(Lifetime.Singleton).AsSelf();
            builder.Register<AttackAllLiveService>(Lifetime.Singleton).AsSelf();
            builder.Register<ProjectileAttackService>(Lifetime.Singleton).AsSelf();
            
            builder.Register<TickScheduler>(Lifetime.Singleton).AsSelf().As<ITickable>();
        }
    }
}