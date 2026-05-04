using System.Collections.Generic;
using UnityEngine;

namespace UI.SpecialItems.Enable
{
    public class SwitchObjects : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _targets;
        
        private int _index;

        private void OnValidate()
        {
            if (_targets == null || _targets.Count == 0)
            {
                Debug.LogWarning($"{nameof(SwitchObjects)}: Target не найден на {gameObject.name}");
            }
        }

        public void MoveRight()
        {
            _targets[_index].SetActive(false);
            _index++;
            NextIndex();
        }

        public void MoveLeft()
        {
            _targets[_index].SetActive(false);
            _index--;
            NextIndex();
        }
        
        private void NextIndex()
        {
            if (_index >= _targets.Count)
                _index = 0;
            if (_index < 0)
                _index = _targets.Count - 1;
            
            _targets[_index].SetActive(true);
        }
    }
}