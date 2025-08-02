using JHelpers;
using System.Collections.Generic;
using UnityEngine;

namespace Field
{
    public class UnitsSpawner : Spawner
    {
        public void SpawnUnits(List<SerializebleKeyValuePair<UnitSO, int>> unitsWithCount, Material mat)
        {
            for (int i = 0; i < unitsWithCount.Count; i++)
            {
                for (int j = 0; j < unitsWithCount[i].Value; j++)
                {
                    var unit = TrySpawnObject(unitsWithCount[i].Key.ModelPrefab.gameObject);
                    unit.GetComponent<MeshRenderer>().SetMaterials(new List<Material>() { mat });
                }
            }
        }
    }
}
