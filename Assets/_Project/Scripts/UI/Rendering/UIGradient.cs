using UnityEngine;
using UnityEngine.UI;

namespace UI.Rendering
{
    public class UIGradient : BaseMeshEffect
    {
        public enum GradientMode
        {
            Linear,
            Radial,
            Edges
        }

        public GradientMode m_mode = GradientMode.Linear;

        public Color m_color1 = Color.white;
        public Color m_color2 = Color.white;

        [Range(-180f, 180f)]
        public float m_angle = 0f;

        public bool m_ignoreRatio = true;

        [Header("Radial Settings")]
        [Range(0.01f, 8f)]
        public float m_power = 1f;     // степень градиента

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!enabled || vh.currentVertCount == 0)
                return;

            switch (m_mode)
            {
                case GradientMode.Linear:
                    ApplyLinear(vh);
                    break;

                case GradientMode.Radial:
                    ApplyRadial(vh);
                    break;

                case GradientMode.Edges:
                    ApplyEdges(vh);
                    break;
            }
        }

        // ---------- LINEAR GRADIENT ----------
        private void ApplyLinear(VertexHelper vh)
        {
            Rect rect = graphic.rectTransform.rect;
            Vector2 dir = Utils.UIGradientUtils.RotationDir(m_angle);

            if (!m_ignoreRatio)
                dir = Utils.UIGradientUtils.CompensateAspectRatio(rect, dir);

            var localPosMatrix = Utils.UIGradientUtils.LocalPositionMatrix(rect, dir);

            UIVertex v = default;
            for (int i = 0; i < vh.currentVertCount; i++)
            {
                vh.PopulateUIVertex(ref v, i);
                float t = (localPosMatrix * v.position).y;
                v.color *= Color.Lerp(m_color2, m_color1, t);
                vh.SetUIVertex(v, i);
            }
        }
        
        private void ApplyRadial(VertexHelper vh)
        {
            Image img = graphic as Image;
            if (img == null || img.sprite == null || vh.currentVertCount == 0)
                return;

            Sprite sprite = img.sprite;

            // Пиксельные размеры спрайта
            float w = sprite.rect.width;
            float h = sprite.rect.height;

            UIVertex v = default;

            for (int i = 0; i < vh.currentVertCount; i++)
            {
                vh.PopulateUIVertex(ref v, i);

                // Нормализация по прямоугольнику спрайта
                Vector2 pos = v.position;

                // Преобразуем позицию в 0..1
                float nx = (pos.x / w) + 0.5f;
                float ny = (pos.y / h) + 0.5f;

                Vector2 uv = new Vector2(nx, ny);

                // Центр спрайта
                Vector2 center = new Vector2(0.5f, 0.5f);

                float dist = Vector2.Distance(uv, center);
                float t = Mathf.Pow(Mathf.Clamp01(dist * 2f), m_power);

                v.color *= Color.Lerp(m_color1, m_color2, t);
                vh.SetUIVertex(v, i);
            }
        }

private void ApplyEdges(VertexHelper vh)
{
    Image img = graphic as Image;
    if (img == null || img.sprite == null || vh.currentVertCount == 0)
        return;

    Sprite sprite = img.sprite;
    Texture2D tex = sprite.texture;

    Vector2 uvMin = sprite.textureRect.min / new Vector2(tex.width, tex.height);
    Vector2 uvMax = sprite.textureRect.max / new Vector2(tex.width, tex.height);
    Vector2 denom = uvMax - uvMin;
    const float eps = 1e-6f;

    UIVertex v = default;
    for (int i = 0; i < vh.currentVertCount; i++)
    {
        vh.PopulateUIVertex(ref v, i);

        float nx = denom.x > eps ? (v.uv0.x - uvMin.x) / denom.x : 0.5f;
        float ny = denom.y > eps ? (v.uv0.y - uvMin.y) / denom.y : 0.5f;

        Vector2 uv = new Vector2(nx, ny);
        uv.x = Mathf.Clamp01(uv.x);
        uv.y = Mathf.Clamp01(uv.y);

        Vector2 center = new Vector2(0.5f, 0.5f);

        float dist = Vector2.Distance(uv, center);
        float t = 1f - Mathf.Clamp01(dist * 2f);

        v.color *= Color.Lerp(m_color2, m_color1, t);
        vh.SetUIVertex(v, i);
    }
}
    }
}
