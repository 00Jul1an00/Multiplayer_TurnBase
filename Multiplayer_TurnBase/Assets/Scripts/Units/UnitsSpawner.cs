using JHelpers;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Field
{
    public class UnitsSpawner : Spawner
    {
        public void SpawnUnits(List<SerializebleKeyValuePair<Unit, int>> unitsWithCount, Material mat, Membersip membersip)
        {
            for (int i = 0; i < unitsWithCount.Count; i++)
            {
                for (int j = 0; j < unitsWithCount[i].Value; j++)
                {
                    var unit = TrySpawnObject(unitsWithCount[i].Key.gameObject);
                    
                    if (unit == null)
                    {
                        continue;
                    }
                    Unit unitComponent = unit.GetComponent<Unit>();
                    unitComponent.Init(membersip);
                    unitComponent.UnitMesh.SetMaterials(new List<Material>() { mat });
                }
            }
        }
    }
}
