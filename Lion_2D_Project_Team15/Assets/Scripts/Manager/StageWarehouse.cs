using System.Collections.Generic;
using UnityEngine;

public class StageWarehouse : Singleton<StageWarehouse>
{
    // Key - id
    public Dictionary<string, GameObject> Warehouse { get; private set; } = new();
}
