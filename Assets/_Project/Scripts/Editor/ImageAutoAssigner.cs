using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;

public class ImageAssignByOrder : EditorWindow
{
    [MenuItem("Tools/Assign Existing Sprites By Order")]
    public static void ShowWindow()
    {
        GetWindow<ImageAssignByOrder>("Assign Existing Sprites By Order");
    }

    private bool searchInChildren = false;

    private void OnGUI()
    {
        GUILayout.Label("Assign Existing Sprites to Images by Order", EditorStyles.boldLabel);

        searchInChildren = EditorGUILayout.Toggle("Search in Children", searchInChildren);

        if (GUILayout.Button("Assign Selected Sprites to Selected Images"))
        {
            AssignToSelectedImages();
        }
    }

    private void AssignToSelectedImages()
    {
        // 1️⃣ Получаем все Image на выделенных объектах
        GameObject[] selectedObjects = Selection.gameObjects;
        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("No objects selected in scene!");
            return;
        }

        List<Image> imagesList = new List<Image>();
        foreach (GameObject go in selectedObjects)
        {
            Image[] images = searchInChildren ? go.GetComponentsInChildren<Image>(true) : go.GetComponents<Image>();
            imagesList.AddRange(images);
        }

        if (imagesList.Count == 0)
        {
            Debug.LogWarning("No Image components found in selected objects.");
            return;
        }

        // 2️⃣ Получаем выделенные объекты в Project
        List<Sprite> sprites = new List<Sprite>();
        foreach (Object obj in Selection.objects)
        {
            if (obj is Sprite s)
            {
                sprites.Add(s);
            }
            else if (obj is Texture2D tex)
            {
                // ищем уже существующий спрайт для этой текстуры
                string path = AssetDatabase.GetAssetPath(tex);
                Sprite[] existingSprites = AssetDatabase.LoadAllAssetsAtPath(path) as Sprite[];
                if (existingSprites != null && existingSprites.Length > 0)
                {
                    // берём первый спрайт из текстуры
                    sprites.Add(existingSprites[0]);
                }
            }
        }

        if (sprites.Count == 0)
        {
            Debug.LogWarning("No existing sprites found for selected objects.");
            return;
        }

        // 3️⃣ Присваиваем спрайты по порядку
        int count = Mathf.Min(imagesList.Count, sprites.Count);
        for (int i = 0; i < count; i++)
        {
            Undo.RecordObject(imagesList[i], "Assign Sprite");
            imagesList[i].sprite = sprites[i];
        }

        Debug.Log($"Assigned {count} existing sprites to {imagesList.Count} Images.");
    }
}
