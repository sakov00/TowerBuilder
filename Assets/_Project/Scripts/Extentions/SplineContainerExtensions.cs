using _Project.Scripts.DTO;
using UnityEngine.Splines;

namespace _Project.Scripts.Extentions
{
    public static class SplineContainerExtensions
    {
        public static SplineContainerData ToData(this SplineContainer container)
        {
            var data = new SplineContainerData();

            foreach (var knot in container.Spline)
            {
                data.Splines.Add(new BezierKnotData(
                    knot.Position,
                    knot.TangentIn,
                    knot.TangentOut,
                    knot.Rotation
                ));
            }

            return data;
        }
        
        public static void ApplyData(this SplineContainer container, SplineContainerData data)
        {
            var spline = new Spline();

            foreach (var knotData in data.Splines)
            {
                var knot = new BezierKnot(
                    knotData.Position,
                    knotData.TangentIn,
                    knotData.TangentOut,
                    knotData.Rotation
                );

                spline.Add(knot);
            }

            container.Spline = spline;
        }
    }
}