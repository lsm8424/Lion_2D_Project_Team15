using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    readonly Dictionary<string, ObjectPool> _poolDictionary = new();


    public void CreatePool(GameObject prefab, int size) => CreatePool(prefab.name, prefab, size);

    public void CreatePool(string poolName, GameObject prefab, int size)
    {
        if (!_poolDictionary.ContainsKey(poolName))
            _poolDictionary.Add(poolName, new ObjectPool(transform, prefab));

        var pool = _poolDictionary[poolName];
        pool.AddObject(size);
    }

    public GameObject Get(string poolName)
    {
        if (!_poolDictionary.ContainsKey(poolName))
            return null;

        return _poolDictionary[poolName].Get();
    }

    public void ReturnToPool(GameObject pooledObject) => ReturnToPool(pooledObject.name, pooledObject);

    public void ReturnToPool(string poolName, GameObject pooledObject)
    {
        if (!_poolDictionary.ContainsKey(poolName))
        {
            Destroy(pooledObject);
            return;
        }

        _poolDictionary[poolName].ReturnToPool(pooledObject);
    }
}
