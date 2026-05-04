using System.Collections.Generic;
using _Project.Scripts.UI.TweenFeature.TweenActions;
using DG.Tweening;
using UI.TweenActions;
using UnityEngine;

namespace TweenControllers
{
    public class TweenGroup : TweenAction
    {
        [SerializeField] private bool _playParallel = false;
        [SerializeField] private bool _playOnAwake = false;
        [SerializeField] private bool _withoutSequence = false;
        [SerializeField] private bool _loop = false;

        [SerializeField] private List<TweenAction> _tweenActions = new List<TweenAction>();
        [SerializeField] private List<TweenAction> _disposeTweens = new List<TweenAction>();

        private Sequence _activeSequence;
        private Tween _lastTween;

        private void Awake()
        {
            if (_playOnAwake)
                Play();
        }

        public void CollectTweens()
        {
            _tweenActions.Clear();

            var tweens = GetComponents<TweenAction>();
            foreach (var tweenAction in tweens)
            {
                if (tweenAction != this)
                    _tweenActions.Add(tweenAction);
            }
        }

        // -------------------------------
        // Normal / Parallel Sequence Mode
        // -------------------------------
        public override Tween GetTween()
        {
            if (_withoutSequence)
                return CreateLazySequence();

            if (_tweenActions == null || _tweenActions.Count == 0)
                return DOTween.Sequence();

            _activeSequence = DOTween.Sequence();

            foreach (var action in _tweenActions)
            {
                var tween = action.GetTween();
                if (tween == null)
                    continue;

                if (_playParallel)
                    _activeSequence.Join(tween);
                else
                    _activeSequence.Append(tween);
            }

            return _activeSequence;
        }
        
        private Sequence CreateLazySequence()
        {
            var seq = DOTween.Sequence();

            foreach (var action in _tweenActions)
            {
                seq.AppendCallback(() =>
                {
                    var tween = action.GetTween();
                    _lastTween = tween;
                    tween?.Play();
                });

                if (action is MoveSequentiallyToObjectsTween)
                {
                    seq.AppendInterval(0f);
                }
                else
                {
                    seq.AppendInterval(action.GetTween()?.Duration() ?? 0f);
                }
            }

            return seq;
        }
        
        public void Play()
        {
            Dispose();
            _activeSequence = GetTween() as Sequence;
            if (_loop)
                _activeSequence?.SetLoops(-1).Play();
            else
                _activeSequence?.Play();
        }

        public void RemoveTween(TweenAction tweenAction)
        {
            _tweenActions.Remove(tweenAction);
        }

        public override void Dispose()
        {
            foreach (var disposeTween in _disposeTweens)
                disposeTween.GetTween()?.Play();

            _activeSequence?.Kill();
            _lastTween?.Kill();
        }
    }
}
