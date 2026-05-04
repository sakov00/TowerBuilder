using UnityEngine;

[ExecuteAlways]
public class UIPositionConstraint : MonoBehaviour
{
    [SerializeField] private RectTransform target;
    [SerializeField] private Vector2 offset;

    private RectTransform self;

    void Awake()
    {
        self = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (target == null || self == null)
            return;
        
        self.anchoredPosition = target.anchoredPosition + offset;
    }
}