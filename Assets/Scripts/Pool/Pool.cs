using System;
using System.Collections.Generic;

[System.Serializable]
public class Pool<T>
{
    public delegate void HandlePoolEvent(T poolObj);
    public event HandlePoolEvent OnAddToPool;
    public event HandlePoolEvent OnGetFromPool;

    public int Size {
        get {
            return _pool.Count;
        }
    }

    protected List<T> _pool;
    
    public Pool()
    {
        _pool = new List<T>();
    }

    public virtual void AddToPool(params T[] objs)
    {
        if (_pool == null)
            _pool = new List<T>();

        foreach (T obj in objs)
        {
            _pool.Add(obj);
            OnAddToPool?.Invoke(obj);
        }
    }

    public virtual void AddToPool(T obj)
    {
        if (_pool == null)
            _pool = new List<T>();

        _pool.Add(obj);
        OnAddToPool?.Invoke(obj);
    }

    public virtual T GetFromPool(Predicate<T> conditional)
    {
        foreach(T obj in _pool)
        {
            if (conditional(obj))
            {
                OnGetFromPool?.Invoke(obj);
                return obj;
            }
        }

        return default;
    }

    public virtual void IteratePool(Action<T> iterationAction)
    {
        foreach (T i in _pool) iterationAction(i);
    }

    public virtual void ClearPool()
    {
        _pool.Clear();
    }
}
