using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Registries;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.ServicesGameplay
{
    public class DetectionService : ITickable
    {
        [Inject] private AppData _appData;
        [Inject] private LiveRegistry _liveRegistry;
        
        private readonly List<ISearchController> _allSearch = new();

        private NativeArray<float3> _positions;
        private NativeArray<WarSide> _warSides;
        private NativeArray<float> _health;

        public void Tick()
        {
            UpdateArrays();
            DetectAll();
        }

        private void UpdateArrays()
        {
            var all = _liveRegistry.GetAllReactive();
            int count = all.Count;

            if (_positions.IsCreated) _positions.Dispose();
            if (_warSides.IsCreated) _warSides.Dispose();
            if (_health.IsCreated) _health.Dispose();

            _positions = new NativeArray<float3>(count, Allocator.Persistent);
            _warSides = new NativeArray<WarSide>(count, Allocator.Persistent);
            _health = new NativeArray<float>(count, Allocator.Persistent);

            for (int i = 0; i < count; i++)
            {
                var obj = all[i];
                _positions[i] = obj.transform.position;
                _warSides[i] = obj.WarSide;
                _health[i] = obj.Model.CurrentHealth;
            }
        }

        public void DetectAll()
        {
            _liveRegistry.GetAllByType(_allSearch);
            int count = _allSearch.Count;

            var results = new NativeArray<int>(count, Allocator.TempJob);

            var job = new DetectNearestJob
            {
                GlobalPositions = _positions,
                GlobalWarSides = _warSides,
                GlobalHealth = _health,
                DetectionPositions = new NativeArray<float3>(count, Allocator.TempJob),
                DetectionRadii = new NativeArray<float>(count, Allocator.TempJob),
                AttackRange = new NativeArray<float>(count, Allocator.TempJob),
                LocalWarSides = new NativeArray<WarSide>(count, Allocator.TempJob),
                Results = results,
            };

            for (int i = 0; i < count; i++)
            {
                job.DetectionPositions[i] = _allSearch[i].Position;
                job.DetectionRadii[i] = _allSearch[i].DetectionRadius;
                job.DetectionRadii[i] = _allSearch[i].DetectionRadius;
                job.LocalWarSides[i] = _allSearch[i].WarSide;
            }
            
            JobHandle handle = job.Schedule(count, 64);
            handle.Complete();

            for (int i = 0; i < count; i++)
            {
                int nearestIndex = results[i];
                _allSearch[i].CurrentAim = nearestIndex >= 0 ? _liveRegistry.GetAllReactive()[nearestIndex] : _allSearch[i].DefaultAim;
            }

            job.DetectionPositions.Dispose();
            job.DetectionRadii.Dispose();
            results.Dispose();
        }

        [BurstCompile]
        struct DetectNearestJob : IJobParallelFor
        {
            [ReadOnly] public NativeArray<float3> GlobalPositions;
            [ReadOnly] public NativeArray<WarSide> GlobalWarSides;
            [ReadOnly] public NativeArray<float> GlobalHealth;

            [ReadOnly] public NativeArray<float3> DetectionPositions;
            [ReadOnly] public NativeArray<float> DetectionRadii;
            [ReadOnly] public NativeArray<float> AttackRange;
            [ReadOnly] public NativeArray<WarSide> LocalWarSides;

            public NativeArray<int> Results;

            public void Execute(int index)
            {
                float3 myPos = DetectionPositions[index];
                float radius = DetectionRadii[index];
                float attackRange = AttackRange[index];
                float radiusSqr = radius * radius;
                float attackRangeSqr = attackRange * attackRange; // Квадрат радиуса атаки для оптимизации

                int nearest = -1;
                float nearestDist = radius;

                WarSide mySide = LocalWarSides[index]; 
                WarSide targetSide = mySide == WarSide.Enemy ? WarSide.Friend : WarSide.Enemy;

                for (int i = 0; i < GlobalPositions.Length; i++)
                {
                    if (GlobalHealth[i] <= 0)
                        continue;

                    if (GlobalWarSides[i] != targetSide)
                        continue;

                    float3 delta = GlobalPositions[i] - myPos;
                    float sqrDist = math.lengthsq(delta);

                    if (sqrDist <= attackRangeSqr)
                        continue;

                    if (sqrDist < radiusSqr)
                    {
                        float dist = math.sqrt(sqrDist);
                        if (dist < nearestDist)
                        {
                            nearestDist = dist;
                            nearest = i;
                        }
                    }
                }

                Results[index] = nearest;
            }
        }
    }
}
