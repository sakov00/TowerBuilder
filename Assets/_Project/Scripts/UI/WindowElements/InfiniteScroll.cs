using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SpecialItems
{
    public class InfiniteScroll : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private RectTransform _viewport;
        [SerializeField] private RectTransform _content;
        [SerializeField] private List<RectTransform> _items;

        [Header("Settings")]
        [SerializeField] private bool _horizontal = true;
        [SerializeField] private bool _vertical = false;
        [SerializeField] private float _spacing = 0f;

        [Header("Modes")]
        [SerializeField] private bool _enableInfiniteScroll = true;
        [SerializeField] private bool _autoScrolling = false;
        [SerializeField] private float _autoScrollSpeed = 300f;

        private int _lastHeight;
        private int _lastWidth;

        private void Awake()
        {
            _lastWidth = Screen.width;
            _lastHeight = Screen.height;
        }

        private void LateUpdate()
        {
            // Проверяем смену ориентации / размера экрана
            if (Screen.width != _lastWidth || Screen.height != _lastHeight)
            {
                // обнуляем позиции
                _items.ForEach(x => x.anchoredPosition = Vector2.zero);
                _content.anchoredPosition = Vector2.zero;

                // регулярка для извлечения числа из имени
                Regex regex = new Regex(@"\d+");

                // сортировка по имени + числу
                _items.Sort((a, b) =>
                {
                    // текстовая часть имени без числа
                    string nameA = Regex.Replace(a.name, @"\d+", "");
                    string nameB = Regex.Replace(b.name, @"\d+", "");

                    int cmp = string.Compare(nameA, nameB, StringComparison.Ordinal);
                    if (cmp != 0) return cmp;

                    // извлекаем число из имени
                    int numA = regex.IsMatch(a.name) ? int.Parse(regex.Match(a.name).Value) : 0;
                    int numB = regex.IsMatch(b.name) ? int.Parse(regex.Match(b.name).Value) : 0;

                    return numA.CompareTo(numB);
                });

                _lastWidth = Screen.width;
                _lastHeight = Screen.height;
            }
            
            if (_autoScrolling)
            {
                Vector2 offset = Vector2.zero;
                if (_horizontal) offset.x = _autoScrollSpeed * Time.deltaTime;
                if (_vertical) offset.y = _autoScrollSpeed * Time.deltaTime;
                _content.anchoredPosition += offset;
            }
        
            if (_enableInfiniteScroll)
            {
                if (_horizontal) HandleHorizontalLoop();
                if (_vertical) HandleVerticalLoop();
            }
        }
        
        private readonly Vector3[] _corners = new Vector3[4];

        private void HandleHorizontalLoop()
        {
            RectTransform first = _items[0];
            RectTransform last = _items[_items.Count - 1];

            _viewport.GetWorldCorners(_corners);
            float vpRight = _corners[3].x;

            last.GetWorldCorners(_corners);
            float lastLeft = _corners[0].x;

            float step = _items[1].position.x - _items[0].position.x;

            if (lastLeft > vpRight)
            {
                first.GetWorldCorners(_corners);
                float firstLeft = _corners[0].x;

                last.position = new Vector3(firstLeft - step, last.position.y, last.position.z);

                _items.RemoveAt(_items.Count - 1);
                _items.Insert(0, last);
            }
        }

        private void HandleVerticalLoop()
        {
            float total = 0f;
            for (int i = 0; i < _items.Count; i++)
                total += _items[i].rect.height + _spacing;

            float contentY = _content.anchoredPosition.y;

            for (int i = 0; i < _items.Count; i++)
            {
                RectTransform child = _items[i];
                float height = child.rect.height;
                float step = height + _spacing;
                float relative = child.anchoredPosition.y - contentY;

                if (relative > step)
                    child.anchoredPosition -= new Vector2(0, total);
                else if (relative < -total)
                    child.anchoredPosition += new Vector2(0, total);
            }
        }
    }
}
