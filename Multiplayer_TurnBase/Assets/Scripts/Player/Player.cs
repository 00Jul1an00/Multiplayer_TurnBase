using JHelpers;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class Player : MonoBehaviour
{
    [field: SerializeField] public Material PlayerMat { get; private set; }
    [field: SerializeField] public List<SerializebleKeyValuePair<Unit, int>> Army { get; private set; }
    [field: SerializeField] public Membersip Membership { get; private set; }
}
