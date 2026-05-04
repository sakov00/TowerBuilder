using System;
using _Project.Scripts.UI.TweenFeature.TweenActions;
using DG.Tweening;
using UnityEngine;

namespace UI.TweenActions
{
    public class SizeTween : TweenAction
    {
        [SerializeField] private RectTransform _target;

        [Header("Portrait Size & Position")]
        [SerializeField] private Vector2 _sizePortrait = new Vector2(300f, 300f);

        [Header("Landscape Size & Position")]
        [SerializeField] private Vector2 _sizeLandscape = new Vector2(400f, 200f);

        [Header("Tween Settings")]
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _delay = 0f;
        [SerializeField] private Ease _ease = Ease.OutCubic;
        [SerializeField] private bool _playOnAwake;
        [SerializeField] private bool _checkState;

        private bool _lastIsPortrait;

        private void OnValidate()
        {
            if (_target == null)
                Debug.LogWarning($"{nameof(SizeTween)}: Target не найден на {gameObject.name}");
        }

        private void Awake()
        {
            _lastIsPortrait = Screen.height > Screen.width;

            if (_playOnAwake)
                GetTween().Play();
        }

        private void Update()
        {
            if (!_checkState)
                return;

            bool isPortrait = Screen.height > Screen.width;

            if (isPortrait != _lastIsPortrait)
            {
                _lastIsPortrait = isPortrait;
                GetTween().Play();
            }
        }

        public override Tween GetTween()
        {
            if (_target == null)
            {
                Debug.LogWarning($"{nameof(SizeTween)}: Target не найден на {gameObject.name}");
                return DOTween.Sequence();
            }

            bool isPortrait = Screen.height > Screen.width;

            Vector2 size = isPortrait ? _sizePortrait : _sizeLandscape;

            var seq = DOTween.Sequence();

            seq.Join(_target.DOSizeDelta(size, _duration))
               .SetEase(_ease)
               .SetDelay(_delay);

            return seq;
        }
    }
}
