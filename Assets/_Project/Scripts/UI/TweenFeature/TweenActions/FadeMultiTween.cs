using System.Collections.Generic;
using _Project.Scripts.UI.TweenFeature.TweenActions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TweenActions
{
    public class FadeMultiTween : TweenAction
    {
        public enum Mode
        {
            Parallel,
            SequenceInOut,
            SequenceOutIn
        }

        [Header("Targets")]
        [SerializeField] private List<CanvasGroup> _fadeInCanvasGroups = new List<CanvasGroup>();
        [SerializeField] private List<Image> _fadeInImages = new List<Image>();

        [Space(5)]
        [SerializeField] private List<CanvasGroup> _fadeOutCanvasGroups = new List<CanvasGroup>();
        [SerializeField] private List<Image> _fadeOutImages = new List<Image>();

        [Header("Settings")]
        [SerializeField] private bool _useCurrentAsStart = false;
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _initialDelay = 0f;
        [SerializeField] private float _delayBetween = 0f;
        [SerializeField] private Ease _ease = Ease.Linear;
        [SerializeField] private bool _controlRaycast = true;

        [Header("Mode")]
        [SerializeField] private Mode _mode = Mode.Parallel;

        private void OnValidate()
        {
            if (_fadeInCanvasGroups.Count == 0 && _fadeInImages.Count == 0 &&
                _fadeOutCanvasGroups.Count == 0 && _fadeOutImages.Count == 0)
            {
                Debug.LogWarning($"{nameof(FadeMultiTween)}: не назначены цели на {gameObject.name}");
            }
        }

        public override Tween GetTween()
        {
            var sequence = DOTween.Sequence();

            switch (_mode)
            {
                case Mode.Parallel:
                    JoinFadeIn(sequence);
                    JoinFadeOut(sequence);
                    break;

                case Mode.SequenceInOut:
                    sequence.Append(CreateFadeInSequence());
                    sequence.AppendInterval(_delayBetween);
                    sequence.Append(CreateFadeOutSequence());
                    break;

                case Mode.SequenceOutIn:
                    sequence.Append(CreateFadeOutSequence());
                    sequence.AppendInterval(_delayBetween);
                    sequence.Append(CreateFadeInSequence());
                    break;
            }
            
            if (_initialDelay > 0f)
                return sequence.SetDelay(_initialDelay);

            return sequence;
        }

        public void PlayTween()
        {
            GetTween().Play();
        }

        private Sequence CreateFadeInSequence()
        {
            var seq = DOTween.Sequence();
            JoinFadeIn(seq);
            return seq;
        }

        private Sequence CreateFadeOutSequence()
        {
            var seq = DOTween.Sequence();
            JoinFadeOut(seq);
            return seq;
        }

        private void JoinFadeIn(Sequence seq)
        {
            foreach (var cg in _fadeInCanvasGroups)
            {
                if (cg == null) continue;
                var tween = CreateFadeTween(cg, true);
                if (tween != null) seq.Join(tween);
            }

            foreach (var img in _fadeInImages)
            {
                if (img == null) continue;
                var tween = CreateFadeTween(img, true);
                if (tween != null) seq.Join(tween);
            }
        }

        private void JoinFadeOut(Sequence seq)
        {
            foreach (var cg in _fadeOutCanvasGroups)
            {
                if (cg == null) continue;
                var tween = CreateFadeTween(cg, false);
                if (tween != null) seq.Join(tween);
            }

            foreach (var img in _fadeOutImages)
            {
                if (img == null) continue;
                var tween = CreateFadeTween(img, false);
                if (tween != null) seq.Join(tween);
            }
        }

        private Tween CreateFadeTween(CanvasGroup cg, bool fadeIn)
        {
            float start, end;

            if (_useCurrentAsStart)
                start = cg.alpha;
            else
            {
                start = fadeIn ? 0f : 1f;
            }

            end = fadeIn ? 1f : 0f;
            
            var seq = DOTween.Sequence();
            seq.AppendCallback(() => cg.alpha = start);
            seq.Append(cg.DOFade(end, _duration));
            var t = seq.SetEase(_ease);

            if (_controlRaycast)
            {
                t.OnStart(() => { if (!fadeIn) cg.blocksRaycasts = false; });
                t.OnComplete(() => { if (fadeIn) cg.blocksRaycasts = true; });
            }

            return t;
        }

        private Tween CreateFadeTween(Image img, bool fadeIn)
        {
            float start, end;

            if (_useCurrentAsStart)
                start = img.color.a;
            else
            {
                start = fadeIn ? 0f : 1f;
            }

            end = fadeIn ? 1f : 0f;

            var seq = DOTween.Sequence();
            seq.AppendCallback(() => SetImageAlpha(img, start));
            seq.Append(img.DOFade(end, _duration));
            var t = seq.SetEase(_ease);

            if (_controlRaycast)
            {
                t.OnStart(() => { if (!fadeIn) img.raycastTarget = false; });
                t.OnComplete(() => { if (fadeIn) img.raycastTarget = true; });
            }

            return t;
        }

        private void SetImageAlpha(Image img, float value)
        {
            var color = img.color;
            color.a = value;
            img.color = color;
        }
    }
}
