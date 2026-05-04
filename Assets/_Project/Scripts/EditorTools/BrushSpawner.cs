using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BrushSpawner : MonoBehaviour
{
    public GameObject[] prefabs;       // префабы для спавна
    public float brushRadius = 1f;     // радиус кисти
    public int density = 5;            // сколько объектов за клик
    public LayerMask surfaceMask = ~0; // слой, на котором можно спавнить
}