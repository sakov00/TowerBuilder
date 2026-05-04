using System.Collections.Generic;
using _Project.Scripts.UI.TweenFeature.TweenActions;
using DG.Tweening;
using UnityEngine;

public class ZoomTween : TweenAction
{
    [Header("Настройки")]
    public RectTransform target;
    public List<RectTransform> zoomPoints;
    public float scale = 3f;
    public float tweenDuration = 0.2f;
    public Vector2 offset = Vector3.zero;

    private Vector2 originalPos;
    private Vector3 originalScale;
    private Vector2 originalPivot;

    private void Awake()
    {
        if (target != null)
        {
            originalPos = target.anchoredPosition;
            originalScale = target.localScale;
            originalPivot = target.pivot;
        }
    }

    // ===== Анимация pivot с сохранением позиции =====
    Tween AnimatePivot(RectTransform rect, Vector2 newPivot)
    {
        return DOTween.To(
            () => rect.pivot,
            p =>
            {
                rect.pivot = p;
            },
            newPivot,
            tweenDuration
        ).SetEase(Ease.Linear);
    }


    Vector2 GetPivotFromZoomPoint(RectTransform point)
    {
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, point.position);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            target,
            screenPos,
            null,
            out var localPoint
        );

        return new Vector2(
            Mathf.InverseLerp(target.rect.xMin, target.rect.xMax, localPoint.x),
            Mathf.InverseLerp(target.rect.yMin, target.rect.yMax, localPoint.y)
        );
    }


    public void PlayZoom(int index)
    {
        if (target == null || zoomPoints == null || zoomPoints.Count == 0) return;
        if (index < 0 || index >= zoomPoints.Count) return;

        RectTransform point = zoomPoints[index];
        Vector2 pivot = GetPivotFromZoomPoint(point);
        var newPos = target.anchoredPosition + offset;

        target.DOKill();

        Sequence seq = DOTween.Sequence();

        seq.Join(AnimatePivot(target, pivot));
        seq.Join(target.DOAnchorPos(newPos, tweenDuration).SetEase(Ease.Linear));
        seq.Join(target.DOScale(scale, tweenDuration).SetEase(Ease.Linear));

        seq.Play();
    }


    public void ResetZoom()
    {
        if (target == null) return;

        target.DOKill();

        Sequence seq = DOTween.Sequence();

        seq.Join(AnimatePivot(target, originalPivot));
        seq.Join(target.DOAnchorPos(originalPos, tweenDuration).SetEase(Ease.Linear));
        seq.Join(target.DOScale(originalScale, tweenDuration).SetEase(Ease.Linear));

        seq.Play();
    }

    public override Tween GetTween() => null;
}
