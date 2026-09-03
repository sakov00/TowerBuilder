using _Project.Scripts.UI.TweenFeature.TweenActions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TweenActions
{
    public class InvokeClickTween : TweenAction
    {
        [SerializeField] private Button _button;

        public override Tween GetTween()
        {
            Sequence seq = DOTween.Sequence();
            seq.AppendCallback(() => _button.onClick.Invoke());
            return seq;
        }

        public void Play()
        {
            _button.onClick.Invoke();
        }
    }
}