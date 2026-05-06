using System.Linq;
using UnityEngine;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects;
using _Project.Scripts.Registries;
using VContainer.Unity;

namespace _Project.Scripts.ServicesGameplay
{
    public class BlockPlacementService
    {
        private readonly BlockSpawnService _spawn;

        private float _tolerance = 0.75f;

        public BlockPlacementService(BlockSpawnService spawn)
        {
            _spawn = spawn;
        }

        public void Resolve(BuildController current, BuildController previous)
        {
            Vector3 curPos = current.transform.position;
            Vector3 prevPos = previous.transform.position;

            float offset = curPos.x - prevPos.x;

            if (Mathf.Abs(offset) <= _tolerance)
            {
                //curPos.x = prevPos.x;
                current.transform.position = curPos;

                current.SetState(BuildState.Placed);
                current.SetKinematicState(RigidbodyType2D.Static);

                Debug.Log("Success");

                _spawn.SpawnNext();
            }
            else
            {
                current.SetState(BuildState.Failed);
                current.DisposeDelayed().Forget();
                Debug.Log("Failed");

                _spawn.SpawnNext();
            }
        }
    }
}