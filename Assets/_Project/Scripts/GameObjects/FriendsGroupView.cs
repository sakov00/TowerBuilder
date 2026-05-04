using System.Collections.Generic;
using _Project.Scripts.GameObjects.Abstract.Unit;
using UnityEngine;

namespace _Project.Scripts.GameObjects
{
    public class FriendsGroupView : MonoBehaviour
    {
        public void ArrangeUnitsInRadius(IReadOnlyList<UnitController> units, float radius, float minDistance = 1f)
        {
            var center = transform.position;
            var placedPositions = new List<Vector3>(units.Count);

            for (int i = 0; i < units.Count; i++)
            {
                Vector3 newPos;
                int attempts = 0;

                do
                {
                    var angle = Random.Range(0f, Mathf.PI * 2f);
                    var r = Mathf.Sqrt(Random.Range(0f, 1f)) * radius;
                    var x = Mathf.Cos(angle) * r;
                    var z = Mathf.Sin(angle) * r;
                    newPos = center + new Vector3(x, 0, z);
                    attempts++;
                }
                while (attempts < 10 && placedPositions.Exists(p => Vector3.Distance(p, newPos) < minDistance));

                placedPositions.Add(newPos);
                // units[i].MoveTo(newPos);
                // units[i].SetWayToPoint(new List<Vector3> { units[i].transform.position });
            }
        }
    }
}