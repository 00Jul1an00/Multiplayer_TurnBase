using UnityEngine;

namespace Units
{
    public class Unit : MonoBehaviour
    {
        [field: SerializeField] public SelectComponent SelectComponent { get; private set; }
        [field: SerializeField] public MoveComponent MoveComponent { get; private set; }
    }
}