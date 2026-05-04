using _Project.Scripts.GameObjects.Additional.EnemyRoads;
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Editor
{
    [CustomPropertyDrawer(typeof(EnemyWithTime))]
    public class EnemyWithTimeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var timeProp = property.FindPropertyRelative("time");
            var typeProp = property.FindPropertyRelative("enemyType");

            EditorGUI.BeginProperty(position, label, property);

            float spacing = 8f;
            float labelWidth = 40f;
            float fieldHeight = EditorGUIUtility.singleLineHeight;

            // Определение ширин
            float halfWidth = (position.width - spacing - labelWidth * 2) / 2;

            // Прямоугольники для отрисовки
            Rect timeLabelRect = new(position.x, position.y, labelWidth, fieldHeight);
            Rect timeFieldRect = new(position.x + labelWidth, position.y, halfWidth, fieldHeight);

            Rect typeLabelRect = new(position.x + labelWidth + halfWidth + spacing, position.y, labelWidth, fieldHeight);
            Rect typeFieldRect = new(position.x + labelWidth * 2 + halfWidth + spacing, position.y, halfWidth, fieldHeight);

            // Рисуем поля
            EditorGUI.LabelField(timeLabelRect, "Время:");
            timeProp.floatValue = EditorGUI.FloatField(timeFieldRect, timeProp.floatValue);

            EditorGUI.LabelField(typeLabelRect, "Тип:");
            EditorGUI.PropertyField(typeFieldRect, typeProp, GUIContent.none);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight + 2;
        }
    }
}