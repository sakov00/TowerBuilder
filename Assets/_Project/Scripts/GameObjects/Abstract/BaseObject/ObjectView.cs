using System;
using _Project.Scripts.UI.Info;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.GameObjects.Abstract.BaseObject
{
    [Serializable]
    public abstract class ObjectView
    {
        [SerializeField] protected Transform _transform;
        [SerializeField] protected Collider2D _collider;
        [SerializeField] protected Rigidbody2D _rigidbody;
        [SerializeField] protected SpriteRenderer _spriteRenderer;
        
        public Transform Transform => _transform;

        public bool IsVisible => _spriteRenderer.isVisible;

        public virtual void Initialize()
        {
        }
    }
}