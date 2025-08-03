using Units;
using JHelpers;
using UnityEngine;
using Signals;
using Unity.Netcode;
using UnityEngine.AI;

namespace GameSystems
{
    public class InputEventsReceiver : NetworkBehaviour
    {
        private readonly PathVisualizer _visualizer;
        private readonly InputHandler _inputHandler;
        private readonly Camera _mainCamera;
        private readonly Membersip _membership;
        private readonly EventBus _eventBus;

        private Unit _selectedUnit;
        private bool _canMove;
        private bool _canAttack;
        private bool _canInterract;

        public InputEventsReceiver(
            InputHandler inputHandler,
            PathVisualizer visualizer,
            Membersip membersip,
            EventBus eventBus)
        {
            _inputHandler = inputHandler;
            _visualizer = visualizer;
            _mainCamera = Camera.main;
            _membership = membersip;
            _eventBus = eventBus;

            Subscribe();
        }

        ~InputEventsReceiver()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            _inputHandler.OnLeftBtnClick += HandleLeftClick;
            _inputHandler.OnRightBtnClick += HandleRightClick;
            _inputHandler.OnDoubleRightBtnClick += HandleDoubleRightClick;
            _eventBus.Subscribe<ChangeTurnSignal>(OnTurnChanged);
        }

        private void Unsubscribe()
        {
            _inputHandler.OnLeftBtnClick -= HandleLeftClick;
            _inputHandler.OnRightBtnClick -= HandleRightClick;
            _inputHandler.OnDoubleRightBtnClick -= HandleDoubleRightClick;
            _eventBus.Unsubscribe<ChangeTurnSignal>(OnTurnChanged);
        }

        private void HandleLeftClick()
        {
            if (!_canInterract)
            {
                return;
            }

            ClearSelection();

            if (TryGetUnitUnderCursor(out Unit unit) && IsSelectable(unit))
            {
                SelectUnit(unit);
            }
        }

        private void HandleRightClick()
        {
            if (!_canInterract || _selectedUnit == null)
            {
                return;
            }

            if (TryGetUnitUnderCursor(out Unit targetUnit))
            {
                HandleUnitInteraction(targetUnit);
            }
            else if (_canMove)
            {
                HandleMovementInteraction();
            }
        }

        private void HandleDoubleRightClick()
        {
            if (!_canInterract || !_canMove || _selectedUnit == null)
            {
                return;
            }

            if (_selectedUnit.MoveComponent.LastValidPath == null)
            {
                return;
            }

            ExecuteMovement();
        }

        private void HandleUnitInteraction(Unit targetUnit)
        {
            if (IsEnemyUnit(targetUnit) && _canAttack)
            {
                TryAttackTarget(targetUnit);
            }
        }

        private void HandleMovementInteraction()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                VisualizeMovementPath(hit.point);
            }
        }

        private void SelectUnit(Unit unit)
        {
            _selectedUnit = unit;
            _selectedUnit.SelectComponent.Select();
            _selectedUnit.AttackComponent.ShowAttackRadiusAt(_selectedUnit.transform.position);
        }

        private void ClearSelection()
        {
            if (_selectedUnit == null)
            {
                return;
            }

            _visualizer.ClearPath();
            _selectedUnit.SelectComponent.Deselect();
            _selectedUnit.AttackComponent.HideAttackRadius();
            _selectedUnit = null;
        }

        private void VisualizeMovementPath(Vector3 position)
        {
            var path = _selectedUnit.MoveComponent.TryGetPath(position);
            _visualizer.SetPath(path);

            if (path != null && path.corners.Length > 0)
            {
                _selectedUnit.AttackComponent.ShowAttackRadiusAt(position);
            }
            else
            {
                _selectedUnit.AttackComponent.HideAttackRadius();
            }
        }

        private void TryAttackTarget(Unit target)
        {
            if (!_selectedUnit.AttackComponent.TryAttackTarget(target))
            {
                return;
            }

            ClearSelection();
            _canAttack = false;
            _eventBus.Invoke(new AttackSuccesSignal());
            CheckTurnCompletion();
        }

        private void ExecuteMovement()
        {
            _canMove = false;
            _visualizer.ClearPath();
            _selectedUnit.AttackComponent.HideAttackRadius();
            _selectedUnit.MoveComponent.MoveToLastValidPath();
            _eventBus.Invoke(new MoveSuccesSignal());
            CheckTurnCompletion();
        }

        [ServerRpc]
        private void TryAttackTargetServerRpc(ulong targetId)
        {
            if (NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(targetId, out NetworkObject targetObj))
            {
                Unit targetUnit = targetObj.GetComponent<Unit>();
                TryAttackTarget(targetUnit);
            }
        }

        [ServerRpc]
        private void MoveUnitServerRpc(ulong unitId)
        {
            if (NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(unitId, out NetworkObject unitObj))
            {
                Unit unit = unitObj.GetComponent<Unit>();
                unit.MoveComponent.MoveToLastValidPath();
            }
        }

        private bool TryGetUnitUnderCursor(out Unit unit)
        {
            unit = null;
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                unit = hit.collider.GetComponent<Unit>();
            }

            return unit != null;
        }

        private bool IsSelectable(Unit unit)
        {
            return unit.Membersip.Value == _membership;
        }

        private bool IsEnemyUnit(Unit unit)
        {
            return unit.Membersip.Value != _selectedUnit.Membersip.Value;
        }

        private void CheckTurnCompletion()
        {
            if (!_canAttack && !_canMove)
            {
                _canInterract = false;
                _eventBus.Invoke(new RequestToChangeTurnSignal());
            }
        }

        private void OnTurnChanged(ChangeTurnSignal signal)
        {
            ClearSelection();

            if (signal.ChangeToMembersip != _membership)
            {
                _canInterract = false;
                return;
            }

            ResetActions();
        }

        private void ResetActions()
        {
            _canAttack = true;
            _canMove = true;
            _canInterract = true;
        }
    }
}