using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

namespace Field 
{
    public class PlayingField : MonoBehaviour
    {
        [SerializeField] private List<UnitsSpawner> _unitsSpawners;
        [SerializeField] private ObstacleSpawner _obstacleSpawner;
        [SerializeField] private List<Player> _players;
        [SerializeField] private NavMeshSurface _navMeshSurface;

        public void Init()
        {
            _obstacleSpawner.SpawnObstacles();

            if (_players.Count != _unitsSpawners.Count)
            {
                Debug.LogError("Players List and Units Spawner List has different Count");
            }

            for (int i = 0; i < _unitsSpawners.Count; i++) 
            {
                _unitsSpawners[i].SpawnUnits(_players[i].Army, _players[i].PlayerMat, _players[i].Membership);
            }
        }
    }
}