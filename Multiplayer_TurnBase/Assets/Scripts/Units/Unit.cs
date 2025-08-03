using Unity.Netcode;
using UnityEngine;

namespace Units
{
    public class Unit : NetworkBehaviour
    {
        [field: SerializeField] public SelectComponent SelectComponent { get; private set; }
        [field: SerializeField] public MoveComponent MoveComponent { get; private set; }
        [field: SerializeField] public AttackComponent AttackComponent { get; private set; }
        [field: SerializeField] public MeshRenderer UnitMesh { get; private set; }
        
        [SerializeField] private UnitSO _config;

        public NetworkVariable<Membersip> Membersip { get; private set; } = new();

        public void Init(Membersip membersip)
        {
            Membersip.Value = membersip;
            SelectComponent.InitComponent();
            MoveComponent.InitComponent(_config.Speed);
            AttackComponent.InitComponent(_config.Range, Membersip.Value);
        }
    }
}