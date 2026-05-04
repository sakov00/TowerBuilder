using System.Collections.Generic;
using _Project.Scripts.UI.TweenFeature.TweenActions;
using DG.Tweening;
using UnityEngine;

namespace UI.TweenActions
{
    public class ScaleMultiTween : TweenAction
    {
        public enum Mode
        {
            Parallel,
            SequenceInOut,
            SequenceOutIn
        }

        [Header("Targets")]
        [SerializeField] private List<Transform> _scaleInTargets = new List<Transform>();
        [SerializeField] private List<Transform> _scaleOutTargets = new List<Transform>();

        [Header("Settings")]
        [SerializeField] private bool _useCurrentAsStart = false;
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _delayBetween = 0f;
        [SerializeField] private Ease _ease = Ease.OutBack;
        [SerializeField] private Vector3 _minScale = Vector3.zero;
        [SerializeField] private Vector3 _maxScale = Vector3.one;

        [Header("Mode")]
        [SerializeField] private Mode _mode = Mode.Parallel;

        private void OnValidate()
        {
            if (_scaleInTargets.Count == 0 && _scaleOutTargets.Count == 0)
                Debug.LogWarning($"{nameof(ScaleMultiTween)}: не назначены цели на {gameObject.name}");
        }

        public override Tween GetTween()
        {
            KillAllTweens();

            var sequence = DOTween.Sequence();

            switch (_mode)
            {
                case Mode.Parallel:
                    JoinScaleIn(sequence);
                    JoinScaleOut(sequence);
                    break;

                case Mode.SequenceInOut:
                    sequence.Append(CreateScaleInSequence());
                    sequence.AppendInterval(_delayBetween);
                    sequence.Append(CreateScaleOutSequence());
                    break;

                case Mode.SequenceOutIn:
                    sequence.Append(CreateScaleOutSequence());
                    sequence.AppendInterval(_delayBetween);
                    sequence.Append(CreateScaleInSequence());
                    break;
            }

            return sequence;
        }

        private Sequence CreateScaleInSequence()
        {
            var seq = DOTween.Sequence();
            JoinScaleIn(seq);
            return seq;
        }

        private Sequence CreateScaleOutSequence()
        {
            var seq = DOTween.Sequence();
            JoinScaleOut(seq);
            return seq;
        }

        private void JoinScaleIn(Sequence seq)
        {
            foreach (var t in _scaleInTargets)
            {
                if (t == null) continue;
                var tween = CreateScaleTween(t, true);
                if (tween != null)
                    seq.Join(tween);
            }
        }

        private void JoinScaleOut(Sequence seq)
        {
            foreach (var t in _scaleOutTargets)
            {
                if (t == null) continue;
                var tween = CreateScaleTween(t, false);
                if (tween != null)
                    seq.Join(tween);
            }
        }

        private Tween CreateScaleTween(Transform t, bool scaleIn)
        {
            Vector3 start, end;

            if (_useCurrentAsStart)
                start = t.localScale;
            else
            {
                start = scaleIn ? _minScale : _maxScale;
                t.localScale = start;
            }

            end = scaleIn ? _maxScale : _minScale;

            // 🔹 Проверяем, если уже нужный масштаб — не создаем tween
            if (Vector3.Distance(t.localScale, end) < 0.0001f)
                return null;

            return t.DOScale(end, _duration).SetEase(_ease);
        }

        public void KillAllTweens()
        {
            foreach (var sInTarget in _scaleInTargets)
                sInTarget?.DOKill(complete: true);

            foreach (var sOutTarget in _scaleOutTargets)
                sOutTarget?.DOKill(complete: true);
        }

        private void OnDestroy()
        {
            KillAllTweens();
        }
    }
}
