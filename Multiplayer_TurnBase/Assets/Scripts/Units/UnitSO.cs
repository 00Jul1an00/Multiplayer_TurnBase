using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit")]
public class UnitSO : ScriptableObject
{
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public float Range { get; private set; }
    [field: SerializeField] public GameObject ModelPrefab { get; private set; }

}
