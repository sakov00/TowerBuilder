using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.AllAppData;
using _Project.Scripts.UI.TweenFeature.TweenActions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Project.Scripts.UI.TweenFeature
{
    public class HintController : MonoBehaviour
    {
        [Inject] private AppData _appData; 
         
        [SerializeField] private RectTransform _cursor;
        [SerializeField] private Image _cursorImage;
        [SerializeField] private List<TweenAction> _tweenActions = new List<TweenAction>();
        [SerializeField] private float _delayBeforeStart = 4f;
        [SerializeField] private bool _startWithoutDelay = false;

        private Tween _loopTween;
        private TweenAction _currentTweenAction;
        private Coroutine _startRoutine;
        private int _width;
        private int _height;

        private void OnEnable()
        {
            _width = Screen.width;
            _height = Screen.height;
            _currentTweenAction = _tweenActions.FirstOrDefault();
            if (_startWithoutDelay)
                _startRoutine = StartCoroutine(StartSequenceWithMiniDelay());
            else
                _startRoutine = StartCoroutine(StartSequenceWithDelay());
        }

        private void Update()
        {
            if (_width != Screen.width || _height != Screen.height)
            {
                _width = Screen.width;
                _height = Screen.height;
                RestartSequence();
            }
        }
        
        private IEnumerator StartSequenceWithMiniDelay()
        {
            yield return new WaitForSeconds(0.5f);
            StartSequence();
        }

        private IEnumerator StartSequenceWithDelay()
        {
            yield return new WaitForSeconds(_delayBeforeStart);
            StartSequence();
        }

        private void StartSequence()
        {
            if (_cursor == null || _tweenActions == null || _tweenActions.Count == 0)
                return;
            
            // if(_currentTweenAction == null)
            //     _currentTweenAction = _tweenActions[Random.Range(0, _tweenActions.Count)];
            
            _loopTween = _currentTweenAction?.GetTween().SetLoops(-1).Play();
        }

        private void RestartSequence()
        {
            Dispose();

            if (_startRoutine != null)
                StopCoroutine(_startRoutine);

            _startRoutine = StartCoroutine(StartSequenceWithDelay());
        }

        public void Restart()
        {
            RestartSequence();
        }
        
        public void RemoveTween(TweenAction tweenAction)
        {
            if (tweenAction == null)
                return;
            
            if (!_tweenActions.Contains(tweenAction))
                return;

            _tweenActions.Remove(tweenAction);

            // Тут надо тоже проверить, иначе Destroy(null) не нужен
            if (tweenAction.gameObject != null)
                Destroy(tweenAction.gameObject);

            if (_currentTweenAction == tweenAction)
                _currentTweenAction = null;
        }
        
        public void AddTween(TweenAction tweenAction)
        {
            if (tweenAction == null)
                return;

            if (_tweenActions.Contains(tweenAction))
                return;

            _tweenActions.Add(tweenAction);
            _currentTweenAction = null;
        }

        public void SetDelay(float time)
        {
            _delayBeforeStart = time;
        }

        public void SetTweenByIndex(int index)
        {
            _currentTweenAction = _tweenActions[index];
        }

        public void Dispose()
        {
            _currentTweenAction?.Dispose();
            _loopTween?.Kill();
            _loopTween = null;

            if (_startRoutine != null)
            {
                StopCoroutine(_startRoutine);
                _startRoutine = null;
            }

            if (_cursorImage != null)
            {
                var c = _cursorImage.color;
                _cursorImage.color = new Color(c.r, c.g, c.b, 0f);
            }
        }

        private void OnDisable() => Dispose();

        private void OnDestroy() => Dispose();
    }
}