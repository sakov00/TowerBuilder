using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteGradientChanger : MonoBehaviour
{
    [Tooltip("Градиент для изменения цвета. Если оставить пустым, создастся автоматически.")]
    [SerializeField] private Gradient gradient;
    
    [Tooltip("Длительность полного цикла изменения цвета в секундах.")]
    [SerializeField] private float duration = 2.0f;

    private SpriteRenderer spriteRenderer;
    private float timer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Автоматическая инициализация градиента, если он не задан в инспекторе
        if (gradient == null)
        {
            gradient = new Gradient();
            
            // Задаем цвета (от красного к синему)
            GradientColorKey[] colorKeys = new GradientColorKey[2];
            colorKeys[0] = new GradientColorKey(Color.red, 0.0f);
            colorKeys[1] = new GradientColorKey(Color.blue, 1.0f);

            // Задаем прозрачность
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
            alphaKeys[0] = new GradientAlphaKey(1.0f, 0.0f);
            alphaKeys[1] = new GradientAlphaKey(1.0f, 1.0f);

            gradient.SetKeys(colorKeys, alphaKeys);
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        
        // Вычисляем прогресс от 0 до 1 с зацикливанием
        float progress = (timer / duration) % 1.0f;

        // Применяем цвет из градиента
        spriteRenderer.color = gradient.Evaluate(progress);
    }
}