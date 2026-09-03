using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.UI;
using TweenControllers;

[CustomEditor(typeof(TweenGroup))]
[CanEditMultipleObjects]
public class TweenGroupEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();

        if (GUILayout.Button("Collect Tweens"))
        {
            foreach (var targetObj in targets)
            {
                var group = (TweenGroup)targetObj;
                Undo.RecordObject(group, "Collect Tweens");
                group.CollectTweens();
                EditorUtility.SetDirty(group);
            }
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Add Play() to Button.OnClick"))
        {
            foreach (var targetObj in targets)
            {
                var group = (TweenGroup)targetObj;
                AddPlayPersistent(group);
            }
        }
    }

    private void AddPlayPersistent(TweenGroup group)
    {
        var button = group.GetComponent<Button>();
        if (button == null)
        {
            Debug.LogWarning($"[{group.name}] Button component not found on this GameObject.");
            return;
        }

        Undo.RecordObject(button, "Add TweenGroup.Play to Button.OnClick");

        // Проверяем, не добавлен ли уже Play()
        int eventCount = button.onClick.GetPersistentEventCount();
        for (int i = 0; i < eventCount; i++)
        {
            if (button.onClick.GetPersistentTarget(i) == group &&
                button.onClick.GetPersistentMethodName(i) == nameof(TweenGroup.Play))
            {
                Debug.Log($"[{group.name}] TweenGroup.Play() already exists in Button.OnClick.");
                return;
            }
        }

        // ✅ Добавляем persistent listener — будет отображаться в инспекторе
        UnityEventTools.AddPersistentListener(button.onClick, group.Play);
        EditorUtility.SetDirty(button);

        Debug.Log($"[{group.name}] Added persistent TweenGroup.Play() to Button.OnClick.");
    }
}
