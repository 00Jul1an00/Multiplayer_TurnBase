using System;
using Units;
using UnityEngine;

namespace GameSystems
{
    public class InputEventsReciver
    {
        private PathVisualizer _visualizer;
        private InputHandler _inputHandler;
        private Camera _mainCamera;
        private Unit _selectedUnit;
        private Membersip _membership;

        private bool _canMove;
        private bool _canAttack;
        private bool _canInterract;

        public event Action IsNoOptionsToTurn;

        public InputEventsReciver(
            InputHandler inputHandler,
            PathVisualizer visualizer,
            Membersip membersip)
        {
            _inputHandler = inputHandler;
            _visualizer = visualizer;
            _mainCamera = Camera.main;
            _membership = membersip;
            Subribes();
        }

        ~InputEventsReciver()
        {
            Unsubribes();
        }

        private void Subribes()
        {
            _inputHandler.OnLeftBtnClick += HandleLeftBtnClick;
            _inputHandler.OnRightBtnClick += HandleRightBtClick;
            _inputHandler.OnDoubleRightBtnClick += HandleDoubleRightBtnClick;
        }

        private void Unsubribes()
        {
            _inputHandler.OnLeftBtnClick -= HandleLeftBtnClick;
            _inputHandler.OnRightBtnClick -= HandleRightBtClick;
            _inputHandler.OnDoubleRightBtnClick -= HandleDoubleRightBtnClick;
        }

        private void HandleLeftBtnClick()
        {
            if (!_canInterract)
            {
                return;
            }

            if (_selectedUnit != null)
            {
                _visualizer.SetPath(null);
                _selectedUnit.AttackComponent.HideAttackRadius();
            }

            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                Unit unit = hit.collider.GetComponent<Unit>();

                if (unit == null)
                {
                    return;
                }

                if (unit.Membersip != _membership)
                {
                    return;
                }

                if (_selectedUnit != null && unit != _selectedUnit)
                {
                    _selectedUnit.SelectComponent.Deselect();
                }

                _selectedUnit = unit;
                _visualizer.SetPath(null);
                _selectedUnit.SelectComponent.Select();
                _selectedUnit.AttackComponent.ShowAttackRadiusAt(_selectedUnit.transform.position);
            }
        }

        private void HandleRightBtClick()
        {
            if (!_canInterract)
            {
                return;
            }

            if (_selectedUnit == null)
            {
                return;
            }

            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.collider.TryGetComponent<Unit>(out var unit))
                {
                    if (unit.Membersip != _selectedUnit.Membersip)
                    {
                        if (!_canAttack)
                        {
                            return;
                        }

                        bool isAttackSucces = _selectedUnit.AttackComponent.TryAttackTarget(unit);

                        if (isAttackSucces)
                        {
                            _canAttack = false;
                            CheckIsAnyOptionToTurn();
                        }

                        return;
                    }
                }

                if (!_canMove)
                {
                    return;
                }

                var path = _selectedUnit.MoveComponent.TryGetPath(hit.point);
                _visualizer.SetPath(path);
                _selectedUnit.AttackComponent.ShowAttackRadiusAt(hit.point);

                if (path == null || path.corners.Length == 0)
                {
                    _selectedUnit.AttackComponent.HideAttackRadius();
                }
            }
        }

        private void HandleDoubleRightBtnClick()
        {
            if (!_canInterract)
            {
                return;
            }

            if (!_canMove)
            {
                return;
            }

            if (_selectedUnit == null)
            {
                return;
            }

            if (_selectedUnit.MoveComponent.LastValidPath == null)
            {
                return;
            }

            _canMove = false;
            _visualizer.SetPath(null);
            _selectedUnit.AttackComponent.HideAttackRadius();
            _selectedUnit.MoveComponent.MoveToLastValidPath();
            CheckIsAnyOptionToTurn();
        }

        private void CheckIsAnyOptionToTurn()
        {
            if (!_canAttack && !_canMove)
            {
                IsNoOptionsToTurn?.Invoke();
            }
        }
    }
}
