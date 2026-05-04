using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using UnityEngine;

namespace _Project.Scripts.Interfaces
{
    public interface ISearchController
    {
        WarSide WarSide { get; }
        Vector3 Position { get; }
        float DetectionRadius { get; }
        float AttackRange { get; }
        ObjectController CurrentAim { get; set; }
        ObjectController DefaultAim { get; set; }
    }
}