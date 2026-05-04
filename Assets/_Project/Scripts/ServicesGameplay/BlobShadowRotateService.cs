using System.Collections.Generic;
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
    public class BlobShadowRotateService : ITickable
    {
        [Inject] private LiveRegistry _liveRegistry;

        private readonly List<IShadowed> _owners = new();
        
        public void Tick()
        {
            _liveRegistry.GetAllByType(_owners);

            int count = _owners.Count;
            if (count == 0) return;
            
            var commands = new NativeArray<RaycastCommand>(count, Allocator.TempJob);
            var hits = new NativeArray<RaycastHit>(count, Allocator.TempJob);
            var rotations = new NativeArray<quaternion>(count, Allocator.TempJob);

            for (int i = 0; i < count; i++)
            {
                var owner = _owners[i];
                if (owner == null) continue;

                var queryParams = new QueryParameters
                {
                    layerMask = LayerMask.GetMask("Ground"),
                    hitTriggers = QueryTriggerInteraction.Ignore
                };

                commands[i] = new RaycastCommand(
                    Physics.defaultPhysicsScene,
                    owner.Position,
                    Vector3.down,
                    queryParams,
                    5f
                );
            }

            JobHandle rayHandle = RaycastCommand.ScheduleBatch(commands, hits, 1);

            var rotationJob = new BlobShadowRotationJob
            {
                Hits = hits,
                Rotations = rotations
            };

            JobHandle rotationHandle = rotationJob.Schedule(count, 32, rayHandle);
            rotationHandle.Complete();

            for (int i = 0; i < count; i++)
            {
                var shadow = _owners[i].ShadowTransform;
                var hit = hits[i];

                if (shadow == null) continue;

                if (hit.distance > 0f)
                {
                    shadow.position = hit.point + hit.normal * 0.1f;
                    shadow.rotation = rotations[i];
                }
            }
        }

        [BurstCompile]
        private struct BlobShadowRotationJob : IJobParallelFor
        {
            [ReadOnly] public NativeArray<RaycastHit> Hits;
            public NativeArray<quaternion> Rotations;

            public void Execute(int index)
            {
                var hit = Hits[index];

                if (hit.distance <= 0f)
                {
                    Rotations[index] = quaternion.identity;
                    return;
                }

                Rotations[index] = Quaternion.FromToRotation(Vector3.up, hit.normal);
            }
        }
    }
}
