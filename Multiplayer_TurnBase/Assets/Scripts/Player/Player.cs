using JHelpers;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [field: SerializeField] public Material PlayerMat { get; private set; }
    [field: SerializeField] public List<SerializebleKeyValuePair<UnitSO, int>> Army { get; private set; }
}
