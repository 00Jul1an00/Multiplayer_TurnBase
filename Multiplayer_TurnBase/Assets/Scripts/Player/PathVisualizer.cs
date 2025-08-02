using Units;
using UnityEngine;
using UnityEngine.AI;

public class PathVisualizer : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _yOffset = 0.1f;

    private Unit _selectedUnit;

    private void Start()
    {
        _lineRenderer.positionCount = 0;
    }

    private void Update()
    {
        if (_selectedUnit == null) return;

        NavMeshAgent agent = _selectedUnit.GetComponent<NavMeshAgent>();
        if (agent.path == null || agent.path.corners.Length == 0)
        {
            _lineRenderer.positionCount = 0;
            return;
        }

        // Обновляем линию пути
        _lineRenderer.positionCount = agent.path.corners.Length;
        for (int i = 0; i < agent.path.corners.Length; i++)
        {
            _lineRenderer.SetPosition(i, agent.path.corners[i] + Vector3.up * _yOffset);
        }
    }

    public void SetSelectedUnit(Unit unit)
    {
        _selectedUnit = unit;
        _lineRenderer.positionCount = 0;
    }
}
