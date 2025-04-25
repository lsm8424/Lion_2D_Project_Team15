using UnityEngine;
using System.Collections.Generic;
using System;

public class ObjectPool
{
    readonly GameObject _prefab;
    readonly Queue<GameObject> _pool = new();
    Transform _parent;


    public ObjectPool(Transform parent, GameObject prefab, int initialSize = 0)
    {
        _parent = parent;
        _prefab = prefab;

        for (int i = 0; i < initialSize; ++i)
        {
            GameObject spawnedObject = GameObject.Instantiate(prefab, parent);
            spawnedObject.SetActive(false);
            _pool.Enqueue(spawnedObject);
        }
    }

    public GameObject Get()
    {
        if (_pool.Count == 0)
        {
            GameObject spawnedObject = Spawn();
            return spawnedObject;
        }

        GameObject poolObject = _pool.Dequeue();
        poolObject.SetActive(true);
        return poolObject;
    }

    public void ReturnToPool(GameObject spawnedObject)
    {
        _pool.Enqueue(spawnedObject);
        spawnedObject.SetActive(false);
    }

    public void AddObject()
    {
        GameObject spawnedObject = Spawn();
        spawnedObject.SetActive(false);
        _pool.Enqueue(spawnedObject);
    }

    public void AddObject(int size)
    {
        for (int i = 0; i < size; ++i)
        {
            GameObject spawnedObject = Spawn();
            spawnedObject.SetActive(false);
            _pool.Enqueue(spawnedObject);
        }
    }

    GameObject Spawn()
    {
        GameObject spawnedObject = GameObject.Instantiate(_prefab, _parent);
        spawnedObject.name = _prefab.name;
        return spawnedObject;
    }
}
