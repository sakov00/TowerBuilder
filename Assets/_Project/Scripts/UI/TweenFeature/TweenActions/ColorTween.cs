using _Project.Scripts.UI.TweenFeature.TweenActions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TweenActions
{
    public class ColorTween : TweenAction
    {
        [SerializeField] private Graphic _target;
        [SerializeField] private Color _to;
        [SerializeField] private float _duration = 0.3f;
        [SerializeField] private Ease _ease = Ease.Linear;
        [SerializeField] private bool _yoyo = false;

        private Color _initialColor = Color.white;
        private Tween _tween;

        private void OnValidate()
        {
            if (_target == null)
            {
                _target ??= GetComponent<Graphic>();
                Debug.LogWarning($"{nameof(ColorTween)}: не назначен ни один целевой компонент на {gameObject.name}");
            }
        }
        
        private void Awake()
        {
            if (_target != null)
                _initialColor = _target.color;
        }

        public override Tween GetTween()
        {
            if (_target == null)
            {
                Debug.LogWarning($"{nameof(ColorTween)}: не назначен ни один целевой компонент на {gameObject.name}");
                return DOTween.Sequence();
            }

            _tween?.Kill();

            if (_yoyo)
            {
                var sequence = DOTween.Sequence();
                sequence.Append(_target.DOColor(_to, _duration).SetEase(_ease));
                sequence.Append(_target.DOColor(_initialColor, _duration).SetEase(_ease));
                _tween = sequence;
            }
            else
            {
                _tween = _target.DOColor(_to, _duration).SetEase(_ease);
            }

            return _tween;
        }
    }
}