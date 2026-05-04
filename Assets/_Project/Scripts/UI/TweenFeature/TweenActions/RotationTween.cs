using _Project.Scripts.UI.TweenFeature.TweenActions;
using DG.Tweening;
using UnityEngine;

namespace UI.TweenActions
{
    public class RotationTween : TweenAction
    {
        [SerializeField] private Transform _target;

        [Header("Portrait")]
        [SerializeField] private Vector3 _rotationDeltaPortrait = Vector3.zero;
        [SerializeField] private Vector3 _minRotationPortrait = Vector3.zero;
        [SerializeField] private Vector3 _maxRotationPortrait = Vector3.zero;

        [Header("Landscape")]
        [SerializeField] private Vector3 _rotationDeltaLandscape = Vector3.zero;
        [SerializeField] private Vector3 _minRotationLandscape = Vector3.zero;
        [SerializeField] private Vector3 _maxRotationLandscape = Vector3.zero;

        [Header("Tween Settings")]
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _delay = 0f;
        [SerializeField] private Ease _ease = Ease.Linear;
        [SerializeField] private bool _playOnAwake;
        [SerializeField] private bool _checkState;

        private int _width;
        private int _height;

        private void OnValidate()
        {
            if (_target == null)
                Debug.LogWarning($"{nameof(RotationTween)}: Target не найден на {gameObject.name}");
        }

        private void Awake()
        {
            _width = Screen.width;
            _height = Screen.height;

            if (_playOnAwake)
                GetTween().Play();
        }

        private void Update()
        {
            if (_checkState && (_width != Screen.width || _height != Screen.height))
            {
                _width = Screen.width;
                _height = Screen.height;
                GetTween().Play();
            }
        }

        public override Tween GetTween()
        {
            if (_target == null)
            {
                Debug.LogWarning($"{nameof(RotationTween)}: Target не найден на {gameObject.name}");
                return DOTween.Sequence();
            }

            bool isPortrait = Screen.height >= Screen.width;

            Vector3 delta = isPortrait ? _rotationDeltaPortrait : _rotationDeltaLandscape;
            Vector3 min = isPortrait ? _minRotationPortrait : _minRotationLandscape;
            Vector3 max = isPortrait ? _maxRotationPortrait : _maxRotationLandscape;

            // Рассчитываем новый масштаб с учётом текущего
            Vector3 newRotation = _target.localRotation.eulerAngles + delta;

            newRotation.x = Mathf.Clamp(newRotation.x, min.x, max.x);
            newRotation.y = Mathf.Clamp(newRotation.y, min.y, max.y);
            newRotation.z = Mathf.Clamp(newRotation.z, min.z, max.z);

            return _target
                .DORotate(newRotation, _duration)
                .SetEase(_ease)
                .SetDelay(_delay);
        }
    }
}
