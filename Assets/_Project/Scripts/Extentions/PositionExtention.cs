using UnityEngine;

namespace _Project.Scripts.Extentions
{
    public static class PositionExtension
    {
        public static float GetDistanceBetweenObjects(Vector3 aPosition, Vector3 aLocalScale, Vector3 bPosition,  Vector3 bLocalScale)
        {
            Vector3 delta = bPosition - aPosition;
            float centerDistance = delta.magnitude;

            // Если центры совпадают — объекты в одной точке
            if (centerDistance <= Mathf.Epsilon)
                return 0f;

            Vector3 dir = delta / centerDistance;

            float aExtent = Vector3.Dot(aLocalScale * 0.5f, Abs(dir));
            float bExtent = Vector3.Dot(bLocalScale * 0.5f, Abs(dir));

            float edgeDistance = centerDistance - aExtent - bExtent;

            return Mathf.Max(0f, edgeDistance);
        }

        public static float GetDistanceBetweenObjects(Vector3 aPosition, Vector3 aLocalScale, Vector3 bPosition, Vector3 bLocalScale,
            out Vector3 pointA, out Vector3 pointB)
        {
            Vector3 delta = bPosition - aPosition;
            float centerDistance = delta.magnitude;

            if (centerDistance <= Mathf.Epsilon)
            {
                pointA = aPosition;
                pointB = bPosition;
                return 0f;
            }

            Vector3 dir = delta / centerDistance;

            float aExtent = Vector3.Dot(aLocalScale * 0.5f, Abs(dir));
            float bExtent = Vector3.Dot(bLocalScale * 0.5f, Abs(dir));

            pointA = aPosition + dir * aExtent;
            pointB = bPosition - dir * bExtent;

            float edgeDistance = centerDistance - aExtent - bExtent;

            return Mathf.Max(0f, edgeDistance);
        }

        private static Vector3 Abs(Vector3 v) => new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
    }
}
