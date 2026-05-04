using _Project.Scripts._GlobalLogic;
using _Project.Scripts.Factories;
using _Project.Scripts.GameObjects.Abstract.Build;
using _Project.Scripts.GameObjects.Abstract.Unit;
using _Project.Scripts.Pools;
using VContainer;

namespace _Project.Scripts.GameObjects
{
    public class FriendsBuildController : BuildController<FriendsBuildModel, FriendsBuildView>
    {
        [Inject] private GameTimer _gameTimer;
        [Inject] private UnitPool _unitPool;
        [Inject] private UnitFactory _unitFactory;
        
        private FriendsGroupController FriendsGroup { get; set; }

        public override void Initialize()
        {
            base.Initialize();
            
            Model.CurrentHealth = Model.MaxHealth;

            SetFriendsGroup();
            
            View.Initialize();
        }

        private void SetFriendsGroup()
        {
            if (Model.FriendsGroupId == 0)
            {
                // FriendsGroup = _unitFactory.CreateFriendsGroup(Model.UnitType, View.GroupPoint.position);
                // FriendsGroup.InitializeAsync();
                // Model.FriendsGroupId = FriendsGroup.Id;
            }
            else
            {
                // FriendsGroup = (FriendsGroupController)IdsRegistry.Get(Model.FriendsGroupId);
            }
            FriendsGroup.UnitOnKilled += CheckRemovedUnit;
        }

        private void CheckRemovedUnit(UnitController removedUnit)
        {
            Model.NeedRestoreUnitsCount++;

            if (Model.NeedRestoreUnitsCount > 0 && Model.CurrentSpawnTimer < 0)
            {
                Model.CurrentSpawnTimer = 0;
                _gameTimer.Subscribe(1f, HandleSpawnTimer);
            }
        }

        private void HandleSpawnTimer()
        {
            Model.CurrentSpawnTimer++;
            View.UpdateLoadBar(Model.CurrentSpawnTimer, Model.TimeCreateUnits);

            if (Model.CurrentSpawnTimer < Model.TimeCreateUnits) return;

            if (Model.NeedRestoreUnitsCount > 0)
            {
                // var unitController = _unitPool.Get(Model.UnitType, View.SpawnPoint.position);
                // unitController.InitializeAsync();
                // FriendsGroup.Units.Add(unitController);
                // Model.NeedRestoreUnitsCount--;
            }

            if (Model.NeedRestoreUnitsCount > 0)
            {
                Model.CurrentSpawnTimer = 0;
            }
            else
            {
                _gameTimer.Unsubscribe(HandleSpawnTimer);
                Model.CurrentSpawnTimer = -1;
            }
        }
        
        public override void Dispose(bool returnToPool = true, bool clearFromRegistry = true)
        {
            base.Dispose(returnToPool, clearFromRegistry);
            if (returnToPool)
            {
                Model.FriendsGroupId = 0;
            }
            _gameTimer.Unsubscribe(HandleSpawnTimer);
            if(FriendsGroup != null) FriendsGroup.UnitOnKilled -= CheckRemovedUnit;
        }
    }
}