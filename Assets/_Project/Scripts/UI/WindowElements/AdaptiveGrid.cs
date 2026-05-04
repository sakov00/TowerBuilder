using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SpecialItems
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class AdaptiveGrid : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup grid;
        [SerializeField] private RectTransform rect;
        [SerializeField] private RectTransform focusPoint;

        [Header("Portrait Mode")]
        public int portraitColumns = 3;

        [Header("Landscape Mode")]
        public int landscapeColumns = 5;

        [Header("Settings")]
        public Vector2 spacing = new Vector2(10, 10);

        private RectOffset _padding;
        private Vector2 _startCellSize;
        private float _aspectRatio = 1f;
        private int _lastWidth; 
        private int _lastHeight;

        private void Start()
        {
            grid.spacing = spacing;
            _padding = grid.padding;

            _startCellSize = grid.cellSize;
            _aspectRatio = _startCellSize.y / _startCellSize.x;

            _lastWidth = Screen.width;
            _lastHeight = Screen.height;
            StartCoroutine(RecalculateNextFrame());
        }
        
        private void Update()
        {
            if (_lastWidth != Screen.width || _lastHeight != Screen.height)
            {
                _lastWidth = Screen.width;
                _lastHeight = Screen.height;
                
                StartCoroutine(RecalculateNextFrame());
            }
        }
        
        private IEnumerator RecalculateNextFrame()
        {
            yield return null;
            int columns = Screen.height > Screen.width ? portraitColumns : landscapeColumns;

            // --- расчет ширины ячейки ---
            float totalWidth = rect.rect.width - _padding.left - _padding.right - spacing.x * (columns - 1);
            float cellWidth = totalWidth / columns;

            // --- высота по aspectRatio ---
            float cellHeight = cellWidth * _aspectRatio;

            grid.cellSize = new Vector2(cellWidth, cellHeight);

            // --- перерасчет высоты RectTransform, чтобы убрать пустое место ---
            int rowsNeeded = Mathf.CeilToInt((float)rect.childCount / columns);
            float totalHeight = rowsNeeded * cellHeight + (rowsNeeded - 1) * spacing.y + _padding.top + _padding.bottom;

            if (totalHeight != rect.rect.height)
            {
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);
            }
            rect.anchoredPosition = Vector2.zero;
            
            focusPoint.sizeDelta = new Vector2(grid.cellSize.x, grid.cellSize.y);
            focusPoint.anchoredPosition = Vector2.zero;
            focusPoint.anchoredPosition = new Vector2(focusPoint.anchoredPosition.x + spacing.x, focusPoint.anchoredPosition.y - spacing.y);
        }
    }
}

