#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(BrushSpawner))]
public class BrushSpawnerEditor : Editor
{
    private BrushSpawner spawner;

    // Данные для проверки пересечения и истории спавна
    private List<SpawnedObjectData> spawnedObjects = new List<SpawnedObjectData>();
    private List<List<GameObject>> undoStack = new List<List<GameObject>>(); // каждый элемент = объекты спавн-кадра

    private void OnEnable()
    {
        spawner = (BrushSpawner)target;
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        // Ctrl + ЛКМ — кистевой спавн
        if ((e.type == EventType.MouseDrag || e.type == EventType.MouseDown) && e.button == 0 && e.control)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

            if (TryRaycastMesh(spawner.gameObject, ray, out RaycastHit hit))
            {
                Vector3 hitPoint = hit.point;

                int attempts = 0;
                int spawnedThisFrame = 0;
                List<GameObject> currentBatch = new List<GameObject>();

                while (spawnedThisFrame < spawner.density && attempts < spawner.density * 20)
                {
                    attempts++;

                    // случайная точка внутри радиуса кисти
                    Vector3 offset = new Vector3(
                        Random.Range(-spawner.brushRadius, spawner.brushRadius),
                        0,
                        Random.Range(-spawner.brushRadius, spawner.brushRadius)
                    );
                    Vector3 randomPos = hitPoint + offset;

                    // проверяем пересечение с уже существующими объектами
                    bool overlaps = false;
                    foreach (var obj in spawnedObjects)
                    {
                        float minDist = obj.radius + 0.5f; // можно заменить на buffer
                        if (Vector3.Distance(obj.position, randomPos) < minDist)
                        {
                            overlaps = true;
                            break;
                        }
                    }
                    if (overlaps) continue;

                    // высота на рельефе
                    if (!TryRaycastMesh(spawner.gameObject, new Ray(randomPos + Vector3.up * 50f, Vector3.down), out RaycastHit finalHit))
                        continue;

                    // спавн префаба
                    GameObject prefab = spawner.prefabs[Random.Range(0, spawner.prefabs.Length)];
                    GameObject instance = Instantiate(prefab);
                    instance.transform.position = finalHit.point;
                    instance.transform.rotation = Quaternion.FromToRotation(Vector3.up, finalHit.normal) * Quaternion.Euler(0, Random.Range(0f, 360f), 0);
                    instance.transform.parent = spawner.transform;

                    // вычисляем радиус объекта
                    float radius = 0.5f;
                    var renderer = instance.GetComponentInChildren<Renderer>();
                    if (renderer != null)
                        radius = Mathf.Max(renderer.bounds.extents.x, renderer.bounds.extents.z);

                    spawnedObjects.Add(new SpawnedObjectData
                    {
                        position = finalHit.point,
                        radius = radius
                    });

                    currentBatch.Add(instance);
                    spawnedThisFrame++;
                }

                if (currentBatch.Count > 0)
                    undoStack.Add(currentBatch); // добавляем в стек для отмены
            }

            e.Use();
        }

        // визуализация кисти
        Handles.color = new Color(0, 1, 0, 0.3f);
        Handles.DrawSolidDisc(spawner.transform.position, Vector3.up, spawner.brushRadius);

        // сохраняем выделение
        if (Selection.activeGameObject != spawner.gameObject)
            Selection.activeGameObject = spawner.gameObject;
    }

    // Структура данных для проверки пересечений
    private class SpawnedObjectData
    {
        public Vector3 position;
        public float radius;
    }

    // Undo последнего действия
    private void UndoLast()
    {
        if (undoStack.Count == 0) return;

        List<GameObject> lastBatch = undoStack[undoStack.Count - 1];
        undoStack.RemoveAt(undoStack.Count - 1);

        foreach (var obj in lastBatch)
        {
            spawnedObjects.RemoveAll(o => Vector3.Distance(o.position, obj.transform.position) < 0.01f);
            if (obj != null)
                Undo.DestroyObjectImmediate(obj);
        }
    }

    // Raycast по мешу без коллайдера
    private bool TryRaycastMesh(GameObject root, Ray ray, out RaycastHit hit)
    {
        hit = new RaycastHit();
        MeshFilter[] meshFilters = root.GetComponentsInChildren<MeshFilter>();
        bool hasHit = false;
        float closestDist = Mathf.Infinity;

        foreach (var mf in meshFilters)
        {
            Mesh mesh = mf.sharedMesh;
            if (mesh == null) continue;

            Transform t = mf.transform;
            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;

            for (int i = 0; i < triangles.Length; i += 3)
            {
                Vector3 v0 = t.TransformPoint(vertices[triangles[i]]);
                Vector3 v1 = t.TransformPoint(vertices[triangles[i + 1]]);
                Vector3 v2 = t.TransformPoint(vertices[triangles[i + 2]]);

                if (RayTriangleIntersection(ray, v0, v1, v2, out Vector3 point, out float dist))
                {
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        hit.point = point;
                        hit.normal = Vector3.Cross(v1 - v0, v2 - v0).normalized;
                        hasHit = true;
                    }
                }
            }
        }

        return hasHit;
    }

    private bool RayTriangleIntersection(Ray ray, Vector3 v0, Vector3 v1, Vector3 v2, out Vector3 hitPoint, out float distance)
    {
        hitPoint = Vector3.zero;
        distance = 0f;

        Vector3 edge1 = v1 - v0;
        Vector3 edge2 = v2 - v0;
        Vector3 h = Vector3.Cross(ray.direction, edge2);
        float a = Vector3.Dot(edge1, h);
        if (Mathf.Abs(a) < 0.00001f) return false;

        float f = 1.0f / a;
        Vector3 s = ray.origin - v0;
        float u = f * Vector3.Dot(s, h);
        if (u < 0.0f || u > 1.0f) return false;

        Vector3 q = Vector3.Cross(s, edge1);
        float v = f * Vector3.Dot(ray.direction, q);
        if (v < 0.0f || u + v > 1.0f) return false;

        float t = f * Vector3.Dot(edge2, q);
        if (t > 0.00001f)
        {
            hitPoint = ray.origin + ray.direction * t;
            distance = t;
            return true;
        }

        return false;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("Ctrl + ЛКМ — кистевой спавн по холмистой поверхности.\nКнопка 'Undo Last' отменяет последний спавн.", MessageType.Info);

        if (GUILayout.Button("Undo Last"))
            UndoLast();

        if (GUILayout.Button("Reset Spawned Objects"))
        {
            foreach (var batch in undoStack)
                foreach (var obj in batch)
                    if (obj != null)
                        Undo.DestroyObjectImmediate(obj);

            undoStack.Clear();
            spawnedObjects.Clear();
        }
    }
}
#endif
