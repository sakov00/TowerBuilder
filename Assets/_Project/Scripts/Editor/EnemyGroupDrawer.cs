using _Project.Scripts.GameObjects.Additional.EnemyRoads;
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Editor
{
    [CustomPropertyDrawer(typeof(EnemyGroup))]
    public class EnemyGroupDrawer : PropertyDrawer
    {
        private const float VerticalSpacing = 4f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var enemiesProp = property.FindPropertyRelative("enemies");

            int index = GetIndexFromPropertyPath(property.propertyPath);
            string header = $"Раунд {index + 1}";

            EditorGUI.BeginProperty(position, label, property);

            // Заголовок группы
            Rect headerRect = new(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(headerRect, header, EditorStyles.boldLabel);

            // Список врагов
            Rect listRect = new(position.x, position.y + EditorGUIUtility.singleLineHeight + VerticalSpacing, position.width, EditorGUI.GetPropertyHeight(enemiesProp));
            EditorGUI.PropertyField(listRect, enemiesProp, GUIContent.none, true);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var enemiesProp = property.FindPropertyRelative("enemies");
            return EditorGUI.GetPropertyHeight(enemiesProp, true) + EditorGUIUtility.singleLineHeight + VerticalSpacing;
        }

        // Определяет индекс группы по пути свойства, например: roundEnemyList.Array.data[1]
        private int GetIndexFromPropertyPath(string path)
        {
            int start = path.IndexOf('[') + 1;
            int end = path.IndexOf(']');
            if (start >= 0 && end > start)
            {
                string number = path.Substring(start, end - start);
                if (int.TryParse(number, out int index))
                    return index;
            }

            return -1;
        }
    }
}