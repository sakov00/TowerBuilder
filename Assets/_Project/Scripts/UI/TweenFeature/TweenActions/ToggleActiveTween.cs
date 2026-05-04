using _Project.Scripts.UI.TweenFeature.TweenActions;
using DG.Tweening;
using UnityEngine;

namespace TweenActions
{
    public class ToggleActiveTween : TweenAction
    {
        [SerializeField] private GameObject _target;
        [SerializeField] private bool _setActive = true;

        private void OnValidate()
        {
            if (_target == null)
            {
                Debug.LogWarning($"{nameof(ToggleActiveTween)}: Target не найден на {gameObject.name}");
            }
        }
        
        public override Tween GetTween()
        {
            if (_target == null)
            {
                Debug.LogWarning($"{nameof(ToggleActiveTween)}: Target не найден на {gameObject.name}");
                return DOTween.Sequence();
            }
            
            var tween = DOTween.Sequence();
            tween.AppendCallback(() =>
            {
                _target.SetActive(_setActive);
                Debug.Log(_target.name + _setActive);
            });

            return tween;
        }
    }
}