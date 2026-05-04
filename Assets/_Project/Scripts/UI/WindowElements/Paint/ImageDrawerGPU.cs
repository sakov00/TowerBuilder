using _Project.Scripts._GlobalLogic;
using UnityEngine;
using UnityEngine.UI;

public class ImageStampDrawer : MonoBehaviour
{
    [Header("UI")]
    public RawImage image;

    [Header("Stamp")]
    public Texture2D stampTexture;   // картинка, которую накладываем
    public Texture2D brushMask;      // мягкий край (опционально)
    public Material brushMaterial;

    [Range(0.01f, 5f)]
    public float brushSize = 0.2f;

    [SerializeField] public RenderTexture renderTexture;
    public bool isEnabled = true;

    private void Awake()
    {
        if (image)
            image.texture = renderTexture;
    }
    
    public void HandlePointer(Vector2 screenPosition)
    {
        if (!image || !brushMaterial || !stampTexture || !isEnabled)
            return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            image.rectTransform,
            screenPosition,
            GlobalObjects.Camera,
            out Vector2 localPoint
        );

        Rect rect = image.rectTransform.rect;

        float uvX = Mathf.Clamp01((localPoint.x + rect.width * 0.5f) / rect.width);
        float uvY = Mathf.Clamp01((localPoint.y + rect.height * 0.5f) / rect.height);

        DrawStamp(new Vector2(uvX, uvY));
    }

    private void DrawStamp(Vector2 uv)
    {
        brushMaterial.SetFloat("_UsePattern", 0);
        brushMaterial.SetFloat("_UseStamp", 1);
        brushMaterial.SetTexture("_MainTex", renderTexture);
        brushMaterial.SetTexture("_StampTex", stampTexture);
        brushMaterial.SetVector("_BrushUV", new Vector4(uv.x, uv.y, 0, 0));
        brushMaterial.SetFloat("_BrushSize", brushSize);

        RenderTexture temp = RenderTexture.GetTemporary(renderTexture.width, renderTexture.height);

        Graphics.Blit(renderTexture, temp, brushMaterial);
        Graphics.Blit(temp, renderTexture);

        RenderTexture.ReleaseTemporary(temp);
    }

    public void SetStamp(Texture2D tex)
    {
        stampTexture = tex;
    }
}