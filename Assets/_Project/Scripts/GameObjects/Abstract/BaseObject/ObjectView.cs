using System;
using _Project.Scripts.UI.Info;
using UnityEngine;

namespace _Project.Scripts.GameObjects.Abstract.BaseObject
{
    [Serializable]
    public abstract class ObjectView
    {
        [SerializeField] protected RectTransform _rectTransform;
        [SerializeField] protected Renderer _objRenderer;
        [SerializeField] protected Collider2D _collider;
        [SerializeField] protected Rigidbody2D _rigidbody;
        
        public RectTransform RectTransform => _rectTransform;

        public bool IsVisible => _objRenderer.isVisible;

        public virtual void Initialize()
        {
            _rectTransform.SetParent(null);
            _rectTransform.gameObject.SetActive(true);
        }
    }
}