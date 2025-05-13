using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class RuntimeSet<T> : ScriptableObject
{
    [TextArea(4, 6)]
    [SerializeField] string note;

    [Tooltip("Just for debugging")]
    [SerializeField] int currentLength;

    [HideInInspector] private List<T> list = new List<T>();

    public event Action RemoveEvent;
    public event Action EmptyEvent;

    void OnValidate()
    {
        currentLength = Count;
    }

    public void Add(T item)
    {
        list.Add(item);
#if UNITY_EDITOR
        currentLength = Count;
#endif
    }

    public void Remove(T item)
    {
        list.Remove(item);
        RemoveEvent?.Invoke();
        if (list.Count <= 0)
        {
            EmptyEvent?.Invoke();
        }
#if UNITY_EDITOR
        currentLength = Count;
#endif
    }

    public List<T> GetList()
    {
        return list;
    }

    public T this[int index]
    {
        get => list[index];
    }

    public T GetAndRemove(int index) {
        T temp = list[index];
        Remove(temp);
        return temp;
    }

    public int Count
    {
        get
        {
            return list.Count;
        }
    }

    public void ApplyToAllElementsInSet(Action<T> action)
    {
        foreach(var e in list)
        {
            action(e);
        }
    }
}