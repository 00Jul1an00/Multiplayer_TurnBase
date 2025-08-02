using Units;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private LayerMask _unitLayer;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private PathVisualizer _visualizer;

    private Camera _mainCamera;
    private Unit _selectedUnit;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        //if (!TurnManager.Instance.CanSelectUnit())
        //    return;

        if (Input.GetMouseButtonDown(0))
        {
            HandleSelectionClick();
        }

        if (Input.GetMouseButtonDown(1))
        {
            HandleMovementClick();
        }
    }

    private void HandleSelectionClick()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _unitLayer))
        {
            Unit unit = hit.collider.GetComponent<Unit>();
            if (unit != null)
            {
                _selectedUnit = unit;
                _selectedUnit.SelectComponent.ToggleSelection();
            }
        }
    }

    private void HandleMovementClick()
    {
        if(_selectedUnit == null)
        {
            return;
        }

        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _groundLayer))
        {
            _visualizer.SetSelectedUnit(_selectedUnit);
            _selectedUnit.MoveComponent.TryMoveTo(hit.point);
        }
    }
}
