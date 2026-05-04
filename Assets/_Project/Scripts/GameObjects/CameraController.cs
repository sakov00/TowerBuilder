using System.Collections.Generic;
using _Project.Scripts._VContainer;
using _Project.Scripts.Registries;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.GameObjects
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [Inject] private LiveRegistry _liveRegistry;
        
        private void OnValidate()
        {
            CurrentCamera ??= new Camera();
        }

        [SerializeField] public Camera CurrentCamera;
        [SerializeField] private Transform Target;
        [SerializeField] private Vector3 _offset = new(0f, 20f, -12f);
        [SerializeField] private float _followSpeed = 5f;
        [SerializeField] private float _returnSpeed = 30f;
        [SerializeField] private float _limitSecondsReturn = 1f;
        
        private Tween _moveTween;
        private bool _isFollow;

        public void Start()
        {
            InjectManager.Inject(this);
            if (Target == null)
            {
                var players = new List<PlayerController>();
                _liveRegistry.GetAllByType(players);
                Target = players[0].transform;
            }
            transform.position = Target.position + _offset;
            transform.LookAt(Target);
            _isFollow = true;
        }

        public async UniTask EnableFollowAnimation()
        {
            _moveTween?.Kill();
            if (Target == null) return; 
            var desiredPosition = Target.position + _offset;

            var distance = Vector3.Distance(CurrentCamera.transform.position, desiredPosition);
            if (distance < 0.01f) return;

            var duration = Mathf.Min(distance / _returnSpeed, _limitSecondsReturn);

            _moveTween?.Kill();
            _moveTween = CurrentCamera.transform.DOMove(desiredPosition, duration).SetEase(Ease.Linear);
            await _moveTween.Play();
            _isFollow = true;
        }
        
        public void DisableFollowAnimation()
        {
            _moveTween?.Kill();
            _isFollow = false;
        }

        private void LateUpdate()
        {
            if (Target == null) return; 
            if (!_isFollow) return; 
            
            Vector3 desiredPosition = Target.position + _offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, _followSpeed * Time.deltaTime);
            transform.LookAt(Target);
        }
    }
}