using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    public class MoveComponent : MonoBehaviour
    {
        public NavMeshPath LastValidPath { get; private set; }

        private float _maxMovementRange;
        private NavMeshAgent _agent;

        public void InitComponent(float maxMovementRange)
        {
            _agent = GetComponent<NavMeshAgent>();
            _maxMovementRange = maxMovementRange;
        }

        public NavMeshPath TryGetPath(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            if (!_agent.CalculatePath(destination, path))
            {
                LastValidPath = null;
                return null;
            }

            float pathLength = CalculatePathLength(path);

            if (pathLength > _maxMovementRange)
            {
                LastValidPath = null;
                return null;
            }

            LastValidPath = path;
            return path;
        }

        public void MoveToLastValidPath()
        {
            _agent.avoidancePriority = 100;
            _agent.path = LastValidPath;
        }

        private float CalculatePathLength(NavMeshPath path)
        {
            float length = 0f;
            
            if (path.corners.Length < 2)
            {
                return length;
            }

            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                length += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return length;
        }

        private void Update()
        {
            if (_agent.pathPending || _agent.remainingDistance < 0.1f)
            {
                _agent.avoidancePriority = 50;
                return;
            }

            // Плавное перемещение
            transform.position = Vector3.MoveTowards(
                transform.position,
                _agent.nextPosition,
                _agent.speed * Time.deltaTime
            );
        }
    }
}