using System;
using _Project.Scripts._VContainer;
using _Project.Scripts.Enums;
using DG.Tweening;
using UniRx;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.UI.Windows
{
    public abstract class BaseWindow : MonoBehaviour
    {
        [Inject] protected WindowsManager WindowsManager { get; private set; }
        
        [SerializeField] protected CanvasGroup _canvasGroup;
        
        protected bool IsShowed;
        protected CompositeDisposable Disposables;

        protected Sequence TweenShow;
        protected Sequence TweenHide;
        
        [field:SerializeField] public WindowType WindowType { get; private set; }
        [field: SerializeField] public bool DestroyAfterHide { get; private set; } = true;
        
        protected virtual void Awake()
        {
            InjectManager.Inject(this);
            Disposables = new CompositeDisposable();
        }

        public virtual void Initialize()
        {
        }
        
        public virtual Tween Show()
        {
            if(IsShowed == true) return TweenShow;
            IsShowed = true;
            
            TweenShow = DOTween.Sequence();
            TweenShow.AppendCallback(() => gameObject.SetActive(true));
            TweenShow.Append(_canvasGroup.DOFade(1f, 0.5f).From(0));
            TweenShow.SetUpdate(true);
            TweenShow.OnComplete(() => TweenShow = null);
            return TweenShow;
        }

        public virtual Tween Hide()
        {
            if(IsShowed == false) return TweenHide;
            IsShowed = false;
            
            TweenHide = DOTween.Sequence();
            TweenHide.Append(_canvasGroup.DOFade(0f, 0.5f).From(1));
            TweenHide.AppendCallback(() => gameObject.SetActive(false));
            TweenHide.SetUpdate(true);
            TweenHide.OnComplete(() =>
            {
                TweenHide = null;
                if (DestroyAfterHide) Destroy(gameObject);
            });
            return TweenHide;
        }
        
        public virtual void ShowFast()
        {
            TweenShow?.Complete();
            IsShowed = true;
            gameObject.SetActive(true);
            _canvasGroup.alpha = 1f;
        }

        public virtual void HideFast()
        {
            TweenHide?.Complete();
            IsShowed = false;
            gameObject.SetActive(false);
            _canvasGroup.alpha = 0;
        }
        
        public virtual void Dispose()
        {
            Disposables?.Dispose();
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}