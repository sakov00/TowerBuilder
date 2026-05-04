using System;
using _Project.Scripts.UI.TweenFeature.TweenActions;
using DG.Tweening;
using UnityEngine;

namespace UI.TweenActions
{
    public class ClickTween : TweenAction
    {
        [SerializeField] private RectTransform _target;
        [SerializeField] private float _duration = 0.15f;
        [SerializeField] private float _scaleFactor = 0.9f;
        [SerializeField] private Ease _ease = Ease.OutQuad;
        [SerializeField] private Vector3 _originalScalePortrait = Vector3.one;
        [SerializeField] private Vector3 _originalScaleLandscape = Vector3.one;
        private Vector3 _originalScale;
        private Tween _currentTween;

        private void OnValidate()
        {
            if (_target == null)
            {
                Debug.LogWarning($"{nameof(ClickTween)}: Target не найден на {gameObject.name}");
            }
        }

        public override Tween GetTween()
        {
            if (_target == null)
            {
                Debug.LogWarning($"{nameof(ClickTween)}: Target не найден на {gameObject.name}");
                return DOTween.Sequence();
            }

            _currentTween?.Kill();

            if (Screen.height > Screen.width)
                _originalScale = _originalScalePortrait;
            if (Screen.width > Screen.height)
                _originalScale = _originalScaleLandscape;
            
            var targetScale = _originalScale * _scaleFactor;

            Sequence sequence = DOTween.Sequence()
                .AppendCallback(() => _target.localScale = _originalScale)
                .Append(_target.DOScale(targetScale, _duration).SetEase(_ease))
                .Append(_target.DOScale(_originalScale, _duration).SetEase(_ease));

            _currentTween = sequence;
            return sequence;
        }
        
        public void Play() => GetTween().Play();
    }
}