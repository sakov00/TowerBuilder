using _Project.Scripts.GameObjects.Abstract.Build;
using Cysharp.Threading.Tasks;
using VContainer;

namespace _Project.Scripts.GameObjects
{
    public class MoneyBuildController : BuildController<MoneyBuildModel, MoneyBuildingView>
    {
        [Inject] private GameManager _gameManager;
        
        public override void Initialize()
        {
            base.Initialize();
            
            Model.CurrentHealth = Model.MaxHealth;
            
            View.Initialize();
            _gameManager.WinEvent += AddMoneyToPlayer;
        }

        private UniTaskVoid AddMoneyToPlayer()
        {
            var moneyAmount = Model.AddMoneyValue;
            AppData.LevelData.LevelMoney += moneyAmount;
            View.MoneyUp(moneyAmount);
            return default;
        }
        
        public override void Dispose(bool returnToPool = true, bool clearFromRegistry = true)
        {
            base.Dispose(returnToPool, clearFromRegistry);
            _gameManager.WinEvent -= AddMoneyToPlayer;
        }
    }
}