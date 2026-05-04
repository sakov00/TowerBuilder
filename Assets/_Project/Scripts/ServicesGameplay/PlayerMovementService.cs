using System.Collections.Generic;
using _Project.Scripts.GameObjects;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Registries;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.ServicesGameplay
{
    public class PlayerMovementService : ITickable
    {
        [Inject] private LiveRegistry _liveRegistry;

        private PlayerController _player;

        public void Tick()
        {
            if (_player == null)
            {
                var players = new List<PlayerController>();
                _liveRegistry.GetAllByType(players);
                _player = players[0];
            }

            HandleKeyboardInput(_player);
        }

        private void HandleKeyboardInput(IMovable movable)
        {
            Vector3 input = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) input += Vector3.back * 0.1f;
            if (Input.GetKey(KeyCode.S)) input += Vector3.forward * 0.1f;
            if (Input.GetKey(KeyCode.A)) input += Vector3.right * 0.1f;
            if (Input.GetKey(KeyCode.D)) input += Vector3.left * 0.1f;

            if (input == Vector3.zero)
            {
                if (movable.IsMoving)
                    movable.Stop();
                return;
            }

            input.Normalize();

            var targetPosition = movable.Position + input;
            movable.MoveTo(targetPosition);
        }
    }
}