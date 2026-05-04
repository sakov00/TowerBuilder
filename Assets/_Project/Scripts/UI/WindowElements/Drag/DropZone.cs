using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UI.SpecialItems.Drag
{
    public class DropZone : MonoBehaviour
    {
        public ImageStampDrawer _stamptarget;
        private int _countDropped = 0;
        public UnityEvent OnDropped;
        public UnityEvent OnSeveralDropped;
        
        private void OnValidate()
        {
            _stamptarget ??= GetComponent<ImageStampDrawer>();
        }

        public void Reset()
        {
            _countDropped = 0;
        }

        public void OnDrop(Draggable draggable, Vector2 position)
        {
            Destroy(draggable.gameObject);
            if (_stamptarget.isEnabled && _stamptarget.gameObject.activeInHierarchy)
            {
                _stamptarget.HandlePointer(position);
                _countDropped++;
                OnDropped?.Invoke();
            }

            if (_countDropped >= 4)
            {
                OnSeveralDropped?.Invoke();
            }
        }
    }
}