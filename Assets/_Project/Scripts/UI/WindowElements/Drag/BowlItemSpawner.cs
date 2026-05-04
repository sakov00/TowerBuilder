using UI.SpecialItems.Drag;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BowlItemSpawner : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private UnityEvent _onPointerDown;
    [SerializeField] private Draggable _vegetablePrefab;
    [SerializeField] private RectTransform _spawnParent;

    private Draggable _currentDraggable;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _currentDraggable = null;
        _onPointerDown?.Invoke();
        StartCoroutine(SpawnAndDrag(eventData));
    }

    private System.Collections.IEnumerator SpawnAndDrag(PointerEventData eventData)
    {
        _currentDraggable = Instantiate(_vegetablePrefab, _spawnParent);
        _currentDraggable.transform.SetAsLastSibling();

        // 🔹 СТАВИМ В ТОЧКУ КЛИКА
        var rect = _currentDraggable.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _spawnParent,
            eventData.position,
            eventData.pressEventCamera,
            out var localPoint
        );
        rect.anchoredPosition = localPoint;

        yield return null; // дождаться кадра для EventSystem

        eventData.pointerPress = _currentDraggable.gameObject;
        eventData.pointerDrag  = _currentDraggable.gameObject;
        eventData.rawPointerPress = _currentDraggable.gameObject;

        ExecuteEvents.Execute(
            _currentDraggable.gameObject,
            eventData,
            ExecuteEvents.pointerDownHandler
        );

        ExecuteEvents.Execute(
            _currentDraggable.gameObject,
            eventData,
            ExecuteEvents.beginDragHandler
        );
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _currentDraggable.OnPointerUp(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _currentDraggable.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _currentDraggable.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _currentDraggable.OnEndDrag(eventData);
    }
}
