using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.WindowElements
{
    [RequireComponent(typeof(Button))]
    public class ToggleButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _icon;
        [SerializeField] private Sprite _onSprite;
        [SerializeField] private Sprite _offSprite;

        private bool _isMuted;

        private void OnValidate()
        {
            _button ??= GetComponent<Button>();
        }

        private void Awake()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _isMuted = !_isMuted;
            ApplyState();
        }

        private void ApplyState()
        {
            if (_icon != null)
                _icon.sprite = _isMuted ? _offSprite : _onSprite;
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClick);
        }
    }
}