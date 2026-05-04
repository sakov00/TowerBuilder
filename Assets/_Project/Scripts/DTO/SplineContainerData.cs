using System.Collections.Generic;
using MemoryPack;
using UnityEngine;

namespace _Project.Scripts.DTO
{
    [MemoryPackable]
    public partial class SplineContainerData
    {
        public List<BezierKnotData> Splines = new();
    }

    [MemoryPackable]
    public partial struct BezierKnotData
    {
        public Vector3 Position;
        public Vector3 TangentIn;
        public Vector3 TangentOut;
        public Quaternion Rotation;

        public BezierKnotData(Vector3 p, Vector3 tin, Vector3 tout, Quaternion r)
        {
            Position = p;
            TangentIn = tin;
            TangentOut = tout;
            Rotation = r;
        }
    }
}