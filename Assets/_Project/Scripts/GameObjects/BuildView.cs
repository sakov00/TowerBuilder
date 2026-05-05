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
    }
}