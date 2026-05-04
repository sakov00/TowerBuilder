using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Project.Scripts.UI.WindowElements
{
    public class LevelScrollRect : ScrollRect
    {
        private int _currentLevel;
        private List<int> _dataLevels;
        private List<LevelPanel> _levelPanels;
        
        private float _levelWidth;
        private float _spaceBetweenLevels;
        private int _totalLevels;   
        
        private float _spacingRatio;
        
        private float _minX;
        private float _maxX;
        private float _lastContentX;
        private int _offsetStepsAutoScroll = -2;
        
        public void Initialize(int currentLevel, List<LevelPanel> levelPanels, int totalLevels, float spacingRatio = 0.18f)
        {
            _currentLevel = currentLevel;
            _levelPanels = levelPanels;
            _totalLevels = totalLevels;
            _dataLevels = Enumerable.Range(0, 101).ToList();
            _spacingRatio = Mathf.Clamp01(spacingRatio);
            CalculateDynamicSizes();
            ApplyPanelSizes();
            CalculateLimits();
            ScrollToLevel(_currentLevel);
        }
        
        public void ScrollToLevel(int levelIndex)
        {
            var step = _levelWidth + _spaceBetweenLevels;
            content.anchoredPosition = new Vector2(-(levelIndex + _offsetStepsAutoScroll) * step , 0);
            ClampContentPosition();
            _lastContentX = content.anchoredPosition.x;
            UpdatePanelPositions();
            UpdateAllPanels();
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            ClampContentPosition();
        }

        public override void OnScroll(PointerEventData data)
        {
            base.OnScroll(data);
            ClampContentPosition();
        }
        
        protected override void LateUpdate()
        {
            base.LateUpdate();
        
            if (velocity.sqrMagnitude < 0.001f) return;
            if (_levelPanels == null || _levelPanels.Count == 0) return;
        
            ClampContentPosition();
        
            float step = _levelWidth + _spaceBetweenLevels;
            float delta = content.anchoredPosition.x - _lastContentX;
        
            while (Mathf.Abs(delta) >= step)
            {
                if (delta > 0)
                {
                    MoveLeftToRight();
                    _lastContentX += step;
                }
                else
                {
                    MoveRightToLeft();
                    _lastContentX -= step;
                }
        
                delta = content.anchoredPosition.x - _lastContentX;
            }
        }
        
        private void MoveLeftToRight()
        {
            var last = _levelPanels[^1];
            _levelPanels.RemoveAt(_levelPanels.Count - 1);
            _levelPanels.Insert(0, last);

            var first = _levelPanels[1];
            last.transform.SetAsFirstSibling();
            last.SetPosition(first.GetPosition() - new Vector2(_levelWidth + _spaceBetweenLevels, 0));
            
            UpdateAllPanels();
        }

        private void MoveRightToLeft()
        {
            var first = _levelPanels[0];
            _levelPanels.RemoveAt(0);
            _levelPanels.Add(first);

            var last = _levelPanels[^2];
            first.transform.SetAsLastSibling();
            first.SetPosition(last.GetPosition() + new Vector2(_levelWidth + _spaceBetweenLevels, 0));
            
            UpdateAllPanels();
        }

        private void ClampContentPosition()
        {
            if (horizontal && content != null)
            {
                float x = Mathf.Clamp(content.anchoredPosition.x, -_maxX, -_minX);
                content.anchoredPosition = new Vector2(x, content.anchoredPosition.y);
            }
        }
        
        private void CalculateDynamicSizes()
        {
            var viewportWidth = viewport.rect.width;
            var totalSpacing = viewportWidth * _spacingRatio;
            _spaceBetweenLevels = totalSpacing / _levelPanels.Count;
            _levelWidth = (viewportWidth - totalSpacing) / _levelPanels.Count;
        }

        private void ApplyPanelSizes()
        {
            foreach (var panel in _levelPanels)
            {
                var rect = panel.GetComponent<RectTransform>();

                rect.SetSizeWithCurrentAnchors(
                    RectTransform.Axis.Horizontal,
                    _levelWidth
                );
            }
        }
        
        private void UpdatePanelPositions()
        {
            float step = _levelWidth + _spaceBetweenLevels;
            float halfSpacing = _spaceBetweenLevels * 0.5f;

            for (int i = 0; i < _levelPanels.Count; i++)
            {
                var panel = _levelPanels[i];

                panel.SetPosition(
                    new Vector2(-content.anchoredPosition.x + halfSpacing + i * step, panel.GetPosition().y)
                );
            }
        }

        private void CalculateLimits()
        {
            float contentWidth = _totalLevels * _levelWidth + (_totalLevels - 1) * _spaceBetweenLevels;
            float viewportWidth = 5 * _levelWidth + (5 - 1) * _spaceBetweenLevels;

            _minX = 0f;
            _maxX = Mathf.Max(contentWidth - viewportWidth, 0f);
        }
        
        private void UpdateAllPanels()
        {
            float contentX = -content.anchoredPosition.x;
            float step = _levelWidth + _spaceBetweenLevels;

            int firstVisibleIndex = Mathf.RoundToInt(contentX / step);
            firstVisibleIndex = Mathf.Clamp(firstVisibleIndex, 0, _totalLevels - 1);

            _currentLevel = _dataLevels[firstVisibleIndex];

            int half = _levelPanels.Count / 2 - 2;

            for (int i = 0; i < _levelPanels.Count; i++)
            {
                int levelIndex = firstVisibleIndex + (i - half);

                if (levelIndex < 0 || levelIndex >= _totalLevels)
                {
                    continue;
                }
                
                _levelPanels[i].Initialize(
                    _dataLevels[levelIndex],
                    10,
                    LevelState.Ready
                );
            }
        }
    }
}