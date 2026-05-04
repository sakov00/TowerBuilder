using System.Collections.Generic;
using _Project.Scripts.UI.TweenFeature.TweenActions;
using DG.Tweening;
using UnityEngine;

namespace UI.TweenControllers
{
    public class TweenQueue : TweenAction
    {
        [SerializeField] private List<TweenAction> _tweenSequence = new List<TweenAction>();

        public override Tween GetTween()
        {
            if (_tweenSequence == null || _tweenSequence.Count == 0)
                return DOTween.Sequence();

            var sequence = DOTween.Sequence();

            foreach (var action in _tweenSequence)
            {
                var tween = action.GetTween();
                if (tween == null)
                    continue;

                sequence.Append(tween);
            }

            return sequence;
        }
    }
}