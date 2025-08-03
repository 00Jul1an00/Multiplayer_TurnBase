using UnityEngine;

namespace Units
{
    public class Unit : MonoBehaviour
    {
        [field: SerializeField] public SelectComponent SelectComponent { get; private set; }
        [field: SerializeField] public MoveComponent MoveComponent { get; private set; }
        [field: SerializeField] public AttackComponent AttackComponent { get; private set; }
        [field: SerializeField] public MeshRenderer UnitMesh { get; private set; }
        
        [SerializeField] private UnitSO _config;

        public Membersip Membersip { get; private set; }

        public void Init(Membersip membersip)
        {
            Membersip = membersip;
            SelectComponent.InitComponent();
            MoveComponent.InitComponent(_config.Speed);
            AttackComponent.InitComponent(_config.Range, Membersip);
        }
    }
}