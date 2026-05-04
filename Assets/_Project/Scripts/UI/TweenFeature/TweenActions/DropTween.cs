using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.UI.TweenFeature.TweenActions
{
    public class DropTween : MonoBehaviour
    {
        [SerializeField] private RectTransform _target;
        
        // --- Параметры, которые будут обновляться через ApplyData ---
        [Header("Конвейер (Горизонталь)")]
        [SerializeField] public float _conveyorDuration = 1.0f;
        [SerializeField] public float _conveyorDistanceX = 300f;
        [SerializeField] public Ease _conveyorEase = Ease.Linear;
        
        [Header("Наклонное Движение (Под Углом)")]
        [SerializeField] public float _angleMoveDuration = 0.5f;
        [SerializeField] public float _angleMoveDistance = 100f; 
        [SerializeField] public float _angleMoveDegrees = -45f;
        [SerializeField] public Ease _angleMoveEase = Ease.OutQuad;

        [Header("Падение (Вертикаль)")]
        [SerializeField] public float _fallDuration = 2.0f;
        [SerializeField] public float _fallDistanceY = -1200f;
        [SerializeField] public Ease _fallEase = Ease.InExpo;
        
        [SerializeField] public UnityEvent _endAction;
        
        private Tween _currentTween;

        private void Awake()
        {
            if (_target == null)
                _target = GetComponent<RectTransform>();
        }
        
        // Удалите метод Start(), так как запуск будет контролироваться спавнером через ApplyData

        // --- НОВЫЙ МЕТОД: ПРИМЕНЕНИЕ ДАННЫХ И ПЕРЕЗАПУСК ---
        public void ApplyData(RectTransform target, DropTween data, Action endAction)
        {
            // 1. Обновляем параметры текущего компонента
            _target = target;
            _conveyorDuration = data._conveyorDuration;
            _conveyorDistanceX = data._conveyorDistanceX;
            _angleMoveDuration = data._angleMoveDuration;
            _angleMoveDistance = data._angleMoveDistance;
            _angleMoveDegrees = data._angleMoveDegrees;
            _fallDuration = data._fallDuration;
            _fallDistanceY = data._fallDistanceY;
            
            _endAction.RemoveAllListeners();
            _endAction.AddListener(() => endAction?.Invoke());
        }

        public Tween GetTween()
        {
            if (_target == null)
            {
                Debug.LogError($"{nameof(DropTween)}: Target (фрукт) не назначен!");
                return DOTween.Sequence();
            }

            _currentTween?.Kill(); // Обязательно убиваем предыдущую анимацию

            Sequence sequence = DOTween.Sequence();
            
            Vector2 currentPos = _target.anchoredPosition;
            
            // ... (Далее ваша существующая логика Sequence, использующая обновленные поля) ...
            
            // 1. Конвейер
            Vector2 targetConveyorPos = currentPos + new Vector2(_conveyorDistanceX, 0);
            sequence.Append(
                _target.DOAnchorPos(targetConveyorPos, _conveyorDuration)
                    .SetEase(_conveyorEase)
                    .SetRelative(false)
            );
            
            // 2. Наклонное Движение
            float angleRadians = _angleMoveDegrees * Mathf.Deg2Rad;
            float deltaX = _angleMoveDistance * Mathf.Cos(angleRadians);
            float deltaY = _angleMoveDistance * Mathf.Sin(angleRadians);

            Vector2 offsetAngle = new Vector2(deltaX, deltaY);
            Vector2 targetAnglePos = targetConveyorPos + offsetAngle;

            sequence.Append(
                _target.DOAnchorPos(targetAnglePos, _angleMoveDuration)
                    .SetEase(_angleMoveEase)
                    .SetRelative(false)
            );
            
            // 3. Падение
            Vector2 targetFallPos = targetAnglePos + new Vector2(0, _fallDistanceY);
            
            sequence.Append(
                _target.DOAnchorPos(targetFallPos, _fallDuration)
                    .SetEase(_fallEase)
                    .SetRelative(false)
            );

            // 4. Завершение: Возврат в пул
            sequence.OnComplete(() =>
            {
                _endAction?.Invoke();
            });

            _currentTween = sequence;
            return sequence;
        }
        
        public void Kill() => _currentTween?.Kill();
        
        public void Play() => GetTween().Play();
    }
}