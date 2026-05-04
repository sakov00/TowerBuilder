using _Project.Scripts._GlobalLogic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI.SpecialItems.Drag
{
    [RequireComponent(typeof(RectTransform))]
    public class Draggable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] public RectTransform _rectTransform;

        [SerializeField] private bool _checkZone = false;
        [SerializeField] private bool _isDraggable = true;
        public bool IsDraggable
        {
            get => _isDraggable;
            set
            {
                _isDraggable = value;
                if (!_isDraggable)
                    _isDragging = false;
            }
        }

        public Vector2 StartAnchorMin { get; private set; }
        public Vector2 StartAnchorMax { get; private set; }
        public Vector2 StartSize { get; private set; }
        
        public Transform StartParent { get; private set; }
        public int SiblingNumber { get; private set; }
        public Vector3 StartAnchoredPosition { get; private set; }

        private readonly Vector3 _offset = new Vector3(0, 50, 0);
        private bool _isDragging;

        public DraggableEvent OnPointerDowned;
        public DraggableEvent OnBeginedDrag;
        public DraggableEvent OnDragging;
        public DraggableEvent OnEndedDrag;
        public DraggableEvent OnPointerUpped;

        private void OnValidate()
        {
            _rectTransform ??= GetComponent<RectTransform>();
        }

        private void Awake()
        {
            StartAnchorMin = _rectTransform.anchorMin;
            StartAnchorMax = _rectTransform.anchorMax;
            StartSize = new Vector2(_rectTransform.rect.width, _rectTransform.rect.height);
            StartParent = transform.parent;
            SiblingNumber = transform.GetSiblingIndex();
            StartAnchoredPosition = _rectTransform.anchoredPosition;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!IsDraggable) return;
            _rectTransform.DOKill();
            OnPointerDowned?.Invoke(this);
            UpdatePosition(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!IsDraggable) return;

            _rectTransform.DOKill();
            _isDragging = true;

            UpdatePosition(eventData);
            OnBeginedDrag?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isDragging || !IsDraggable) return;
            UpdatePosition(eventData);
        }

        private void UpdatePosition(PointerEventData eventData)
        {
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                    _rectTransform, eventData.position, GlobalObjects.Camera, out var worldPoint))
            {
                Vector3 worldOffset = _rectTransform.TransformVector(_offset);
                var pos = worldPoint + worldOffset;
                
                var halfWidth = _rectTransform.rect.width * 0.5f * _rectTransform.lossyScale.x;
                var halfHeight = _rectTransform.rect.height * 0.5f * _rectTransform.lossyScale.y;

                var min = GlobalObjects.Camera.ScreenToWorldPoint(new Vector3(0, 0, GlobalObjects.Camera.nearClipPlane));
                var max = GlobalObjects.Camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, GlobalObjects.Camera.nearClipPlane));

                pos.x = Mathf.Clamp(pos.x, min.x + halfWidth, max.x - halfWidth);
                pos.y = Mathf.Clamp(pos.y, min.y + halfHeight, max.y - halfHeight);

                _rectTransform.position = pos;
                OnDragging?.Invoke(this);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_isDragging || !IsDraggable) return;

            _isDragging = false;
            OnEndedDrag?.Invoke(this);
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            OnPointerUpped?.Invoke(this);
            if(_checkZone)
                CheckZone(eventData);
        }

        public void CancelDrag()
        {
            IsDraggable = false;
            _rectTransform.DOAnchorPos(StartAnchoredPosition, 0.25f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    _rectTransform.SetParent(StartParent);
                    _rectTransform.SetSiblingIndex(SiblingNumber);
                    IsDraggable = true;
                })
                .Play();
        }

        private void CheckZone(PointerEventData eventData)
        {
            var results = new System.Collections.Generic.List<RaycastResult>();

            EventSystem.current.RaycastAll(eventData, results);

            foreach (var r in results)
            {
                if (r.gameObject.TryGetComponent<DropZone>(out var zone))
                {
                    zone.OnDrop(this, eventData.position);
                    return;
                }
            }
            
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            OnBeginedDrag = null;
            OnEndedDrag = null;
        }
    }
    [System.Serializable]
    public class DraggableEvent : UnityEvent<Draggable> { }
}
