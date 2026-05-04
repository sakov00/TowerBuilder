using System;
using _Project.Scripts._GlobalLogic;
using UnityEngine;
using UnityEngine.UI;

public class FingerDrawerGPU : MonoBehaviour
{
    [Header("UI & Textures")]
    public RawImage image;
    public Texture2D brushMask;      // мягкая кисть (круг)
    public Material brushMaterial;   // материал с BrushDraw.shader
    
    [Header("Optional Mask")]
    public Texture2D drawMask;

    [Header("Brush Settings")]
    public Color brushColor = Color.red;
    
    [Header("Pattern Brush")]
    public Texture2D brushPattern;   // текстура узора
    public float brushAlphaPattern;
    public bool usePattern = true;
    
    [Range(0.01f, 5f)]
    public float brushSize = 0.1f;   // регулируемая видимая кисть
    public bool isEnabled = true;

    [SerializeField] public RenderTexture renderTexture;

    private void Awake()
    {
        var rect = GetComponent<RectTransform>();

        // Очистка прозрачным цветом
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, new Color(0, 0, 0, 0));
        RenderTexture.active = null;

        // Показываем RenderTexture в RawImage
        if (image)
            image.texture = renderTexture;
    }

    // Вызываем при каждом движении курсора/пальца
    public void HandlePointer(Vector2 screenPosition)
    {
        if (!image || !brushMaterial || !brushMask || !isEnabled) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            image.rectTransform,
            screenPosition,
            GlobalObjects.Camera,
            out Vector2 localPoint
        );

        Rect rect = image.rectTransform.rect;

        float uvX = Mathf.Clamp01((localPoint.x + rect.width  * 0.5f) / rect.width);
        float uvY = Mathf.Clamp01((localPoint.y + rect.height * 0.5f) / rect.height);

        DrawBrush(new Vector2(uvX, uvY));
    }

    private void DrawBrush(Vector2 uv)
    {
        brushMaterial.SetTexture("_MainTex", renderTexture);
        brushMaterial.SetTexture("_BrushTex", brushMask);
        brushMaterial.SetFloat("_UseStamp", 0);
        if (usePattern && brushPattern != null)
        {
            brushMaterial.SetColor("_BrushColor", Color.white);
            brushMaterial.SetTexture("_PatternTex", brushPattern);
            brushMaterial.SetFloat("_UsePattern", 1);
            brushMaterial.SetFloat("_Erase", brushAlphaPattern);
        }
        else
        {
            brushMaterial.SetFloat("_UsePattern", 0);
            brushMaterial.SetColor("_BrushColor", brushColor);
            brushMaterial.SetFloat("_Erase", brushColor.a);
        }
        brushMaterial.SetVector("_BrushUV", new Vector4(uv.x, uv.y, 0, 0));
        brushMaterial.SetFloat("_BrushSize", brushSize);
        

        if(drawMask != null)
            brushMaterial.SetTexture("_MaskTex", drawMask);

        RenderTexture temp = RenderTexture.GetTemporary(renderTexture.width, renderTexture.height);
        Graphics.Blit(renderTexture, temp, brushMaterial);
        Graphics.Blit(temp, renderTexture);
        RenderTexture.ReleaseTemporary(temp);
    }
    
    public void SetEnabled(bool enabled) => isEnabled = enabled;
    public void SetUsePattern(bool enabled) => usePattern = enabled;
    
    public float GetFillPercent()
    {
        var rt = renderTexture;
        RenderTexture prev = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0,0,rt.width,rt.height), 0, 0);
        tex.Apply();

        RenderTexture.active = prev;

        int filled = 0;
        int total = rt.width * rt.height;

        var pixels = tex.GetPixels32();
        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i].a > 10)       // или по цвету
                filled++;
        }

        return (float)filled / total;
    }
}
