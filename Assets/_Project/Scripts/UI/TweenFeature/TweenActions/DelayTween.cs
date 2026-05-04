using _Project.Scripts.UI.TweenFeature.TweenActions;
using DG.Tweening;
using UnityEngine;

namespace UI.TweenActions
{
    public class DelayTween : TweenAction
    {
        [SerializeField] private float _delay = 0.5f;

        public override Tween GetTween()
        {
            return DOVirtual.DelayedCall(_delay, () => { });
        }
    }
}