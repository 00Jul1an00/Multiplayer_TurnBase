using UnityEngine;

namespace Field 
{
    public class Obstacle : MonoBehaviour
    {
        [field: SerializeField] public int MinCountOnField { get; private set; }
        [field: SerializeField] public int MaxCountOnField { get; private set; }
    }
}