using System;
using _Project.Scripts.UI.Info;
using UnityEngine;

namespace _Project.Scripts.GameObjects.Abstract.BaseObject
{
    [Serializable]
    public abstract class ObjectView
    {
        [SerializeField] protected Transform _transform;
        [SerializeField] protected Renderer _objRenderer;
        [SerializeField] protected Collider _collider;
        [SerializeField] protected Rigidbody2D _rigidbody;

        public bool IsVisible => _objRenderer.isVisible;

        public virtual void Initialize()
        {
            _transform.SetParent(null);
            _transform.gameObject.SetActive(true);
        }
    }
}