using _Project.Scripts.GameObjects.Abstract.Unit;

namespace _Project.Scripts.GameObjects
{
    public class FlyingController : UnitController<FlyingModel, FlyingView>
    {
        public override void Initialize()
        {
            base.Initialize();
            
            Model.CurrentHealth = Model.MaxHealth;
            View.Initialize();
        }

        public override void Attack()
        {
            
        }

        public override void Dispose(bool returnToPool = true, bool clearFromRegistry = true)
        {
            base.Dispose(returnToPool, clearFromRegistry);
        }
    }
}