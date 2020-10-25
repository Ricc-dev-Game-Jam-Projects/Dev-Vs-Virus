using System;
using UnityEngine;

public class PrefabPool : Pool<GameObject> 
{
    private readonly Predicate<GameObject> poolCondition = (obj) => !obj.activeInHierarchy;

    public GameObject GetPrefabFromPool(GameObject prefab, Vector2 position = default)
    {
        GameObject objFromPool = GetFromPool(poolCondition);
        if (objFromPool == null)
        {
            objFromPool = GameObject.Instantiate(prefab);
            AddToPool(objFromPool);
        }

        objFromPool.SetActive(true);
        if(position != null) objFromPool.transform.position = position;
        return objFromPool;
    }

    public override void ClearPool()
    {
        IteratePool((obj) =>
        {
            GameObject.Destroy(obj);
        });
        base.ClearPool();
    }

    public int AliveObjects()
    {
        int counter = 0;
        IteratePool((obj) => { if (obj.activeInHierarchy) counter++; });
        return counter;
    }
}
