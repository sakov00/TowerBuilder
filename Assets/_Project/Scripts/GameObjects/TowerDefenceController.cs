using _Project.Scripts.GameObjects.Abstract.Build;

namespace _Project.Scripts.GameObjects
{
    public class TowerDefenceController : BuildController<TowerDefenceModel, TowerDefenceView>
    {
        public override void Initialize()
        {
            base.Initialize();
            
            Model.CurrentHealth = Model.MaxHealth;

            // _attackService = new AttackService(this, transform);
            View.Initialize();
        }
        
        public override void Dispose(bool returnToPool = true, bool clearFromRegistry = true)
        {
            base.Dispose(returnToPool, clearFromRegistry);
            Model.AimObject = null;
        }
    }
}