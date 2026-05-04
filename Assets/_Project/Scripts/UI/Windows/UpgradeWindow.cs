using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.Windows
{
    public class UpgradeWindow : BaseWindow
    {
        [Header("UI Elements")]
        [SerializeField] private TMP_Text _loadingText;
        
        [Header("Load Tween Settings")]
        [SerializeField] private string _baseText = "Loading";
        [SerializeField] private float _interval = 0.3f;
        private int _dotsCount = 0;
        
        private Tween _tweenShow;
        private Tween _tweenHide;
        private Tween _loadTween;
        
        private bool _isShowed;

        private void StartLoadingTween()
        {
            _loadTween = DOTween.Sequence()
                .AppendCallback(UpdateText)
                .AppendInterval(_interval)
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
            if(_isShowed == true) return _tweenShow;
            
            _isShowed = true;
            StartLoadingTween();
            _tweenShow = base.Show().OnComplete(() => _tweenShow = null);
            return _tweenShow;
        }
        
        public override Tween Hide()
        {
            if(_isShowed == false) return _tweenHide;
            
            _isShowed = false;
            StopLoadingTween();
            _tweenHide = base.Hide().OnComplete(() => _tweenHide = null);
            return _tweenHide;
        }
        
        public override void ShowFast()
        {
            _tweenShow?.Complete();
            _isShowed = true;
            base.ShowFast();
            StartLoadingTween();
        }

        public override void HideFast()
        {
            _tweenHide?.Complete();
            _isShowed = false;
            base.HideFast();
            StopLoadingTween();
        }
    }
}