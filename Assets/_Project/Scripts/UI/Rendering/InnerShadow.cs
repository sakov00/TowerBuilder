using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/Inner Shadow")]
public class InnerShadow : BaseMeshEffect
{
    public Color shadowColor = new Color(0, 0, 0, 0.5f);
    public Vector2 shadowDistance = new Vector2(5f, -5f);
    [Range(0, 1)] public float strength = 0.5f;

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive()) return;

        var verts = new System.Collections.Generic.List<UIVertex>();
        vh.GetUIVertexStream(verts);

        int count = verts.Count;
        for (int i = 0; i < count; i++)
        {
            UIVertex original = verts[i];
            UIVertex shadow = original;

            // Сдвигаем вершину внутрь
            shadow.position -= new Vector3(shadowDistance.x, shadowDistance.y, 0) * strength;

            // Меняем цвет на цвет тени
            shadow.color = shadowColor;

            verts.Add(shadow);
        }

        vh.Clear();
        vh.AddUIVertexTriangleStream(verts);
    }
}