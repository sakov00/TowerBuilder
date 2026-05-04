using System.Collections.Generic;
using _Project.Scripts.UI.TweenFeature.TweenActions;
using DG.Tweening;
using UnityEngine;

namespace UI.TweenControllers
{
    public class IndependentTweenQueue : TweenAction
    {
        [SerializeField] private List<TweenAction> _tweenActions = new List<TweenAction>();

        private Tween _currentTween;
        private bool _isStopped;

        public override Tween GetTween()
        {
            if (_tweenActions == null || _tweenActions.Count == 0)
                return null;

            _isStopped = false;

            var controller = DOTween.To(() => 0f, _ => { }, 1f, 0f)
                .Pause();

            PlayNext(0, controller);

            return controller;
        }

        private void PlayNext(int index, Tween controller)
        {
            if (_isStopped || index >= _tweenActions.Count)
            {
                controller.Complete();
                return;
            }

            _currentTween = _tweenActions[index]?.GetTween();
            if (_currentTween == null)
            {
                PlayNext(index + 1, controller);
                return;
            }

            _currentTween.OnComplete(() =>
            {
                if (_isStopped)
                    return;

                PlayNext(index + 1, controller);
            });

            _currentTween.Play();
        }
    }
}