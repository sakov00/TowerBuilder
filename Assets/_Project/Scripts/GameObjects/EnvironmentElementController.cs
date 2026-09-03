using UnityEngine;

namespace _Project.Scripts.GameObjects
{
    public class EnvironmentElementController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public float Direction { get; private set; }

        public void Initialize(Sprite sprite, float direction)
        {
            _spriteRenderer.sprite = sprite;

            SetDirection(direction);
        }

        public void SetDirection(float direction)
        {
            Direction = Mathf.Sign(direction);

            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Direction;
            transform.localScale = scale;
        }

        public void ReverseDirection()
        {
            SetDirection(-Direction);
        }
    }
}