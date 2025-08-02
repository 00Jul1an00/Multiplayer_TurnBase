using System.Collections.Generic;
using UnityEngine;

namespace JHelpers
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] protected float _minRangeBetweenObjects;
        [SerializeField] protected BoxCollider _spawnZone;
        [SerializeField] protected int _maxAttemptsToFindRandPos;

        protected List<Vector3> _spawnedPositions = new();
        protected Dictionary<GameObject, Bounds> _cachedBounds = new();

        protected GameObject TrySpawnObject(GameObject gameObjectToSpawn, bool randomizeRotation = false)
        {
            Vector3 spawnPoint;
            bool positionFound = false;
            int attempts = 0;
            Bounds bounds = new();
            GameObject objectToReturn = null;

            if (_cachedBounds.ContainsKey(gameObjectToSpawn))
            {
                bounds = _cachedBounds[gameObjectToSpawn];
            }
            else
            {
                bounds = CalculateObjectBounds(gameObjectToSpawn);
                _cachedBounds.Add(gameObjectToSpawn, bounds);
            }

            float gameObjectRadius = Mathf.Max(bounds.extents.x, bounds.extents.z);

            while (!positionFound && attempts < _maxAttemptsToFindRandPos)
            {
                spawnPoint = GetRandomPointInCollider();
                Quaternion rotation = Quaternion.identity;

                if (randomizeRotation)
                {
                    rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
                }

                if (IsPositionValid(spawnPoint, bounds, gameObjectRadius))
                {
                    objectToReturn = SpawnObjectAtPosition(gameObjectToSpawn, spawnPoint, rotation);
                    positionFound = true;
                }

                attempts++;
            }

            if (!positionFound)
            {
                Debug.LogWarning($"Ќе удалось найти место дл€ {gameObjectToSpawn.name} после {_maxAttemptsToFindRandPos} попыток");
            }

            return objectToReturn;
        }

        private Bounds CalculateObjectBounds(GameObject gameObjectToSpawn)
        {
            GameObject tempObj = Instantiate(gameObjectToSpawn, Vector3.one, Quaternion.identity);
            tempObj.SetActive(true);
            Renderer[] renderers = tempObj.GetComponentsInChildren<Renderer>();
            Bounds bounds = new Bounds();

            if (renderers.Length > 0)
            {
                bounds = renderers[0].bounds;
                foreach (Renderer r in renderers)
                {
                    bounds.Encapsulate(r.bounds);
                }
            }

            Destroy(tempObj);
            return bounds;
        }

        private Vector3 GetRandomPointInCollider()
        {
            Bounds bounds = _spawnZone.bounds;
            return new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                0f,
                Random.Range(bounds.min.z, bounds.max.z)
            );
        }

        private GameObject SpawnObjectAtPosition(GameObject gameObjectToSpawn, Vector3 position, Quaternion rotation)
        {
            var objectToReturn = Instantiate(gameObjectToSpawn, position, rotation, transform);
            _spawnedPositions.Add(position);
            return objectToReturn;
        }

        private bool IsPositionValid(Vector3 position, Bounds gameObjectBounds, float gameObjectRadius)
        {
            foreach (Vector3 otherPos in _spawnedPositions)
            {
                float requiredDistance = _minRangeBetweenObjects + gameObjectRadius;
                if (Vector3.Distance(position, otherPos) < requiredDistance)
                {
                    return false;
                }
            }

            Bounds worldBounds = new(position, gameObjectBounds.size);
            Vector3 min = worldBounds.min;
            Vector3 max = worldBounds.max;
            Vector3[] corners = new Vector3[]
            {
                new (min.x, 0f, min.z),
                new (min.x, 0f, max.z),
                new (max.x, 0f, min.z),
                new (max.x, 0f, max.z)
            };

            foreach (Vector3 corner in corners)
            {
                if (!_spawnZone.bounds.Contains(corner))
                {
                    return false;
                }
            }

            return true;
        }
    }
}