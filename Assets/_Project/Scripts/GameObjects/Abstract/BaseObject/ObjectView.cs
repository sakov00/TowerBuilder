using _Project.Scripts.UI.Info;
using UnityEngine;

namespace _Project.Scripts.GameObjects.Abstract.BaseObject
{
    public abstract class ObjectView
    {
        [SerializeField] protected Transform _transform;
        [SerializeField] protected Renderer _objRenderer;
        [SerializeField] protected Collider _collider;
        [SerializeField] protected UniversalBar _healthBar;
        [SerializeField] protected Outline _outline;

        public bool IsVisible => _objRenderer.isVisible;

        public virtual void Initialize()
        {
            _transform.SetParent(null);
            _transform.gameObject.SetActive(true);
        }

        public void UpdateHealthBar(float currentHealth, float maxHealth)
        {
            _healthBar?.UpdateBar(currentHealth, maxHealth);
        }

        public Vector3 GetAttackPoint(Vector3 fromPosition) => _collider.ClosestPoint(fromPosition);
        
        public void EnableOutline(bool enable)
        {
            if (_outline != null)
                _outline.enabled = enable;
        }
    }
}