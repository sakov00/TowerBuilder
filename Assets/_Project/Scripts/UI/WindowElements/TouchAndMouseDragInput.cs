using System;
using _Project.Scripts._GlobalLogic;
using _Project.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.UI.WindowElements
{
    public class TouchAndMouseDragInput : MonoBehaviour
    {
        [Header("Camera Drag Settings")]
        [SerializeField] private float _dragSpeed = 2f;
        [SerializeField] private float _minDragThreshold = 4f;

        [Header("Raycast Layers")]
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private LayerMask _ignoreMask;

        private Vector3 _lastPointerPosition;
        private bool _isDragging = false;
        private bool _clickedOnObject = false;
        private bool _justEnabled;

        private ISelectable _selected;
        
        private void OnEnable()
        {
            _justEnabled = true;
        }

        private void Update()
        {
            if (_justEnabled)
            {
                _justEnabled = false;
                _isDragging = false;
                return;
            }

#if ANDROID_MODE
            HandleTouchInput();
#else
            HandleMouseInput();
#endif
        }
        
        private void HandleMouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _lastPointerPosition = Input.mousePosition;
                _isDragging = true;

                // Проверяем, попали ли по объекту
                _clickedOnObject = TrySelectUnit(Input.mousePosition);
            }

            if (Input.GetMouseButton(0) && _isDragging && !_clickedOnObject)
            {
                Vector3 delta = Input.mousePosition - _lastPointerPosition;
                _lastPointerPosition = Input.mousePosition;

                if (delta.sqrMagnitude > _minDragThreshold)
                {
                    DragCamera(delta);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
                if (_selected != null)
                {
                    if (TryGetGroundPoint(Input.mousePosition, out var target))
                    {
                        _selected.MoveTo(target);
                    }
                    _selected.Deselect();
                    _selected = null;
                }
                else
                {
                    TryHandleBuy(Input.mousePosition);
                }

                _clickedOnObject = false;
            }
        }

        private void HandleTouchInput()
        {
            if (Input.touchCount != 1)
            {
                _isDragging = false;
                _clickedOnObject = false;
                return;
            }

            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _lastPointerPosition = touch.position;
                _isDragging = true;

                _clickedOnObject = TrySelectUnit(touch.position);
            }
            else if (touch.phase == TouchPhase.Moved && _isDragging && !_clickedOnObject)
            {
                Vector2 delta = touch.position - (Vector2)_lastPointerPosition;
                _lastPointerPosition = touch.position;

                if (delta.sqrMagnitude > _minDragThreshold)
                {
                    DragCamera(delta);
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                _isDragging = false;

                if (_selected != null)
                {
                    if (TryGetGroundPoint(touch.position, out var target))
                    {
                        _selected.MoveTo(target);
                        _selected.Deselect();
                        _selected = null;
                    }
                }
                else
                {
                    TryHandleBuy(touch.position);
                }

                _clickedOnObject = false;
            }
        }

        private void DragCamera(Vector2 delta)
        {
            Vector3 right = GlobalObjects.Camera.transform.right;
            Vector3 forward = Vector3.Cross(right, Vector3.up);

            Vector3 move = (-right * delta.x - forward * delta.y) * _dragSpeed * Time.deltaTime;
            GlobalObjects.Camera.transform.position += move;
        }

        private bool TrySelectUnit(Vector2 screenPos)
        {
            var ray = GlobalObjects.Camera.ScreenPointToRay(screenPos);
            if (!Physics.Raycast(ray, out RaycastHit hit, 1000f)) return false;

            if (hit.collider.TryGetComponent<ISelectable>(out var unit))
            {
                if (_selected != null)
                    _selected.Deselect();

                _selected = unit;
                _selected.Select();
                return true;
            }

            return false;
        }

        public bool TryGetGroundPoint(Vector2 screenPos, out Vector3 point)
        {
            var ray = GlobalObjects.Camera.ScreenPointToRay(screenPos);
            float maxDistance = 1000f;

            var hits = Physics.RaycastAll(ray, maxDistance);

            Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            foreach (var hit in hits)
            {
                int layerBit = 1 << hit.collider.gameObject.layer;

                if ((_groundMask & layerBit) != 0)
                {
                    point = hit.point;
                    return true;
                }

                if ((_ignoreMask & layerBit) != 0)
                {
                    continue;
                }

                break;
            }

            point = default;
            return false;
        }


        private void TryHandleBuy(Vector2 screenPos)
        {
            var ray = GlobalObjects.Camera.ScreenPointToRay(screenPos);
            if (!Physics.Raycast(ray, out RaycastHit hit, 1000f)) return;

            if (hit.collider.TryGetComponent<IBuy>(out var buyable))
            {
                buyable.TryBuy().Forget();
            }
        }
    }
}
