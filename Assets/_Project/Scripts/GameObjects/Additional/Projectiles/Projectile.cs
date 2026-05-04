using _Project.Scripts._VContainer;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using _Project.Scripts.Pools;
using _Project.Scripts.Registries;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.GameObjects.Additional.Projectiles
{
    public abstract class Projectile : MonoBehaviour
    {
        [Inject] private ProjectileRegistry _projectileRegistry;
        [Inject] private ProjectilePool _projectilePool;

        [field: SerializeField] public WarSide OwnerWarSide { get; set; }
        [field: SerializeField] public ProjectileType ProjectileType { get; set; }
        [field: SerializeField] public float Damage { get; set; }
        [field: SerializeField] public float PowerAttack { get; set; }
        [field: SerializeField] public float Speed { get; set; } = 10f;
        
        public Vector3 Direction { get; set; }

        private void Start()
        {
            InjectManager.Inject(this);
        }

        private void OnEnable()
        {
            _projectileRegistry.Register(this);
            CancelInvoke(nameof(ReturnToPool));
            Invoke(nameof(ReturnToPool), 5f); // авто-возврат
        }

        public virtual void OnHit(ObjectController target)
        {
            if (target != null && target.WarSide != OwnerWarSide)
            {
                target.TakeDamage(Damage, transform.forward, PowerAttack);
            }

            ReturnToPool();
        }

        protected void ReturnToPool()
        {
            _projectileRegistry.Unregister(this);
            CancelInvoke(nameof(ReturnToPool));
            _projectilePool.Return(this);
        }
    }
}
