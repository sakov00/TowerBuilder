using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.UI.TweenFeature.TweenActions
{
    public class MoveRandomInAreaTween : TweenAction
    {
        [SerializeField] private RectTransform _target;
        [SerializeField] private RectTransform _moveArea;

        [Header("Movement")]
        [SerializeField] private float _minDuration = 0.8f;
        [SerializeField] private float _maxDuration = 1.5f;
        [SerializeField] private float _delayBetweenMoves = 0f;
        [SerializeField] private Ease _ease = Ease.InOutSine;

        [Header("Loop")]
        [SerializeField] private bool _loop = true;

        [SerializeField] private bool _playOnAwake;
        [SerializeField] private bool _checkState;

        private int _width;
        private int _height;
        private Sequence _sequence;

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
                GetTween().Restart();
            }
        }

        public override Tween GetTween()
        {
            if (_target == null || _moveArea == null)
                return DOTween.Sequence();

            _sequence = DOTween.Sequence();

            void AppendMove()
            {
                Vector3 worldPos = GetRandomWorldPointInArea();
                float duration = Random.Range(_minDuration, _maxDuration);

                _sequence.Append(
                    _target
                        .DOMove(worldPos, duration)
                        .SetEase(_ease)
                );

                if (_delayBetweenMoves > 0)
                    _sequence.AppendInterval(_delayBetweenMoves);
            }

            AppendMove();

            if (_loop)
            {
                _sequence.OnComplete(() =>
                {
                    _sequence.Kill();
                    GetTween().Play();
                });
            }

            return _sequence;
        }

        private Vector3 GetRandomWorldPointInArea()
        {
            Rect rect = _moveArea.rect;

            Vector2 localPoint = new Vector2(
                Random.Range(rect.xMin, rect.xMax),
                Random.Range(rect.yMin, rect.yMax)
            );

            return _moveArea.TransformPoint(localPoint);
        }

        public override void Dispose()
        {
            _loop = false;
            _target.DOKill();
            _sequence.Kill();
        }
    }
}
