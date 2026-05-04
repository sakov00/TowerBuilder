using _Project.Scripts.GameObjects.Abstract.BaseObject;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Registries;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.ServicesGameplay
{
    public class ProjectileAttackService : ITickable
    {
        [Inject] private ProjectileRegistry _projectileRegistry;
        [Inject] private LiveRegistry _liveRegistry;

        public void Tick()
        {
            var projectiles = _projectileRegistry.GetAllByType();
            var lives = _liveRegistry.GetAllReactive();

            int projectileCount = projectiles.Count;
            int liveCount = lives.Count;

            if (projectileCount == 0 || liveCount == 0)
                return;

            // Создаем NativeArray для позиции и целей снарядов
            NativeArray<float3> projectilePositions = new NativeArray<float3>(projectileCount, Allocator.TempJob);
            NativeArray<float3> projectileDirections = new NativeArray<float3>(projectileCount, Allocator.TempJob);
            NativeArray<float> projectileSpeeds = new NativeArray<float>(projectileCount, Allocator.TempJob);
            NativeArray<int> projectileOwnerSides = new NativeArray<int>(projectileCount, Allocator.TempJob);
            NativeArray<int> projectileHitIndex = new NativeArray<int>(projectileCount, Allocator.TempJob);

            // Заполняем массивы
            for (int i = 0; i < projectileCount; i++)
            {
                var p = projectiles[i];
                projectilePositions[i] = p.transform.position;
                projectileDirections[i] = p.Direction;
                projectileSpeeds[i] = p.Speed;
                projectileOwnerSides[i] = (int)p.OwnerWarSide;
                projectileHitIndex[i] = -1;
            }

            // Жизни
            NativeArray<float3> livePositions = new NativeArray<float3>(liveCount, Allocator.TempJob);
            NativeArray<int> liveSides = new NativeArray<int>(liveCount, Allocator.TempJob);

            for (int i = 0; i < liveCount; i++)
            {
                var l = lives[i];
                livePositions[i] = l.transform.position;
                liveSides[i] = (int)l.WarSide;
            }
            
            var job = new ProjectileJob
            {
                deltaTime = Time.deltaTime,
                projectilePositions = projectilePositions,
                projectileDirections = projectileDirections,
                projectileSpeeds = projectileSpeeds,
                projectileOwnerSides = projectileOwnerSides,
                projectileHitIndex = projectileHitIndex,
                livePositions = livePositions,
                liveSides = liveSides,
                liveCount = liveCount,
                hitRadius = 1.5f
            };

            JobHandle handle = job.Schedule(projectileCount, 64);
            handle.Complete();
            
            for (int i = 0; i < projectileCount; i++)
            {
                projectiles[i].transform.position = projectilePositions[i];

                if (projectileHitIndex[i] >= 0)
                {
                    projectiles[i].OnHit(lives[projectileHitIndex[i]]);
                }
            }

            projectilePositions.Dispose();
            projectileDirections.Dispose();
            projectileSpeeds.Dispose();
            projectileOwnerSides.Dispose();
            projectileHitIndex.Dispose();
            livePositions.Dispose();
            liveSides.Dispose();
        }

        [BurstCompile]
        struct ProjectileJob : IJobParallelFor
        {
            public float deltaTime;
            public float hitRadius;
            public int liveCount;

            public NativeArray<float3> projectilePositions;
            [ReadOnly] public NativeArray<float3> projectileDirections; // <- новое
            [ReadOnly] public NativeArray<float> projectileSpeeds;
            [ReadOnly] public NativeArray<int> projectileOwnerSides;
            public NativeArray<int> projectileHitIndex;

            [ReadOnly] public NativeArray<float3> livePositions;
            [ReadOnly] public NativeArray<int> liveSides;

            public void Execute(int index)
            {
                float3 pos = projectilePositions[index];
                float3 dir = projectileDirections[index]; // <- используем направление движения
        
                pos += dir * projectileSpeeds[index] * deltaTime; // движение снаряда
                projectilePositions[index] = pos;

                for (int i = 0; i < liveCount; i++)
                {
                    if (liveSides[i] == projectileOwnerSides[index])
                        continue;

                    float distance = math.distance(pos, livePositions[i]);
                    if (distance < hitRadius)
                    {
                        projectileHitIndex[index] = i;
                        break;
                    }
                }
            }
        }
    }
}
