using _Project.Scripts.GameObjects;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using _Project.Scripts.GameObjects.Abstract.Build;
using _Project.Scripts.GameObjects.Abstract.Unit;
using _Project.Scripts.GameObjects.Additional.EnemyRoads;
using _Project.Scripts.GameObjects.Additional.LevelEnvironment.Environment;
using _Project.Scripts.GameObjects.Additional.LevelEnvironment.Terrain;
using MemoryPack;
using UnityEngine;

namespace _Project.Scripts.Interfaces
{
    [MemoryPackable]
    [MemoryPackUnion(0, typeof(BuildingZoneModel))]
    [MemoryPackUnion(1, typeof(EnemyRoadModel))]
    [MemoryPackUnion(2, typeof(EnvironmentModel))]
    [MemoryPackUnion(3, typeof(TerrainModel))]
    [MemoryPackUnion(4, typeof(ObjectModel))]
    [MemoryPackUnion(5, typeof(BuildModel))]
    [MemoryPackUnion(6, typeof(MainBuildModel))]
    [MemoryPackUnion(7, typeof(MoneyBuildModel))]
    [MemoryPackUnion(8, typeof(FriendsBuildModel))]
    [MemoryPackUnion(9, typeof(TowerDefenceModel))]
    [MemoryPackUnion(10, typeof(UnitModel))]
    [MemoryPackUnion(11, typeof(PlayerModel))]
    [MemoryPackUnion(12, typeof(WarriorModel))]
    [MemoryPackUnion(13, typeof(ArcherModel))]
    [MemoryPackUnion(14, typeof(FlyingModel))]
    [MemoryPackUnion(15, typeof(FriendsGroupModel))]
    public partial interface ISavableModel
    {
        public Vector3 SavePosition { get; set; }
        public Quaternion SaveRotation { get; set; }
    }
}