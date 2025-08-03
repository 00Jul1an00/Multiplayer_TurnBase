using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    public class AttackComponent : MonoBehaviour
    {
        private float _attackRange;
        private GameObject rangeIndicator;
        private List<Unit> _unitsInRange = new();
        private Membersip _membersip;

        public void InitComponent(float attackRange, Membersip membersip)
        {
            _attackRange = attackRange;
            _membersip = membersip;
            CreateRangeIndicator();
        }

        public void ShowAttackRadiusAt(Vector3 position)
        {
            rangeIndicator.transform.position = new Vector3(position.x, 0, position.z);
            rangeIndicator.SetActive(true);
            ClearEnemyHighlights();
            HighlightEnemiesInRange(position);
        }

        public void HideAttackRadius()
        {
            rangeIndicator.SetActive(false);
            ClearEnemyHighlights();
        }

        public bool TryAttackTarget(Unit unit)
        {
            if (unit.Membersip.Value == _membersip)
            {
                return false;
            }

            if (!UnitsInAttackRange().Contains(unit))
            {
                return false;
            }

            _unitsInRange.Remove(unit);
            Destroy(unit.gameObject);
            return true;
        }
        
        private void CreateRangeIndicator()
        {
            rangeIndicator = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            rangeIndicator.transform.SetParent(transform);
            rangeIndicator.transform.localScale = new Vector3(_attackRange * 2, 0.01f, _attackRange * 2);
            rangeIndicator.transform.localPosition = Vector3.zero;
            Destroy(rangeIndicator.GetComponent<Collider>());
            rangeIndicator.GetComponent<Renderer>().material.color = new Color(0.7f, 0.7f, 0.7f, 0.01f);
            rangeIndicator.SetActive(false);
        }

        private List<Unit> UnitsInAttackRange() 
        {
            List<Unit> listToReturn = new List<Unit>();

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _attackRange);
            foreach (var collider in hitColliders)
            {
                if (collider.TryGetComponent<Unit>(out var unit))
                {
                    if (unit.Membersip.Value == _membersip)
                    {
                        continue;
                    }

                    if (IsNoObstaclesOnWay(transform.position, unit.transform))
                    {
                        listToReturn.Add(unit);
                    }
                }
            }

            return listToReturn;
        }

        private void HighlightEnemiesInRange(Vector3 center)
        {
            Collider[] hitColliders = Physics.OverlapSphere(center, _attackRange);
            foreach (var collider in hitColliders)
            {
                if (collider.TryGetComponent<Unit>(out var unit))
                {
                    if (unit.Membersip.Value == _membersip)
                    {
                        continue;
                    }

                    if (IsNoObstaclesOnWay(center, unit.transform))
                    {
                        unit.SelectComponent.Select();
                        _unitsInRange.Add(unit);
                    }
                }
            }
        }

        private bool IsNoObstaclesOnWay(Vector3 from, Transform unitTransform)
        {
            Vector3 start = new Vector3(from.x, from.y, from.z);
            Vector3 end = new Vector3(unitTransform.position.x, unitTransform.position.y, unitTransform.position.z);

            if (Physics.Linecast(start, end, out RaycastHit hit))
            {
                return !hit.collider.TryGetComponent<NavMeshObstacle>(out var _);
            }
            return true;
        }

        private void ClearEnemyHighlights()
        {
            foreach (var unit in _unitsInRange)
            {
                if (unit == null)
                {
                    continue;
                }

                unit.SelectComponent.Deselect();
            }

            _unitsInRange.Clear();
        }
    }
}