using _Project.Scripts.UI.TweenFeature.TweenActions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace UI.TweenActions
{
    public class ActionTween : TweenAction
    {
        [SerializeField] private UnityEvent _action;

        private void OnValidate()
        {
            if (_action == null)
                Debug.LogWarning($"{nameof(ActionTween)}: Action не найден на {gameObject.name}");
        }

        public override Tween GetTween()
        {
            if (_action == null)
            {
                Debug.LogWarning($"{nameof(ActionTween)}: Action не найден на {gameObject.name}");
                return DOTween.Sequence();
            }

            var sequence = DOTween.Sequence();
            sequence.AppendCallback(() => _action?.Invoke());

            return sequence;
        }
    }
}