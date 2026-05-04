using _Project.Scripts.GameObjects.Abstract.Build;

namespace _Project.Scripts.GameObjects
{
    public class MainBuildController : BuildController<MainBuildModel, MainBuildingView>
    {
        public override void Initialize()
        {
            base.Initialize();
            
            Model.CurrentHealth = Model.MaxHealth;
            View.Initialize();
        }
        
        public override void Dispose(bool returnToPool = true, bool clearFromRegistry = true)
        {
            base.Dispose(returnToPool, clearFromRegistry);
            Model.AimObject = null;
        }
    }
}