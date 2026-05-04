using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.Windows
{
    public class LoadingWindow : BaseWindow
    {
        [Header("UI Elements")]
        [SerializeField] private TMP_Text _loadingText;
        
        [Header("Load Tween Settings")]
        [SerializeField] private string _baseText = "Loading";
        [SerializeField] private float _interval = 0.3f;
        private int _dotsCount = 0;
        
        private Tween _loadTween;

        private void StartLoadingTween()
        {
            _loadTween = DOTween.Sequence()
                .AppendCallback(UpdateText)
                .AppendInterval(_interval)
                .SetUpdate(true)
                .SetLoops(-1);
        }
        
        private void StopLoadingTween()
        {
            _loadTween.Kill();
        }
        
        private void UpdateText()
        {
            _dotsCount = (_dotsCount + 1) % 4;
            _loadingText.text = _baseText + new string('.', _dotsCount);
        }

        public override Tween Show()
        {
            if(IsShowed == true) return TweenShow;
 
            StartLoadingTween();
            base.Show();
            return TweenShow;
        }
        
        public override Tween Hide()
        {
            if(IsShowed == false) return TweenHide;
            
            StopLoadingTween();
            base.Hide();
            return TweenHide;
        }
        
        public override void ShowFast()
        {
            base.ShowFast();
            StartLoadingTween();
        }

        public override void HideFast()
        {
            base.HideFast();
            StopLoadingTween();
        }
    }
}