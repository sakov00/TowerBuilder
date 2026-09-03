using System.Collections.Generic;
using _Project.Scripts._GlobalLogic;
using _Project.Scripts.GameObjects;
using _Project.Scripts.SO;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace _Project.Scripts.ServicesGameplay
{
    public class EnvironmentMovementService : ITickable
    {
        private readonly EnvironmentMovementConfig _config;

        private readonly List<EnvironmentElementController> _elements = new();

        [Inject]
        public EnvironmentMovementService(EnvironmentMovementConfig config)
        {
            _config = config;
        }

        public void Tick()
        {
            MoveElements();
            RecycleElements();
        }

        public void Restart()
        {
            if (_elements.Count == 0)
            {
                InitializeElements();
                return;
            }

            float cameraCenterX =
                GlobalObjects.Camera.transform.position.x;

            float cameraTop = GetCameraTop();
            float cameraBottom = GetCameraBottom();

            for (int i = 0; i < _elements.Count; i++)
            {
                ResetElement(
                    _elements[i],
                    cameraCenterX,
                    cameraBottom,
                    cameraTop);
            }
        }

        private void ResetElement(
            EnvironmentElementController element,
            float cameraCenterX,
            float cameraBottom,
            float cameraTop)
        {
            float x = cameraCenterX + Random.Range(
                _config.SpawnXRange.x,
                _config.SpawnXRange.y);

            float y = Random.Range(
                cameraBottom - _config.DespawnBelowCamera,
                cameraTop + _config.SpawnAboveCamera);

            element.transform.position = new Vector3(x, y, 0f);

            element.Initialize(
                GetRandomSprite(),
                GetRandomDirection());
        }

        private void InitializeElements()
        {
            while (_elements.Count < _config.ElementsCount)
            {
                SpawnInitialElement();
            }
        }

        private void SpawnInitialElement()
        {
            float cameraCenterX =
                GlobalObjects.Camera.transform.position.x;

            float cameraTop = GetCameraTop();
            float cameraBottom = GetCameraBottom();

            float x = cameraCenterX + Random.Range(
                _config.SpawnXRange.x,
                _config.SpawnXRange.y);

            float y = Random.Range(
                cameraBottom - _config.DespawnBelowCamera,
                cameraTop + _config.SpawnAboveCamera);

            CreateElement(x, y);
        }

        private void RecycleElements()
        {
            float cameraBottom = GetCameraBottom();

            float recycleY =
                cameraBottom - _config.DespawnBelowCamera;

            for (int i = 0; i < _elements.Count; i++)
            {
                EnvironmentElementController element = _elements[i];

                if (element.transform.position.y >= recycleY)
                    continue;

                RecycleElement(element);
            }
        }

        private void RecycleElement(
            EnvironmentElementController element)
        {
            float cameraCenterX =
                GlobalObjects.Camera.transform.position.x;

            float cameraTop = GetCameraTop();

            float x = cameraCenterX + Random.Range(
                _config.SpawnXRange.x,
                _config.SpawnXRange.y);

            float y = cameraTop
                      + _config.SpawnAboveCamera
                      + Random.Range(
                          _config.SpawnYRange.x,
                          _config.SpawnYRange.y);

            element.transform.position = new Vector3(x, y, 0f);

            element.Initialize(
                GetRandomSprite(),
                GetRandomDirection());
        }

        private void CreateElement(float x, float y)
        {
            EnvironmentElementController element = Object.Instantiate(
                _config.Prefab,
                new Vector3(x, y, 0f),
                Quaternion.identity);

            element.Initialize(
                GetRandomSprite(),
                GetRandomDirection());

            _elements.Add(element);
        }

        private void MoveElements()
        {
            float delta = _config.Speed * Time.deltaTime;

            float cameraLeft =
                GetCameraLeft() - _config.TurnOffset;

            float cameraRight =
                GetCameraRight() + _config.TurnOffset;

            for (int i = 0; i < _elements.Count; i++)
            {
                EnvironmentElementController element = _elements[i];

                Vector3 position = element.transform.position;

                position.x += delta * element.Direction;

                if (position.x <= cameraLeft &&
                    element.Direction < 0f)
                {
                    element.ReverseDirection();
                }
                else if (position.x >= cameraRight &&
                         element.Direction > 0f)
                {
                    element.ReverseDirection();
                }

                element.transform.position = position;
            }
        }

        private float GetRandomDirection()
        {
            return Random.value < 0.5f
                ? -1f
                : 1f;
        }

        private Sprite GetRandomSprite()
        {
            IReadOnlyList<Sprite> sprites =
                _config.EnvironmentSprites;

            if (sprites == null || sprites.Count == 0)
                return null;

            return sprites[Random.Range(0, sprites.Count)];
        }

        private float GetCameraLeft()
        {
            return GlobalObjects.Camera
                .ViewportToWorldPoint(
                    new Vector3(0f, 0.5f, 0f))
                .x;
        }

        private float GetCameraRight()
        {
            return GlobalObjects.Camera
                .ViewportToWorldPoint(
                    new Vector3(1f, 0.5f, 0f))
                .x;
        }

        private float GetCameraBottom()
        {
            return GlobalObjects.Camera
                .ViewportToWorldPoint(
                    new Vector3(0.5f, 0f, 0f))
                .y;
        }

        private float GetCameraTop()
        {
            return GlobalObjects.Camera
                .ViewportToWorldPoint(
                    new Vector3(0.5f, 1f, 0f))
                .y;
        }
    }
}