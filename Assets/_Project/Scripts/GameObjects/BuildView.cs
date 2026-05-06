using System;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using UnityEngine;

namespace _Project.Scripts.GameObjects
{
    [Serializable]
    public class BuildView : ObjectView
    {
        public override void Initialize()
        { 
            base.Initialize();
        }
        
        public void SetKinematicState(RigidbodyType2D state)
        {
            if (_rigidbody != null)
                _rigidbody.bodyType = state;
        }
        
        public void MoveRigidbody(Vector3 targetPos)
        {
            if (_rigidbody != null)
                _rigidbody.MovePosition(targetPos);
        }
        
        public void SetImage(Sprite sprite)
        {
            if (_spriteRenderer != null)
                _spriteRenderer.sprite = sprite;
        }
    }
}