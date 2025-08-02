using JHelpers;
using System.Collections.Generic;
using UnityEngine;

namespace Field
{
    public class ObstacleSpawner : Spawner
    {
        [SerializeField] private List<Obstacle> _obstacles;

        public void SpawnObstacles()
        {
            // Spawn min count
            for (int i = 0; i < _obstacles.Count; i++)
            {
                for (int j = 0; j < _obstacles[i].MinCountOnField; j++)
                {
                    TrySpawnObject(_obstacles[i].gameObject, true);
                }
            }

            // Try spawn max count
            for (int i = 0; i < _obstacles.Count; i++)
            {
                for (int j = 0; j < _obstacles[i].MaxCountOnField - _obstacles[i].MinCountOnField; j++)
                {
                    TrySpawnObject(_obstacles[i].gameObject, true);
                }
            }
        }
    }
}