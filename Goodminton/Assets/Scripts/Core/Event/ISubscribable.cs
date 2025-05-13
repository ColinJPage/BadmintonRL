using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ISubscribable<T>
{
    public void Subscribe(T function)
    {
    }

    public void Unsubscribe(T function)
    {
    }
}