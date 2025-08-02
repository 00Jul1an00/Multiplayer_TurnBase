using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    public class MoveComponent : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private int _maxMovementRange = 5;

        private NavMeshAgent _agent;
        private Vector3 _originalPosition;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _originalPosition = transform.position;
        }

        public bool TryMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            if (!_agent.CalculatePath(destination, path))
            {
                return false;
            }

            float pathLength = CalculatePathLength(path);
            int movementCost = Mathf.CeilToInt(pathLength);

            if (movementCost > _maxMovementRange)
            {
                return false;
            }

            _agent.path = path;
            return true;
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
                return;

            // Плавное перемещение
            transform.position = Vector3.MoveTowards(
                transform.position,
                _agent.nextPosition,
                _agent.speed * Time.deltaTime
            );
        }
    }
}