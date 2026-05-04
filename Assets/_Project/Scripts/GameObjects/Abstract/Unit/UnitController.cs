using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Pools;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.GameObjects.Abstract.Unit
{
    public abstract class UnitController<TModel, TView> : UnitController
        where TModel : UnitModel
        where TView : UnitView 
    {
        protected new TModel Model => (TModel)base.Model;
        protected new TView View => (TView)base.View;
    }
    
    public abstract class UnitController : ObjectController<UnitModel, UnitView>, ISearchController, IMovable, IAttackable, IShadowed
    {
        [Inject] protected UnitPool UnitPool;
        [field:SerializeField] public ObjectController CurrentAim { get; set; }
        [field:SerializeField] public ObjectController DefaultAim { get; set; }

        public UnitType UnitType => Model.UnitType;
        public Vector3 Position => transform.position;
        public float DetectionRadius => Model.DetectionRadius;
        public bool IsMoving => View.IsMoving;
        public float AttackRange => Model.AttackRange;
        public void MoveTo(Vector3 point) => View.MoveTo(point);
        public void Stop() => View.Stop();
        public void SetAttacking(bool isAttacking) => View.SetAttacking(isAttacking);
        public Vector3 AttackPoint() => CurrentAim.GetOwnAttackPoint(transform.position);
        public Transform ShadowTransform => View.ShadowTransform;

        public abstract void Attack();

        public override async UniTask Killed(Vector3 forceDirection = default, float forceAmount = 0f)
        {
            if (IsVisible)
            {
                Dispose(false, true);
                ShadowTransform.gameObject.SetActive(false);
                View.RagdollIsActive(true, forceDirection, forceAmount);
                await UniTask.Delay(2000);
                Dispose();
            }
            else
            {
                Dispose();
            }
        }

        public override void Dispose(bool returnToPool = true, bool clearFromRegistry = true)
        {
            if (returnToPool)
            {
                UnitPool.Return(this);
                CurrentAim = null;
            }
            if (clearFromRegistry)
            {
                LiveRegistry.Unregister(this);
                SaveRegistry.Unregister(this);
            }
        }
    }
}