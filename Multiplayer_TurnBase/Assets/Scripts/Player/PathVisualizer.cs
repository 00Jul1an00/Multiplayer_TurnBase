using Units;
using UnityEngine;
using UnityEngine.AI;

public class PathVisualizer : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _yOffset = 0.1f;

    public void SetPath(NavMeshPath path)
    {
        _lineRenderer.positionCount = 0;
        
        if (path == null || path.corners.Length == 0)
        {
            _lineRenderer.positionCount = 0;
            return;
        }

        _lineRenderer.positionCount = path.corners.Length;

        for (int i = 0; i < path.corners.Length; i++)
        {
            _lineRenderer.SetPosition(i, path.corners[i] + Vector3.up * _yOffset);
        }
    }
}
