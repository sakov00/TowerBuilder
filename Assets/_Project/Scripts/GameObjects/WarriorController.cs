using _Project.Scripts.GameObjects.Abstract.Unit;

namespace _Project.Scripts.GameObjects
{
    public class WarriorController : UnitController<WarriorModel, WarriorView>
    {
        public override void Initialize()
        {
            base.Initialize();
            
            Model.CurrentHealth = Model.MaxHealth;
            View.Initialize();
        }

        public override void Attack()
        {
            CurrentAim?.TakeDamage(Model.DamageAmount, transform.forward, Model.PowerAttack);
        }
    }
}